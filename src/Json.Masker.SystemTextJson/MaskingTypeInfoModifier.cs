using System.Collections;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Json.Masker.Abstract;

namespace Json.Masker.SystemTextJson;

/// <summary>
/// Applies masking converters to properties marked with <see cref="SensitiveAttribute"/>, 
/// using aggressive caching and delegate factories for minimal overhead.
/// </summary>
public class MaskingTypeInfoModifier(IMaskingService maskingService)
{
    private static readonly ConcurrentDictionary<(Type DeclaringType, string PropertyName), SensitiveAttribute?> AttributeCache = new();
    private static readonly ConcurrentDictionary<(Type Type, MaskingStrategy Strategy, string? Pattern, bool IsCollection), JsonConverter> ConverterCache = new();
    private static readonly ConcurrentDictionary<Type, Func<IMaskingService, MaskingStrategy, string?, JsonConverter?, JsonConverter>> ScalarFactoryCache = new();

    public void Modify(JsonTypeInfo typeInfo)
    {
        foreach (var prop in typeInfo.Properties)
        {
            var attr = GetSensitiveAttribute(prop);
            if (attr is null)
            {
                continue;
            }

            var converter = GetOrCreateConverter(
                prop.PropertyType,
                attr.Strategy,
                attr.Pattern,
                prop.CustomConverter,
                typeInfo.Options);

            prop.CustomConverter = converter;
        }
    }

    private SensitiveAttribute? GetSensitiveAttribute(JsonPropertyInfo prop)
    {
        var member = prop.AttributeProvider as MemberInfo;
        if (member == null)
        {
            return null;
        }

        var key = (member.DeclaringType!, member.Name);
        return AttributeCache.GetOrAdd(key, static k =>
        {
            var (declType, name) = k;
            var memberInfo = declType.GetMember(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault();
            if (memberInfo == null)
            {
                return null;
            }

            return memberInfo.GetCustomAttribute<SensitiveAttribute>(true);
        });
    }

    private JsonConverter GetOrCreateConverter(
        Type type,
        MaskingStrategy strategy,
        string? pattern,
        JsonConverter? existing,
        JsonSerializerOptions options)
    {
        bool isCollection = IsCollection(type);
        var key = (type, strategy, pattern, isCollection);

        return ConverterCache.GetOrAdd(key, static (k, state) =>
        {
            var (type, strategy, pattern, isCollection) = k;
            var (maskingService, existing, options) = state;

            if (isCollection)
            {
                var factory = new MaskingEnumerableConverterFactory(maskingService, strategy, pattern, existing);
                return factory.CreateConverter(type, options);
            }

            var ctor = 
                ScalarFactoryCache.GetOrAdd(type, CreateScalarConverterFactory);
            
            return ctor(maskingService, strategy, pattern, existing);

        }, (maskingService, existing, options));
    }

    private static Func<IMaskingService, MaskingStrategy, string?, JsonConverter?, JsonConverter> CreateScalarConverterFactory(Type type)
    {
        var converterType = typeof(MaskingScalarConverter<>).MakeGenericType(type);
        var ctorInfo = converterType.GetConstructor(
        [typeof(IMaskingService), typeof(MaskingStrategy), typeof(string), typeof(JsonConverter)]) 
                       ?? throw new InvalidOperationException($"No matching constructor found for {converterType}.");

        var serviceParam = Expression.Parameter(typeof(IMaskingService));
        var strategyParam = Expression.Parameter(typeof(MaskingStrategy));
        var patternParam = Expression.Parameter(typeof(string));
        var innerConvParam = Expression.Parameter(typeof(JsonConverter));

        var newExpr = Expression.New(ctorInfo, serviceParam, strategyParam, patternParam, innerConvParam);
        var lambda = Expression.Lambda<Func<IMaskingService, MaskingStrategy, string?, JsonConverter?, JsonConverter>>(
            newExpr, serviceParam, strategyParam, patternParam, innerConvParam);

        return lambda.Compile();
    }

    private static bool IsCollection(Type t) =>
        typeof(IEnumerable).IsAssignableFrom(t) && t != typeof(string);
}

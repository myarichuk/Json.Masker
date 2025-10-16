using System.Collections.Concurrent;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Json.Masker.Abstract;

namespace Json.Masker.SystemTextJson;

/// <summary>
/// Applies masking converters to properties that are marked with <see cref="SensitiveAttribute"/>.
/// </summary>
/// <param name="maskingService">The masking service that will generate masked values.</param>
public class MaskingTypeInfoModifier(IMaskingService maskingService)
{
    private static readonly ConcurrentDictionary<ConverterKey, JsonConverter> Cache = new();
    
    /// <summary>
    /// Configures the provided <see cref="JsonTypeInfo"/> to use masking converters for sensitive members.
    /// </summary>
    /// <param name="typeInfo">The type information to modify.</param>
    public void Modify(JsonTypeInfo typeInfo)
    {
        foreach (var prop in typeInfo.Properties)
        {
            var attr = prop.AttributeProvider?
                .GetCustomAttributes(typeof(SensitiveAttribute), true)
                .OfType<SensitiveAttribute>()
                .FirstOrDefault();

            if (attr is not null && prop.PropertyType.IsProperPrimitive())
            {
                prop.CustomConverter = GetOrAddScalarConverter(prop.PropertyType, attr);
                continue;
            }
            
            if (attr is not null && TryParseDictTypes(prop.PropertyType, out var keyT, out var valueT))
            {
                if (keyT == typeof(string))
                {
                    prop.CustomConverter = GetOrAddStringDictConverter(prop.PropertyType, valueT!, attr);
                    continue;
                }

                throw new NotSupportedException("Masking dictionaries with non-string keys is not supported");
            }

            if (attr is not null && TryParseEnumType(prop.PropertyType, out var elemOfT))
            {
                prop.CustomConverter = GetOrAddEnumerableConverter(prop.PropertyType, elemOfT!, attr);
            }
        }        
    }

    private static bool TryParseEnumType(Type type, out Type? elem)
    {
        if (type.IsArray)
        {
            elem = type.GetElementType();
            return elem is not null;
        }
        
        // costly, but don't see alternatives!
        if (type.IsGenericType)
        {
            var @enum = type.GetInterfaces()
                .Concat([type])
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            
            if (@enum is not null)
            {
                elem = @enum.GetGenericArguments()[0];
                return true;
            }
        }

        elem = null;
        return false;
    }

    private static bool TryParseDictTypes(Type t, out Type? key, out Type? value)
    {
        key = value = null;
        var dict = t.GetInterfaces()
            .Concat([t])
            .FirstOrDefault(@interface => 
                @interface.IsGenericType && 
                @interface.GetGenericTypeDefinition() == typeof(IDictionary<,>));
        
        if (dict is null)
        {
            return false;
        }

        var args = dict.GetGenericArguments();
        key = args[0];
        value = args[1];
        return true;
    }
    
    private JsonConverter GetOrAddScalarConverter(Type type, SensitiveAttribute attr)
    {
        var key = new ConverterKey(type, attr.Strategy, attr.Pattern);
        return Cache.GetOrAdd(key, k =>
        {
            var converter = typeof(MaskingScalarConverter<>).MakeGenericType(k.Type);
            return (JsonConverter)Activator.CreateInstance(converter, maskingService, k.Strategy, k.Pattern)!;
        });
    }

    private JsonConverter GetOrAddEnumerableConverter(Type collectionT, Type elementT, SensitiveAttribute attr)
    {
        var key = new ConverterKey(collectionT, attr.Strategy, attr.Pattern);
        return Cache.GetOrAdd(key, k =>
        {
            var converter = typeof(MaskingEnumerableConverter<,>).MakeGenericType(collectionT, elementT);
            return (JsonConverter)Activator.CreateInstance(converter, maskingService, attr.Strategy, attr.Pattern)!;
        });
    }

    private JsonConverter GetOrAddStringDictConverter(Type dictT, Type valueT, SensitiveAttribute attr)
    {
        var key = new ConverterKey(dictT, attr.Strategy, attr.Pattern);
        return Cache.GetOrAdd(key, k =>
        {
            var converter = typeof(MaskingStringDictionaryConverter<,>).MakeGenericType(dictT, valueT);
            return (JsonConverter)Activator.CreateInstance(converter, maskingService, attr.Strategy, attr.Pattern)!;
        });
    }
    
    private record struct ConverterKey(Type Type, MaskingStrategy Strategy, string? Pattern);
}
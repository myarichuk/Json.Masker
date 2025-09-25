using System.Reflection;
using Json.Masker.Abstract;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Json.Masker.Newtonsoft;

public sealed class MaskingContractResolver : DefaultContractResolver
{
    private readonly IMaskingService _maskingService;

    public MaskingContractResolver(IMaskingService maskingService)
    {
        _maskingService = maskingService;
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var prop = base.CreateProperty(member, memberSerialization);

        var sensitiveAttr = member.GetCustomAttribute<SensitiveAttribute>();
        if (sensitiveAttr is null)
        {
            return prop;
        }

        var inner = prop.ValueProvider;
        
        if (inner != null) 
        {
            prop.ValueProvider = new MaskingValueProvider(inner, _maskingService, sensitiveAttr);
        }

        return prop;
    }
}

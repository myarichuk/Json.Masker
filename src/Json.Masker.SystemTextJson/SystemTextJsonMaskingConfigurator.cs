using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Json.Masker.Abstract;

namespace Json.Masker.SystemTextJson;

internal class SystemTextJsonMaskingConfigurator(IMaskingService maskingService) : IJsonMaskingConfigurator
{
    public void Configure(object settings)
    {
        if (settings is not JsonSerializerOptions options)
        {
            return;
        }

        options.TypeInfoResolver = new DefaultJsonTypeInfoResolver
        {
            Modifiers =
            {
                new MaskingTypeInfoModifier(maskingService).Modify,
            },
        };
    }
}
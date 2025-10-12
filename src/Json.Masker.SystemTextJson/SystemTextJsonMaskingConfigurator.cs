using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Json.Masker.Abstract;

namespace Json.Masker.SystemTextJson;

/// <summary>
/// Configures System.Text.Json to apply masking during serialization.
/// </summary>
/// <param name="maskingService">The masking service to use.</param>
public class SystemTextJsonMaskingConfigurator(IMaskingService maskingService) : IJsonMaskingConfigurator
{
    /// <inheritdoc />
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

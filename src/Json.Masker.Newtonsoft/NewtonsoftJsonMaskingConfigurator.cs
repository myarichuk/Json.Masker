using Json.Masker.Abstract;
using Newtonsoft.Json;

namespace Json.Masker.Newtonsoft;

/// <summary>
/// Configures Newtonsoft.Json to apply masking during serialization.
/// </summary>
/// <param name="maskingService">The masking service to use.</param>
public class NewtonsoftJsonMaskingConfigurator(IMaskingService maskingService) : IJsonMaskingConfigurator
{
    /// <inheritdoc />
    public void Configure(object settings)
    {
        if (settings is not JsonSerializerSettings opts)
        {
            return;
        }

        opts.ContractResolver = new MaskingContractResolver(maskingService);
    }
}

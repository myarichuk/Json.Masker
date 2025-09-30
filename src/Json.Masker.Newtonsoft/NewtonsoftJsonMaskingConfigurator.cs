using Json.Masker.Abstract;
using Newtonsoft.Json;

namespace Json.Masker.Newtonsoft;

internal class NewtonsoftJsonMaskingConfigurator(IMaskingService maskingService) : IJsonMaskingConfigurator
{
    public void Configure(object settings)
    {
        if (settings is not JsonSerializerSettings opts)
        {
            return;
        }

        opts.ContractResolver = new MaskingContractResolver(maskingService);
    }
}
// Copyright (c) Michael Yarichuk. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Json.Masker.Abstract;
using Json.Masker.Newtonsoft;
using Json.Masker.Samples.Shared;
using Newtonsoft.Json;

var sampleCustomer = SampleData.CreateCustomer();
var settings = new JsonSerializerSettings
{
    Formatting = Formatting.Indented,
    ContractResolver = new MaskingContractResolver(new DefaultMaskingService()),
};

Console.WriteLine("Newtonsoft.Json sample\n");

Print("Masking disabled", settings, sampleCustomer);

MaskingContextAccessor.Set(new MaskingContext { Enabled = true });
Print("Masking enabled", settings, sampleCustomer);

MaskingContextAccessor.Set(new MaskingContext { Enabled = false });

static void Print(string title, JsonSerializerSettings settings, Customer customer)
{
    Console.WriteLine($"=== {title} ===");
    Console.WriteLine(JsonConvert.SerializeObject(customer, settings));
    Console.WriteLine();
}

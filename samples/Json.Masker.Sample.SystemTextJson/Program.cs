// Copyright (c) Michael Yarichuk. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Json.Masker.Abstract;
using Json.Masker.Samples.Shared;
using Json.Masker.SystemTextJson;

var sampleCustomer = SampleData.CreateCustomer();
var maskingService = new DefaultMaskingService();

var options = new JsonSerializerOptions
{
    WriteIndented = true,
    TypeInfoResolver = new DefaultJsonTypeInfoResolver
    {
        Modifiers = { new MaskingTypeInfoModifier(maskingService).Modify },
    },
};

Console.WriteLine("System.Text.Json sample\n");

Print("Masking disabled", options, sampleCustomer);

MaskingContextAccessor.Set(new MaskingContext { Enabled = true });
Print("Masking enabled", options, sampleCustomer);

MaskingContextAccessor.Set(new MaskingContext { Enabled = false });

static void Print(string title, JsonSerializerOptions options, Customer customer)
{
    Console.WriteLine($"=== {title} ===");
    Console.WriteLine(JsonSerializer.Serialize(customer, options));
    Console.WriteLine();
}

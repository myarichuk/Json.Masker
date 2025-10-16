// Copyright (c) Michael Yarichuk. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Json.Masker.Abstract;

namespace Json.Masker.Samples.Shared;

internal sealed class Address
{
    [Sensitive]
    public string Street { get; set; } = string.Empty;

    [Sensitive]
    public string Apartment { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;
}

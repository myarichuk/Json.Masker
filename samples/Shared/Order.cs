// Copyright (c) Michael Yarichuk. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Json.Masker.Abstract;

namespace Json.Masker.Samples.Shared;

internal sealed class Order
{
    public string Number { get; set; } = string.Empty;

    public decimal Total { get; set; }

    [Sensitive(MaskingStrategy.Redacted)]
    public string Notes { get; set; } = string.Empty;
}

// Copyright (c) Michael Yarichuk. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Json.Masker.Abstract;

namespace Json.Masker.Samples.Shared;

internal sealed class Customer
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    [Sensitive(MaskingStrategy.Email)]
    public string Email { get; set; } = string.Empty;

    [Sensitive]
    public string PhoneNumber { get; set; } = string.Empty;

    [Sensitive(MaskingStrategy.Ssn)]
    public string SocialSecurityNumber { get; set; } = string.Empty;

    [Sensitive(MaskingStrategy.Creditcard)]
    public string CreditCardNumber { get; set; } = string.Empty;

    [Sensitive("##-####-####")]
    public string LoyaltyCode { get; set; } = string.Empty;

    public Address ShippingAddress { get; set; } = new();

    public List<Order> Orders { get; set; } = new();
}

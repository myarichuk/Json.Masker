// Copyright (c) Michael Yarichuk. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Json.Masker.Samples.Shared;

internal static class SampleData
{
    public static Customer CreateCustomer() => new()
    {
        Id = Guid.Parse("ec4d676a-48bf-4a1e-905c-bf9f5a3089a6"),
        FullName = "Avery Johnson",
        Email = "avery.johnson@example.com",
        PhoneNumber = "+1 (555) 123-4567",
        SocialSecurityNumber = "123-45-6789",
        CreditCardNumber = "4111 1111 1111 1111",
        LoyaltyCode = "LC-99-42-17",
        ShippingAddress = new Address
        {
            Street = "742 Evergreen Terrace",
            Apartment = "Unit 2B",
            City = "Springfield",
            State = "IL",
            PostalCode = "62704",
        },
        Orders =
        [
            new Order
            {
                Number = "A12345",
                Total = 249.99m,
                Notes = "Customer requested gift wrapping.",
            },
            new Order
            {
                Number = "A12390",
                Total = 19.95m,
                Notes = "Internal discount applied by support agent.",
            },
        ],
    };
}

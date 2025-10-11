using System.Linq;
using Bogus;
using Json.Masker.IntegrationSample.Models;

namespace Json.Masker.IntegrationSample.Services;

public class FakeBusinessDataService
{
    private readonly IReadOnlyList<Customer> _customers;
    private readonly IReadOnlyList<Order> _orders;

    public FakeBusinessDataService()
    {
        (_customers, _orders) = GenerateData();
    }

    public IReadOnlyList<Customer> GetCustomers() => _customers;

    public Customer? GetCustomer(Guid id) => _customers.FirstOrDefault(customer => customer.Id == id);

    public IReadOnlyList<Order> GetOrders() => _orders;

    public Order? GetOrder(Guid id) => _orders.FirstOrDefault(order => order.Id == id);

    public Order? GetOrderByNumber(string orderNumber) =>
        _orders.FirstOrDefault(order => string.Equals(order.OrderNumber, orderNumber, StringComparison.OrdinalIgnoreCase));

    private static (IReadOnlyList<Customer> Customers, IReadOnlyList<Order> Orders) GenerateData()
    {
        Randomizer.Seed = new Random(8675309);

        var addressFaker = new Faker<Address>()
            .RuleFor(address => address.Line1, faker => faker.Address.StreetAddress())
            .RuleFor(address => address.Line2, faker => faker.Random.Float(0f, 1f) < 0.2f ? faker.Address.SecondaryAddress() : null)
            .RuleFor(address => address.City, faker => faker.Address.City())
            .RuleFor(address => address.StateOrProvince, faker => faker.Address.StateAbbr())
            .RuleFor(address => address.PostalCode, faker => faker.Address.ZipCode())
            .RuleFor(address => address.Country, faker => faker.Address.Country());

        var paymentFaker = new Faker<PaymentDetail>()
            .RuleFor(payment => payment.Method, faker => faker.PickRandom("Credit Card", "ACH", "Wire Transfer"))
            .RuleFor(payment => payment.CardIssuer, (faker, payment) =>
                payment.Method == "Credit Card" ? faker.Finance.CreditCardIssuer() : string.Empty)
            .RuleFor(payment => payment.CardNumber, (faker, payment) =>
                payment.Method == "Credit Card" ? faker.Finance.CreditCardNumber() : string.Empty)
            .RuleFor(payment => payment.BankAccountIban, (faker, payment) =>
                payment.Method == "Credit Card" ? string.Empty : faker.Finance.Iban())
            .RuleFor(payment => payment.TransactionReference, faker => faker.Random.Replace("####-########-####"))
            .RuleFor(payment => payment.PaidAt, faker => faker.Date.PastOffset(1))
            .RuleFor(payment => payment.Refunded, faker => faker.Random.Bool(0.05f))
            .RuleFor(payment => payment.InternalNotes, faker => faker.Lorem.Sentence());

        var orderItemFaker = new Faker<OrderItem>()
            .RuleFor(item => item.Sku, faker => faker.Commerce.Ean13())
            .RuleFor(item => item.ProductName, faker => faker.Commerce.ProductName())
            .RuleFor(item => item.Quantity, faker => faker.Random.Int(1, 5))
            .RuleFor(item => item.UnitPrice, faker => decimal.Round(faker.Random.Decimal(20m, 500m), 2))
            .RuleFor(item => item.Discount, faker => decimal.Round(faker.Random.Decimal(0m, 20m), 2));

        var orderFaker = new Faker<Order>()
            .RuleFor(order => order.Id, _ => Guid.NewGuid())
            .RuleFor(order => order.OrderNumber, faker => $"SO-{faker.Random.Number(100000, 999999)}")
            .RuleFor(order => order.OrderedAt, faker => faker.Date.PastOffset(1))
            .RuleFor(order => order.Status, faker => faker.PickRandom<OrderStatus>())
            .RuleFor(order => order.Currency, _ => "USD")
            .RuleFor(order => order.FulfillmentChannel, faker => faker.PickRandom("Warehouse", "Drop-Ship", "In-Store Pickup"))
            .RuleFor(order => order.Items, faker => orderItemFaker.Generate(faker.Random.Int(1, 5)))
            .RuleFor(order => order.Payment, faker => paymentFaker.Generate())
            .RuleFor(order => order.InternalReviewNotes, faker => faker.Lorem.Sentence())
            .FinishWith((faker, order) =>
            {
                order.Subtotal = order.Items.Sum(item => item.LineTotal);
                order.Tax = decimal.Round(order.Subtotal * faker.Random.Decimal(0.05m, 0.095m), 2);
                order.Total = decimal.Round(order.Subtotal + order.Tax, 2);

                if (order.Status is OrderStatus.Shipped or OrderStatus.Fulfilled)
                {
                    order.FulfilledAt = order.OrderedAt.AddDays(faker.Random.Int(1, 7));
                }

                if (order.Status is OrderStatus.Cancelled)
                {
                    order.Payment.Refunded = true;
                }
            });

        var customerFaker = new Faker<Customer>()
            .RuleFor(customer => customer.Id, _ => Guid.NewGuid())
            .RuleFor(customer => customer.FirstName, faker => faker.Name.FirstName())
            .RuleFor(customer => customer.LastName, faker => faker.Name.LastName())
            .RuleFor(customer => customer.Email, (faker, customer) => faker.Internet.Email(customer.FirstName, customer.LastName))
            .RuleFor(customer => customer.PhoneNumber, faker => faker.Phone.PhoneNumber("###-###-####"))
            .RuleFor(customer => customer.NationalId, faker => faker.Person.Ssn())
            .RuleFor(customer => customer.LoyaltyNumber, faker =>
                $"LOY-{faker.Random.Number(100, 999)}-{faker.Random.Number(1000, 9999)}")
            .RuleFor(customer => customer.BillingAddress, faker => addressFaker.Generate())
            .RuleFor(customer => customer.ShippingAddress, (faker, customer) => faker.Random.Bool() ? customer.BillingAddress : addressFaker.Generate())
            .RuleFor(customer => customer.RegisteredAt, faker => faker.Date.PastOffset(3))
            .RuleFor(customer => customer.Status, faker => faker.PickRandom<CustomerStatus>())
            .RuleFor(customer => customer.Orders, _ => new List<Order>())
            .FinishWith((faker, customer) =>
            {
                var orderCount = faker.Random.Int(1, 6);

                for (var i = 0; i < orderCount; i++)
                {
                    var order = orderFaker.Generate();
                    order.CustomerId = customer.Id;
                    order.CustomerName = customer.FullName;
                    order.Payment.CustomerEmail = customer.Email;

                    if (order.Payment.PaidAt is null && order.Status is not OrderStatus.PendingPayment and not OrderStatus.Draft)
                    {
                        order.Payment.PaidAt = order.OrderedAt.AddHours(faker.Random.Int(1, 48));
                    }

                    customer.Orders.Add(order);
                }
            });

        var customers = customerFaker.Generate(25);
        var orders = customers.SelectMany(customer => customer.Orders)
            .OrderByDescending(order => order.OrderedAt)
            .ToList();

        return (customers, orders);
    }
}

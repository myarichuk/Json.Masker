using System.Linq;
using Json.Masker.IntegrationSample.Models;
using Json.Masker.IntegrationSample.Models.Dtos;
using Json.Masker.IntegrationSample.Services;
using Microsoft.AspNetCore.Mvc;

namespace Json.Masker.IntegrationSample.Controllers;

/// <summary>
/// Provides endpoints for working with customer information in the integration sample.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly FakeBusinessDataService _dataService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomersController"/> class.
    /// </summary>
    /// <param name="dataService">The data service that supplies customer information.</param>
    public CustomersController(FakeBusinessDataService dataService)
    {
        _dataService = dataService;
    }

    /// <summary>
    /// Gets an overview for all customers available in the sample data set.
    /// </summary>
    /// <returns>A collection of customers ordered by their registration date.</returns>
    [HttpGet]
    public ActionResult<IEnumerable<CustomerOverview>> GetCustomers()
    {
        var customers = _dataService
            .GetCustomers()
            .Select(customer => new CustomerOverview
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                NationalId = customer.NationalId,
                LoyaltyNumber = customer.LoyaltyNumber,
                Status = customer.Status,
                RegisteredAt = customer.RegisteredAt,
                OrderCount = customer.Orders.Count,
                LifetimeValue = customer.LifetimeValue,
            })
            .OrderByDescending(customer => customer.RegisteredAt)
            .ToList();

        return Ok(customers);
    }

    /// <summary>
    /// Gets the full details for a specific customer.
    /// </summary>
    /// <param name="id">The unique identifier of the customer.</param>
    /// <returns>The customer that matches the provided identifier.</returns>
    [HttpGet("{id:guid}")]
    public ActionResult<Customer> GetCustomer(Guid id)
    {
        var customer = _dataService.GetCustomer(id);
        if (customer is null)
        {
            return NotFound();
        }

        return Ok(customer);
    }
}

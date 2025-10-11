using System.Linq;
using Json.Masker.IntegrationSample.Models;
using Json.Masker.IntegrationSample.Models.Dtos;
using Json.Masker.IntegrationSample.Services;
using Microsoft.AspNetCore.Mvc;

namespace Json.Masker.IntegrationSample.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly FakeBusinessDataService _dataService;

    public CustomersController(FakeBusinessDataService dataService)
    {
        _dataService = dataService;
    }

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
                Status = customer.Status,
                RegisteredAt = customer.RegisteredAt,
                OrderCount = customer.Orders.Count,
                LifetimeValue = customer.LifetimeValue
            })
            .OrderByDescending(customer => customer.RegisteredAt)
            .ToList();

        return Ok(customers);
    }

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

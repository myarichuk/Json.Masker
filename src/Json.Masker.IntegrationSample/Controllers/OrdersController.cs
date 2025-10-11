using System.Linq;
using Json.Masker.IntegrationSample.Models;
using Json.Masker.IntegrationSample.Models.Dtos;
using Json.Masker.IntegrationSample.Services;
using Microsoft.AspNetCore.Mvc;

namespace Json.Masker.IntegrationSample.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly FakeBusinessDataService _dataService;

    public OrdersController(FakeBusinessDataService dataService)
    {
        _dataService = dataService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<OrderSummary>> GetOrders()
    {
        var orders = _dataService
            .GetOrders()
            .Select(OrderSummary.FromOrder)
            .ToList();

        return Ok(orders);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<Order> GetOrder(Guid id)
    {
        var order = _dataService.GetOrder(id);
        if (order is null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    [HttpGet("number/{orderNumber}")]
    public ActionResult<Order> GetOrderByNumber(string orderNumber)
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
        {
            return BadRequest("Order number is required.");
        }

        var order = _dataService.GetOrderByNumber(orderNumber);
        if (order is null)
        {
            return NotFound();
        }

        return Ok(order);
    }
}

using System.Linq;
using Json.Masker.IntegrationSample.Models;
using Json.Masker.IntegrationSample.Models.Dtos;
using Json.Masker.IntegrationSample.Services;
using Microsoft.AspNetCore.Mvc;

namespace Json.Masker.IntegrationSample.Controllers;

/// <summary>
/// Provides endpoints for retrieving order data from the sample data store.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly FakeBusinessDataService _dataService;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrdersController"/> class.
    /// </summary>
    /// <param name="dataService">The data service that supplies order information.</param>
    public OrdersController(FakeBusinessDataService dataService)
    {
        _dataService = dataService;
    }

    /// <summary>
    /// Gets a summary for all orders in the sample data set.
    /// </summary>
    /// <returns>A collection of order summaries.</returns>
    [HttpGet]
    public ActionResult<IEnumerable<OrderSummary>> GetOrders()
    {
        var orders = _dataService
            .GetOrders()
            .Select(OrderSummary.FromOrder)
            .ToList();

        return Ok(orders);
    }

    /// <summary>
    /// Gets the details for a specific order by identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the order.</param>
    /// <returns>The order that matches the provided identifier.</returns>
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

    /// <summary>
    /// Gets the details for a specific order using its order number.
    /// </summary>
    /// <param name="orderNumber">The human-readable order number.</param>
    /// <returns>The order that matches the provided order number.</returns>
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

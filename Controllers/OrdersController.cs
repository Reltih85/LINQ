using Lab8_Bernaola_Pacheco.Data;
using Microsoft.AspNetCore.Mvc;

namespace Lab8_Bernaola_Pacheco.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IUnitOfWork unitOfWork, ILogger<OrdersController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    [HttpGet("{orderId}/products")]
    public IActionResult GetProductsByOrderId(int orderId)
    {
        try
        {
            var orderDetails = _unitOfWork.Repository.GetProductsByOrderId(orderId)
                .Select(od => new
                {
                    ProductName = od.Product.Name,
                    od.Quantity
                });

            if (!orderDetails.Any())
            {
                return NotFound($"No se encontraron productos para la orden {orderId}");
            }

            return Ok(orderDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener productos de la orden {orderId}");
            return StatusCode(500, "Error interno del servidor");
        }
    }
    
    //Ejercicio 4
    [HttpGet("{orderId}/total-quantity")]
    public IActionResult GetTotalQuantityByOrderId(int orderId)
    {
        try
        {
            var totalQuantity = _unitOfWork.Repository.GetTotalQuantityByOrderId(orderId);
        
            return Ok(new 
            {
                OrderId = orderId,
                TotalQuantity = totalQuantity
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al calcular cantidad total para la orden {orderId}");
            return StatusCode(500, "Error interno del servidor");
        }
    }
    //6
    [HttpGet("after-date/{date}")]
    public IActionResult GetOrdersAfterDate(DateTime date)
    {
        try
        {
            var orders = _unitOfWork.Repository.GetOrdersAfterDate(date);
        
            if (!orders.Any())
            {
                return NotFound($"No se encontraron pedidos después de {date:yyyy-MM-dd}");
            }

            return Ok(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener pedidos después de {date:yyyy-MM-dd}");
            return StatusCode(500, "Error interno del servidor");
        }
    }
    
    [HttpGet("with-details")]
    public IActionResult GetAllOrdersWithDetails()
    {
        try
        {
            var ordersWithDetails = _unitOfWork.Repository.GetAllOrdersWithDetails();
        
            if (!ordersWithDetails.Any())
            {
                return NotFound("No se encontraron pedidos con detalles");
            }

            return Ok(ordersWithDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener pedidos con detalles");
            return StatusCode(500, "Error interno del servidor");
        }
    }
    
}
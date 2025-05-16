using Lab8_Bernaola_Pacheco.Data;
using Lab8_Bernaola_Pacheco.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lab8_Bernaola_Pacheco.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ClientsController> _logger;

    public ClientsController(IUnitOfWork unitOfWork, ILogger<ClientsController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    [HttpGet("by-name/{name}")]
    public IActionResult GetClientsByName(string name)
    {
        try
        {
            var clients = _unitOfWork.Repository.GetClientsByName(name);
            
            if (!clients.Any())
            {
                return NotFound($"No se encontraron clientes con el nombre: {name}");
            }

            return Ok(clients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar clientes por nombre");
            return StatusCode(500, "Error interno del servidor");
        }
    }
    
    [HttpGet("with-most-orders")]
    public IActionResult GetClientWithMostOrders()
    {
        try
        {
            var result = _unitOfWork.Repository.GetClientWithMostOrders();
        
            if (result == null)
            {
                return NotFound("No se encontraron clientes con pedidos");
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener cliente con más pedidos");
            return StatusCode(500, "Error interno del servidor");
        }
    }
    
    [HttpGet("{clientId}/purchased-products")]
    public IActionResult GetPurchasedProducts(int clientId)
    {
        try
        {
            var products = _unitOfWork.Repository.GetProductsSoldToClient(clientId);
        
            if (!products.Any())
            {
                return NotFound($"El cliente {clientId} no ha comprado productos");
            }

            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener productos comprados por cliente {clientId}");
            return StatusCode(500, "Error interno del servidor");
        }
    }
    
    //lab9
    [HttpGet("with-orders")]
    public IActionResult GetClientsWithOrders()
    {
        try
        {
            var clients = _unitOfWork.Repository.GetClientsWithOrders();

            if (!clients.Any())
            {
                return NotFound("No se encontraron clientes con pedidos.");
            }

            return Ok(clients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener clientes con pedidos");
            return StatusCode(500, "Error interno del servidor");
        }
    }
    
    [HttpGet("orders-with-details")]
    public IActionResult GetOrdersWithDetails()
    {
        var orders = _unitOfWork.Repository.GetOrdersWithDetails();
        return orders.Any() ? Ok(orders) : NotFound("No se encontraron pedidos con detalles.");
    }

    [HttpGet("product-summary")]
    public IActionResult GetClientsWithProductCount()
    {
        var result = _unitOfWork.Repository.GetClientsWithProductCount();
        return Ok(result);
    }
    
    [HttpGet("sales-summary")]
    public IActionResult GetSalesByClient()
    {
        var result = _unitOfWork.Repository.GetSalesByClient();
        return Ok(result);
    }

}
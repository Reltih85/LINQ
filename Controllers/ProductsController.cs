using Lab8_Bernaola_Pacheco.Data;
using Lab8_Bernaola_Pacheco.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab8_Bernaola_Pacheco.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IUnitOfWork unitOfWork, ILogger<ProductsController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    [HttpGet("price-greater-than/{minPrice}")]
    public IActionResult GetProductsByMinPrice(decimal minPrice)
    {
        try
        {
            var products = _unitOfWork.Repository.GetProductsByMinPrice(minPrice);
            
            if (!products.Any())
            {
                return NotFound($"No se encontraron productos con precio mayor a {minPrice}");
            }

            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al buscar productos con precio mayor a {minPrice}");
            return StatusCode(500, "Error interno del servidor");
        }
    }
    
    //Ejercicio 5
    [HttpGet("most-expensive")]
    public IActionResult GetMostExpensiveProduct()
    {
        try
        {
            var product = _unitOfWork.Repository.GetMostExpensiveProduct();
        
            if (product == null)
            {
                return NotFound("No se encontraron productos");
            }

            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el producto más caro");
            return StatusCode(500, "Error interno del servidor");
        }
    }
    
    //7
    [HttpGet("average-price")]
    public IActionResult GetAverageProductPrice()
    {
        try
        {
            var average = _unitOfWork.Repository.GetAverageProductPrice();
            return Ok(new 
            {
                AveragePrice = Math.Round(average, 2)  // Redondeo a 2 decimales
            });
        }
        catch (InvalidOperationException) // Cuando no hay productos
        {
            return NotFound("No hay productos registrados");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al calcular promedio");
            return StatusCode(500, "Error interno");
        }
    }
    
    //8
    [HttpGet("without-description")]
    public IActionResult GetProductsWithoutDescription()
    {
        try
        {
            var products = _unitOfWork.Repository.GetProductsWithoutDescription();
        
            if (!products.Any())
            {
                return NotFound("No se encontraron productos sin descripción");
            }

            return Ok(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener productos sin descripción");
            return StatusCode(500, "Error interno del servidor");
        }
    }
    
    [HttpGet("{productId}/purchasing-clients")]
    public IActionResult GetPurchasingClients(int productId)
    {
        try
        {
            var clients = _unitOfWork.Repository.GetClientsWhoPurchasedProduct(productId);
        
            if (!clients.Any())
            {
                return NotFound($"Ningún cliente ha comprado el producto {productId}");
            }

            return Ok(clients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener clientes del producto {productId}");
            return StatusCode(500, "Error interno del servidor");
        }
    }
}
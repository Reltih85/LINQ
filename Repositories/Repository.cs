using Lab8_Bernaola_Pacheco.Data;
using Lab8_Bernaola_Pacheco.Models;
using Lab8_Bernaola_Pacheco.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace Lab8_Bernaola_Pacheco.Repositories;

public class Repository : IRepository
{
    private readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public Client? GetClientById(int id)
    {
        return _context.Clients.Find(id);
    }

    public IEnumerable<Client> GetClientsByName(string name)
    {
        return _context.Clients
            .Where(c => c.Name.Contains(name))
            .AsNoTracking()
            .ToList();
    }

    public IEnumerable<Product> GetProductsByMinPrice(decimal minPrice)
    {
        return _context.Products
            .Where(p => p.Price > minPrice)
            .AsNoTracking()
            .ToList();
    }
    
    public IEnumerable<Orderdetail> GetProductsByOrderId(int orderId)
    {
        return _context.Orderdetails
            .Where(od => od.OrderId == orderId)
            .Include(od => od.Product) 
            .AsNoTracking()
            .ToList();
    }
    
    public int GetTotalQuantityByOrderId(int orderId)
    {
        return _context.Orderdetails
            .Where(od => od.OrderId == orderId)
            .Sum(od => od.Quantity);
    }
    
    public Product? GetMostExpensiveProduct()
    {
        return _context.Products
            .OrderByDescending(p => p.Price)
            .FirstOrDefault();
    }
    //6
    public IEnumerable<Order> GetOrdersAfterDate(DateTime date)
    {
        return _context.Orders
            .Where(o => o.OrderDate > date)
            .Select(o => new Order // Proyecta solo los datos necesarios
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                ClientId = o.ClientId,
                Client = new Client // Incluye solo datos básicos del cliente
                {
                    ClientId = o.Client.ClientId,
                    Name = o.Client.Name,
                    Email = o.Client.Email
                    // Excluye la propiedad Orders del cliente
                }
            })
            .AsNoTracking()
            .ToList();
    }
    
    public decimal GetAverageProductPrice()
    {
        return _context.Products
            .Select(p => p.Price) 
            .Average(); 
    }
    public IEnumerable<Product> GetProductsWithoutDescription()
    {
        return _context.Products
            .Where(p => string.IsNullOrEmpty(p.Description))  
            .AsNoTracking()
            .ToList();                                      
    }

    //9
    public dynamic GetClientWithMostOrders()
    {
        return _context.Orders
            .Include(o => o.Client)
            .GroupBy(o => o.ClientId)
            .Select(g => new
            {
                ClientId = g.Key,
                ClientName = g.First().Client.Name,
                ClientEmail = g.First().Client.Email,
                OrderCount = g.Count()
            })
            .OrderByDescending(x => x.OrderCount)
            .FirstOrDefault();
    }
    
    //10
    public IEnumerable<dynamic> GetAllOrdersWithDetails()
    {
        return _context.Orderdetails
            .Include(od => od.Order)  
            .Include(od => od.Product)  
            .Select(od => new  
            {
                OrderId = od.OrderId,
                OrderDate = od.Order.OrderDate,
                ProductName = od.Product.Name,
                Quantity = od.Quantity
            })
            .AsNoTracking()
            .ToList();
    }
    
    public IEnumerable<string> GetProductsSoldToClient(int clientId)
    {
        return _context.Orderdetails
            .Include(od => od.Order)
            .Include(od => od.Product)
            .Where(od => od.Order.ClientId == clientId) 
            .Select(od => od.Product.Name)         
            .Distinct()                              
            .ToList();
    }
    
    public IEnumerable<string> GetClientsWhoPurchasedProduct(int productId)
    {
        return _context.Orderdetails
            .Include(od => od.Order)
            .ThenInclude(o => o.Client)  
            .Where(od => od.ProductId == productId)  
            .Select(od => od.Order.Client.Name)     
            .Distinct()                             
            .ToList();
    }
}
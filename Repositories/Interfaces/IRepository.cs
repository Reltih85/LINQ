using Lab8_Bernaola_Pacheco.Models;

namespace Lab8_Bernaola_Pacheco.Repositories.Interfaces;

public interface IRepository
{
    Client? GetClientById(int id);
    IEnumerable<Client> GetClientsByName(string name);
    IEnumerable<Product> GetProductsByMinPrice(decimal minPrice);
    IEnumerable<Orderdetail> GetProductsByOrderId(int orderId);
    int GetTotalQuantityByOrderId(int orderId);
    Product? GetMostExpensiveProduct();
    IEnumerable<Order> GetOrdersAfterDate(DateTime date);
    decimal GetAverageProductPrice(); 
    IEnumerable<Product> GetProductsWithoutDescription();
    dynamic GetClientWithMostOrders();
    IEnumerable<dynamic> GetAllOrdersWithDetails();
    IEnumerable<string> GetProductsSoldToClient(int clientId);
    IEnumerable<string> GetClientsWhoPurchasedProduct(int productId);
    List<ClientOrderDto> GetClientsWithOrders();
    List<OrderDetailsDto> GetOrdersWithDetails();
    List<ClientProductSummaryDto> GetClientsWithProductCount();
    List<SalesByClientDto> GetSalesByClient();
}
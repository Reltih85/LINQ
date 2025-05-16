namespace Lab8_Bernaola_Pacheco.Models;

public class ClientOrderDto
{
    public string ClientName { get; set; }
    public List<OrderDto> Orders { get; set; }
}
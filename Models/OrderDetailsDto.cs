namespace Lab8_Bernaola_Pacheco.Models;

public class OrderDetailsDto
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public List<ProductDto> Products { get; set; }
}
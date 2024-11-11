namespace TiendaOnline.Application.Responses;

public class StockResponse
{
    public int ID { get; set; }
    public string? Codigo { get; set; }
    public string? Producto { get; set; }
    public int CantidadMinima { get; set; }
    public int CantidadMaxima { get; set; }
    public int CantidadActual { get; set; }
    public string? Existencial { get; set; }
    public decimal IVA { get; set; }
    public decimal PrecioUnidad { get; set; }
    public decimal PrecioMasIVA { get; set; }
}

namespace TiendaOnline.Domain.Stocks;

public class Stock
{
    public ID? ID { get; set; }
    public Codigo? Codigo { get; set; }
    public Producto? Producto { get; set; }
    public CantidadMinima? CantidadMinima { get; set; }
    public CantidadMaxima? CantidadMaxima { get; set; }
    public CantidadActual? CantidadActual { get; set; }
    public Existencial? Existencial { get; set; }
    public IVA? IVA { get; set; }
    public PrecioUnidad? PrecioUnidad { get; set; }
    public PrecioMasIVA? PrecioMasIVA { get; set; }
}

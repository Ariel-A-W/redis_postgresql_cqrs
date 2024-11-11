namespace TiendaOnline.Domain.Stocks;

public record PrecioMasIVA(
    decimal Unidad, decimal Impuesto
)
{
    public decimal GetPrecioMasIVA() => Unidad + ((Unidad * Impuesto) / 100);
}

namespace TiendaOnline.Domain.Stocks;

public record Existencial(
    int Maxima, int Minima, int Actual
)
{
    public string GetEstadoExistencial()
    {
        string estado = string.Empty;
        if (Actual > Minima && Actual < Maxima)
        {
            estado = "Normal";
        }
        else if (Actual < Minima)
        {
            estado = "Reponer";
        }
        else if (Actual > Maxima)
        {
            estado = "Excedido";
        }
        return estado;
    }
}

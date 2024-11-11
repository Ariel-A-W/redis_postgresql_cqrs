using MediatR;
using TiendaOnline.Application.Requests;
using TiendaOnline.Domain.Stocks;

namespace TiendaOnline.Application.Commands;

public sealed class StockAddCommand
    : IRequestHandler<StockAddRequest, int>
{
    private readonly IStock _stock;

    public StockAddCommand(IStock stock)
    {
        _stock = stock;
    }

    public async Task<int> Handle(
        StockAddRequest request, 
        CancellationToken cancellationToken
    )
    {
        try
        {
            var registro = new Stock()
            { 
                //ID = new ID(0), // No requerido porque es autonumérico.
                Codigo = new Codigo(request.Codigo!), 
                Producto = new Producto(request.Producto!), 
                CantidadMinima = new CantidadMinima(request.CantidadMinima!), 
                CantidadMaxima = new CantidadMaxima(request.CantidadMaxima!),
                CantidadActual = new CantidadActual(request.CantidadActual!), 
                Existencial = new Existencial(
                    request.CantidadMaxima, request.CantidadMinima, request.CantidadActual    
                ), 
                IVA = new IVA(request.IVA!),
                PrecioUnidad = new PrecioUnidad(request.PrecioUnidad), 
                PrecioMasIVA = new PrecioMasIVA(
                    request.PrecioUnidad, request.IVA
                )
            };
            var addRegistro = _stock.Add(registro, cancellationToken);
            return await Task.FromResult<int>(addRegistro);
        }
        catch (Exception ex)
        { 
            System.Diagnostics.Debug.WriteLine(ex.Message.ToString());
            return await Task.FromResult<int>(0);
        }
    }
}

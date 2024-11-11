using MediatR;
using TiendaOnline.Application.Requests;
using TiendaOnline.Domain.Stocks;

namespace TiendaOnline.Application.Commands;

public class StockUpdateCommand
    : IRequestHandler<StockUpdateRequest, int>
{
    private readonly IStock _stock;

    public StockUpdateCommand(IStock stock)
    {
        _stock = stock;
    }

    public async Task<int> Handle(
        StockUpdateRequest request, 
        CancellationToken cancellationToken
    )
    {
        try
        {
            var registro = new Stock()
            {
                ID = new ID(request.ID), 
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
            var addRegistro = _stock.Update(request.ID, registro, cancellationToken);
            return await Task.FromResult<int>(addRegistro);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message.ToString());
            return await Task.FromResult<int>(0);
        }
    }
}
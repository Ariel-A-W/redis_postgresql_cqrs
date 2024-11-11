using MediatR;
using TiendaOnline.Application.Requests;
using TiendaOnline.Domain.Stocks;

namespace TiendaOnline.Application.Commands;

public class StockDeleteCommand
    : IRequestHandler<StockDeleteRequest, int>
{
    private readonly IStock _stock;

    public StockDeleteCommand(IStock stock)
    {
        _stock = stock;
    }

    public async Task<int> Handle(
        StockDeleteRequest request, 
        CancellationToken cancellationToken
    )
    {
        try
        {
            var stock = new Stock() { ID = new ID(request.ID) };
            var removeRegistro = _stock.Delete(stock, cancellationToken);
            return await Task.FromResult<int>(removeRegistro);
        }
        catch
        {
            return await Task.FromResult<int>(0);
        }
    }
}

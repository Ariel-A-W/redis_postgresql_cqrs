using MediatR;
using System.Text.Json;
using StackExchange.Redis;
using TiendaOnline.Application.Requests;
using TiendaOnline.Application.Responses;
using TiendaOnline.Domain.Stocks;

namespace TiendaOnline.Application.Queries;

public class StockByIdHandler
    : IRequestHandler<StockByIdRequest, IEnumerable<StockResponse>>
{
    private readonly IStock _stock;
    private readonly IConnectionMultiplexer _redis;

    public StockByIdHandler(IStock stock, IConnectionMultiplexer redis)
    {
        _stock = stock;
        _redis = redis;
    }

    public async Task<IEnumerable<StockResponse>> Handle(
        StockByIdRequest request, 
        CancellationToken cancellationToken
    )
    {
        var lst = new List<StockResponse>();
        try
        {
            var db = _redis.GetDatabase();
            string cacheKey = $"Listado";
            var cachedEntity = await db.StringGetAsync(cacheKey);
            if (!cachedEntity.IsNullOrEmpty)
            {
                // Retorna los datos desde la caché de Redis.
                var strObjStock = String.Format(@"{0}", cachedEntity);
                var lstStock = JsonSerializer.Deserialize<List<Stock>>(strObjStock)!;

                foreach (var item in lstStock)
                {
                    if (item == null) { break; }

                    lst.Add(
                        new StockResponse()
                        {
                            ID = item.ID!.Value,
                            Codigo = item.Codigo!.Value,
                            Producto = item.Producto!.Value,
                            CantidadMinima = item.CantidadMinima!.Value,
                            CantidadMaxima = item.CantidadMaxima!.Value,
                            CantidadActual = item.CantidadActual!.Value,
                            Existencial = new Existencial(
                                item.CantidadMaxima!.Value,
                                item.CantidadMinima!.Value,
                                item.CantidadActual!.Value
                            ).GetEstadoExistencial(),
                            IVA = item.IVA!.Value,
                            PrecioUnidad = item.PrecioUnidad!.Value,
                            PrecioMasIVA = new PrecioMasIVA(
                                item!.PrecioUnidad.Value, item!.IVA.Value
                            ).GetPrecioMasIVA()
                        }
                    );
                }

                var lstOut = lst.ToList().SingleOrDefault(x => x.ID == request.ID);

                if (lstOut != null)
                {
                    lst.Clear();
                    lst.Add(lstOut!);

                    return await Task.FromResult<IEnumerable<StockResponse>>(lst);
                }
                else
                {
                    return await Task.FromResult<IEnumerable<StockResponse>>
                    (
                        new List<StockResponse>()
                    );
                }
            }
            else
            {
                return await Task.FromResult<IEnumerable<StockResponse>>
                (
                    new List<StockResponse>()
                );
            }
        }
        catch (Exception ex)
        {
            return await Task.FromResult<IEnumerable<StockResponse>>
            (
                new List<StockResponse>()
            );
        }
    }
}

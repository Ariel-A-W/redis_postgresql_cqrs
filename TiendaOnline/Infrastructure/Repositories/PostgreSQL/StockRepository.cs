using Microsoft.EntityFrameworkCore;
using TiendaOnline.DBContexts;
using TiendaOnline.Domain.Stocks;
using TiendaOnline.Infrastructure.DBContexts;

namespace TiendaOnline.Infrastructure.Repositories.PostgreSQL;

public sealed class StockRepository : IStock
{
    private readonly AppEnvironmentDbContext _dbContext;
    private readonly DbSet<Stock> _dbSet;
    private readonly IUnitOfWork _unitOfWork;

    public StockRepository(
        AppEnvironmentDbContext dbContext,
        IUnitOfWork unitOfWork
    )
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<Stock>();
        _unitOfWork = unitOfWork;
    }

    public IEnumerable<Stock> GetAll()
    {
        var lists = _dbSet.ToList();
        return lists;
    }

    public IEnumerable<Stock> GetById(int id)
    {
        var lists = _dbSet.ToList().SingleOrDefault(x => x.ID!.Value == id)!;
        var lst = new List<Stock>() { lists! };
        return lst;
    }

    public int Add(Stock stock, CancellationToken cancellationToken)
    {
        try
        {
            _dbSet.Add(stock);
            var res = _unitOfWork.SaveChangesAsync(cancellationToken);
            return res.Result;
        }
        catch
        {
            return 0;
        }
    }

    public int Delete(Stock stock, CancellationToken cancellationToken)
    {
        try
        {
            var regUpdate = new Stock()
            {
                ID = stock.ID,
            };
            _dbSet.Remove(regUpdate);
            var res = _dbContext.SaveChangesAsync(cancellationToken);
            return res.Result;
        }
        catch
        {
            return 0;
        }
    }

    public int Update(int id, Stock stock, CancellationToken cancellationToken)
    {
        try
        {
            var regUpdate = new Stock()
            {
                ID = stock.ID,
                Codigo = stock.Codigo,
                Producto = stock.Producto,
                CantidadMinima = stock.CantidadMinima,
                CantidadMaxima = stock.CantidadMaxima,
                CantidadActual = stock.CantidadActual,
                Existencial = new Existencial(
                    stock.CantidadMinima!.Value,
                    stock.CantidadMaxima!.Value,
                    stock.CantidadActual!.Value
                ),
                IVA = stock.IVA,
                PrecioUnidad = stock.PrecioUnidad,
                PrecioMasIVA = stock.PrecioMasIVA,
            };
            _dbSet.Update(regUpdate);
            var res = _unitOfWork.SaveChangesAsync(cancellationToken);
            return res.Result;
        }
        catch
        {
            return 0;
        }
    }
}

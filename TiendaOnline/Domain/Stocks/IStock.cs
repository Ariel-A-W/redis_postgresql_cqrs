namespace TiendaOnline.Domain.Stocks;

public interface IStock
{
    public IEnumerable<Stock> GetAll();
    public IEnumerable<Stock> GetById(int id);
    public int Add(Stock stock, CancellationToken cancellationToken);
    public int Delete(Stock stock, CancellationToken cancellationToken);
    public int Update(int id, Stock stock, CancellationToken cancellationToken);
}

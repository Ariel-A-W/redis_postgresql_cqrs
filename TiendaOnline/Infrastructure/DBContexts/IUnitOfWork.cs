namespace TiendaOnline.Infrastructure.DBContexts;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

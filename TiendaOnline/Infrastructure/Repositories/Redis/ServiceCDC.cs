using TiendaOnline.Infrastructure.Repositories.Replicators;

namespace TiendaOnline.Infrastructure.Repositories.Redis;

public class ServiceCDC : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ServiceReplicator _replicator;

    public ServiceCDC(IServiceScopeFactory serviceScopeFactory, ServiceReplicator replicator)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _replicator = replicator;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _replicator.ReplicationPostgreSQLToRedis(stoppingToken);  
            await Task.Delay(2000, stoppingToken); // Cada 2 segundos actualiza la Caché.
        }
    }
}
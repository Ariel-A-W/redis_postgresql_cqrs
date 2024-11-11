using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;
using TiendaOnline.Infrastructure.DBContexts;

namespace TiendaOnline.Infrastructure.Repositories.Replicators;

public class ServiceReplicator
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IConnectionMultiplexer _redis;

    public ServiceReplicator(IServiceScopeFactory serviceScopeFactory, IConnectionMultiplexer redis)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _redis = redis;
    }

    public async Task ReplicationPostgreSQLToRedis(CancellationToken cancellationToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<RedisDbContext>();
            var db = _redis.GetDatabase();
            var stocks = await dbContext.Stocks.ToListAsync(cancellationToken);
            await db.StringSetAsync("Listado", JsonConvert.SerializeObject(stocks));
        }
    }
}

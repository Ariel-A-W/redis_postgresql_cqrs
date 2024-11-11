using StackExchange.Redis;
using TiendaOnline;

var builder = WebApplication.CreateBuilder(args);

var redisConnectionString = builder.Configuration.GetValue<string>("Redis:RedisConnectionString");
var redis = ConnectionMultiplexer.Connect(redisConnectionString!);
builder.Services.AddSingleton<IConnectionMultiplexer>(redis);


// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseAuthorization();

app.MapControllers();

app.Run();
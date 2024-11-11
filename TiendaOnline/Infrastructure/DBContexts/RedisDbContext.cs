using Microsoft.EntityFrameworkCore;
using TiendaOnline.DBContexts;
using TiendaOnline.Domain.Stocks;

namespace TiendaOnline.Infrastructure.DBContexts;

public sealed class RedisDbContext : DbContext
{
    public DbSet<Stock> Stocks { get; set; }

    public RedisDbContext(
        DbContextOptions<RedisDbContext> options
    ) : base(options) { }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    base.OnModelCreating(modelBuilder);
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Adapta las entidades de Domain a Models automáticamente.
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(RedisDbContext).Assembly
        );

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.ToTable("stocks");
            entity.HasKey(e => e.ID);
            entity.Property(id => id.ID)
                .HasConversion(
                    id => id!.Value,
                    value => new ID(value)
                )
                .HasColumnType("serial")
                .UseIdentityColumn();

            entity.Property(codigo => codigo.Codigo)
                .HasMaxLength(10)
                .HasConversion(
                    codigo => codigo!.Value,
                    value => new Codigo(value)
                );

            entity.Property(producto => producto.Producto)
                .HasMaxLength(100)
                .HasConversion(
                    producto => producto!.Value,
                    value => new Producto(value)
                );

            entity.Property(cantidadminima => cantidadminima!.CantidadMinima)
                .HasConversion(
                    cantidadminima => cantidadminima!.Value,
                    value => new CantidadMinima(value)
                );

            entity.Property(cantidadmaxima => cantidadmaxima!.CantidadMaxima)
                .HasConversion(
                    cantidadmaxima => cantidadmaxima!.Value,
                    value => new CantidadMaxima(value)
                );

            entity.Property(cantidadactual => cantidadactual!.CantidadActual)
                .HasConversion(
                    cantidadactual => cantidadactual!.Value,
                    value => new CantidadActual(value)
                );

            entity.Property(iva => iva!.IVA)
                .HasConversion(
                    iva => iva!.Value,
                    value => new IVA(value)
                );

            entity.Property(preciounidad => preciounidad!.PrecioUnidad)
                .HasConversion(
                    preciounidad => preciounidad!.Value,
                    value => new PrecioUnidad(value)
                );

            entity.Property(existencial => existencial!.Existencial)
                .HasConversion(
                    existencial => existencial!.GetEstadoExistencial(),
                    value => new Existencial(0, 0, 0)
                );

            entity.Property(preciomasiva => preciomasiva!.PrecioMasIVA)
                .HasConversion(
                    preciomasiva => preciomasiva!.GetPrecioMasIVA(),
                    value => new PrecioMasIVA(0, 0)
                );

            // Campos ignorados dado que son calculados en
            // la lógica del dominio Domain.

            //entity.Ignore(existencial => existencial.Existencial);

            //entity.Ignore(preciomasiva => preciomasiva.PrecioMasIVA);
        });

        base.OnModelCreating(modelBuilder);
    }
}

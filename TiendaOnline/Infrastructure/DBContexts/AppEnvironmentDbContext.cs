using Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using TiendaOnline.Domain.Stocks;
using TiendaOnline.Infrastructure.DBContexts;

namespace TiendaOnline.DBContexts;

public sealed class AppEnvironmentDbContext : DbContext, IUnitOfWork
{
    public AppEnvironmentDbContext(
        DbContextOptions<AppEnvironmentDbContext> options
    ) : base(options) { }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    var connectionString = "server=localhost;user=root;password=root;database=sistema;";
    //    var serverVersion = new MySqlServerVersion(new Version(5, 6, 21));
    //    optionsBuilder.UseMySql(connectionString, serverVersion);
    //    base.OnConfiguring(optionsBuilder);
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Adapta las entidades de Domain a Models automáticamente.
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppEnvironmentDbContext).Assembly
        );

        modelBuilder.Entity<Stock>(entity => 
        {
            entity.ToTable("stocks");
            entity.HasKey(e => e.ID);
            entity.Property(id => id.ID)
                .HasConversion(
                    id => id!.Value, 
                    value=> new ID(value)
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

    // public AppEnvironmentDBContext(DbContextOptions options): base(options) { }

    // Sección de los DbSet 
    // Importante: Añadir a las Entidades nombres una letra (s) al final.
    // public DbSet<SomeClassEntity> SomeClassEntity(s) <- aqui { get; set; }

    //public DbSet<User> Users { get; set; }

    //// Sección para el constructor del modelo de cada entidad.
    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    // Modo Manual.
    //    //modelBuilder.Entity<SomeClassEntity>()
    //    //   .Property(a => a.SomethingFiled).IsConcurrencyToken(); 

    //    // Modo Automático.
    //    // No requiere declarar entidad por entidad con DBSet<...>
    //    modelBuilder.ApplyConfigurationsFromAssembly(
    //        typeof(AppEnvironmentDBContext).Assembly
    //    );
    //    base.OnModelCreating(modelBuilder);

    //}

    /// <summary>
    /// Control de cambios sobrescrito para funcionalidades personalizadas.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ConcurrencyException"></exception>
    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException(
                "Infrastructure: Fallo en la concurrencia de los datos. Detalle:", ex
            );
        }
        catch (Exception ex)
        {
            throw new ConcurrencyException(
                "Infrastructure: Fallo genérico. Detalle:", ex
            );
        }
    }
}

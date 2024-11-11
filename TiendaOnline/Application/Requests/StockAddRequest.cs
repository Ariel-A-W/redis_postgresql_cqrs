using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TiendaOnline.Application.Requests;

public class StockAddRequest 
    : IRequest<int>
{
    [Required]
    public string? Codigo { get; set; }

    [Required]
    public string? Producto { get; set; }

    [Required]
    public int CantidadMinima { get; set; }

    [Required]
    public int CantidadMaxima { get; set; }

    [Required]
    public int CantidadActual { get; set; }

    [Required]
    public decimal IVA { get; set; }

    [Required]
    public decimal PrecioUnidad { get; set; }
}

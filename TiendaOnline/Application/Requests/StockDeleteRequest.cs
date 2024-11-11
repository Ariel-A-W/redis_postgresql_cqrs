using MediatR;
using System.ComponentModel.DataAnnotations;

namespace TiendaOnline.Application.Requests;

public class StockDeleteRequest 
    : IRequest<int>
{
    [Required]
    public int ID { get; set; }
}

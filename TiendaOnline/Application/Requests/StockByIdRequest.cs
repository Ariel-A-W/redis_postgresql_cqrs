using MediatR;
using System.ComponentModel.DataAnnotations;
using TiendaOnline.Application.Responses;

namespace TiendaOnline.Application.Requests;

public class StockByIdRequest : IRequest<IEnumerable<StockResponse>> 
{
    [Required]
    public int ID { get; set; }
}

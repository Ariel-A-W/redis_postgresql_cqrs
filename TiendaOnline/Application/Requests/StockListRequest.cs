using MediatR;
using TiendaOnline.Application.Responses;

namespace TiendaOnline.Application.Requests;

public class StockListRequest : IRequest<IEnumerable<StockResponse>> { }

using MediatR;
using Microsoft.AspNetCore.Mvc;
using TiendaOnline.Application.Requests;
using TiendaOnline.Application.Responses;

namespace TiendaOnline.Controllers;

[ApiController]
[Route("[controller]")]
public class StockController : ControllerBase
{
    private readonly ISender _sender;

    public StockController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("/[controller]/getall")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StockResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<StockResponse>> GetAll(
        CancellationToken cancellationToken    
    )
    {
        var request = new StockListRequest(); 
        var registros = _sender.Send(request, cancellationToken);

        if (registros is null)
            return NotFound("No se han hallado registros en la base de datos.");

        if (registros.Result.Count() == 0)
            return NotFound("No se han hallado registros en la base de datos.");

        return Ok(registros.Result);
    }

    [HttpGet("/[controller]/getbyid")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StockResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<StockResponse>> GetById(
        [FromQuery] StockByIdRequest request, 
        CancellationToken cancellationToken
    )
    {
        var registros = _sender.Send(request, cancellationToken);

        if (registros is null)
            return NotFound($"No se ha hallado el registro. ID: {request.ID}");

        if (registros.Result.Count() == 0)
            return NotFound($"No se ha hallado el registro. ID: {request.ID}");

        return Ok(registros.Result);
    }

    [HttpPost("/[controller]/add")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<int> Add(
        [FromBody] StockAddRequest request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var resultado = _sender.Send(request, cancellationToken);
            return Created("1", resultado.Result);
        }
        catch
        {
            return BadRequest("La solictud ha fallado.");
        }
    }

    [HttpPut("/[controller]/update")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<int> Update(
        [FromBody] StockUpdateRequest request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var resultado = _sender.Send(request, cancellationToken);
            return Created("1", resultado.Result);
        }
        catch
        {
            return BadRequest("La solictud ha fallado.");
        }
    }

    [HttpDelete("/[controller]/delete")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<int> Delete(
        [FromBody] StockDeleteRequest request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var resultado = _sender.Send(request, cancellationToken);
            return Accepted("1", resultado.Result);
        }
        catch
        {
            return BadRequest("La solictud ha fallado.");
        }
    }
}
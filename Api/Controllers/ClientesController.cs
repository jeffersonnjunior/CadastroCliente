using Application.Clientes.CadastrarCliente;
using Application.Clientes.ObterCliente;
using Application.Exceptions;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/clientes")]
public sealed class ClientesController : ControllerBase
{
    private readonly CadastrarClienteHandler _cadastrar;
    private readonly ObterClienteHandler _obter;

    public ClientesController(CadastrarClienteHandler cadastrar, ObterClienteHandler obter)
    {
        _cadastrar = cadastrar;
        _obter = obter;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ClienteResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Cadastrar(
        [FromBody] CadastrarClienteRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _cadastrar.HandleAsync(
                new CadastrarClienteCommand(request.Nome, request.Cpf, request.Email),
                cancellationToken);

            return CreatedAtAction(nameof(ObterPorId), new { id = result.Id }, result);
        }
        catch (DomainException ex)
        {
            return BadRequest(CreateProblem(StatusCodes.Status400BadRequest, "Validação", ex.Message));
        }
        catch (CpfDuplicadoException ex)
        {
            return Conflict(CreateProblem(StatusCodes.Status409Conflict, "CPF duplicado", ex.Message));
        }
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ClienteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var result = await _obter.HandleAsync(id, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    private ProblemDetails CreateProblem(int status, string title, string detail) =>
        new()
        {
            Status = status,
            Title = title,
            Detail = detail,
            Instance = HttpContext.Request.Path
        };
}

public sealed record CadastrarClienteRequest(string Nome, string Cpf, string Email);

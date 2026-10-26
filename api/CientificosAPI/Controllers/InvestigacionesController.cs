using CientificosAPI.Dtos;
using CientificosAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CientificosAPI.Controllers;

/// <summary>
/// Gestiona las investigaciones científicas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[ProducesResponseType(
    typeof(ProblemDetails),
    StatusCodes.Status500InternalServerError)]
public class InvestigacionesController : ControllerBase
{
    private readonly IInvestigacionService _service;

    public InvestigacionesController(
        IInvestigacionService service)
    {
        _service = service;
    }

    /// <summary>
    /// Lista todas las investigaciones.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(
        typeof(List<InvestigacionDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<List<InvestigacionDto>>>
        GetInvestigaciones()
    {
        var investigaciones =
            await _service.ObtenerTodasAsync();

        return Ok(investigaciones);
    }

    /// <summary>
    /// Filtra investigaciones por estado.
    /// </summary>
    /// <param name="estado">
    /// Estado: En curso, Finalizada o Publicada.
    /// </param>
    [HttpGet("estado/{estado}")]
    [ProducesResponseType(
        typeof(List<InvestigacionDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<InvestigacionDto>>>
        GetPorEstado(string estado)
    {
        var investigaciones =
            await _service.ObtenerPorEstadoAsync(estado);

        return Ok(investigaciones);
    }

    /// <summary>
    /// Crea una investigación.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(
        typeof(InvestigacionDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        typeof(ValidationProblemDetails),
        StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<InvestigacionDto>>
        PostInvestigacion(
            [FromBody] InvestigacionGuardarDto dto)
    {
        var creada = await _service.CrearAsync(dto);

        return Created(
            $"/api/investigaciones/{creada.Id}",
            creada);
    }

    /// <summary>
    /// Actualiza una investigación.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        typeof(ValidationProblemDetails),
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutInvestigacion(
        int id,
        [FromBody] InvestigacionGuardarDto dto)
    {
        await _service.ActualizarAsync(id, dto);
        return NoContent();
    }

    /// <summary>
    /// Elimina una investigación.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteInvestigacion(int id)
    {
        await _service.EliminarAsync(id);
        return NoContent();
    }
}
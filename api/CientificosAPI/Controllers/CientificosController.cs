using CientificosAPI.Dtos;
using CientificosAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CientificosAPI.Controllers;

/// <summary>
/// Gestiona científicos y sus especialidades.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[ProducesResponseType(
    typeof(ProblemDetails),
    StatusCodes.Status500InternalServerError)]
public class CientificosController : ControllerBase
{
    private readonly ICientificoService _service;

    public CientificosController(ICientificoService service)
    {
        _service = service;
    }

    /// <summary>
    /// Lista todos los científicos con sus especialidades.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(
        typeof(List<CientificoListaDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CientificoListaDto>>>
        GetCientificos()
    {
        var cientificos = await _service.ObtenerTodosAsync();
        return Ok(cientificos);
    }

    /// <summary>
    /// Obtiene el detalle de un científico.
    /// </summary>
    /// <param name="id">Identificador del científico.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(
        typeof(CientificoDetalleDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CientificoDetalleDto>>
        GetCientifico(int id)
    {
        var cientifico = await _service.ObtenerPorIdAsync(id);
        return Ok(cientifico);
    }

    /// <summary>
    /// Crea un científico y asigna sus especialidades.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(
        typeof(CientificoDetalleDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        typeof(ValidationProblemDetails),
        StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CientificoDetalleDto>>
        PostCientifico([FromBody] CientificoGuardarDto dto)
    {
        var creado = await _service.CrearAsync(dto);

        return CreatedAtAction(
            nameof(GetCientifico),
            new { id = creado.Id },
            creado);
    }

    /// <summary>
    /// Actualiza un científico y, si se envían, sus especialidades.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        typeof(ValidationProblemDetails),
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutCientifico(
        int id,
        [FromBody] CientificoGuardarDto dto)
    {
        await _service.ActualizarAsync(id, dto);
        return NoContent();
    }

    /// <summary>
    /// Elimina un científico y sus registros relacionados.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCientifico(int id)
    {
        await _service.EliminarAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Asigna una especialidad a un científico.
    /// </summary>
    [HttpPost("{id:int}/especialidades/{especialidadId:int}")]
    [ProducesResponseType(
        typeof(CientificoDetalleDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CientificoDetalleDto>>
        AsignarEspecialidad(int id, int especialidadId)
    {
        var cientifico = await _service
            .AsignarEspecialidadAsync(id, especialidadId);

        return Ok(cientifico);
    }

    /// <summary>
    /// Quita una especialidad de un científico.
    /// </summary>
    [HttpDelete("{id:int}/especialidades/{especialidadId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoverEspecialidad(
        int id,
        int especialidadId)
    {
        await _service.RemoverEspecialidadAsync(
            id,
            especialidadId);

        return NoContent();
    }
}
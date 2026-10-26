using CientificosAPI.Dtos;
using CientificosAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CientificosAPI.Controllers;

/// <summary>
/// Gestiona los descubrimientos científicos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[ProducesResponseType(
    typeof(ProblemDetails),
    StatusCodes.Status500InternalServerError)]
public class DescubrimientosController : ControllerBase
{
    private readonly IDescubrimientoService _service;

    public DescubrimientosController(
        IDescubrimientoService service)
    {
        _service = service;
    }

    /// <summary>
    /// Lista todos los descubrimientos.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(
        typeof(List<DescubrimientoDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<List<DescubrimientoDto>>>
        GetDescubrimientos()
    {
        var descubrimientos =
            await _service.ObtenerTodosAsync();

        return Ok(descubrimientos);
    }

    /// <summary>
    /// Lista los descubrimientos de un científico.
    /// </summary>
    /// <param name="cientificoId">
    /// Identificador del científico.
    /// </param>
    [HttpGet(
        "/api/cientificos/{cientificoId:int}/descubrimientos")]
    [ProducesResponseType(
        typeof(List<DescubrimientoDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<DescubrimientoDto>>>
        GetPorCientifico(int cientificoId)
    {
        var descubrimientos = await _service
            .ObtenerPorCientificoAsync(cientificoId);

        return Ok(descubrimientos);
    }

    /// <summary>
    /// Crea un descubrimiento.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(
        typeof(DescubrimientoDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        typeof(ValidationProblemDetails),
        StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DescubrimientoDto>>
        PostDescubrimiento(
            [FromBody] DescubrimientoGuardarDto dto)
    {
        var creado = await _service.CrearAsync(dto);

        return Created(
            $"/api/descubrimientos/{creado.Id}",
            creado);
    }

    /// <summary>
    /// Actualiza un descubrimiento.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        typeof(ValidationProblemDetails),
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutDescubrimiento(
        int id,
        [FromBody] DescubrimientoGuardarDto dto)
    {
        await _service.ActualizarAsync(id, dto);
        return NoContent();
    }

    /// <summary>
    /// Elimina un descubrimiento.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDescubrimiento(int id)
    {
        await _service.EliminarAsync(id);
        return NoContent();
    }
}
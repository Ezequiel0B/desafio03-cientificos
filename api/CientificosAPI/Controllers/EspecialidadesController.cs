using CientificosAPI.Dtos;
using CientificosAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CientificosAPI.Controllers;

/// <summary>
/// Gestiona el catálogo de especialidades.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[ProducesResponseType(
    typeof(ProblemDetails),
    StatusCodes.Status500InternalServerError)]
public class EspecialidadesController : ControllerBase
{
    private readonly IEspecialidadService _service;

    public EspecialidadesController(IEspecialidadService service)
    {
        _service = service;
    }

    /// <summary>
    /// Lista las especialidades disponibles.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(
        typeof(List<EspecialidadDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<List<EspecialidadDto>>>
        GetEspecialidades()
    {
        var especialidades =
            await _service.ObtenerTodasAsync();

        return Ok(especialidades);
    }

    /// <summary>
    /// Crea una especialidad.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(
        typeof(EspecialidadDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        typeof(ValidationProblemDetails),
        StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EspecialidadDto>>
        PostEspecialidad([FromBody] EspecialidadCrearDto dto)
    {
        var creada = await _service.CrearAsync(dto);

        return Created(
            $"/api/especialidades/{creada.Id}",
            creada);
    }
}
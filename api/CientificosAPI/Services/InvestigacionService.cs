using CientificosAPI.Data;
using CientificosAPI.Dtos;
using CientificosAPI.Exceptions;
using CientificosAPI.Mappings;
using CientificosAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CientificosAPI.Services;

public interface IInvestigacionService
{
    Task<List<InvestigacionDto>> ObtenerTodasAsync();

    Task<List<InvestigacionDto>> ObtenerPorEstadoAsync(
        string estado);

    Task<InvestigacionDto> CrearAsync(
        InvestigacionGuardarDto dto);

    Task ActualizarAsync(
        int id,
        InvestigacionGuardarDto dto);

    Task EliminarAsync(int id);
}

public class InvestigacionService : IInvestigacionService
{
    private readonly AppDbContext _context;

    public InvestigacionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<InvestigacionDto>> ObtenerTodasAsync()
    {
        var investigaciones = await _context.Investigaciones
            .AsNoTracking()
            .Include(i => i.Cientifico)
            .OrderByDescending(i => i.FechaInicio)
            .ToListAsync();

        return investigaciones
            .Select(i => i.ToDto())
            .ToList();
    }

    public async Task<List<InvestigacionDto>>
        ObtenerPorEstadoAsync(string estado)
    {
        var estadoCanonico = ObtenerEstadoCanonico(estado);

        var investigaciones = await _context.Investigaciones
            .AsNoTracking()
            .Include(i => i.Cientifico)
            .Where(i => i.Estado == estadoCanonico)
            .OrderByDescending(i => i.FechaInicio)
            .ToListAsync();

        return investigaciones
            .Select(i => i.ToDto())
            .ToList();
    }

    public async Task<InvestigacionDto> CrearAsync(
        InvestigacionGuardarDto dto)
    {
        var estadoCanonico = await ValidarAsync(dto);

        var investigacion = new Investigacion
        {
            Titulo = dto.Titulo.Trim(),
            AreaEstudio = dto.AreaEstudio.Trim(),
            FechaInicio = dto.FechaInicio!.Value.Date,
            FechaFin = dto.FechaFin?.Date,
            Estado = estadoCanonico,
            CientificoId = dto.CientificoId
        };

        _context.Investigaciones.Add(investigacion);
        await _context.SaveChangesAsync();

        await _context.Entry(investigacion)
            .Reference(i => i.Cientifico)
            .LoadAsync();

        return investigacion.ToDto();
    }

    public async Task ActualizarAsync(
        int id,
        InvestigacionGuardarDto dto)
    {
        var investigacion =
            await _context.Investigaciones.FindAsync(id);

        if (investigacion is null)
        {
            throw new NotFoundException(
                $"No existe una investigación con id {id}.");
        }

        var estadoCanonico = await ValidarAsync(dto);

        investigacion.Titulo = dto.Titulo.Trim();
        investigacion.AreaEstudio = dto.AreaEstudio.Trim();
        investigacion.FechaInicio =
            dto.FechaInicio!.Value.Date;
        investigacion.FechaFin = dto.FechaFin?.Date;
        investigacion.Estado = estadoCanonico;
        investigacion.CientificoId = dto.CientificoId;

        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(int id)
    {
        var investigacion =
            await _context.Investigaciones.FindAsync(id);

        if (investigacion is null)
        {
            throw new NotFoundException(
                $"No existe una investigación con id {id}.");
        }

        _context.Investigaciones.Remove(investigacion);
        await _context.SaveChangesAsync();
    }

    private async Task<string> ValidarAsync(
        InvestigacionGuardarDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Titulo))
        {
            throw new BadRequestException(
                "El título de la investigación es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(dto.AreaEstudio))
        {
            throw new BadRequestException(
                "El área de estudio es obligatoria.");
        }

        if (!dto.FechaInicio.HasValue)
        {
            throw new BadRequestException(
                "La fecha de inicio es obligatoria.");
        }

        if (dto.FechaFin.HasValue &&
            dto.FechaFin.Value.Date <
            dto.FechaInicio.Value.Date)
        {
            throw new BadRequestException(
                "La fecha de fin no puede ser anterior " +
                "a la fecha de inicio.");
        }

        var estadoCanonico = ObtenerEstadoCanonico(dto.Estado);

        var cientificoExiste = await _context.Cientificos
            .AnyAsync(c => c.Id == dto.CientificoId);

        if (!cientificoExiste)
        {
            throw new BadRequestException(
                $"El científico con id {dto.CientificoId} no existe.");
        }

        return estadoCanonico;
    }

    private static string ObtenerEstadoCanonico(string estado)
    {
        if (string.IsNullOrWhiteSpace(estado))
        {
            throw new BadRequestException(
                "El estado de la investigación es obligatorio.");
        }

        var estadoCanonico = EstadosInvestigacion.Todos
            .FirstOrDefault(valor =>
                valor.Equals(
                    estado.Trim(),
                    StringComparison.OrdinalIgnoreCase));

        if (estadoCanonico is null)
        {
            throw new BadRequestException(
                "Estado inválido. Los valores permitidos son: " +
                $"{string.Join(", ", EstadosInvestigacion.Todos)}.");
        }

        return estadoCanonico;
    }
}
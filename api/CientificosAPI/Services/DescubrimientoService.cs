using CientificosAPI.Data;
using CientificosAPI.Dtos;
using CientificosAPI.Exceptions;
using CientificosAPI.Mappings;
using CientificosAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CientificosAPI.Services;

public interface IDescubrimientoService
{
    Task<List<DescubrimientoDto>> ObtenerTodosAsync();

    Task<List<DescubrimientoDto>> ObtenerPorCientificoAsync(
        int cientificoId);

    Task<DescubrimientoDto> CrearAsync(
        DescubrimientoGuardarDto dto);

    Task ActualizarAsync(
        int id,
        DescubrimientoGuardarDto dto);

    Task EliminarAsync(int id);
}

public class DescubrimientoService : IDescubrimientoService
{
    private readonly AppDbContext _context;

    public DescubrimientoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<DescubrimientoDto>> ObtenerTodosAsync()
    {
        var descubrimientos = await _context.Descubrimientos
            .AsNoTracking()
            .Include(d => d.Cientifico)
            .OrderByDescending(d => d.FechaDescubrimiento)
            .ToListAsync();

        return descubrimientos
            .Select(d => d.ToDto())
            .ToList();
    }

    public async Task<List<DescubrimientoDto>>
        ObtenerPorCientificoAsync(int cientificoId)
    {
        var cientificoExiste = await _context.Cientificos
            .AnyAsync(c => c.Id == cientificoId);

        if (!cientificoExiste)
        {
            throw new NotFoundException(
                $"No existe un científico con id {cientificoId}.");
        }

        var descubrimientos = await _context.Descubrimientos
            .AsNoTracking()
            .Include(d => d.Cientifico)
            .Where(d => d.CientificoId == cientificoId)
            .OrderByDescending(d => d.FechaDescubrimiento)
            .ToListAsync();

        return descubrimientos
            .Select(d => d.ToDto())
            .ToList();
    }

    public async Task<DescubrimientoDto> CrearAsync(
        DescubrimientoGuardarDto dto)
    {
        await ValidarAsync(dto);

        var descubrimiento = new Descubrimiento
        {
            Titulo = dto.Titulo.Trim(),
            Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion)
                ? null
                : dto.Descripcion.Trim(),
            FechaDescubrimiento =
                dto.FechaDescubrimiento!.Value.Date,
            Lugar = dto.Lugar.Trim(),
            CientificoId = dto.CientificoId
        };

        _context.Descubrimientos.Add(descubrimiento);
        await _context.SaveChangesAsync();

        await _context.Entry(descubrimiento)
            .Reference(d => d.Cientifico)
            .LoadAsync();

        return descubrimiento.ToDto();
    }

    public async Task ActualizarAsync(
        int id,
        DescubrimientoGuardarDto dto)
    {
        var descubrimiento =
            await _context.Descubrimientos.FindAsync(id);

        if (descubrimiento is null)
        {
            throw new NotFoundException(
                $"No existe un descubrimiento con id {id}.");
        }

        await ValidarAsync(dto);

        descubrimiento.Titulo = dto.Titulo.Trim();

        descubrimiento.Descripcion =
            string.IsNullOrWhiteSpace(dto.Descripcion)
                ? null
                : dto.Descripcion.Trim();

        descubrimiento.FechaDescubrimiento =
            dto.FechaDescubrimiento!.Value.Date;

        descubrimiento.Lugar = dto.Lugar.Trim();
        descubrimiento.CientificoId = dto.CientificoId;

        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(int id)
    {
        var descubrimiento =
            await _context.Descubrimientos.FindAsync(id);

        if (descubrimiento is null)
        {
            throw new NotFoundException(
                $"No existe un descubrimiento con id {id}.");
        }

        _context.Descubrimientos.Remove(descubrimiento);
        await _context.SaveChangesAsync();
    }

    private async Task ValidarAsync(
        DescubrimientoGuardarDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Titulo))
        {
            throw new BadRequestException(
                "El título del descubrimiento es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(dto.Lugar))
        {
            throw new BadRequestException(
                "El lugar del descubrimiento es obligatorio.");
        }

        if (!dto.FechaDescubrimiento.HasValue)
        {
            throw new BadRequestException(
                "La fecha del descubrimiento es obligatoria.");
        }

        if (dto.FechaDescubrimiento.Value.Date > DateTime.Today)
        {
            throw new BadRequestException(
                "La fecha del descubrimiento no puede ser futura.");
        }

        var cientificoExiste = await _context.Cientificos
            .AnyAsync(c => c.Id == dto.CientificoId);

        if (!cientificoExiste)
        {
            throw new BadRequestException(
                $"El científico con id {dto.CientificoId} no existe.");
        }
    }
}
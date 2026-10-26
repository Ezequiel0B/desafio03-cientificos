using CientificosAPI.Data;
using CientificosAPI.Dtos;
using CientificosAPI.Exceptions;
using CientificosAPI.Mappings;
using CientificosAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CientificosAPI.Services;

public interface IEspecialidadService
{
    Task<List<EspecialidadDto>> ObtenerTodasAsync();

    Task<EspecialidadDto> CrearAsync(EspecialidadCrearDto dto);
}

public class EspecialidadService : IEspecialidadService
{
    private readonly AppDbContext _context;

    public EspecialidadService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<EspecialidadDto>> ObtenerTodasAsync()
    {
        var especialidades = await _context.Especialidades
            .AsNoTracking()
            .OrderBy(e => e.Nombre)
            .ToListAsync();

        return especialidades
            .Select(e => e.ToDto())
            .ToList();
    }

    public async Task<EspecialidadDto> CrearAsync(
        EspecialidadCrearDto dto)
    {
        var nombre = dto.Nombre.Trim();

        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new BadRequestException(
                "El nombre de la especialidad es obligatorio.");
        }

        var nombreRepetido = await _context.Especialidades
            .AnyAsync(e => e.Nombre == nombre);

        if (nombreRepetido)
        {
            throw new BadRequestException(
                $"Ya existe una especialidad llamada '{nombre}'.");
        }

        var especialidad = new Especialidad
        {
            Nombre = nombre,
            Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion)
                ? null
                : dto.Descripcion.Trim()
        };

        _context.Especialidades.Add(especialidad);
        await _context.SaveChangesAsync();

        return especialidad.ToDto();
    }
}
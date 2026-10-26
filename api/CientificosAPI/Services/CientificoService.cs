using CientificosAPI.Data;
using CientificosAPI.Dtos;
using CientificosAPI.Exceptions;
using CientificosAPI.Mappings;
using CientificosAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CientificosAPI.Services;

public interface ICientificoService
{
    Task<List<CientificoListaDto>> ObtenerTodosAsync();

    Task<CientificoDetalleDto> ObtenerPorIdAsync(int id);

    Task<CientificoDetalleDto> CrearAsync(CientificoGuardarDto dto);

    Task ActualizarAsync(int id, CientificoGuardarDto dto);

    Task EliminarAsync(int id);

    Task<CientificoDetalleDto> AsignarEspecialidadAsync(
        int id,
        int especialidadId);

    Task RemoverEspecialidadAsync(int id, int especialidadId);
}

public class CientificoService : ICientificoService
{
    private readonly AppDbContext _context;

    public CientificoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CientificoListaDto>> ObtenerTodosAsync()
    {
        var cientificos = await _context.Cientificos
            .AsNoTracking()
            .Include(c => c.CientificoEspecialidades)
                .ThenInclude(ce => ce.Especialidad)
            .OrderBy(c => c.Apellido)
            .ThenBy(c => c.Nombre)
            .ToListAsync();

        return cientificos
            .Select(c => c.ToListaDto())
            .ToList();
    }

    public async Task<CientificoDetalleDto> ObtenerPorIdAsync(int id)
    {
        var cientifico = await _context.Cientificos
            .AsNoTracking()
            .Include(c => c.CientificoEspecialidades)
                .ThenInclude(ce => ce.Especialidad)
            .Include(c => c.Descubrimientos)
            .Include(c => c.Investigaciones)
            .AsSplitQuery()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cientifico is null)
        {
            throw new NotFoundException(
                $"No existe un científico con id {id}.");
        }

        return cientifico.ToDetalleDto();
    }

    public async Task<CientificoDetalleDto> CrearAsync(
        CientificoGuardarDto dto)
    {
        ValidarDatosObligatorios(dto);
        ValidarFechaNacimiento(dto.FechaNacimiento);

        var correo = dto.Email.Trim().ToLowerInvariant();

        await ValidarCorreoUnicoAsync(correo, null);

        var especialidades = await ValidarEspecialidadesAsync(
            dto.EspecialidadIds);

        var cientifico = new Cientifico
        {
            Nombre = dto.Nombre.Trim(),
            Apellido = dto.Apellido.Trim(),
            Nacionalidad = dto.Nacionalidad.Trim(),
            FechaNacimiento = dto.FechaNacimiento!.Value.Date,
            Email = correo
        };

        foreach (var especialidadId in especialidades)
        {
            cientifico.CientificoEspecialidades.Add(
                new CientificoEspecialidad
                {
                    EspecialidadId = especialidadId
                });
        }

        _context.Cientificos.Add(cientifico);
        await _context.SaveChangesAsync();

        return await ObtenerPorIdAsync(cientifico.Id);
    }

    public async Task ActualizarAsync(
        int id,
        CientificoGuardarDto dto)
    {
        var cientifico = await _context.Cientificos
            .Include(c => c.CientificoEspecialidades)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cientifico is null)
        {
            throw new NotFoundException(
                $"No existe un científico con id {id}.");
        }

        ValidarDatosObligatorios(dto);
        ValidarFechaNacimiento(dto.FechaNacimiento);

        var correo = dto.Email.Trim().ToLowerInvariant();

        await ValidarCorreoUnicoAsync(correo, id);

        cientifico.Nombre = dto.Nombre.Trim();
        cientifico.Apellido = dto.Apellido.Trim();
        cientifico.Nacionalidad = dto.Nacionalidad.Trim();
        cientifico.FechaNacimiento =
            dto.FechaNacimiento!.Value.Date;
        cientifico.Email = correo;

        if (dto.EspecialidadIds is not null)
        {
            var nuevas = await ValidarEspecialidadesAsync(
                dto.EspecialidadIds);

            var relacionesAEliminar =
                cientifico.CientificoEspecialidades
                    .Where(ce =>
                        !nuevas.Contains(ce.EspecialidadId))
                    .ToList();

            _context.CientificoEspecialidades.RemoveRange(
                relacionesAEliminar);

            var actuales = cientifico.CientificoEspecialidades
                .Select(ce => ce.EspecialidadId)
                .ToHashSet();

            foreach (var especialidadId in nuevas
                         .Where(idEspecialidad =>
                             !actuales.Contains(idEspecialidad)))
            {
                cientifico.CientificoEspecialidades.Add(
                    new CientificoEspecialidad
                    {
                        EspecialidadId = especialidadId
                    });
            }
        }

        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(int id)
    {
        var cientifico = await _context.Cientificos.FindAsync(id);

        if (cientifico is null)
        {
            throw new NotFoundException(
                $"No existe un científico con id {id}.");
        }

        _context.Cientificos.Remove(cientifico);
        await _context.SaveChangesAsync();
    }

    public async Task<CientificoDetalleDto>
        AsignarEspecialidadAsync(int id, int especialidadId)
    {
        var existeCientifico = await _context.Cientificos
            .AnyAsync(c => c.Id == id);

        if (!existeCientifico)
        {
            throw new NotFoundException(
                $"No existe un científico con id {id}.");
        }

        var existeEspecialidad = await _context.Especialidades
            .AnyAsync(e => e.Id == especialidadId);

        if (!existeEspecialidad)
        {
            throw new NotFoundException(
                $"No existe una especialidad con id {especialidadId}.");
        }

        var yaAsignada = await _context.CientificoEspecialidades
            .AnyAsync(ce =>
                ce.CientificoId == id &&
                ce.EspecialidadId == especialidadId);

        if (yaAsignada)
        {
            throw new BadRequestException(
                "El científico ya tiene asignada esa especialidad.");
        }

        var relacion = new CientificoEspecialidad
        {
            CientificoId = id,
            EspecialidadId = especialidadId
        };

        _context.CientificoEspecialidades.Add(relacion);
        await _context.SaveChangesAsync();

        return await ObtenerPorIdAsync(id);
    }

    public async Task RemoverEspecialidadAsync(
        int id,
        int especialidadId)
    {
        var existeCientifico = await _context.Cientificos
            .AnyAsync(c => c.Id == id);

        if (!existeCientifico)
        {
            throw new NotFoundException(
                $"No existe un científico con id {id}.");
        }

        var existeEspecialidad = await _context.Especialidades
            .AnyAsync(e => e.Id == especialidadId);

        if (!existeEspecialidad)
        {
            throw new NotFoundException(
                $"No existe una especialidad con id {especialidadId}.");
        }

        var relacion = await _context.CientificoEspecialidades
            .FindAsync(id, especialidadId);

        if (relacion is null)
        {
            throw new NotFoundException(
                "El científico no tiene asignada esa especialidad.");
        }

        _context.CientificoEspecialidades.Remove(relacion);
        await _context.SaveChangesAsync();
    }

    private static void ValidarDatosObligatorios(
        CientificoGuardarDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
        {
            throw new BadRequestException(
                "El nombre es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(dto.Apellido))
        {
            throw new BadRequestException(
                "El apellido es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(dto.Nacionalidad))
        {
            throw new BadRequestException(
                "La nacionalidad es obligatoria.");
        }

        if (string.IsNullOrWhiteSpace(dto.Email))
        {
            throw new BadRequestException(
                "El correo electrónico es obligatorio.");
        }
    }

    private static void ValidarFechaNacimiento(
        DateTime? fechaNacimiento)
    {
        if (!fechaNacimiento.HasValue)
        {
            throw new BadRequestException(
                "La fecha de nacimiento es obligatoria.");
        }

        if (fechaNacimiento.Value.Date > DateTime.Today)
        {
            throw new BadRequestException(
                "La fecha de nacimiento no puede ser futura.");
        }
    }

    private async Task ValidarCorreoUnicoAsync(
        string correo,
        int? idActual)
    {
        var correoExiste = await _context.Cientificos
            .AnyAsync(c =>
                c.Email == correo &&
                c.Id != idActual);

        if (correoExiste)
        {
            throw new BadRequestException(
                $"Ya existe un científico con el correo {correo}.");
        }
    }

    private async Task<List<int>> ValidarEspecialidadesAsync(
        List<int>? ids)
    {
        var distintos = (ids ?? new List<int>())
            .Distinct()
            .ToList();

        if (distintos.Count == 0)
        {
            return distintos;
        }

        var encontrados = await _context.Especialidades
            .Where(e => distintos.Contains(e.Id))
            .Select(e => e.Id)
            .ToListAsync();

        var inexistentes = distintos
            .Except(encontrados)
            .ToList();

        if (inexistentes.Count > 0)
        {
            throw new BadRequestException(
                $"No existen las especialidades con id: " +
                $"{string.Join(", ", inexistentes)}.");
        }

        return distintos;
    }
}
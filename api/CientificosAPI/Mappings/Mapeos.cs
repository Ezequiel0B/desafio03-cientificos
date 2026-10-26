using CientificosAPI.Dtos;
using CientificosAPI.Models;

namespace CientificosAPI.Mappings;

public static class Mapeos
{
    public static EspecialidadDto ToDto(this Especialidad especialidad)
    {
        return new EspecialidadDto
        {
            Id = especialidad.Id,
            Nombre = especialidad.Nombre,
            Descripcion = especialidad.Descripcion
        };
    }

    public static DescubrimientoDto ToDto(
        this Descubrimiento descubrimiento)
    {
        return new DescubrimientoDto
        {
            Id = descubrimiento.Id,
            Titulo = descubrimiento.Titulo,
            Descripcion = descubrimiento.Descripcion,
            FechaDescubrimiento = descubrimiento.FechaDescubrimiento,
            Lugar = descubrimiento.Lugar,
            CientificoId = descubrimiento.CientificoId,
            CientificoNombre = descubrimiento.Cientifico is null
                ? null
                : $"{descubrimiento.Cientifico.Nombre} " +
                  $"{descubrimiento.Cientifico.Apellido}"
        };
    }

    public static InvestigacionDto ToDto(
        this Investigacion investigacion)
    {
        return new InvestigacionDto
        {
            Id = investigacion.Id,
            Titulo = investigacion.Titulo,
            AreaEstudio = investigacion.AreaEstudio,
            FechaInicio = investigacion.FechaInicio,
            FechaFin = investigacion.FechaFin,
            Estado = investigacion.Estado,
            CientificoId = investigacion.CientificoId,
            CientificoNombre = investigacion.Cientifico is null
                ? null
                : $"{investigacion.Cientifico.Nombre} " +
                  $"{investigacion.Cientifico.Apellido}"
        };
    }

    public static CientificoListaDto ToListaDto(
        this Cientifico cientifico)
    {
        return new CientificoListaDto
        {
            Id = cientifico.Id,
            Nombre = cientifico.Nombre,
            Apellido = cientifico.Apellido,
            Nacionalidad = cientifico.Nacionalidad,
            FechaNacimiento = cientifico.FechaNacimiento,
            Email = cientifico.Email,
            Especialidades = cientifico.CientificoEspecialidades
                .Select(ce => ce.Especialidad.ToDto())
                .OrderBy(e => e.Nombre)
                .ToList()
        };
    }

    public static CientificoDetalleDto ToDetalleDto(
        this Cientifico cientifico)
    {
        return new CientificoDetalleDto
        {
            Id = cientifico.Id,
            Nombre = cientifico.Nombre,
            Apellido = cientifico.Apellido,
            Nacionalidad = cientifico.Nacionalidad,
            FechaNacimiento = cientifico.FechaNacimiento,
            Email = cientifico.Email,

            Especialidades = cientifico.CientificoEspecialidades
                .Select(ce => ce.Especialidad.ToDto())
                .OrderBy(e => e.Nombre)
                .ToList(),

            Descubrimientos = cientifico.Descubrimientos
                .OrderByDescending(d => d.FechaDescubrimiento)
                .Select(d => d.ToDto())
                .ToList(),

            Investigaciones = cientifico.Investigaciones
                .OrderByDescending(i => i.FechaInicio)
                .Select(i => i.ToDto())
                .ToList()
        };
    }
}
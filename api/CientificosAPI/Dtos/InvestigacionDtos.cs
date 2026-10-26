using System.ComponentModel.DataAnnotations;

namespace CientificosAPI.Dtos;

public class InvestigacionGuardarDto
{
    [Required(ErrorMessage = "El título es obligatorio")]
    [StringLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El área de estudio es obligatoria")]
    [StringLength(150)]
    public string AreaEstudio { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    [Required(ErrorMessage = "El estado es obligatorio")]
    [RegularExpression(
        "^(En curso|Finalizada|Publicada)$",
        ErrorMessage =
            "El estado debe ser En curso, Finalizada o Publicada")]
    public string Estado { get; set; } = string.Empty;

    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "Debe indicar un científico válido")]
    public int CientificoId { get; set; }
}

public class InvestigacionDto
{
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string AreaEstudio { get; set; } = string.Empty;

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public string Estado { get; set; } = string.Empty;

    public int CientificoId { get; set; }

    public string? CientificoNombre { get; set; }
}

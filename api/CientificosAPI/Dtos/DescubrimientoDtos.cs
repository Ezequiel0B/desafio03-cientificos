using System.ComponentModel.DataAnnotations;

namespace CientificosAPI.Dtos;

public class DescubrimientoGuardarDto
{
    [Required(ErrorMessage = "El título es obligatorio")]
    [StringLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "La fecha del descubrimiento es obligatoria")]
    public DateTime? FechaDescubrimiento { get; set; }

    [Required(ErrorMessage = "El lugar es obligatorio")]
    [StringLength(150)]
    public string Lugar { get; set; } = string.Empty;

    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "Debe indicar un científico válido")]
    public int CientificoId { get; set; }
}

public class DescubrimientoDto
{
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public DateTime FechaDescubrimiento { get; set; }

    public string Lugar { get; set; } = string.Empty;

    public int CientificoId { get; set; }

    public string? CientificoNombre { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace CientificosAPI.Dtos;

public class EspecialidadDto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }
}

public class EspecialidadCrearDto
{
    [Required(ErrorMessage = "El nombre de la especialidad es obligatorio")]
    [StringLength(
        100,
        ErrorMessage = "El nombre no puede superar los 100 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(
        500,
        ErrorMessage = "La descripción no puede superar los 500 caracteres")]
    public string? Descripcion { get; set; }
}
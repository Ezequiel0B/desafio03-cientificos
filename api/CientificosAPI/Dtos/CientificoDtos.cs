using System.ComponentModel.DataAnnotations;

namespace CientificosAPI.Dtos;

public class CientificoGuardarDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio")]
    [StringLength(100)]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "La nacionalidad es obligatoria")]
    [StringLength(80)]
    public string Nacionalidad { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
    public DateTime? FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El correo electrónico es obligatorio")]
    [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Identificadores de las especialidades.
    /// En PUT, si se omite la lista, no se modifican las actuales.
    /// </summary>
    public List<int>? EspecialidadIds { get; set; }
}

public class CientificoListaDto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Apellido { get; set; } = string.Empty;

    public string Nacionalidad { get; set; } = string.Empty;

    public DateTime FechaNacimiento { get; set; }

    public string Email { get; set; } = string.Empty;

    public List<EspecialidadDto> Especialidades { get; set; } = new();
}

public class CientificoDetalleDto : CientificoListaDto
{
    public List<DescubrimientoDto> Descubrimientos { get; set; } = new();

    public List<InvestigacionDto> Investigaciones { get; set; } = new();
}
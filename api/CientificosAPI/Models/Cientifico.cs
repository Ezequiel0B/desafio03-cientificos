namespace CientificosAPI.Models;

public class Cientifico
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Apellido { get; set; } = string.Empty;

    public string Nacionalidad { get; set; } = string.Empty;

    public DateTime FechaNacimiento { get; set; }

    public string Email { get; set; } = string.Empty;

    // Relación muchos a muchos con Especialidad
    public ICollection<CientificoEspecialidad> CientificoEspecialidades
    { get; set; } = new List<CientificoEspecialidad>();

    // Relación uno a muchos con Descubrimiento
    public ICollection<Descubrimiento> Descubrimientos
    { get; set; } = new List<Descubrimiento>();

    // Relación uno a muchos con Investigación
    public ICollection<Investigacion> Investigaciones
    { get; set; } = new List<Investigacion>();
}
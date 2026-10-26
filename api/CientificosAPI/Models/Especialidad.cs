namespace CientificosAPI.Models;

public class Especialidad
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    // Relación muchos a muchos con Científico
    public ICollection<CientificoEspecialidad> CientificoEspecialidades
    { get; set; } = new List<CientificoEspecialidad>();
}
namespace CientificosAPI.Models;

public class Descubrimiento
{
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public DateTime FechaDescubrimiento { get; set; }

    public string Lugar { get; set; } = string.Empty;

    // Llave foránea
    public int CientificoId { get; set; }

    // Propiedad de navegación
    public Cientifico Cientifico { get; set; } = null!;
}
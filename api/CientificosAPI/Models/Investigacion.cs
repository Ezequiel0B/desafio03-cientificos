namespace CientificosAPI.Models;

public static class EstadosInvestigacion
{
    public const string EnCurso = "En curso";
    public const string Finalizada = "Finalizada";
    public const string Publicada = "Publicada";

    public static readonly string[] Todos =
    {
        EnCurso,
        Finalizada,
        Publicada
    };
}

public class Investigacion
{
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string AreaEstudio { get; set; } = string.Empty;

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public string Estado { get; set; } = EstadosInvestigacion.EnCurso;

    // Llave foránea
    public int CientificoId { get; set; }

    // Propiedad de navegación
    public Cientifico Cientifico { get; set; } = null!;
}
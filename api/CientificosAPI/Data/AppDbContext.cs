using CientificosAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CientificosAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cientifico> Cientificos => Set<Cientifico>();

    public DbSet<Especialidad> Especialidades => Set<Especialidad>();

    public DbSet<CientificoEspecialidad> CientificoEspecialidades
        => Set<CientificoEspecialidad>();

    public DbSet<Descubrimiento> Descubrimientos
        => Set<Descubrimiento>();

    public DbSet<Investigacion> Investigaciones
        => Set<Investigacion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigurarCientificos(modelBuilder);
        ConfigurarEspecialidades(modelBuilder);
        ConfigurarRelacionCientificoEspecialidad(modelBuilder);
        ConfigurarDescubrimientos(modelBuilder);
        ConfigurarInvestigaciones(modelBuilder);
        AgregarDatosIniciales(modelBuilder);
    }

    private static void ConfigurarCientificos(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cientifico>(entidad =>
        {
            entidad.Property(c => c.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            entidad.Property(c => c.Apellido)
                .IsRequired()
                .HasMaxLength(100);

            entidad.Property(c => c.Nacionalidad)
                .IsRequired()
                .HasMaxLength(80);

            entidad.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(150);

            entidad.Property(c => c.FechaNacimiento)
                .HasColumnType("date");

            entidad.HasIndex(c => c.Email)
                .IsUnique();
        });
    }

    private static void ConfigurarEspecialidades(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Especialidad>(entidad =>
        {
            entidad.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            entidad.Property(e => e.Descripcion)
                .HasMaxLength(500);

            entidad.HasIndex(e => e.Nombre)
                .IsUnique();
        });
    }

    private static void ConfigurarRelacionCientificoEspecialidad(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CientificoEspecialidad>(entidad =>
        {
            entidad.HasKey(ce => new
            {
                ce.CientificoId,
                ce.EspecialidadId
            });

            entidad.HasOne(ce => ce.Cientifico)
                .WithMany(c => c.CientificoEspecialidades)
                .HasForeignKey(ce => ce.CientificoId)
                .OnDelete(DeleteBehavior.Cascade);

            entidad.HasOne(ce => ce.Especialidad)
                .WithMany(e => e.CientificoEspecialidades)
                .HasForeignKey(ce => ce.EspecialidadId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigurarDescubrimientos(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Descubrimiento>(entidad =>
        {
            entidad.Property(d => d.Titulo)
                .IsRequired()
                .HasMaxLength(200);

            entidad.Property(d => d.Descripcion)
                .HasMaxLength(1000);

            entidad.Property(d => d.Lugar)
                .IsRequired()
                .HasMaxLength(150);

            entidad.Property(d => d.FechaDescubrimiento)
                .HasColumnType("date");

            entidad.HasOne(d => d.Cientifico)
                .WithMany(c => c.Descubrimientos)
                .HasForeignKey(d => d.CientificoId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigurarInvestigaciones(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Investigacion>(entidad =>
        {
            entidad.Property(i => i.Titulo)
                .IsRequired()
                .HasMaxLength(200);

            entidad.Property(i => i.AreaEstudio)
                .IsRequired()
                .HasMaxLength(150);

            entidad.Property(i => i.Estado)
                .IsRequired()
                .HasMaxLength(20);

            entidad.Property(i => i.FechaInicio)
                .HasColumnType("date");

            entidad.Property(i => i.FechaFin)
                .HasColumnType("date");

            entidad.ToTable(tabla =>
                tabla.HasCheckConstraint(
                    "CK_Investigaciones_Estado",
                    "[Estado] IN ('En curso', 'Finalizada', 'Publicada')"));

            entidad.HasOne(i => i.Cientifico)
                .WithMany(c => c.Investigaciones)
                .HasForeignKey(i => i.CientificoId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void AgregarDatosIniciales(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Especialidad>().HasData(
            new Especialidad
            {
                Id = 1,
                Nombre = "Física",
                Descripcion = "Estudio de la materia, energía e interacciones"
            },
            new Especialidad
            {
                Id = 2,
                Nombre = "Química",
                Descripcion = "Estudio de la composición de la materia"
            },
            new Especialidad
            {
                Id = 3,
                Nombre = "Biología",
                Descripcion = "Estudio de los seres vivos"
            },
            new Especialidad
            {
                Id = 4,
                Nombre = "Matemáticas",
                Descripcion = "Estudio de números, estructuras y patrones"
            },
            new Especialidad
            {
                Id = 5,
                Nombre = "Informática",
                Descripcion = "Estudio del procesamiento de información"
            }
        );
    }
}
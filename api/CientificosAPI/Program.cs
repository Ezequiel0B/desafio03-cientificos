using System.Reflection;
using CientificosAPI.Data;
using CientificosAPI.Middleware;
using CientificosAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// SQL Server y Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection")));

// Servicios e inyección de dependencias
builder.Services.AddScoped<
    ICientificoService,
    CientificoService>();

builder.Services.AddScoped<
    IEspecialidadService,
    EspecialidadService>();

builder.Services.AddScoped<
    IDescubrimientoService,
    DescubrimientoService>();

builder.Services.AddScoped<
    IInvestigacionService,
    InvestigacionService>();

// Permite que el cliente web llame a la API
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirClientes", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuración de Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "API de Científicos",
            Version = "v1",
            Description =
                "API REST para gestionar científicos, " +
                "especialidades, descubrimientos e investigaciones."
        });

    var nombreXml =
        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

    var rutaXml = Path.Combine(
        AppContext.BaseDirectory,
        nombreXml);

    options.IncludeXmlComments(rutaXml);
});

var app = builder.Build();

// Debe colocarse primero para capturar los errores posteriores
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(
        "/swagger/v1/swagger.json",
        "API de Científicos v1");
});

app.UseCors("PermitirClientes");

app.UseAuthorization();

app.MapControllers();

app.Run();
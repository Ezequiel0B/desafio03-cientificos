using CientificosAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registra AppDbContext y utiliza la conexión de appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Habilita los controladores de la API
builder.Services.AddControllers();
// Habilita Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
// Publica Swagger para documentar y probar la API
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
// Busca y activa todos los controladores
app.MapControllers();
app.Run();
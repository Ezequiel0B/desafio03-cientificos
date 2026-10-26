using CientificosAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CientificosAPI.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _siguiente;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(
        RequestDelegate siguiente,
        ILogger<ErrorHandlingMiddleware> logger)
    {
        _siguiente = siguiente;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _siguiente(context);
        }
        catch (NotFoundException ex)
        {
            await EscribirErrorAsync(
                context,
                StatusCodes.Status404NotFound,
                "Recurso no encontrado",
                ex.Message);
        }
        catch (BadRequestException ex)
        {
            await EscribirErrorAsync(
                context,
                StatusCodes.Status400BadRequest,
                "Solicitud inválida",
                ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error no controlado en la ruta {Ruta}",
                context.Request.Path);

            await EscribirErrorAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "Error interno del servidor",
                "Ocurrió un error inesperado al procesar la solicitud.");
        }
    }

    private static Task EscribirErrorAsync(
        HttpContext context,
        int estado,
        string titulo,
        string detalle)
    {
        var problema = new ProblemDetails
        {
            Status = estado,
            Title = titulo,
            Detail = detalle,
            Instance = context.Request.Path
        };

        context.Response.StatusCode = estado;
        context.Response.ContentType = "application/problem+json";

        return context.Response.WriteAsJsonAsync(problema);
    }
}
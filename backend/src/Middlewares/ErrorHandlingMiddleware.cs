using System.Net;
using System.Text.Json;

namespace backend.src.Middlewares
{
    /// <summary>
    /// Middleware para tratamento global de erros e exceções.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro durante a execução da requisição");
                await TratarExcecaoAsync(context, ex);
            }
        }

        private static Task TratarExcecaoAsync(HttpContext context, Exception exception)
        {
            var codigoStatus = HttpStatusCode.InternalServerError;
            var resultado = JsonSerializer.Serialize(new { error = "Ocorreu um erro interno. Por favor, tente novamente." });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)codigoStatus;
            return context.Response.WriteAsync(resultado);
        }
    }
}

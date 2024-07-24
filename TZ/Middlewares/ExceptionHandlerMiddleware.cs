using Api.Contracts;

namespace Api.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;
    private readonly IResponceFactory _responceFactory;
    
    public ExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlerMiddleware> logger,
        IResponceFactory responceFactory)
    {
        _next = next;
        _logger = logger;
        _responceFactory = responceFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error has occurred " + ex.StackTrace);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var errorResponce = _responceFactory.CreateErrorResponce(ex: ex);
            await context.Response.WriteAsJsonAsync(errorResponce);
        }
    }
}
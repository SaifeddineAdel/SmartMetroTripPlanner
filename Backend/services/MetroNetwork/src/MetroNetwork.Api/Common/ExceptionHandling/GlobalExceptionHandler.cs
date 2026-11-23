using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MetroNetwork.Application.Common.Exceptions;
using MetroNetwork.Domain.Common.Exceptions;

namespace MetroNetwork.Api.Common.ExceptionHandling;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title, detail) = exception switch
        {
            DomainValidationException =>
                (StatusCodes.Status400BadRequest,
                 "Validation Error",
                 exception.Message),

            UnauthorizedException =>
                (StatusCodes.Status401Unauthorized,
                 "Unauthorized",
                 exception.Message),

            NotFoundException =>
                (StatusCodes.Status404NotFound,
                 "Resource Not Found",
                 exception.Message),

            ConflictException =>
                (StatusCodes.Status409Conflict,
                 "Conflict",
                 exception.Message),

            _ =>
                (StatusCodes.Status500InternalServerError,
                 "Internal Server Error",
                 "An unexpected error occurred.")
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(
                exception,
                "An unexpected error occurred while processing the request.");
        }
        else
        {
            _logger.LogInformation(
                "Request failed with status code {StatusCode}: {Message}",
                statusCode,
                exception.Message);
        }

        httpContext.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] =
            httpContext.TraceIdentifier;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}
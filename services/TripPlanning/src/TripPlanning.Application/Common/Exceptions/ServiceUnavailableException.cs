namespace TripPlanning.Application.Common.Exceptions;

public sealed class ServiceUnavailableException : AppException
{
    public ServiceUnavailableException(string message)
        : base(message, "Service Unavailable", 503)
    {
    }
}
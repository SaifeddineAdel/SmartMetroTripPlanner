namespace TripPlanning.Application.Common.Exceptions;

public sealed class NotFoundException : AppException
{
    public NotFoundException(string message)
        : base(message, "Resource Not Found", 404)
    {
    }
}
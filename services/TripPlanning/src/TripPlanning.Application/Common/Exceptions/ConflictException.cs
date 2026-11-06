namespace TripPlanning.Application.Common.Exceptions;

public sealed class ConflictException : AppException
{
    public ConflictException(string message)
        : base(message, "Conflict", 409)
    {
    }
}
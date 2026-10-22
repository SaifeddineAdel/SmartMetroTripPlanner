using MetroNetwork.Domain.Common.Exceptions;

namespace MetroNetwork.Domain.Features.Stations;

public sealed class Station
{
    public int Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public bool IsDeleted { get; private set; }

    private Station()
    {
    }

    private Station(string name)
    {
        Name = NormalizeName(name);
    }

    public static Station Create(string name)
    {
        return new Station(name);
    }

    public void Rename(string newName)
    {
        EnsureNotDeleted();

        Name = NormalizeName(newName);
    }

    public void Delete()
    {
        IsDeleted = true;
    }

    private void EnsureNotDeleted()
    {
        if (IsDeleted)
        {
            throw new InvalidOperationException(
                "A deleted station cannot be modified.");
        }
    }

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainValidationException(
                "Station name cannot be empty.");
        }

        var normalizedName = name.Trim();

        if (normalizedName.Length > 100)
        {
            throw new DomainValidationException(
                "Station name cannot exceed 100 characters.");
        }

        return normalizedName;
    }
}

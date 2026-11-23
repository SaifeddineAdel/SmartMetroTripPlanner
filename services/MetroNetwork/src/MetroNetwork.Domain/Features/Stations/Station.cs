using MetroNetwork.Domain.Common.Exceptions;
using MetroNetwork.Domain.Features.LineStations;

namespace MetroNetwork.Domain.Features.Stations;

public sealed class Station
{
    public int Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public bool IsDeleted { get; private set; }

    public ICollection<LineStation> LineStations { get; private set; }
        = new List<LineStation>();

    private Station()
    {
    }

    private Station(string name, string description)
    {
        Name = NormalizeName(name);
        Description = NormalizeDescription(description);
    }

    public static Station Create(string name, string description)
    {
        return new Station(name, description);
    }

    public void Rename(string newName)
    {
        EnsureNotDeleted();
        Name = NormalizeName(newName);
    }

    public void UpdateDescription(string description)
    {
        EnsureNotDeleted();
        Description = NormalizeDescription(description);
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

    private static string NormalizeDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new DomainValidationException(
                "Station description cannot be empty.");
        }

        var normalizedDescription = description.Trim();

        if (normalizedDescription.Length > 500)
        {
            throw new DomainValidationException(
                "Station description cannot exceed 500 characters.");
        }

        return normalizedDescription;
    }
}
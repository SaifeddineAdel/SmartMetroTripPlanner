using MetroNetwork.Domain.Common.Exceptions;

namespace MetroNetwork.Domain.Features.MetroLines;

public sealed class MetroLine
{
    public int Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    private MetroLine()
    {
    }

    private MetroLine(string name)
    {
        Name = NormalizeName(name);
    }

    public static MetroLine Create(string name)
    {
        return new MetroLine(name);
    }

    public void Rename(string newName)
    {
        Name = NormalizeName(newName);
    }

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainValidationException(
                "Metro line name is required.");
        }

        var normalizedName = name.Trim();

        if (normalizedName.Length > 100)
        {
            throw new DomainValidationException(
                "Metro line name cannot exceed 100 characters.");
        }

        return normalizedName;
    }
}

using MetroNetwork.Domain.Common.Exceptions;

namespace MetroNetwork.Domain.Features.LineStations;

public sealed class LineStation
{
    public int MetroLineId { get; private set; }

    public int StationId { get; private set; }

    public int Sequence { get; private set; }

    private LineStation()
    {
    }

    private LineStation(
        int metroLineId,
        int stationId,
        int sequence)
    {
        if (metroLineId <= 0)
        {
            throw new DomainValidationException(
                "Metro line ID must be greater than zero.");
        }

        if (stationId <= 0)
        {
            throw new DomainValidationException(
                "Station ID must be greater than zero.");
        }

        ValidateSequence(sequence);

        MetroLineId = metroLineId;
        StationId = stationId;
        Sequence = sequence;
    }

    public static LineStation Create(
        int metroLineId,
        int stationId,
        int sequence)
    {
        return new LineStation(metroLineId, stationId, sequence);
    }

    public void ChangeSequence(int newSequence)
    {
        ValidateSequence(newSequence);
        Sequence = newSequence;
    }

    private static void ValidateSequence(int sequence)
    {
        if (sequence <= 0)
        {
            throw new DomainValidationException(
                "Sequence must be greater than zero.");
        }
    }
}

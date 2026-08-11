namespace SpaceSector.Api.Dtos.Surveillance;

public class SurveillanceSummary
{
    public int TotalNpcs { get; set; }

    public int CoveredNpcs { get; set; }

    public int BlindSpotNpcs { get; set; }

    public float CoveragePercentage { get; set; }

    public int Score { get; set; }

    public List<Guid> BlindSpotNpcIds { get; set; } = [];
}
namespace AiEngineeringLab.Core.Models.Retrieval;

public sealed class RerankResult
{
    public required string Id { get; init; }
    public required string Text { get; init; }
    public double OriginalScore { get; init; }
    public double RerankScore { get; init; }
}

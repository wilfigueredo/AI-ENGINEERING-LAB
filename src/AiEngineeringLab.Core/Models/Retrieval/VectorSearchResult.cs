namespace AiEngineeringLab.Core.Models.Retrieval;

public sealed class VectorSearchResult
{
    public required string Id { get; init; }

    public required string Text { get; init; }

    public required double Score { get; init; }
}

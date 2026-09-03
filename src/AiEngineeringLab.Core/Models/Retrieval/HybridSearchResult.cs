namespace AiEngineeringLab.Core.Models.Retrieval;

public sealed class HybridSearchResult
{
    public required string Id { get; init; }
    public required string Text { get; init; }

    public double VectorScore { get; init; }
    public double LexicalScore { get; init; }
    public double HybridScore { get; init; }
}

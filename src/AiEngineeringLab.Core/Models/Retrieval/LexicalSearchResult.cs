namespace AiEngineeringLab.Core.Models.Retrieval;

public sealed class LexicalSearchResult
{
    public required string Id { get; init; }
    public required string Text { get; init; }
    public double Score { get; init; }
}


namespace AiEngineeringLab.Core.Models.Retrieval;

public sealed class CompressedContextResult
{
    public required string Id { get; init; }
    public required string OriginalText { get; init; }
    public required string CompressedText { get; init; }
}

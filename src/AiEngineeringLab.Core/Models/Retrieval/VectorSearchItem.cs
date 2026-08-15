namespace AiEngineeringLab.Core.Models.Retrieval;

public sealed class VectorSearchItem
{
    public required string Id { get; init; }

    public required string Text { get; init; }

    public required float[] Vector { get; init; }
}

namespace AiEngineeringLab.Core.Models.Retrieval;

public sealed class VectorSearchFilter
{
    public string? DocumentId { get; init; }
    public string? Source { get; init; }
    public string? Version { get; init; }
}

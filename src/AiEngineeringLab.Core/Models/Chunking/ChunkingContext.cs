namespace AiEngineeringLab.Core.Models.Chunking;

public sealed class ChunkingContext
{
    public required string DocumentId { get; init; }

    public string Source { get; init; } = string.Empty;

    public string Title { get; init; } = string.Empty;

    public string Version { get; init; } = "1";
}

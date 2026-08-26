namespace AiEngineeringLab.Core.Models.Ingestion;

public sealed class IndexedChunk
{
    public required string Id { get; init; }

    public required string DocumentId { get; init; }

    public required string Text { get; init; }

    public required float[] Embedding { get; init; }

    public string Source { get; init; } = string.Empty;

    public string Title { get; init; } = string.Empty;

    public string Version { get; init; } = "1";

    public int ChunkIndex { get; init; }
}

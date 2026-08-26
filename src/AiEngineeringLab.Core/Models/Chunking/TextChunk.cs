namespace AiEngineeringLab.Core.Models.Chunking;

public sealed class TextChunk
{
    public string Id { get; init; } = Guid.NewGuid().ToString();

    public string DocumentId { get; init; } = string.Empty;

    public string Source { get; init; } = string.Empty;

    public string Title { get; init; } = string.Empty;

    public string Version { get; init; } = "1";

    public int Index { get; init; }

    public string Text { get; init; } = string.Empty;

    public int StartPosition { get; init; }

    public int Length { get; init; }

    public DateTimeOffset CreatedAt { get; init; } =
        DateTimeOffset.UtcNow;
}

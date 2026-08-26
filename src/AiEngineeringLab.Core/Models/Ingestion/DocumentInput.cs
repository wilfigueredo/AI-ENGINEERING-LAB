namespace AiEngineeringLab.Core.Models.Ingestion;

public sealed class DocumentInput
{
    public required string Id { get; init; }

    public required string Title { get; init; }

    public required string Content { get; init; }

    public string Source { get; init; } = string.Empty;

    public string Version { get; init; } = "1";
}

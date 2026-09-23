namespace AiEngineeringLab.Core.AI.Evaluation.Harness;

public sealed record RagPipelineResult(
    IReadOnlyList<string> RetrievedDocumentIds,
    IReadOnlyCollection<string> RetrievedContext,
    string GeneratedAnswer);

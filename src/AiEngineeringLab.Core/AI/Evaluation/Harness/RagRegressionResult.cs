namespace AiEngineeringLab.Core.AI.Evaluation.Harness;

public sealed record RagRegressionResult(
    bool HasRegression,
    IReadOnlyCollection<string> RegressedMetrics);

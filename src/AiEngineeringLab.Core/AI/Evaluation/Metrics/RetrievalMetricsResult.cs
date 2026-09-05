namespace AiEngineeringLab.Core.AI.Evaluation.Metrics;

public sealed record RetrievalMetricsResult(
    double Precision,
    double Recall,
    double HitRate,
    double ReciprocalRank);

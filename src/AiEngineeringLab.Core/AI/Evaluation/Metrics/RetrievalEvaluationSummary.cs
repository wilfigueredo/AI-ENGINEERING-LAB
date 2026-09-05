namespace AiEngineeringLab.Core.AI.Evaluation.Metrics;

public sealed record RetrievalEvaluationSummary(
    double MeanPrecision,
    double MeanRecall,
    double HitRate,
    double Mrr);

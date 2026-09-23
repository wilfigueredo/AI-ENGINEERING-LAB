namespace AiEngineeringLab.Core.AI.Evaluation.Harness;

public sealed record RagEvaluationSummary(
    int TotalCases,
    double MeanPrecision,
    double MeanRecall,
    double HitRate,
    double Mrr,
    double MeanCorrectness,
    double MeanRelevance,
    double MeanCompleteness,
    double MeanGenerationScore,
    double MeanFaithfulnessScore,
    double GroundedAnswerRate);

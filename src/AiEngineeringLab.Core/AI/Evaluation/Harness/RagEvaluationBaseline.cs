namespace AiEngineeringLab.Core.AI.Evaluation.Harness;

public sealed record RagEvaluationBaseline(
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

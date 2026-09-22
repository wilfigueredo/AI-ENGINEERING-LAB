namespace AiEngineeringLab.Core.AI.Evaluation.Grounding;

public sealed record GroundingEvaluationSummary(
    double MeanFaithfulnessScore,
    double GroundedAnswerRate,
    int TotalSupportedClaims,
    int TotalUnsupportedClaims);

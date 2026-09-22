namespace AiEngineeringLab.Core.AI.Evaluation.Grounding;

public sealed record GroundingEvaluationResult(
    double FaithfulnessScore,
    IReadOnlyCollection<string> SupportedClaims,
    IReadOnlyCollection<string> UnsupportedClaims);

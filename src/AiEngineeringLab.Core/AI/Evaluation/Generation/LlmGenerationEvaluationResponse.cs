namespace AiEngineeringLab.Core.AI.Evaluation.Generation;

public sealed record LlmGenerationEvaluationResponse(
    double Correctness,
    double Relevance,
    double Completeness,
    double OverallScore);

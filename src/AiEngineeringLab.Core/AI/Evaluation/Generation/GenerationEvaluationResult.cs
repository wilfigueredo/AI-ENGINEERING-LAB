namespace AiEngineeringLab.Core.AI.Evaluation.Generation;

public sealed record GenerationEvaluationResult(
    double Correctness,
    double Relevance,
    double Completeness,
    double OverallScore);

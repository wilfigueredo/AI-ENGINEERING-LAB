namespace AiEngineeringLab.Core.AI.Evaluation.Generation;

public interface IGenerationEvaluator
{
    Task<GenerationEvaluationResult> EvaluateAsync(
        string question,
        string? expectedAnswer,
        string generatedAnswer,
        CancellationToken cancellationToken = default);
}

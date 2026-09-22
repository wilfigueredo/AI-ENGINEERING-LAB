namespace AiEngineeringLab.Core.AI.Evaluation.Grounding;

public interface IGroundingEvaluator
{
    Task<GroundingEvaluationResult> EvaluateAsync(
        string question,
        IReadOnlyCollection<string> context,
        string generatedAnswer,
        CancellationToken cancellationToken = default);
}

namespace AiEngineeringLab.Core.AI.Evaluation.Generation.Interface;

public interface IGenerationJudgeClient
{
    Task<string> EvaluateAsync(
        string prompt,
        CancellationToken cancellationToken = default);
}

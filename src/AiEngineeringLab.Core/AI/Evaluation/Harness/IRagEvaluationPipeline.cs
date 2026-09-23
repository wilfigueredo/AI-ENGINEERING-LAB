namespace AiEngineeringLab.Core.AI.Evaluation.Harness;

public interface IRagEvaluationPipeline
{
    Task<RagPipelineResult> ExecuteAsync(
        string question,
        CancellationToken cancellationToken = default);
}

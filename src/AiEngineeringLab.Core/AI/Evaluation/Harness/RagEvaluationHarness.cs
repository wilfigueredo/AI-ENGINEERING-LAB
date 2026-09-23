using AiEngineeringLab.Core.AI.Evaluation.Generation;
using AiEngineeringLab.Core.AI.Evaluation.Grounding;
using AiEngineeringLab.Core.AI.Evaluation.Metrics;

using EvaluationRetrievalMetrics =
    AiEngineeringLab.Core.AI.Evaluation.Metrics.RetrievalMetrics;
using AiEngineeringLab.Core.AI.Evaluation.Models;

namespace AiEngineeringLab.Core.AI.Evaluation.Harness;

public sealed class RagEvaluationHarness(
    IRagEvaluationPipeline pipeline,
    IGenerationEvaluator generationEvaluator,
    IGroundingEvaluator groundingEvaluator)
{
    public async Task<RagEvaluationExecutionResult> EvaluateAsync(
        RagEvaluationCase evaluationCase,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(evaluationCase);

        var pipelineResult =
            await pipeline.ExecuteAsync(
                evaluationCase.Question,
                cancellationToken);

        var retrieval =
    EvaluationRetrievalMetrics.Calculate(
        evaluationCase.ExpectedDocumentIds,
        pipelineResult.RetrievedDocumentIds);

        var generation =
            await generationEvaluator.EvaluateAsync(
                evaluationCase.Question,
                evaluationCase.ExpectedAnswer,
                pipelineResult.GeneratedAnswer,
                cancellationToken);

        var grounding =
            await groundingEvaluator.EvaluateAsync(
                evaluationCase.Question,
                pipelineResult.RetrievedContext,
                pipelineResult.GeneratedAnswer,
                cancellationToken);

        return new RagEvaluationExecutionResult(
            evaluationCase.Id,
            retrieval,
            generation,
            grounding);
    }
}

using AiEngineeringLab.Core.AI.Evaluation.Models;

namespace AiEngineeringLab.Core.AI.Evaluation.Harness;

public sealed class RagEvaluationRunner(
    RagEvaluationHarness harness)
{
    public async Task<RagEvaluationSummary> RunAsync(
        RagEvaluationDataset dataset,
        double groundedThreshold = 0.8,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dataset);

        if (groundedThreshold is < 0 or > 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(groundedThreshold));
        }

        if (dataset.Cases.Count == 0)
        {
            return new RagEvaluationSummary(
                TotalCases: 0,
                MeanPrecision: 0,
                MeanRecall: 0,
                HitRate: 0,
                Mrr: 0,
                MeanCorrectness: 0,
                MeanRelevance: 0,
                MeanCompleteness: 0,
                MeanGenerationScore: 0,
                MeanFaithfulnessScore: 0,
                GroundedAnswerRate: 0);
        }

        var results =
            new List<RagEvaluationExecutionResult>();

        foreach (var evaluationCase in dataset.Cases)
        {
            var result =
                await harness.EvaluateAsync(
                    evaluationCase,
                    cancellationToken);

            results.Add(result);
        }

        var groundedAnswers =
            results.Count(
                x => x.Grounding.FaithfulnessScore >= groundedThreshold);

        return new RagEvaluationSummary(
            TotalCases: results.Count,

            MeanPrecision:
                results.Average(
                    x => x.Retrieval.Precision),

            MeanRecall:
                results.Average(
                    x => x.Retrieval.Recall),

            HitRate:
                results.Average(
                    x => x.Retrieval.HitRate),

            Mrr:
                results.Average(
                    x => x.Retrieval.ReciprocalRank),

            MeanCorrectness:
                results.Average(
                    x => x.Generation.Correctness),

            MeanRelevance:
                results.Average(
                    x => x.Generation.Relevance),

            MeanCompleteness:
                results.Average(
                    x => x.Generation.Completeness),

            MeanGenerationScore:
                results.Average(
                    x => x.Generation.OverallScore),

            MeanFaithfulnessScore:
                results.Average(
                    x => x.Grounding.FaithfulnessScore),

            GroundedAnswerRate:
                (double)groundedAnswers / results.Count);
    }
}

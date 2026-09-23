using AiEngineeringLab.Core.AI.Evaluation.Harness;

namespace AiEngineeringLab.UnitTests.AI.Evaluation.Harness;

public sealed class RagRegressionDetectorTests
{
    [Fact]
    public void Compare_ShouldDetectRegression()
    {
        var baseline =
            new RagEvaluationBaseline(
                MeanPrecision: 0.90,
                MeanRecall: 0.90,
                HitRate: 1.00,
                Mrr: 0.95,
                MeanCorrectness: 0.90,
                MeanRelevance: 0.90,
                MeanCompleteness: 0.90,
                MeanGenerationScore: 0.90,
                MeanFaithfulnessScore: 0.90,
                GroundedAnswerRate: 0.90);

        var current =
            new RagEvaluationSummary(
                TotalCases: 10,
                MeanPrecision: 0.88,
                MeanRecall: 0.80,
                HitRate: 1.00,
                Mrr: 0.94,
                MeanCorrectness: 0.89,
                MeanRelevance: 0.90,
                MeanCompleteness: 0.88,
                MeanGenerationScore: 0.89,
                MeanFaithfulnessScore: 0.82,
                GroundedAnswerRate: 0.90);

        var result =
            RagRegressionDetector.Compare(
                baseline,
                current,
                tolerance: 0.05);

        Assert.True(result.HasRegression);

        Assert.Contains(
            nameof(current.MeanRecall),
            result.RegressedMetrics);

        Assert.Contains(
            nameof(current.MeanFaithfulnessScore),
            result.RegressedMetrics);
    }

    [Fact]
    public void Compare_ShouldNotDetectRegression_WhenWithinTolerance()
    {
        var baseline =
            new RagEvaluationBaseline(
                MeanPrecision: 0.90,
                MeanRecall: 0.90,
                HitRate: 1.00,
                Mrr: 0.95,
                MeanCorrectness: 0.90,
                MeanRelevance: 0.90,
                MeanCompleteness: 0.90,
                MeanGenerationScore: 0.90,
                MeanFaithfulnessScore: 0.90,
                GroundedAnswerRate: 0.90);

        var current =
            new RagEvaluationSummary(
                TotalCases: 10,
                MeanPrecision: 0.88,
                MeanRecall: 0.86,
                HitRate: 0.97,
                Mrr: 0.91,
                MeanCorrectness: 0.88,
                MeanRelevance: 0.89,
                MeanCompleteness: 0.87,
                MeanGenerationScore: 0.88,
                MeanFaithfulnessScore: 0.86,
                GroundedAnswerRate: 0.87);

        var result =
            RagRegressionDetector.Compare(
                baseline,
                current,
                tolerance: 0.05);

        Assert.False(result.HasRegression);
        Assert.Empty(result.RegressedMetrics);
    }
}

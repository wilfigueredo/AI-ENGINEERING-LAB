using AiEngineeringLab.Core.AI.Evaluation.Metrics;

namespace AiEngineeringLab.UnitTests.AI.Evaluation.Metrics;

public sealed class RetrievalMetricsAggregatorTests
{
    [Fact]
    public void Calculate_ShouldAggregateRetrievalMetrics()
    {
        var results = new[]
        {
            new RetrievalMetricsResult(
                Precision: 1.00,
                Recall: 0.50,
                HitRate: 1.00,
                ReciprocalRank: 1.00),

            new RetrievalMetricsResult(
                Precision: 0.50,
                Recall: 1.00,
                HitRate: 1.00,
                ReciprocalRank: 0.50),

            new RetrievalMetricsResult(
                Precision: 0.00,
                Recall: 0.00,
                HitRate: 0.00,
                ReciprocalRank: 0.00)
        };

        var result =
            RetrievalMetricsAggregator.Calculate(results);

        Assert.Equal(0.50, result.MeanPrecision, 2);
        Assert.Equal(0.50, result.MeanRecall, 2);
        Assert.Equal(0.67, result.HitRate, 2);
        Assert.Equal(0.50, result.Mrr, 2);
    }

    [Fact]
    public void Calculate_ShouldReturnZero_WhenResultsAreEmpty()
    {
        var result =
            RetrievalMetricsAggregator.Calculate(
                Array.Empty<RetrievalMetricsResult>());

        Assert.Equal(0, result.MeanPrecision);
        Assert.Equal(0, result.MeanRecall);
        Assert.Equal(0, result.HitRate);
        Assert.Equal(0, result.Mrr);
    }
}

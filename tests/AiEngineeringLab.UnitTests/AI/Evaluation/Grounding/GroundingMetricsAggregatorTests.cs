using AiEngineeringLab.Core.AI.Evaluation.Grounding;

namespace AiEngineeringLab.UnitTests.AI.Evaluation.Grounding;

public sealed class GroundingMetricsAggregatorTests
{
    [Fact]
    public void Calculate_ShouldAggregateGroundingResults()
    {
        var results = new[]
        {
            new GroundingEvaluationResult(
                FaithfulnessScore: 1.0,
                SupportedClaims: ["claim-a", "claim-b"],
                UnsupportedClaims: []),

            new GroundingEvaluationResult(
                FaithfulnessScore: 0.8,
                SupportedClaims: ["claim-c"],
                UnsupportedClaims: ["claim-x"]),

            new GroundingEvaluationResult(
                FaithfulnessScore: 0.4,
                SupportedClaims: ["claim-d"],
                UnsupportedClaims: ["claim-y", "claim-z"])
        };

        var summary =
            GroundingMetricsAggregator.Calculate(results);

        Assert.Equal(0.73, summary.MeanFaithfulnessScore, 2);
        Assert.Equal(0.67, summary.GroundedAnswerRate, 2);
        Assert.Equal(4, summary.TotalSupportedClaims);
        Assert.Equal(3, summary.TotalUnsupportedClaims);
    }

    [Fact]
    public void Calculate_ShouldReturnZero_WhenResultsAreEmpty()
    {
        var summary =
            GroundingMetricsAggregator.Calculate(
                Array.Empty<GroundingEvaluationResult>());

        Assert.Equal(0, summary.MeanFaithfulnessScore);
        Assert.Equal(0, summary.GroundedAnswerRate);
        Assert.Equal(0, summary.TotalSupportedClaims);
        Assert.Equal(0, summary.TotalUnsupportedClaims);
    }
}

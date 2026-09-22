namespace AiEngineeringLab.Core.AI.Evaluation.Grounding;

public static class GroundingMetricsAggregator
{
    public static GroundingEvaluationSummary Calculate(
        IReadOnlyCollection<GroundingEvaluationResult> results,
        double groundedThreshold = 0.8)
    {
        ArgumentNullException.ThrowIfNull(results);

        if (groundedThreshold is < 0 or > 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(groundedThreshold));
        }

        if (results.Count == 0)
        {
            return new GroundingEvaluationSummary(
                MeanFaithfulnessScore: 0,
                GroundedAnswerRate: 0,
                TotalSupportedClaims: 0,
                TotalUnsupportedClaims: 0);
        }

        var meanFaithfulness =
            results.Average(x => x.FaithfulnessScore);

        var groundedAnswers =
            results.Count(
                x => x.FaithfulnessScore >= groundedThreshold);

        var groundedAnswerRate =
            (double)groundedAnswers / results.Count;

        var supportedClaims =
            results.Sum(x => x.SupportedClaims.Count);

        var unsupportedClaims =
            results.Sum(x => x.UnsupportedClaims.Count);

        return new GroundingEvaluationSummary(
            MeanFaithfulnessScore: meanFaithfulness,
            GroundedAnswerRate: groundedAnswerRate,
            TotalSupportedClaims: supportedClaims,
            TotalUnsupportedClaims: unsupportedClaims);
    }
}

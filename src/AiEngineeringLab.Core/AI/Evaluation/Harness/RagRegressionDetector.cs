namespace AiEngineeringLab.Core.AI.Evaluation.Harness;

public static class RagRegressionDetector
{
    public static RagRegressionResult Compare(
        RagEvaluationBaseline baseline,
        RagEvaluationSummary current,
        double tolerance = 0.05)
    {
        ArgumentNullException.ThrowIfNull(baseline);
        ArgumentNullException.ThrowIfNull(current);

        if (tolerance is < 0 or > 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(tolerance));
        }

        var regressions = new List<string>();

        Check(
            nameof(current.MeanPrecision),
            baseline.MeanPrecision,
            current.MeanPrecision,
            tolerance,
            regressions);

        Check(
            nameof(current.MeanRecall),
            baseline.MeanRecall,
            current.MeanRecall,
            tolerance,
            regressions);

        Check(
            nameof(current.HitRate),
            baseline.HitRate,
            current.HitRate,
            tolerance,
            regressions);

        Check(
            nameof(current.Mrr),
            baseline.Mrr,
            current.Mrr,
            tolerance,
            regressions);

        Check(
            nameof(current.MeanCorrectness),
            baseline.MeanCorrectness,
            current.MeanCorrectness,
            tolerance,
            regressions);

        Check(
            nameof(current.MeanRelevance),
            baseline.MeanRelevance,
            current.MeanRelevance,
            tolerance,
            regressions);

        Check(
            nameof(current.MeanCompleteness),
            baseline.MeanCompleteness,
            current.MeanCompleteness,
            tolerance,
            regressions);

        Check(
            nameof(current.MeanGenerationScore),
            baseline.MeanGenerationScore,
            current.MeanGenerationScore,
            tolerance,
            regressions);

        Check(
            nameof(current.MeanFaithfulnessScore),
            baseline.MeanFaithfulnessScore,
            current.MeanFaithfulnessScore,
            tolerance,
            regressions);

        Check(
            nameof(current.GroundedAnswerRate),
            baseline.GroundedAnswerRate,
            current.GroundedAnswerRate,
            tolerance,
            regressions);

        return new RagRegressionResult(
            HasRegression: regressions.Count > 0,
            RegressedMetrics: regressions);
    }

    private static void Check(
        string metricName,
        double baseline,
        double current,
        double tolerance,
        ICollection<string> regressions)
    {
        if (current < baseline - tolerance)
        {
            regressions.Add(metricName);
        }
    }
}

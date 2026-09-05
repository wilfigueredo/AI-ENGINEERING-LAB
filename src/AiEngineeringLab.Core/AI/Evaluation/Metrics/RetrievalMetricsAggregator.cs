namespace AiEngineeringLab.Core.AI.Evaluation.Metrics;

public static class RetrievalMetricsAggregator
{
    public static RetrievalEvaluationSummary Calculate(
        IReadOnlyCollection<RetrievalMetricsResult> results)
    {
        ArgumentNullException.ThrowIfNull(results);

        if (results.Count == 0)
        {
            return new RetrievalEvaluationSummary(
                MeanPrecision: 0,
                MeanRecall: 0,
                HitRate: 0,
                Mrr: 0);
        }

        return new RetrievalEvaluationSummary(
            MeanPrecision: results.Average(x => x.Precision),
            MeanRecall: results.Average(x => x.Recall),
            HitRate: results.Average(x => x.HitRate),
            Mrr: results.Average(x => x.ReciprocalRank));
    }
}

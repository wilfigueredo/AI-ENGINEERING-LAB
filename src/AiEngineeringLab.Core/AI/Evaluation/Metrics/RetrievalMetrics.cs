namespace AiEngineeringLab.Core.AI.Evaluation.Metrics;

public static class RetrievalMetrics
{
    public static RetrievalMetricsResult Calculate(
        IReadOnlyCollection<string> expectedDocumentIds,
        IReadOnlyList<string> retrievedDocumentIds)
    {
        ArgumentNullException.ThrowIfNull(expectedDocumentIds);
        ArgumentNullException.ThrowIfNull(retrievedDocumentIds);

        var expected = expectedDocumentIds.ToHashSet(
            StringComparer.OrdinalIgnoreCase);

        var retrieved = retrievedDocumentIds.ToArray();

        var relevantRetrieved =
            retrieved.Count(expected.Contains);

        var precision =
            retrieved.Length == 0
                ? 0
                : (double)relevantRetrieved / retrieved.Length;

        var recall =
            expected.Count == 0
                ? 0
                : (double)relevantRetrieved / expected.Count;

        var hitRate =
            relevantRetrieved > 0
                ? 1.0
                : 0.0;

        var reciprocalRank = 0.0;

        for (var i = 0; i < retrieved.Length; i++)
        {
            if (!expected.Contains(retrieved[i]))
                continue;

            reciprocalRank = 1.0 / (i + 1);

            break;
        }

        return new RetrievalMetricsResult(
            Precision: precision,
            Recall: recall,
            HitRate: hitRate,
            ReciprocalRank: reciprocalRank);
    }
}

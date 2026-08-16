namespace AiEngineeringLab.Core.AI;

public static class RetrievalMetrics
{
    public static double Precision(
        IEnumerable<string> retrievedIds,
        IEnumerable<string> relevantIds)
    {
        var retrieved = retrievedIds.ToHashSet();
        var relevant = relevantIds.ToHashSet();

        if (retrieved.Count == 0)
            return 0;

        var relevantRetrieved =
            retrieved.Intersect(relevant).Count();

        return (double)relevantRetrieved / retrieved.Count;
    }

    public static double Recall(
        IEnumerable<string> retrievedIds,
        IEnumerable<string> relevantIds)
    {
        var retrieved = retrievedIds.ToHashSet();
        var relevant = relevantIds.ToHashSet();

        if (relevant.Count == 0)
            return 0;

        var relevantRetrieved =
            retrieved.Intersect(relevant).Count();

        return (double)relevantRetrieved / relevant.Count;
    }
}

using AiEngineeringLab.Core.Models.Retrieval;

namespace AiEngineeringLab.Core.AI;

public static class VectorRetriever
{
    public static IReadOnlyList<VectorSearchResult> TopK(
        ReadOnlySpan<float> queryVector,
        IEnumerable<VectorSearchItem> items,
        int k)
    {
        if (k <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(k),
                "K must be greater than zero.");
        }

        var results = new List<VectorSearchResult>();

        foreach (var item in items)
        {
            var score = VectorSimilarity.CosineSimilarity(
                queryVector,
                item.Vector);

            results.Add(new VectorSearchResult
            {
                Id = item.Id,
                Text = item.Text,
                Score = score
            });
        }

        results.Sort(
            static (left, right) =>
                right.Score.CompareTo(left.Score));

        return results
            .Take(k)
            .ToList();
    }
}

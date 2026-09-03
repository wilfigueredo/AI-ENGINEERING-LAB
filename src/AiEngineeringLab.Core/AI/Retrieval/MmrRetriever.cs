using AiEngineeringLab.Core.Models.Ingestion;
using AiEngineeringLab.Core.Models.Retrieval;

namespace AiEngineeringLab.Core.AI.Retrieval;

public static class MmrRetriever
{
    public static IReadOnlyList<VectorSearchResult> Select(
        ReadOnlyMemory<float> queryVector,
        IReadOnlyList<IndexedChunk> candidates,
        int k,
        double lambda = 0.7)
    {
        if (k <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(k));
        }

        if (lambda is < 0 or > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(lambda));
        }

        if (candidates.Count == 0)
        {
            return [];
        }

        var selected = new List<IndexedChunk>();
        var remaining = candidates.ToList();

        while (selected.Count < k && remaining.Count > 0)
        {
            IndexedChunk? bestCandidate = null;
            double bestScore = double.NegativeInfinity;

            foreach (var candidate in remaining)
            {
                var relevance =
                    VectorSimilarity.CosineSimilarity(
                        queryVector.Span,
                        candidate.Embedding);

                var redundancy = selected.Count == 0
                    ? 0
                    : selected.Max(selectedChunk =>
                        VectorSimilarity.CosineSimilarity(
                            candidate.Embedding,
                            selectedChunk.Embedding));

                var mmrScore =
                    (lambda * relevance)
                    - ((1 - lambda) * redundancy);

                if (mmrScore > bestScore)
                {
                    bestScore = mmrScore;
                    bestCandidate = candidate;
                }
            }

            if (bestCandidate is null)
            {
                break;
            }

            selected.Add(bestCandidate);
            remaining.Remove(bestCandidate);
        }

        return selected
            .Select(chunk => new VectorSearchResult
            {
                Id = chunk.Id,
                Text = chunk.Text,
                Score = VectorSimilarity.CosineSimilarity(
                    queryVector.Span,
                    chunk.Embedding)
            })
            .ToList();
    }
}

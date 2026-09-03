using AiEngineeringLab.Core.AI.Interface;
using AiEngineeringLab.Core.Models.Ingestion;
using AiEngineeringLab.Core.Models.Retrieval;

namespace AiEngineeringLab.Core.AI.VectorStore;

public sealed class InMemoryVectorStore : IVectorStore
{
    private readonly List<IndexedChunk> _chunks = [];

    public Task SaveAsync(
        IEnumerable<IndexedChunk> chunks,
        CancellationToken cancellationToken = default)
    {
        _chunks.AddRange(chunks);

        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<IndexedChunk>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<IndexedChunk> result =
            _chunks.ToList();

        return Task.FromResult(result);
    }

    public Task<IReadOnlyList<VectorSearchResult>> SearchAsync(
        ReadOnlyMemory<float> queryVector,
        int k,
        CancellationToken cancellationToken = default)
    {
        if (k <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(k));
        }

        var results = _chunks
            .Select(chunk => new VectorSearchResult
            {
                Id = chunk.Id,
                Text = chunk.Text,
                Score = VectorSimilarity.CosineSimilarity(
                    queryVector.Span,
                    chunk.Embedding)
            })
            .OrderByDescending(result => result.Score)
            .Take(k)
            .ToList();

        return Task.FromResult<IReadOnlyList<VectorSearchResult>>(
            results);
    }

    public Task DeleteByDocumentIdAsync(
    string documentId,
    CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(documentId);

        _chunks.RemoveAll(
            chunk => chunk.DocumentId == documentId);

        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<VectorSearchResult>> SearchAsync(
     ReadOnlyMemory<float> queryVector,
     int k,
     VectorSearchFilter? filter = null,
     CancellationToken cancellationToken = default)
    {
        if (k <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(k));
        }

        var chunks = _chunks.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(filter?.DocumentId))
        {
            chunks = chunks.Where(
                x => x.DocumentId == filter.DocumentId);
        }

        if (!string.IsNullOrWhiteSpace(filter?.Source))
        {
            chunks = chunks.Where(
                x => x.Source == filter.Source);
        }

        if (!string.IsNullOrWhiteSpace(filter?.Version))
        {
            chunks = chunks.Where(
                x => x.Version == filter.Version);
        }

        var results = chunks
            .Select(chunk => new VectorSearchResult
            {
                Id = chunk.Id,
                Text = chunk.Text,
                Score = VectorSimilarity.CosineSimilarity(
                    queryVector.Span,
                    chunk.Embedding)
            })
            .OrderByDescending(result => result.Score)
            .Take(k)
            .ToList();

        return Task.FromResult<IReadOnlyList<VectorSearchResult>>(
            results);
    }

    public Task<IReadOnlyList<LexicalSearchResult>> LexicalSearchAsync(
    string query,
    int k,
    CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException(
            "Lexical search is only supported by PgVectorStore.");
    }
}

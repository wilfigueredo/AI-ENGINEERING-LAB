using AiEngineeringLab.Core.Models.Ingestion;
using AiEngineeringLab.Core.Models.Retrieval;

namespace AiEngineeringLab.Core.AI.VectorStore;

public interface IVectorStore
{
    Task SaveAsync(
        IEnumerable<IndexedChunk> chunks,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<IndexedChunk>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<VectorSearchResult>> SearchAsync(
        ReadOnlyMemory<float> queryVector,
        int k,
        CancellationToken cancellationToken = default);

    Task DeleteByDocumentIdAsync(
    string documentId,
    CancellationToken cancellationToken = default);
}

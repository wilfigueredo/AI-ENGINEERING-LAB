using AiEngineeringLab.Core.AI.Chunking;
using AiEngineeringLab.Core.AI.VectorStore;
using AiEngineeringLab.Core.Models.Chunking;
using AiEngineeringLab.Core.Models.Ingestion;
using Microsoft.Extensions.AI;

namespace AiEngineeringLab.Core.Services.Ingestion;

public sealed class IngestionService(
    IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator,
    IVectorStore vectorStore)
{
    public async Task<IReadOnlyList<IndexedChunk>> IngestAsync(
        DocumentInput document,
        int chunkSize = 500,
        int overlap = 50,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(document);

        var context = new ChunkingContext
        {
            DocumentId = document.Id,
            Title = document.Title,
            Source = document.Source,
            Version = document.Version
        };

        var chunks = FixedTextChunker.Chunk(
            document.Content,
            chunkSize,
            overlap,
            context);

        var indexedChunks =
            new List<IndexedChunk>();

        foreach (var chunk in chunks)
        {
            var embedding =
                await embeddingGenerator.GenerateAsync(
                    chunk.Text,
                    cancellationToken: cancellationToken);

            indexedChunks.Add(
                new IndexedChunk
                {
                    Id = chunk.Id,
                    DocumentId = chunk.DocumentId,
                    Title = chunk.Title,
                    Source = chunk.Source,
                    Version = chunk.Version,
                    ChunkIndex = chunk.Index,
                    Text = chunk.Text,
                    Embedding = embedding.Vector.ToArray()
                });
        }

        await vectorStore.DeleteByDocumentIdAsync(
     document.Id,
     cancellationToken);

        await vectorStore.SaveAsync(
            indexedChunks,
            cancellationToken);

        return indexedChunks;
    }
}

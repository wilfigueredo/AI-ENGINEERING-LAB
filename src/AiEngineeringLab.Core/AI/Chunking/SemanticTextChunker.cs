using AiEngineeringLab.Core.Models.Chunking;
using Microsoft.Extensions.AI;

namespace AiEngineeringLab.Core.AI.Chunking;

public sealed class SemanticTextChunker(
    IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator)
{
    public async Task<IReadOnlyList<TextChunk>> ChunkAsync(
        string text,
        double similarityThreshold = 0.75,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);

        var paragraphs = text
            .Split(
                "\n\n",
                StringSplitOptions.RemoveEmptyEntries |
                StringSplitOptions.TrimEntries);

        if (paragraphs.Length == 0)
        {
            return [];
        }

        var embeddings = new List<Embedding<float>>();

        foreach (var paragraph in paragraphs)
        {
            var embedding =
                await embeddingGenerator.GenerateAsync(
                    paragraph,
                    cancellationToken: cancellationToken);

            embeddings.Add(embedding);
        }

        var chunks = new List<TextChunk>();

        var currentChunk = paragraphs[0];
        var chunkIndex = 0;

        for (var i = 1; i < paragraphs.Length; i++)
        {
            var similarity =
                VectorSimilarity.CosineSimilarity(
                    embeddings[i - 1].Vector.Span,
                    embeddings[i].Vector.Span);

            if (similarity >= similarityThreshold)
            {
                currentChunk +=
                    "\n\n" +
                    paragraphs[i];

                continue;
            }

            chunks.Add(new TextChunk
            {
                Index = chunkIndex++,
                Text = currentChunk,
                Length = currentChunk.Length
            });

            currentChunk = paragraphs[i];
        }

        chunks.Add(new TextChunk
        {
            Index = chunkIndex,
            Text = currentChunk,
            Length = currentChunk.Length
        });

        return chunks;
    }
}

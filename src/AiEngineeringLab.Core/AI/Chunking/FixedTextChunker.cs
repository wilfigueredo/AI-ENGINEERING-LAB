using AiEngineeringLab.Core.Models.Chunking;

namespace AiEngineeringLab.Core.AI.Chunking;

public static class FixedTextChunker
{
    public static IReadOnlyList<TextChunk> Chunk(
     string text,
     int chunkSize,
     int overlap,
     ChunkingContext context)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);

        if (chunkSize <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(chunkSize),
                "Chunk size must be greater than zero.");
        }

        if (overlap < 0 || overlap >= chunkSize)
        {
            throw new ArgumentOutOfRangeException(
                nameof(overlap),
                "Overlap must be greater than or equal to zero and smaller than chunk size.");
        }

        var chunks = new List<TextChunk>();

        var step = chunkSize - overlap;
        var index = 0;

        for (var position = 0;
             position < text.Length;
             position += step)
        {
            var length = Math.Min(
                chunkSize,
                text.Length - position);

            var chunkText = text.Substring(
                position,
                length);

            chunks.Add(new TextChunk
            {
                DocumentId = context.DocumentId,
                Source = context.Source,
                Title = context.Title,
                Version = context.Version,

                Index = index++,
                Text = chunkText,
                StartPosition = position,
                Length = length
            });
        }

        return chunks;
    }
}

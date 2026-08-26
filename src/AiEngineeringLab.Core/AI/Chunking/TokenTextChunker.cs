using AiEngineeringLab.Core.Models.Chunking;
using Microsoft.ML.Tokenizers;

namespace AiEngineeringLab.Core.AI.Chunking;

public sealed class TokenTextChunker
{
    private readonly Tokenizer _tokenizer;

    public TokenTextChunker(Tokenizer tokenizer)
    {
        _tokenizer = tokenizer;
    }

    public IReadOnlyList<TextChunk> Chunk(
        string text,
        int chunkSize,
        int overlap = 0)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);

        if (chunkSize <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(chunkSize));
        }

        if (overlap < 0 || overlap >= chunkSize)
        {
            throw new ArgumentOutOfRangeException(
                nameof(overlap));
        }

        var tokenIds = _tokenizer.EncodeToIds(text);

        var chunks = new List<TextChunk>();

        var step = chunkSize - overlap;
        var index = 0;

        for (var position = 0;
             position < tokenIds.Count;
             position += step)
        {
            var length = Math.Min(
                chunkSize,
                tokenIds.Count - position);

            var slice = tokenIds
                .Skip(position)
                .Take(length)
                .ToArray();

            var chunkText = _tokenizer.Decode(slice);

            chunks.Add(new TextChunk
            {
                Index = index++,
                Text = chunkText,
                StartPosition = position,
                Length = length
            });
        }

        return chunks;
    }
}

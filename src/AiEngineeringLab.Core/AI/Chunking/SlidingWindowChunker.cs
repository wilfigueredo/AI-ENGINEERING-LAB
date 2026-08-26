using AiEngineeringLab.Core.Models.Chunking;
using Microsoft.ML.Tokenizers;

namespace AiEngineeringLab.Core.AI.Chunking;

public sealed class SlidingWindowChunker
{
    private readonly Tokenizer _tokenizer;

    public SlidingWindowChunker(
        Tokenizer tokenizer)
    {
        _tokenizer = tokenizer;
    }

    public IReadOnlyList<TextChunk> Chunk(
        string text,
        int windowSize,
        int step)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);

        if (windowSize <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(windowSize));
        }

        if (step <= 0 || step > windowSize)
        {
            throw new ArgumentOutOfRangeException(
                nameof(step),
                "Step must be greater than zero and smaller than or equal to window size.");
        }

        var tokenIds =
            _tokenizer.EncodeToIds(text);

        var chunks =
            new List<TextChunk>();

        var index = 0;

        for (var position = 0;
             position < tokenIds.Count;
             position += step)
        {
            var length = Math.Min(
                windowSize,
                tokenIds.Count - position);

            var slice = tokenIds
                .Skip(position)
                .Take(length)
                .ToArray();

            var chunkText =
                _tokenizer.Decode(slice);

            chunks.Add(
                new TextChunk
                {
                    Index = index++,
                    Text = chunkText,
                    StartPosition = position,
                    Length = length
                });

            if (position + length >= tokenIds.Count)
            {
                break;
            }
        }

        return chunks;
    }
}

using AiEngineeringLab.Core.Models.Chunking;

namespace AiEngineeringLab.Core.AI.Chunking;

public static class RecursiveTextChunker
{
    private static readonly string[] Separators =
    {
        "\n\n",
        "\n",
        ". ",
        " ",
        ""
    };

    public static IReadOnlyList<TextChunk> Chunk(
        string text,
        int chunkSize)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);

        if (chunkSize <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(chunkSize));
        }

        var pieces = SplitRecursive(
            text,
            chunkSize,
            separatorIndex: 0);

        var chunks = new List<TextChunk>();

        var position = 0;

        for (var i = 0; i < pieces.Count; i++)
        {
            var piece = pieces[i];

            chunks.Add(new TextChunk
            {
                Index = i,
                Text = piece,
                StartPosition = position,
                Length = piece.Length
            });

            position += piece.Length;
        }

        return chunks;
    }

    private static List<string> SplitRecursive(
        string text,
        int chunkSize,
        int separatorIndex)
    {
        if (text.Length <= chunkSize)
        {
            return [text];
        }

        if (separatorIndex >= Separators.Length)
        {
            return SplitHard(text, chunkSize);
        }

        var separator = Separators[separatorIndex];

        if (separator == string.Empty)
        {
            return SplitHard(text, chunkSize);
        }

        var parts = text.Split(
            separator,
            StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length <= 1)
        {
            return SplitRecursive(
                text,
                chunkSize,
                separatorIndex + 1);
        }

        var result = new List<string>();

        foreach (var part in parts)
        {
            var normalizedPart =
                part + separator;

            if (normalizedPart.Length <= chunkSize)
            {
                result.Add(normalizedPart);
                continue;
            }

            result.AddRange(
                SplitRecursive(
                    normalizedPart,
                    chunkSize,
                    separatorIndex + 1));
        }

        return MergeSmallChunks(
            result,
            chunkSize);
    }

    private static List<string> MergeSmallChunks(
        List<string> chunks,
        int chunkSize)
    {
        var result = new List<string>();

        var current = string.Empty;

        foreach (var chunk in chunks)
        {
            if (current.Length + chunk.Length <= chunkSize)
            {
                current += chunk;
                continue;
            }

            if (!string.IsNullOrEmpty(current))
            {
                result.Add(current);
            }

            current = chunk;
        }

        if (!string.IsNullOrEmpty(current))
        {
            result.Add(current);
        }

        return result;
    }

    private static List<string> SplitHard(
        string text,
        int chunkSize)
    {
        var result = new List<string>();

        for (var position = 0;
             position < text.Length;
             position += chunkSize)
        {
            var length = Math.Min(
                chunkSize,
                text.Length - position);

            result.Add(
                text.Substring(
                    position,
                    length));
        }

        return result;
    }
}

using AiEngineeringLab.Core.AI.Interfaces;
using OpenAI.Embeddings;

namespace AiEngineeringLab.Core.AI.Embeddings;

public sealed class OpenAiEmbeddingGenerator : IEmbeddingGenerator
{
    private readonly EmbeddingClient _client;

    public OpenAiEmbeddingGenerator(
        string apiKey,
        string model = "text-embedding-3-small")
    {
        _client = new EmbeddingClient(model, apiKey);
    }

    public async Task<float[]> GenerateAsync(
        string text,
        CancellationToken cancellationToken = default)
    {
        var embedding = await _client.GenerateEmbeddingAsync(
            text,
            cancellationToken: cancellationToken);

        return embedding.Value.ToFloats().ToArray();
    }
}

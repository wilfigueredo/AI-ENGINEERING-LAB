using AiEngineeringLab.Core.AI.Embeddings;

namespace AiEngineeringLab.UnitTests.AI;

public class OpenAiEmbeddingGeneratorTests
{
    [Fact]
    public async Task GenerateAsync_ShouldReturnEmbedding()
    {
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        Assert.False(string.IsNullOrWhiteSpace(apiKey));

        var generator = new OpenAiEmbeddingGenerator(apiKey);

        var embedding = await generator.GenerateAsync(
            "Como funciona o Semantic Kernel?");

        Assert.NotNull(embedding);
        Assert.NotEmpty(embedding);
    }
}

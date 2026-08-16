using AiEngineeringLab.Core.AI;
using AiEngineeringLab.Core.AI.Embeddings;
using AiEngineeringLab.Core.Models.Retrieval;

namespace AiEngineeringLab.UnitTests.AI;

public class VectorRetrieverSemanticTests
{
    [Fact]
    public async Task TopK_ShouldRankSemanticallyRelatedTextFirst()
    {
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        Assert.False(string.IsNullOrWhiteSpace(apiKey));

        var embeddingGenerator = new OpenAiEmbeddingGenerator(apiKey);

        var texts = new[]
        {
            new
            {
                Id = "A",
                Text = "Semantic Kernel organiza componentes de inteligência artificial em aplicações .NET."
            },
            new
            {
                Id = "B",
                Text = "O cachorro gosta de brincar no parque durante a tarde."
            },
            new
            {
                Id = "C",
                Text = "Embeddings representam significado semântico através de vetores numéricos."
            },
            new
            {
                Id = "D",
                Text = "Function calling permite que modelos de linguagem executem ferramentas."
            }
        };

        var items = new List<VectorSearchItem>();

        foreach (var item in texts)
        {
            var vector = await embeddingGenerator.GenerateAsync(item.Text);

            items.Add(new VectorSearchItem
            {
                Id = item.Id,
                Text = item.Text,
                Vector = vector
            });
        }

        var query = "Como funciona o Semantic Kernel?";

        var queryVector =
            await embeddingGenerator.GenerateAsync(query);

        var results = VectorRetriever.TopK(
            queryVector,
            items,
            4);

        foreach (var result in results)
        {
            Console.WriteLine(
                $"{result.Id} | {result.Score:F4} | {result.Text}");
        }

        Assert.Equal("A", results[0].Id);
    }
}

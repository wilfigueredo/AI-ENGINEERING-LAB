using AiEngineeringLab.Core.AI;
using AiEngineeringLab.Core.AI.Embeddings;
using AiEngineeringLab.Core.Models.Retrieval;
using AiEngineeringLab.Core.AI.Evaluation.Metrics;
using RetrievalMetrics = AiEngineeringLab.Core.AI.RetrievalMetrics;

namespace AiEngineeringLab.UnitTests.AI;

public class RetrievalMetricsTests
{
    [Fact]
    public void Precision_ShouldCalculateCorrectly()
    {
        var relevant = new[] { "A", "B", "C", "D" };

        var retrieved = new[] { "A", "B", "X" };

        var precision =
            RetrievalMetrics.Precision(retrieved, relevant);

        Assert.Equal(2.0 / 3.0, precision, precision: 5);
    }

    [Fact]
    public void Recall_ShouldCalculateCorrectly()
    {
        var relevant = new[] { "A", "B", "C", "D" };

        var retrieved = new[] { "A", "B", "X" };

        var recall =
            RetrievalMetrics.Recall(retrieved, relevant);

        Assert.Equal(0.5, recall);
    }

    [Fact]
    public async Task TopK_ShouldDemonstratePrecisionRecallTradeoff()
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

        var queryVector = await embeddingGenerator.GenerateAsync(
            "Como funcionam os recursos de inteligência artificial do Semantic Kernel?");

        // Ground truth:
        // Para este experimento, consideramos A, C e D relevantes.
        var relevantIds = new[] { "A", "C", "D" };

        for (var k = 1; k <= 4; k++)
        {
            var results = VectorRetriever.TopK(
                queryVector,
                items,
                k);

            var retrievedIds = results
                .Select(x => x.Id);

            var precision = RetrievalMetrics.Precision(
                retrievedIds,
                relevantIds);

            var recall = RetrievalMetrics.Recall(
                retrievedIds,
                relevantIds);

            Console.WriteLine(
                $"K={k} | Precision={precision:P2} | Recall={recall:P2}");
        }
    }

    [Fact]
    public void Calculate_ShouldCalculatePrecisionAndRecall()
    {
        var expected = new[]
        {
            "doc-a",
            "doc-b",
            "doc-c"
        };

        var retrieved = new[]
        {
            "doc-a",
            "doc-b",
            "doc-x",
            "doc-y"
        };

        var result =
            Core.AI.Evaluation.Metrics.RetrievalMetrics.Calculate(
                expected,
                retrieved);

        Assert.Equal(0.50, result.Precision, 2);
        Assert.Equal(0.67, result.Recall, 2);
        Assert.Equal(1.00, result.HitRate, 2);
        Assert.Equal(1.00, result.ReciprocalRank, 2);
    }

    [Fact]
    public void Calculate_ShouldCalculateReciprocalRank()
    {
        var expected = new[]
        {
        "doc-a",
        "doc-b"
    };

        var retrieved = new[]
        {
        "doc-x",
        "doc-y",
        "doc-b",
        "doc-a"
    };

        var result =
            Core.AI.Evaluation.Metrics.RetrievalMetrics.Calculate(
                expected,
                retrieved);

        Assert.Equal(0.50, result.Precision, 2);
        Assert.Equal(1.00, result.Recall, 2);
        Assert.Equal(1.00, result.HitRate, 2);
        Assert.Equal(0.33, result.ReciprocalRank, 2);
    }

    [Fact]
    public void Calculate_ShouldReturnZero_WhenNoRelevantDocumentIsRetrieved()
    {
        var expected = new[]
        {
        "doc-a",
        "doc-b"
    };

        var retrieved = new[]
        {
        "doc-x",
        "doc-y"
    };

        var result =
            Core.AI.Evaluation.Metrics.RetrievalMetrics.Calculate(
                expected,
                retrieved);

        Assert.Equal(0, result.Precision);
        Assert.Equal(0, result.Recall);
        Assert.Equal(0, result.HitRate);
        Assert.Equal(0, result.ReciprocalRank);
    }
}

using AiEngineeringLab.Core.AI;
using AiEngineeringLab.Core.Models.Retrieval;

namespace AiEngineeringLab.UnitTests.AI;

public sealed class VectorRetrieverTests
{
    [Fact]
    public void TopK_ShouldReturnMostSimilarItems()
    {
        float[] query = [1, 0];

        var items = new List<VectorSearchItem>
        {
            new()
            {
                Id = "A",
                Text = "Documento A",
                Vector = [1, 0]
            },
            new()
            {
                Id = "B",
                Text = "Documento B",
                Vector = [0.8f, 0.2f]
            },
            new()
            {
                Id = "C",
                Text = "Documento C",
                Vector = [0, 1]
            },
            new()
            {
                Id = "D",
                Text = "Documento D",
                Vector = [-1, 0]
            }
        };

        var results = VectorRetriever.TopK(
            query,
            items,
            k: 2);

        Assert.Equal(2, results.Count);

        Assert.Equal("A", results[0].Id);
        Assert.Equal("B", results[1].Id);

        Assert.True(
            results[0].Score >= results[1].Score);
    }
}

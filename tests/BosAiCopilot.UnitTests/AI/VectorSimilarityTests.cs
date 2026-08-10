using BosAiCopilot.Core.AI;

namespace BosAiCopilot.UnitTests.AI;

public sealed class VectorSimilarityTests
{
    [Fact]
    public void CosineSimilarity_IdenticalVectors_ShouldReturnOne()
    {
        float[] first = [1, 2, 3];
        float[] second = [1, 2, 3];

        var result = VectorSimilarity.CosineSimilarity(
            first,
            second);

        Assert.Equal(1d, result, precision: 10);
    }

    [Fact]
    public void CosineSimilarity_OrthogonalVectors_ShouldReturnZero()
    {
        float[] first = [1, 0];
        float[] second = [0, 1];

        var result = VectorSimilarity.CosineSimilarity(
            first,
            second);

        Assert.Equal(0d, result, precision: 10);
    }

    [Fact]
    public void CosineSimilarity_OppositeVectors_ShouldReturnMinusOne()
    {
        float[] first = [1, 0];
        float[] second = [-1, 0];

        var result = VectorSimilarity.CosineSimilarity(
            first,
            second);

        Assert.Equal(-1d, result, precision: 10);
    }

    [Fact]
    public void CosineSimilarity_DifferentDimensions_ShouldThrow()
    {
        float[] first = [1, 2];
        float[] second = [1, 2, 3];

        Assert.Throws<ArgumentException>(() =>
            VectorSimilarity.CosineSimilarity(
                first,
                second));
    }
}

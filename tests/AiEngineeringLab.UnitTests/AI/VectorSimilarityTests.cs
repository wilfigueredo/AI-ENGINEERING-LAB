using AiEngineeringLab.Core.AI;

namespace AiEngineeringLab.UnitTests.AI;

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

    [Fact]
    public void DotProduct_ShouldReturnExpectedValue()
    {
        float[] vectorA = [1, 2, 3];
        float[] vectorB = [4, 5, 6];

        var result = VectorSimilarity.DotProduct(vectorA, vectorB);

        Assert.Equal(32f, result);
    }

    [Fact]
    public void DotProduct_ShouldRepresentVectorOrientation()
    {
        float[] reference = [1, 0];

        float[] sameDirection = [1, 0];
        float[] perpendicular = [0, 1];
        float[] oppositeDirection = [-1, 0];

        var sameResult =
            VectorSimilarity.DotProduct(reference, sameDirection);

        var perpendicularResult =
            VectorSimilarity.DotProduct(reference, perpendicular);

        var oppositeResult =
            VectorSimilarity.DotProduct(reference, oppositeDirection);

        Assert.Equal(1f, sameResult);
        Assert.Equal(0f, perpendicularResult);
        Assert.Equal(-1f, oppositeResult);
    }

    [Fact]
    public void EuclideanDistance_ShouldReturnExpectedDistance()
    {
        float[] vectorA = [1, 2];
        float[] vectorB = [4, 6];

        var result =
            VectorSimilarity.EuclideanDistance(vectorA, vectorB);

        Assert.Equal(5f, result);
    }

    [Fact]
    public void EuclideanDistance_ShouldBeZeroForIdenticalVectors()
    {
        float[] vectorA = [1, 2, 3];
        float[] vectorB = [1, 2, 3];

        var result =
            VectorSimilarity.EuclideanDistance(vectorA, vectorB);

        Assert.Equal(0f, result);
    }

    [Fact]
    public void SimilarityMetrics_ShouldShowDifferentBehaviors()
    {
        float[] reference = [1, 0];

        float[] sameDirectionSmallMagnitude = [1, 0];
        float[] sameDirectionLargeMagnitude = [10, 0];
        float[] perpendicular = [0, 1];
        float[] opposite = [-1, 0];

        var cosineSameSmall =
            VectorSimilarity.CosineSimilarity(
                reference,
                sameDirectionSmallMagnitude);

        var cosineSameLarge =
            VectorSimilarity.CosineSimilarity(
                reference,
                sameDirectionLargeMagnitude);

        var dotSameSmall =
            VectorSimilarity.DotProduct(
                reference,
                sameDirectionSmallMagnitude);

        var dotSameLarge =
            VectorSimilarity.DotProduct(
                reference,
                sameDirectionLargeMagnitude);

        var euclideanSameSmall =
            VectorSimilarity.EuclideanDistance(
                reference,
                sameDirectionSmallMagnitude);

        var euclideanSameLarge =
            VectorSimilarity.EuclideanDistance(
                reference,
                sameDirectionLargeMagnitude);

        var cosinePerpendicular =
            VectorSimilarity.CosineSimilarity(
                reference,
                perpendicular);

        var dotPerpendicular =
            VectorSimilarity.DotProduct(
                reference,
                perpendicular);

        var euclideanPerpendicular =
            VectorSimilarity.EuclideanDistance(
                reference,
                perpendicular);

        var cosineOpposite =
            VectorSimilarity.CosineSimilarity(
                reference,
                opposite);

        var dotOpposite =
            VectorSimilarity.DotProduct(
                reference,
                opposite);

        var euclideanOpposite =
            VectorSimilarity.EuclideanDistance(
                reference,
                opposite);

        Assert.Equal(1f, cosineSameSmall);
        Assert.Equal(1f, cosineSameLarge);

        Assert.Equal(1f, dotSameSmall);
        Assert.Equal(10f, dotSameLarge);

        Assert.Equal(0f, euclideanSameSmall);
        Assert.Equal(9f, euclideanSameLarge);

        Assert.Equal(0f, cosinePerpendicular);
        Assert.Equal(0f, dotPerpendicular);
        Assert.Equal(MathF.Sqrt(2), euclideanPerpendicular);

        Assert.Equal(-1f, cosineOpposite);
        Assert.Equal(-1f, dotOpposite);
        Assert.Equal(2f, euclideanOpposite);
    }
}

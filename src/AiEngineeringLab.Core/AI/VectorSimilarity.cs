namespace AiEngineeringLab.Core.AI;

public static class VectorSimilarity
{
    public static double CosineSimilarity(
        ReadOnlySpan<float> first,
        ReadOnlySpan<float> second)
    {
        if (first.Length != second.Length)
        {
            throw new ArgumentException(
                "Os vetores devem possuir a mesma dimensão.");
        }

        double dotProduct = 0;
        double firstMagnitude = 0;
        double secondMagnitude = 0;

        for (var i = 0; i < first.Length; i++)
        {
            dotProduct += first[i] * second[i];
            firstMagnitude += first[i] * first[i];
            secondMagnitude += second[i] * second[i];
        }

        if (firstMagnitude == 0 ||
            secondMagnitude == 0)
        {
            return 0;
        }

        return dotProduct /
            (Math.Sqrt(firstMagnitude) *
             Math.Sqrt(secondMagnitude));
    }
}

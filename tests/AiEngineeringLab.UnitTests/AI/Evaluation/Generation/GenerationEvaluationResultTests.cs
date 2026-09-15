using AiEngineeringLab.Core.AI.Evaluation.Generation;

namespace AiEngineeringLab.UnitTests.AI.Evaluation.Generation;

public sealed class GenerationEvaluationResultTests
{
    [Fact]
    public void Result_ShouldStoreScores()
    {
        var result = new GenerationEvaluationResult(
            Correctness: 0.90,
            Relevance: 0.95,
            Completeness: 0.80,
            OverallScore: 0.88);

        Assert.Equal(0.90, result.Correctness);
        Assert.Equal(0.95, result.Relevance);
        Assert.Equal(0.80, result.Completeness);
        Assert.Equal(0.88, result.OverallScore);
    }
}

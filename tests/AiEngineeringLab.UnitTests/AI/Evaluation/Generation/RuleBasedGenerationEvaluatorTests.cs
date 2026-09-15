using AiEngineeringLab.Core.AI.Evaluation.Generation;

namespace AiEngineeringLab.UnitTests.AI.Evaluation.Generation;

public sealed class RuleBasedGenerationEvaluatorTests
{
    [Fact]
    public async Task EvaluateAsync_ShouldReturnHighScore_ForSimilarAnswer()
    {
        var evaluator =
            new RuleBasedGenerationEvaluator();

        var result =
            await evaluator.EvaluateAsync(
                question:
                    "What is Semantic Kernel?",
                expectedAnswer:
                    "Semantic Kernel is a framework for integrating artificial intelligence into applications.",
                generatedAnswer:
                    "Semantic Kernel is a framework used to integrate artificial intelligence into applications.");

        Assert.True(result.Correctness > 0.70);
        Assert.True(result.Completeness > 0.70);
        Assert.True(result.OverallScore > 0.50);
    }

    [Fact]
    public async Task EvaluateAsync_ShouldReturnLowScore_ForUnrelatedAnswer()
    {
        var evaluator =
            new RuleBasedGenerationEvaluator();

        var result =
            await evaluator.EvaluateAsync(
                question:
                    "What is Semantic Kernel?",
                expectedAnswer:
                    "Semantic Kernel is a framework for integrating artificial intelligence into applications.",
                generatedAnswer:
                    "The weather today is sunny.");

        Assert.True(result.Correctness < 0.30);
        Assert.True(result.Relevance < 0.30);
        Assert.True(result.OverallScore < 0.30);
    }

    [Fact]
    public async Task EvaluateAsync_ShouldHandleMissingExpectedAnswer()
    {
        var evaluator =
            new RuleBasedGenerationEvaluator();

        var result =
            await evaluator.EvaluateAsync(
                question:
                    "What is the annual revenue of the BOS Framework?",
                expectedAnswer: null,
                generatedAnswer:
                    "I could not find this information.");

        Assert.Equal(0, result.Correctness);
        Assert.Equal(0, result.Completeness);
    }
}

using AiEngineeringLab.Core.AI.Evaluation.Generation;
using AiEngineeringLab.Core.AI.Evaluation.Generation.Interface;

namespace AiEngineeringLab.UnitTests.AI.Evaluation.Generation;

public sealed class LlmGenerationEvaluatorTests
{
    [Fact]
    public async Task EvaluateAsync_ShouldReturnScoresFromJudge()
    {
        var judgeClient = new FakeGenerationJudgeClient(
            """
            {
              "correctness": 0.90,
              "relevance": 0.95,
              "completeness": 0.80,
              "overallScore": 0.88
            }
            """);

        var evaluator =
            new LlmGenerationEvaluator(judgeClient);

        var result =
            await evaluator.EvaluateAsync(
                question:
                    "What is Semantic Kernel?",
                expectedAnswer:
                    "Semantic Kernel is an AI orchestration framework.",
                generatedAnswer:
                    "Semantic Kernel helps orchestrate AI capabilities in applications.");

        Assert.Equal(0.90, result.Correctness);
        Assert.Equal(0.95, result.Relevance);
        Assert.Equal(0.80, result.Completeness);
        Assert.Equal(0.88, result.OverallScore);
    }

    [Fact]
    public async Task EvaluateAsync_ShouldRejectScoreOutsideValidRange()
    {
        var judgeClient = new FakeGenerationJudgeClient(
            """
            {
              "correctness": 1.20,
              "relevance": 0.90,
              "completeness": 0.80,
              "overallScore": 0.90
            }
            """);

        var evaluator =
            new LlmGenerationEvaluator(judgeClient);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => evaluator.EvaluateAsync(
                "What is Semantic Kernel?",
                "An AI orchestration framework.",
                "Semantic Kernel orchestrates AI."));
    }

    private sealed class FakeGenerationJudgeClient
        : IGenerationJudgeClient
    {
        private readonly string _response;

        public FakeGenerationJudgeClient(string response)
        {
            _response = response;
        }

        public Task<string> EvaluateAsync(
            string prompt,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_response);
        }
    }
}

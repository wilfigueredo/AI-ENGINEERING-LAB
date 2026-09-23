using AiEngineeringLab.Core.AI.Evaluation.Generation;
using AiEngineeringLab.Core.AI.Evaluation.Grounding;
using AiEngineeringLab.Core.AI.Evaluation.Harness;
using AiEngineeringLab.Core.AI.Evaluation.Models;

namespace AiEngineeringLab.UnitTests.AI.Evaluation.Harness;

public sealed class RagEvaluationRunnerTests
{
    [Fact]
    public async Task RunAsync_ShouldAggregateMultipleEvaluationCases()
    {
        var pipeline =
            new FakeRagEvaluationPipeline();

        var generationEvaluator =
            new FakeGenerationEvaluator();

        var groundingEvaluator =
            new FakeGroundingEvaluator();

        var harness =
            new RagEvaluationHarness(
                pipeline,
                generationEvaluator,
                groundingEvaluator);

        var runner =
            new RagEvaluationRunner(harness);

        var dataset =
            new RagEvaluationDataset(
            [
                new RagEvaluationCase(
                    Id: "case-001",
                    Question: "Question 1",
                    ExpectedAnswer: "Expected answer 1",
                    ExpectedDocumentIds:
                    [
                        "doc-a"
                    ]),

                new RagEvaluationCase(
                    Id: "case-002",
                    Question: "Question 2",
                    ExpectedAnswer: "Expected answer 2",
                    ExpectedDocumentIds:
                    [
                        "doc-b"
                    ])
            ]);

        var result =
            await runner.RunAsync(dataset);

        Assert.Equal(2, result.TotalCases);

        Assert.Equal(0.50, result.MeanPrecision, 2);
        Assert.Equal(1.00, result.MeanRecall, 2);
        Assert.Equal(1.00, result.HitRate, 2);
        Assert.Equal(1.00, result.Mrr, 2);

        Assert.Equal(0.80, result.MeanCorrectness, 2);
        Assert.Equal(0.90, result.MeanRelevance, 2);
        Assert.Equal(0.70, result.MeanCompleteness, 2);
        Assert.Equal(0.80, result.MeanGenerationScore, 2);

        Assert.Equal(0.75, result.MeanFaithfulnessScore, 2);
        Assert.Equal(0.50, result.GroundedAnswerRate, 2);
    }

    private sealed class FakeRagEvaluationPipeline
        : IRagEvaluationPipeline
    {
        public Task<RagPipelineResult> ExecuteAsync(
            string question,
            CancellationToken cancellationToken = default)
        {
            var relevantDocumentId =
                question == "Question 1"
                    ? "doc-a"
                    : "doc-b";

            return Task.FromResult(
                new RagPipelineResult(
                    RetrievedDocumentIds:
                    [
                        relevantDocumentId,
                        "doc-x"
                    ],
                    RetrievedContext:
                    [
                        $"Context for {question}"
                    ],
                    GeneratedAnswer:
                        $"Generated answer for {question}"));
        }
    }

    private sealed class FakeGenerationEvaluator
        : IGenerationEvaluator
    {
        public Task<GenerationEvaluationResult> EvaluateAsync(
            string question,
            string? expectedAnswer,
            string generatedAnswer,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(
                new GenerationEvaluationResult(
                    Correctness: 0.80,
                    Relevance: 0.90,
                    Completeness: 0.70,
                    OverallScore: 0.80));
        }
    }

    private sealed class FakeGroundingEvaluator
        : IGroundingEvaluator
    {
        public Task<GroundingEvaluationResult> EvaluateAsync(
            string question,
            IReadOnlyCollection<string> context,
            string generatedAnswer,
            CancellationToken cancellationToken = default)
        {
            var score =
                question == "Question 1"
                    ? 1.0
                    : 0.5;

            return Task.FromResult(
                new GroundingEvaluationResult(
                    FaithfulnessScore: score,
                    SupportedClaims: [],
                    UnsupportedClaims: []));
        }
    }
}

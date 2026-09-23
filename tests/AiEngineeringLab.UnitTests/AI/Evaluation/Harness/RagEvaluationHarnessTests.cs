using AiEngineeringLab.Core.AI.Evaluation.Generation;
using AiEngineeringLab.Core.AI.Evaluation.Grounding;
using AiEngineeringLab.Core.AI.Evaluation.Harness;
using AiEngineeringLab.Core.AI.Evaluation.Models;

namespace AiEngineeringLab.UnitTests.AI.Evaluation.Harness;

public sealed class RagEvaluationHarnessTests
{
    [Fact]
    public async Task EvaluateAsync_ShouldEvaluateCompleteRagExecution()
    {
        var pipeline =
            new FakeRagEvaluationPipeline(
                new RagPipelineResult(
                    RetrievedDocumentIds:
                    [
                        "doc-a",
                        "doc-x"
                    ],
                    RetrievedContext:
                    [
                        "Semantic Kernel is an SDK for AI orchestration."
                    ],
                    GeneratedAnswer:
                        "Semantic Kernel is an SDK for AI orchestration."));

        var generationEvaluator =
            new FakeGenerationEvaluator(
                new GenerationEvaluationResult(
                    Correctness: 0.90,
                    Relevance: 0.95,
                    Completeness: 0.85,
                    OverallScore: 0.90));

        var groundingEvaluator =
            new FakeGroundingEvaluator(
                new GroundingEvaluationResult(
                    FaithfulnessScore: 1.0,
                    SupportedClaims:
                    [
                        "Semantic Kernel is an SDK for AI orchestration."
                    ],
                    UnsupportedClaims: []));

        var harness =
            new RagEvaluationHarness(
                pipeline,
                generationEvaluator,
                groundingEvaluator);

        var evaluationCase =
            new RagEvaluationCase(
                Id: "case-001",
                Question: "What is Semantic Kernel?",
                ExpectedAnswer:
                    "Semantic Kernel is an SDK for AI orchestration.",
                ExpectedDocumentIds:
                    [
                        "doc-a"
                    ]);

        var result =
            await harness.EvaluateAsync(evaluationCase);

        Assert.Equal("case-001", result.CaseId);

        Assert.Equal(0.50, result.Retrieval.Precision, 2);
        Assert.Equal(1.00, result.Retrieval.Recall, 2);
        Assert.Equal(1.00, result.Retrieval.HitRate, 2);
        Assert.Equal(1.00, result.Retrieval.ReciprocalRank, 2);

        Assert.Equal(0.90, result.Generation.Correctness);
        Assert.Equal(0.95, result.Generation.Relevance);

        Assert.Equal(1.00, result.Grounding.FaithfulnessScore);
        Assert.Empty(result.Grounding.UnsupportedClaims);
    }

    private sealed class FakeRagEvaluationPipeline(
        RagPipelineResult result)
        : IRagEvaluationPipeline
    {
        public Task<RagPipelineResult> ExecuteAsync(
            string question,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(result);
        }
    }

    private sealed class FakeGenerationEvaluator(
        GenerationEvaluationResult result)
        : IGenerationEvaluator
    {
        public Task<GenerationEvaluationResult> EvaluateAsync(
            string question,
            string? expectedAnswer,
            string generatedAnswer,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(result);
        }
    }

    private sealed class FakeGroundingEvaluator(
        GroundingEvaluationResult result)
        : IGroundingEvaluator
    {
        public Task<GroundingEvaluationResult> EvaluateAsync(
            string question,
            IReadOnlyCollection<string> context,
            string generatedAnswer,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(result);
        }
    }
}

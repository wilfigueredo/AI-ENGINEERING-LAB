using AiEngineeringLab.Core.AI.Evaluation.Generation;
using Microsoft.Extensions.AI;
using OpenAI;

namespace AiEngineeringLab.UnitTests.AI.Evaluation.Generation;

public sealed class LlmGenerationEvaluatorLiveTests
{
    [Fact(Skip = "Requires live OpenAI API access.")]
    public async Task EvaluateAsync_ShouldEvaluateAnswerUsingRealModel()
    {
        var apiKey =
            Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        var modelId =
            Environment.GetEnvironmentVariable("OPENAI_MODEL");

        Assert.False(string.IsNullOrWhiteSpace(apiKey));
        Assert.False(string.IsNullOrWhiteSpace(modelId));

        IChatClient chatClient =
            new OpenAIClient(apiKey)
                .GetChatClient(modelId)
                .AsIChatClient();

        var evaluator =
            new LlmGenerationEvaluator(chatClient);

        var result =
            await evaluator.EvaluateAsync(
                question:
                    "What is Semantic Kernel?",
                expectedAnswer:
                    "Semantic Kernel is an SDK for integrating and orchestrating AI capabilities in applications.",
                generatedAnswer:
                    "Semantic Kernel helps developers orchestrate AI capabilities inside applications.");

        Assert.InRange(result.Correctness, 0, 1);
        Assert.InRange(result.Relevance, 0, 1);
        Assert.InRange(result.Completeness, 0, 1);
        Assert.InRange(result.OverallScore, 0, 1);
    }
}

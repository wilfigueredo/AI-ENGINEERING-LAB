using AiEngineeringLab.Core.AI.Evaluation.Grounding;
using Microsoft.Extensions.AI;

namespace AiEngineeringLab.UnitTests.AI.Evaluation.Grounding;

public sealed class LlmGroundingEvaluatorTests
{
    [Fact]
    public async Task EvaluateAsync_ShouldReturnGroundingEvaluation()
    {
        var chatClient = new FakeChatClient(
            """
            {
              "faithfulnessScore": 0.75,
              "supportedClaims": [
                "Semantic Kernel is an SDK.",
                "It integrates AI capabilities into applications."
              ],
              "unsupportedClaims": [
                "It was created in 2023."
              ]
            }
            """);

        var evaluator =
            new LlmGroundingEvaluator(chatClient);

        var result =
            await evaluator.EvaluateAsync(
                question:
                    "What is Semantic Kernel?",
                context:
                [
                    "Semantic Kernel is an SDK for integrating AI capabilities into applications."
                ],
                generatedAnswer:
                    "Semantic Kernel is an SDK created in 2023 for integrating AI capabilities into applications.");

        Assert.Equal(0.75, result.FaithfulnessScore);

        Assert.Contains(
            "Semantic Kernel is an SDK.",
            result.SupportedClaims);

        Assert.Contains(
            "It was created in 2023.",
            result.UnsupportedClaims);
    }

    [Fact]
    public async Task EvaluateAsync_ShouldRejectInvalidFaithfulnessScore()
    {
        var chatClient = new FakeChatClient(
            """
            {
              "faithfulnessScore": 1.20,
              "supportedClaims": [],
              "unsupportedClaims": []
            }
            """);

        var evaluator =
            new LlmGroundingEvaluator(chatClient);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => evaluator.EvaluateAsync(
                "Question",
                ["Context"],
                "Generated answer"));
    }

    private sealed class FakeChatClient : IChatClient
    {
        private readonly string _response;

        public FakeChatClient(string response)
        {
            _response = response;
        }

        public ChatClientMetadata Metadata =>
            new(
                providerName: "Fake",
                providerUri: null,
                defaultModelId: "fake-model");

        public void Dispose()
        {
        }

        public Task<ChatResponse> GetResponseAsync(
            IEnumerable<ChatMessage> messages,
            ChatOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(
                new ChatResponse(
                    new ChatMessage(
                        ChatRole.Assistant,
                        _response)));
        }

        public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
            IEnumerable<ChatMessage> messages,
            ChatOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public object? GetService(
            Type serviceType,
            object? serviceKey = null)
        {
            return null;
        }
    }
}

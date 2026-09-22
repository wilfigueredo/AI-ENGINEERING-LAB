using System.Text.Json;
using Microsoft.Extensions.AI;

namespace AiEngineeringLab.Core.AI.Evaluation.Grounding;

public sealed class LlmGroundingEvaluator(
    IChatClient chatClient)
    : IGroundingEvaluator
{
    private static readonly JsonSerializerOptions SerializerOptions =
        new(JsonSerializerDefaults.Web);

    public async Task<GroundingEvaluationResult> EvaluateAsync(
        string question,
        IReadOnlyCollection<string> context,
        string generatedAnswer,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(question);
        ArgumentNullException.ThrowIfNull(context);
        ArgumentException.ThrowIfNullOrWhiteSpace(generatedAnswer);

        var contextText = string.Join(
            Environment.NewLine + Environment.NewLine,
            context);

        var messages = new List<ChatMessage>
        {
            new(
                ChatRole.System,
                """
                You are evaluating whether an AI-generated answer is grounded
                in the provided context.

                Analyze the factual claims in the generated answer.

                A claim is supported only when it is directly supported
                by the provided context.

                Do not use outside knowledge.

                Return JSON only using exactly this schema:

                {
                  "faithfulnessScore": 0.0,
                  "supportedClaims": [],
                  "unsupportedClaims": []
                }

                faithfulnessScore must be between 0 and 1.

                1.0 means all factual claims are supported by the context.
                0.0 means none of the factual claims are supported.
                """),

            new(
                ChatRole.User,
                $"""
                Question:
                {question}

                Context:
                {contextText}

                Generated answer:
                {generatedAnswer}
                """)
        };

        var response =
            await chatClient.GetResponseAsync(
                messages,
                new ChatOptions
                {
                    Temperature = 0.0f
                },
                cancellationToken);

        var evaluation =
            JsonSerializer.Deserialize<GroundingEvaluationResponse>(
                response.Text,
                SerializerOptions)
            ?? throw new InvalidOperationException(
                "The LLM judge returned an invalid grounding response.");

        if (evaluation.FaithfulnessScore is < 0 or > 1)
        {
            throw new InvalidOperationException(
                "The LLM judge returned a faithfulness score outside 0..1.");
        }

        return new GroundingEvaluationResult(
            FaithfulnessScore: evaluation.FaithfulnessScore,
            SupportedClaims: evaluation.SupportedClaims ?? [],
            UnsupportedClaims: evaluation.UnsupportedClaims ?? []);
    }

    private sealed record GroundingEvaluationResponse(
        double FaithfulnessScore,
        IReadOnlyCollection<string>? SupportedClaims,
        IReadOnlyCollection<string>? UnsupportedClaims);
}

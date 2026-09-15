using System.Text.Json;
using Microsoft.Extensions.AI;

namespace AiEngineeringLab.Core.AI.Evaluation.Generation;

public sealed class LlmGenerationEvaluator(
    IChatClient chatClient)
    : IGenerationEvaluator
{
    private static readonly JsonSerializerOptions SerializerOptions =
        new(JsonSerializerDefaults.Web);

    public async Task<GenerationEvaluationResult> EvaluateAsync(
        string question,
        string? expectedAnswer,
        string generatedAnswer,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(question);
        ArgumentException.ThrowIfNullOrWhiteSpace(generatedAnswer);

        var messages = new List<ChatMessage>
        {
            new(
                ChatRole.System,
                """
                You are an evaluator of AI-generated answers.

                Evaluate the generated answer using scores from 0 to 1.

                Criteria:
                - correctness
                - relevance
                - completeness
                - overallScore

                Return JSON only with exactly this schema:

                {
                  "correctness": 0.0,
                  "relevance": 0.0,
                  "completeness": 0.0,
                  "overallScore": 0.0
                }
                """),

            new(
                ChatRole.User,
                $"""
                Question:
                {question}

                Expected answer:
                {expectedAnswer ?? "No reference answer was provided."}

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
            JsonSerializer.Deserialize<LlmGenerationEvaluationResponse>(
                response.Text,
                SerializerOptions)
            ?? throw new InvalidOperationException(
                "The LLM judge returned an invalid evaluation response.");

        ValidateScore(evaluation.Correctness);
        ValidateScore(evaluation.Relevance);
        ValidateScore(evaluation.Completeness);
        ValidateScore(evaluation.OverallScore);

        return new GenerationEvaluationResult(
            evaluation.Correctness,
            evaluation.Relevance,
            evaluation.Completeness,
            evaluation.OverallScore);
    }

    private static void ValidateScore(double score)
    {
        if (score is < 0 or > 1)
        {
            throw new InvalidOperationException(
                "The LLM judge returned a score outside the valid range 0..1.");
        }
    }
}

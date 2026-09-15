namespace AiEngineeringLab.Core.AI.Evaluation.Generation;

public sealed class RuleBasedGenerationEvaluator : IGenerationEvaluator
{
    public Task<GenerationEvaluationResult> EvaluateAsync(
        string question,
        string? expectedAnswer,
        string generatedAnswer,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(question);
        ArgumentException.ThrowIfNullOrWhiteSpace(generatedAnswer);

        var relevance = CalculateOverlap(
            question,
            generatedAnswer);

        var correctness =
            expectedAnswer is null
                ? 0
                : CalculateOverlap(
                    expectedAnswer,
                    generatedAnswer);

        var completeness =
            expectedAnswer is null
                ? 0
                : CalculateCoverage(
                    expectedAnswer,
                    generatedAnswer);

        var overallScore =
            (correctness + relevance + completeness) / 3.0;

        return Task.FromResult(
            new GenerationEvaluationResult(
                Correctness: correctness,
                Relevance: relevance,
                Completeness: completeness,
                OverallScore: overallScore));
    }

    private static double CalculateOverlap(
        string source,
        string target)
    {
        var sourceTokens = Tokenize(source);
        var targetTokens = Tokenize(target);

        if (sourceTokens.Count == 0)
            return 0;

        var commonTokens =
            sourceTokens.Count(targetTokens.Contains);

        return Math.Min(
            1.0,
            (double)commonTokens / sourceTokens.Count);
    }

    private static double CalculateCoverage(
        string expectedAnswer,
        string generatedAnswer)
    {
        return CalculateOverlap(
            expectedAnswer,
            generatedAnswer);
    }

    private static HashSet<string> Tokenize(string text)
    {
        return text
            .ToLowerInvariant()
            .Split(
                [' ', '.', ',', ';', ':', '!', '?', '\r', '\n', '\t'],
                StringSplitOptions.RemoveEmptyEntries)
            .ToHashSet();
    }
}

namespace AiEngineeringLab.Core.AI.Evaluation.Models;

public sealed record RagEvaluationCase(
    string Id,
    string Question,
    string? ExpectedAnswer,
    IReadOnlyCollection<string> ExpectedDocumentIds,
    bool AnswerExpected = true);

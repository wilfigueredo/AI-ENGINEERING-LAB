namespace AiEngineeringLab.Core.Models.Retrieval;

public sealed class QueryExpansionResult
{
    public required string OriginalQuery { get; init; }

    public required IReadOnlyList<string> ExpandedTerms { get; init; }

    public required string ExpandedQuery { get; init; }
}

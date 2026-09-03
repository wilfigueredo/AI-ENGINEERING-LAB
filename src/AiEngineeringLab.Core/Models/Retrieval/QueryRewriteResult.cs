namespace AiEngineeringLab.Core.Models.Retrieval;

public sealed class QueryRewriteResult
{
    public required string OriginalQuery { get; init; }
    public required string RewrittenQuery { get; init; }
}

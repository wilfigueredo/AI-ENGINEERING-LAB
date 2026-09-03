namespace AiEngineeringLab.Core.AI.Evaluation.Models;

public sealed class RagEvaluationDataset
{
    public IReadOnlyCollection<RagEvaluationCase> Cases { get; }

    public RagEvaluationDataset(IEnumerable<RagEvaluationCase> cases)
    {
        ArgumentNullException.ThrowIfNull(cases);

        Cases = cases.ToArray();
    }
}

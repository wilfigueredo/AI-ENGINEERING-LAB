namespace AiEngineeringLab.Core.Models.Chunking;

public sealed class SemanticBoundary
{
    public int LeftIndex { get; init; }

    public int RightIndex { get; init; }

    public double Similarity { get; init; }

    public bool Split { get; init; }
}

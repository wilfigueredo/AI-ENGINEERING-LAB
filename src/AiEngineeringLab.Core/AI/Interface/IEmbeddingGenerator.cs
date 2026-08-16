namespace AiEngineeringLab.Core.AI.Interfaces;

public interface IEmbeddingGenerator
{
    Task<float[]> GenerateAsync(
        string text,
        CancellationToken cancellationToken = default);
}

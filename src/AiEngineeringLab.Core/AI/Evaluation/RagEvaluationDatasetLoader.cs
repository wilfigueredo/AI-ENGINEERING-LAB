using System.Text.Json;
using AiEngineeringLab.Core.AI.Evaluation.Models;

namespace AiEngineeringLab.Core.AI.Evaluation;

public sealed class RagEvaluationDatasetLoader
{
    private static readonly JsonSerializerOptions SerializerOptions =
        new(JsonSerializerDefaults.Web);

    public async Task<RagEvaluationDataset> LoadAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        await using var stream = File.OpenRead(path);

        var cases =
            await JsonSerializer.DeserializeAsync<List<RagEvaluationCase>>(
                stream,
                SerializerOptions,
                cancellationToken);

        return new RagEvaluationDataset(cases ?? []);
    }
}

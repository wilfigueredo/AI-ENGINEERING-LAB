using System.ComponentModel.DataAnnotations;

namespace BosAiCopilot.Core.Options;

public sealed class AiOptions
{
    public const string SectionName = "AI";

    [Required]
    public string Provider { get; init; } = string.Empty;

    [Required]
    public string ModelId { get; init; } = string.Empty;

    [Required]
    public string ApiKey { get; init; } = string.Empty;
}

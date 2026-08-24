using System.Text.Json.Serialization;

namespace AiEngineeringLab.Core.Models.Chat;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MessageCategory
{
    Technical,
    Commercial,
    Support,
    Other
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MessagePriority
{
    Low,
    Medium,
    High
}

public sealed class MessageAnalysis
{
    public required MessageCategory Category { get; init; }

    public required MessagePriority Priority { get; init; }

    public required string Summary { get; init; }
}

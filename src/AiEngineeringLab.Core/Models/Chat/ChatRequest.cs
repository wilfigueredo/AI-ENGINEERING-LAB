using System.ComponentModel.DataAnnotations;

namespace AiEngineeringLab.Core.Models.Chat;

public sealed class ChatRequest
{
    [Required]
    public string ConversationId { get; init; } = string.Empty;

    [Required]
    public string Message { get; init; } = string.Empty;
}

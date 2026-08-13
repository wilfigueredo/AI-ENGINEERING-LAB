using Microsoft.Extensions.AI;

namespace AiEngineeringLab.Core.Services.Conversations;

public sealed class ConversationState
{
    public List<ChatMessage> Messages { get; } = [];

    public SemaphoreSlim Gate { get; } = new(1, 1);
}

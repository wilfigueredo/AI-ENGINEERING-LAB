using System.Collections.Concurrent;

namespace AiEngineeringLab.Core.Services.Conversations;

public sealed class ConversationHistoryService
{
    private readonly ConcurrentDictionary<string, ConversationState> _conversations =
        new(StringComparer.OrdinalIgnoreCase);

    public ConversationState GetOrCreate(string conversationId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(conversationId);

        return _conversations.GetOrAdd(
            conversationId,
            static _ => new ConversationState());
    }

    public bool Clear(string conversationId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(conversationId);

        return _conversations.TryRemove(conversationId, out _);
    }
}

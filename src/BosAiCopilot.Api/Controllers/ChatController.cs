using System.Text;
using System.Text.Json;
using BosAiCopilot.Core.Models.Chat;
using BosAiCopilot.Core.Services.Conversations;
using BosAiCopilot.Plugins;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;

namespace BosAiCopilot.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ChatController(
    IChatClient chatClient,
    ConversationHistoryService conversationHistory,
    ILogger<ChatController> logger,
    IServiceProvider serviceProvider) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<ActionResult> SendMessageAsync(
        ChatRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ConversationId))
        {
            return BadRequest(new
            {
                error = "O identificador da conversa é obrigatório."
            });
        }

        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest(new
            {
                error = "A mensagem é obrigatória."
            });
        }

        var conversation = conversationHistory.GetOrCreate(
            request.ConversationId);

        await conversation.Gate.WaitAsync(cancellationToken);

        try
        {
            conversation.Messages.Add(
                new ChatMessage(
                    ChatRole.User,
                    request.Message));

            var chatOptions = new ChatOptions
            {
                Tools = AiTools.Create(serviceProvider)
            };

            var response = await chatClient.GetResponseAsync(
                conversation.Messages,
                chatOptions,
                cancellationToken);

            var responseText = response.Text ?? string.Empty;

            conversation.Messages.Add(
                new ChatMessage(
                    ChatRole.Assistant,
                    responseText));

            return Ok(new ChatResult
            {
                ConversationId = request.ConversationId,
                Response = responseText
            });
        }
        finally
        {
            conversation.Gate.Release();
        }
    }

    [HttpDelete("{conversationId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult ClearConversation(
        string conversationId)
    {
        conversationHistory.Clear(conversationId);

        return NoContent();
    }

    [HttpPost("stream")]
    [Produces("text/event-stream")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task StreamMessageAsync(
        ChatRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ConversationId))
        {
            Response.StatusCode =
                StatusCodes.Status400BadRequest;

            await Response.WriteAsJsonAsync(
                new
                {
                    error = "O identificador da conversa é obrigatório."
                },
                cancellationToken);

            return;
        }

        if (string.IsNullOrWhiteSpace(request.Message))
        {
            Response.StatusCode =
                StatusCodes.Status400BadRequest;

            await Response.WriteAsJsonAsync(
                new
                {
                    error = "A mensagem é obrigatória."
                },
                cancellationToken);

            return;
        }

        var conversation = conversationHistory.GetOrCreate(
            request.ConversationId);

        var lockAcquired = false;
        var originalMessageCount = 0;

        try
        {
            await conversation.Gate.WaitAsync(
                cancellationToken);

            lockAcquired = true;

            originalMessageCount =
                conversation.Messages.Count;

            conversation.Messages.Add(
                new ChatMessage(
                    ChatRole.User,
                    request.Message));

            Response.StatusCode =
                StatusCodes.Status200OK;

            Response.ContentType =
                "text/event-stream; charset=utf-8";

            Response.Headers.CacheControl =
                "no-cache";

            Response.Headers.Append(
                "X-Accel-Buffering",
                "no");

            var chatOptions = new ChatOptions
            {
                Tools = AiTools.Create(serviceProvider)
            };

            var responseBuilder =
                new StringBuilder();

            await foreach (
                var update
                in chatClient.GetStreamingResponseAsync(
                    conversation.Messages,
                    chatOptions,
                    cancellationToken))
            {
                if (string.IsNullOrEmpty(update.Text))
                {
                    continue;
                }

                responseBuilder.Append(
                    update.Text);

                await WriteSseEventAsync(
                    "chunk",
                    new
                    {
                        content = update.Text
                    },
                    cancellationToken);
            }

            var completeResponse =
                responseBuilder.ToString();

            conversation.Messages.Add(
                new ChatMessage(
                    ChatRole.Assistant,
                    completeResponse));

            await WriteSseEventAsync(
                "completed",
                new
                {
                    conversationId =
                        request.ConversationId
                },
                cancellationToken);
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
            RollbackConversation(
                conversation,
                originalMessageCount);

            logger.LogInformation(
                "O streaming da conversa {ConversationId} foi cancelado.",
                request.ConversationId);
        }
        catch (Exception exception)
        {
            RollbackConversation(
                conversation,
                originalMessageCount);

            logger.LogError(
                exception,
                "Falha durante o streaming da conversa {ConversationId}.",
                request.ConversationId);

            if (!Response.HasStarted)
            {
                Response.StatusCode =
                    StatusCodes.Status502BadGateway;

                await Response.WriteAsJsonAsync(
                    new
                    {
                        error =
                            "Não foi possível obter uma resposta do modelo."
                    },
                    CancellationToken.None);

                return;
            }

            await WriteSseEventAsync(
                "error",
                new
                {
                    message =
                        "O streaming foi interrompido por um erro."
                },
                CancellationToken.None);
        }
        finally
        {
            if (lockAcquired)
            {
                conversation.Gate.Release();
            }
        }
    }

    private async Task WriteSseEventAsync<T>(
        string eventName,
        T data,
        CancellationToken cancellationToken)
    {
        var json =
            JsonSerializer.Serialize(data);

        await Response.WriteAsync(
            $"event: {eventName}\n",
            cancellationToken);

        await Response.WriteAsync(
            $"data: {json}\n\n",
            cancellationToken);

        await Response.Body.FlushAsync(
            cancellationToken);
    }

    private static void RollbackConversation(
        ConversationState conversation,
        int originalMessageCount)
    {
        if (originalMessageCount < 0 ||
            conversation.Messages.Count <=
            originalMessageCount)
        {
            return;
        }

        conversation.Messages.RemoveRange(
            originalMessageCount,
            conversation.Messages.Count -
            originalMessageCount);
    }
}

using System.Text;
using System.Text.Json;
using AiEngineeringLab.Core.Models.Chat;
using AiEngineeringLab.Core.Models.Embedding;
using AiEngineeringLab.Core.Services.Conversations;
using AiEngineeringLab.Plugins;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel;
using AiEngineeringLab.Core.AI;

namespace AiEngineeringLab.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ChatController(
    IChatClient chatClient,
    ConversationHistoryService conversationHistory,    
    ILogger<ChatController> logger,
    AiTools aiTools,
    IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator,
    Kernel kernel)
    : ControllerBase
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
                Tools = aiTools.Create()
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
                Tools = aiTools.Create()
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

    [HttpGet("kernel")]
    public IActionResult GetKernelInfo()
    {

        if (kernel is null)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    error = "Kernel não foi inicializado."
                });
        }

        return Ok(new
        {
            kernelCreated = kernel is not null,
            pluginCount = kernel!.Plugins.Count,
            plugins = kernel!.Plugins.Select(plugin => new
            {
                plugin.Name,
                functions = plugin.Select(function =>
                    function.Name)
            })
        });
    }

    [HttpPost("kernel/count-words")]
    public async Task<IActionResult> CountWordsWithKernelAsync(
    [FromBody] string text,
    CancellationToken cancellationToken)
    {
        var arguments = new KernelArguments
        {
            ["text"] = text
        };

        var result = await kernel.InvokeAsync(
            pluginName: "Text",
            functionName: "count_words",
            arguments: arguments,
            cancellationToken: cancellationToken);

        return Ok(new
        {
            input = text,
            wordCount = result.GetValue<int>()
        });
    }

    [HttpPost("kernel/upper-case")]
    public async Task<IActionResult> ToUpperCaseWithKernelAsync(
    [FromBody] string text,
    CancellationToken cancellationToken)
    {
        var arguments = new KernelArguments
        {
            ["text"] = text
        };

        var result = await kernel.InvokeAsync(
            pluginName: "Text",
            functionName: "to_upper_case",
            arguments: arguments,
            cancellationToken: cancellationToken);

        return Ok(new
        {
            input = text,
            result = result.GetValue<string>()
        });
    }

    [HttpPost("kernel/chat")]
    public async Task<IActionResult> ChatWithKernelAsync(
    [FromBody] string message,
    CancellationToken cancellationToken)
    {
        var settings = new PromptExecutionSettings
        {
            FunctionChoiceBehavior =
                FunctionChoiceBehavior.Auto()
        };

        var result = await kernel.InvokePromptAsync(
            message,
            new KernelArguments(settings),
            cancellationToken: cancellationToken);

        return Ok(new
        {
            message,
            response = result.ToString()
        });
    }

    [HttpPost("embedding")]
    public async Task<IActionResult> GenerateEmbeddingAsync(
    [FromBody] string text,
    CancellationToken cancellationToken)
    {
        var embedding =
            await embeddingGenerator.GenerateAsync(
                text,
                cancellationToken: cancellationToken);

        return Ok(new
        {
            text,
            dimensions = embedding.Vector.Length,
            preview = embedding.Vector
                .Span[..Math.Min(10, embedding.Vector.Length)]
                .ToArray()
        });
    }

    [HttpPost("embedding/similarity")]
    public async Task<IActionResult> CompareEmbeddingsAsync(
    [FromBody] EmbeddingComparisonRequest request,
    CancellationToken cancellationToken)
    {
        var firstEmbedding =
            await embeddingGenerator.GenerateAsync(
                request.FirstText,
                cancellationToken: cancellationToken);

        var secondEmbedding =
            await embeddingGenerator.GenerateAsync(
                request.SecondText,
                cancellationToken: cancellationToken);

        var similarity = VectorSimilarity.CosineSimilarity(
            firstEmbedding.Vector.Span,
            secondEmbedding.Vector.Span);

        return Ok(new
        {
            request.FirstText,
            request.SecondText,
            similarity
        });
    }

    private static double CosineSimilarity(
    ReadOnlySpan<float> first,
    ReadOnlySpan<float> second)
    {
        if (first.Length != second.Length)
        {
            throw new ArgumentException(
                "Os vetores devem possuir a mesma dimensão.");
        }

        double dotProduct = 0;
        double firstMagnitude = 0;
        double secondMagnitude = 0;

        for (var i = 0; i < first.Length; i++)
        {
            dotProduct += first[i] * second[i];

            firstMagnitude += first[i] * first[i];

            secondMagnitude += second[i] * second[i];
        }

        if (firstMagnitude == 0 ||
            secondMagnitude == 0)
        {
            return 0;
        }

        return dotProduct /
            (Math.Sqrt(firstMagnitude) *
             Math.Sqrt(secondMagnitude));
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

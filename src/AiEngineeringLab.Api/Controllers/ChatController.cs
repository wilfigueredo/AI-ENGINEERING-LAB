using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using AiEngineeringLab.Core.AI;
using AiEngineeringLab.Core.AI.Chunking;
using AiEngineeringLab.Core.AI.Interface;
using AiEngineeringLab.Core.AI.Retrieval;
using AiEngineeringLab.Core.Models.Chat;
using AiEngineeringLab.Core.Models.Chunking;
using AiEngineeringLab.Core.Models.Embedding;
using AiEngineeringLab.Core.Models.Ingestion;
using AiEngineeringLab.Core.Models.Retrieval;
using AiEngineeringLab.Core.Services.Conversations;
using AiEngineeringLab.Core.Services.Ingestion;
using AiEngineeringLab.Plugins;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using Microsoft.ML.Tokenizers;
using Microsoft.SemanticKernel;

namespace AiEngineeringLab.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ChatController(
    IChatClient chatClient,
    ConversationHistoryService conversationHistory,
    ILogger<ChatController> logger,
    AiTools aiTools,
    IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator,
    IngestionService ingestionService,
    IVectorStore vectorStore,
    QueryExpansionService queryExpansionService,
    QueryRewriteService queryRewriteService,
    RerankingService rerankingService,
    ContextCompressionService contextCompressionService,
    Kernel kernel)
    : ControllerBase
{

    [HttpPost("retrieval/context-compression")]
    public async Task<IActionResult> CompressContextAsync(
    [FromBody] string query,
    [FromQuery] int k = 3,
    CancellationToken cancellationToken = default)
    {
        var queryEmbedding =
            await embeddingGenerator.GenerateAsync(
                query,
                cancellationToken: cancellationToken);

        var retrieved =
            await vectorStore.SearchAsync(
                queryEmbedding.Vector,
                k,
                cancellationToken: cancellationToken);

        var compressed =
            await contextCompressionService.CompressAsync(
                query,
                retrieved,
                cancellationToken);

        return Ok(new
        {
            query,
            k,
            retrieved,
            compressed
        });
    }

    [HttpPost("retrieval/mmr")]
    public async Task<IActionResult> MmrSearchAsync(
    [FromBody] string query,
    [FromQuery] int candidates = 10,
    [FromQuery] int k = 3,
    [FromQuery] double lambda = 0.7,
    CancellationToken cancellationToken = default)
    {
        var queryEmbedding =
            await embeddingGenerator.GenerateAsync(
                query,
                cancellationToken: cancellationToken);

        var allChunks =
            await vectorStore.GetAllAsync(
                cancellationToken);

        var topCandidates = allChunks
            .Select(chunk => new
            {
                Chunk = chunk,
                Score = VectorSimilarity.CosineSimilarity(
                    queryEmbedding.Vector.Span,
                    chunk.Embedding)
            })
            .OrderByDescending(x => x.Score)
            .Take(candidates)
            .Select(x => x.Chunk)
            .ToList();

        var mmrResults =
            MmrRetriever.Select(
                queryEmbedding.Vector,
                topCandidates,
                k,
                lambda);

        return Ok(new
        {
            query,
            candidates,
            k,
            lambda,
            mmrResults
        });
    }

    [HttpPost("retrieval/rerank")]
    public async Task<IActionResult> RerankAsync(
    [FromBody] string query,
    [FromQuery] int candidates = 5,
    [FromQuery] int k = 3,
    CancellationToken cancellationToken = default)
    {
        var queryEmbedding =
            await embeddingGenerator.GenerateAsync(
                query,
                cancellationToken: cancellationToken);

        var retrievalResults =
            await vectorStore.SearchAsync(
                queryEmbedding.Vector,
                candidates,
                cancellationToken: cancellationToken);

        var rerankedResults =
            await rerankingService.RerankAsync(
                query,
                retrievalResults,
                k,
                cancellationToken);

        return Ok(new
        {
            query,
            candidates,
            k,
            retrievalResults,
            rerankedResults
        });
    }

    [HttpPost("retrieval/query-rewrite/compare")]
    public async Task<IActionResult> CompareQueryRewriteAsync(
    [FromBody] string query,
    [FromQuery] int k = 3,
    CancellationToken cancellationToken = default)
    {
        var rewrite =
            await queryRewriteService.RewriteAsync(
                query,
                cancellationToken);

        var originalEmbedding =
            await embeddingGenerator.GenerateAsync(
                query,
                cancellationToken: cancellationToken);

        var rewrittenEmbedding =
            await embeddingGenerator.GenerateAsync(
                rewrite.RewrittenQuery,
                cancellationToken: cancellationToken);

        var originalResults =
            await vectorStore.SearchAsync(
                originalEmbedding.Vector,
                k,
                cancellationToken: cancellationToken);

        var rewrittenResults =
            await vectorStore.SearchAsync(
                rewrittenEmbedding.Vector,
                k,
                cancellationToken: cancellationToken);

        return Ok(new
        {
            originalQuery = query,
            rewrittenQuery = rewrite.RewrittenQuery,
            originalResults,
            rewrittenResults
        });
    }

    [HttpPost("retrieval/query-rewrite")]
    public async Task<IActionResult> RewriteQueryAsync(
    [FromBody] string query,
    CancellationToken cancellationToken = default)
    {
        var result =
            await queryRewriteService.RewriteAsync(
                query,
                cancellationToken);

        return Ok(result);
    }

    [HttpPost("retrieval/query-expansion/compare")]
    public async Task<IActionResult> CompareQueryExpansionAsync(
    [FromBody] string query,
    [FromQuery] int k = 3,
    CancellationToken cancellationToken = default)
    {
        var expansion =
            await queryExpansionService.ExpandAsync(
                query,
                cancellationToken);

        var originalEmbedding =
            await embeddingGenerator.GenerateAsync(
                query,
                cancellationToken: cancellationToken);

        var expandedEmbedding =
            await embeddingGenerator.GenerateAsync(
                expansion.ExpandedQuery,
                cancellationToken: cancellationToken);

        var originalResults =
            await vectorStore.SearchAsync(
                originalEmbedding.Vector,
                k,
                cancellationToken: cancellationToken);

        var expandedResults =
            await vectorStore.SearchAsync(
                expandedEmbedding.Vector,
                k,
                cancellationToken: cancellationToken);

        return Ok(new
        {
            originalQuery = query,
            expandedQuery = expansion.ExpandedQuery,
            expansion.ExpandedTerms,
            originalResults,
            expandedResults
        });
    }

    [HttpPost("retrieval/query-expansion")]
    public async Task<IActionResult> ExpandQueryAsync(
    [FromBody] string query,
    CancellationToken cancellationToken = default)
    {
        var result =
            await queryExpansionService.ExpandAsync(
                query,
                cancellationToken);

        return Ok(result);
    }

    [HttpPost("retrieval/bm25")]
    public async Task<IActionResult> Bm25SearchAsync(
    [FromBody] string query,
    [FromQuery] int k = 3,
    CancellationToken cancellationToken = default)
    {
        var chunks =
            await vectorStore.GetAllAsync(
                cancellationToken);

        var results =
            Bm25Retriever.Search(
                query,
                chunks,
                k);

        return Ok(new
        {
            query,
            k,
            results
        });
    }

    [HttpPost("retrieval/hybrid")]
    public async Task<IActionResult> HybridSearchAsync(
    [FromBody] string query,
    [FromQuery] int k = 3,
    CancellationToken cancellationToken = default)
    {
        var queryEmbedding =
            await embeddingGenerator.GenerateAsync(
                query,
                cancellationToken: cancellationToken);

        var vectorResults =
            await vectorStore.SearchAsync(
                queryEmbedding.Vector,
                k * 2,
                cancellationToken: cancellationToken);

        var lexicalResults =
            await vectorStore.LexicalSearchAsync(
                query,
                k * 2,
                cancellationToken);

        var maxVectorScore =
            vectorResults.Count > 0
                ? vectorResults.Max(x => x.Score)
                : 0;

        var maxLexicalScore =
            lexicalResults.Count > 0
                ? lexicalResults.Max(x => x.Score)
                : 0;

        var ids = vectorResults
            .Select(x => x.Id)
            .Union(lexicalResults.Select(x => x.Id))
            .Distinct();

        var results = new List<HybridSearchResult>();

        foreach (var id in ids)
        {
            var vectorResult =
                vectorResults.FirstOrDefault(x => x.Id == id);

            var lexicalResult =
                lexicalResults.FirstOrDefault(x => x.Id == id);

            var normalizedVectorScore =
                maxVectorScore > 0 && vectorResult is not null
                    ? vectorResult.Score / maxVectorScore
                    : 0;

            var normalizedLexicalScore =
                maxLexicalScore > 0 && lexicalResult is not null
                    ? lexicalResult.Score / maxLexicalScore
                    : 0;

            var hybridScore =
                (normalizedVectorScore * 0.7)
                + (normalizedLexicalScore * 0.3);

            results.Add(new HybridSearchResult
            {
                Id = id,

                Text =
                    vectorResult?.Text
                    ?? lexicalResult!.Text,

                VectorScore = normalizedVectorScore,
                LexicalScore = normalizedLexicalScore,
                HybridScore = hybridScore
            });
        }

        var topResults = results
            .OrderByDescending(x => x.HybridScore)
            .Take(k)
            .ToList();

        return Ok(new
        {
            query,
            k,
            vectorWeight = 0.7,
            lexicalWeight = 0.3,
            results = topResults
        });
    }

    [HttpPost("retrieval/search")]
    public async Task<IActionResult> SearchAsync(
    [FromBody] string query,
    [FromQuery] int k = 3,
    [FromQuery] string? documentId = null,
    [FromQuery] string? source = null,
    [FromQuery] string? version = null,
    CancellationToken cancellationToken = default)
    {

        var filter = new VectorSearchFilter
        {
            DocumentId = documentId,
            Source = source,
            Version = version
        };

        var queryEmbedding =
            await embeddingGenerator.GenerateAsync(
                query,
                cancellationToken: cancellationToken);

        var results =
            await vectorStore.SearchAsync(
                queryEmbedding.Vector,
                k,
                filter,
                cancellationToken);

        return Ok(new
        {
            query,
            k,
            results
        });
    }

    [HttpGet("ingestion/chunks")]
    public async Task<IActionResult> GetIndexedChunksAsync(
    CancellationToken cancellationToken)
    {
        var chunks =
            await vectorStore.GetAllAsync(
                cancellationToken);

        return Ok(new
        {
            count = chunks.Count,

            chunks = chunks.Select(chunk => new
            {
                chunk.Id,
                chunk.DocumentId,
                chunk.ChunkIndex,
                chunk.Title,
                chunk.Source,
                chunk.Version,
                chunk.Text,
                embeddingDimensions = chunk.Embedding.Length
            })
        });
    }

    [HttpPost("ingestion")]
    public async Task<IActionResult> IngestDocumentAsync(
    [FromBody] DocumentInput document,
    [FromQuery] int chunkSize = 500,
    [FromQuery] int overlap = 50,
    CancellationToken cancellationToken = default)
    {
        var chunks =
            await ingestionService.IngestAsync(
                document,
                chunkSize,
                overlap,
                cancellationToken);

        return Ok(new
        {
            document.Id,
            document.Title,
            document.Version,
            chunkCount = chunks.Count,

            chunks = chunks.Select(chunk => new
            {
                chunk.Id,
                chunk.DocumentId,
                chunk.ChunkIndex,
                chunk.Title,
                chunk.Source,
                chunk.Version,
                chunk.Text,

                embeddingDimensions =
                    chunk.Embedding.Length,

                embeddingPreview =
                    chunk.Embedding
                        .Take(5)
                        .ToArray()
            })
        });
    }

    [HttpPost("chunk/fixed")]
    public IActionResult FixedChunk(
    [FromBody] string text,
    [FromQuery] int chunkSize = 100,
    [FromQuery] int overlap = 0)
    {
        var context = new ChunkingContext
        {
            DocumentId = "doc-mcp-001",
            Title = "Introdução ao MCP",
            Source = "manual-test",
            Version = "1"
        };

        var chunks = FixedTextChunker.Chunk(
            text,
            chunkSize,
            overlap,
            context);

        return Ok(new
        {
            originalLength = text.Length,
            chunkSize,
            overlap,
            chunkCount = chunks.Count,
            chunks
        });
    }

    [HttpPost("chunk/compare")]
    public IActionResult CompareChunking(
    [FromBody] string text,
    [FromQuery] int charChunkSize = 100,
    [FromQuery] int tokenChunkSize = 20,
    [FromQuery] int overlap = 5)
    {
        var context = new ChunkingContext
        {
            DocumentId = "doc-mcp-001",
            Title = "Introdução ao MCP",
            Source = "manual-test",
            Version = "1"
        };

        var charChunks =
            FixedTextChunker.Chunk(
                text,
                charChunkSize,
                overlap: 0,
                context);

        var tokenizer =
            TiktokenTokenizer.CreateForModel("gpt-4");

        var tokenChunker =
            new TokenTextChunker(tokenizer);

        var tokenChunks =
            tokenChunker.Chunk(
                text,
                tokenChunkSize,
                overlap);

        return Ok(new
        {
            characters = new
            {
                chunkSize = charChunkSize,
                chunkCount = charChunks.Count,
                chunks = charChunks
            },

            tokens = new
            {
                chunkSize = tokenChunkSize,
                overlap,
                chunkCount = tokenChunks.Count,
                chunks = tokenChunks
            }
        });
    }

    [HttpPost("chunk/token")]
    public IActionResult TokenChunk(
    [FromBody] string text,
    [FromQuery] int chunkSize = 50,
    [FromQuery] int overlap = 10)
    {
        var tokenizer =
            TiktokenTokenizer.CreateForModel("gpt-4");

        var chunker =
            new TokenTextChunker(tokenizer);

        var chunks =
            chunker.Chunk(
                text,
                chunkSize,
                overlap);

        return Ok(new
        {
            chunkSize,
            overlap,
            chunkCount = chunks.Count,
            chunks
        });
    }

    [HttpPost("chunk/recursive")]
    public IActionResult RecursiveChunk(
    [FromBody] string text,
    [FromQuery] int chunkSize = 100)
    {
        var chunks =
            RecursiveTextChunker.Chunk(
                text,
                chunkSize);

        return Ok(new
        {
            originalLength = text.Length,
            chunkSize,
            chunkCount = chunks.Count,
            chunks
        });
    }

    [HttpPost("chunk/sliding-window")]
    public IActionResult SlidingWindowChunk(
    [FromBody] string text,
    [FromQuery] int windowSize = 20,
    [FromQuery] int step = 15)
    {
        var tokenizer =
            TiktokenTokenizer.CreateForModel("gpt-4");

        var chunker =
            new SlidingWindowChunker(tokenizer);

        var chunks =
            chunker.Chunk(
                text,
                windowSize,
                step);

        return Ok(new
        {
            windowSize,
            step,
            overlap = windowSize - step,
            chunkCount = chunks.Count,
            chunks
        });
    }

    [HttpPost("chunk/semantic")]
    public async Task<IActionResult> SemanticChunk(
    [FromBody] string text,
    [FromQuery] double threshold = 0.75,
    CancellationToken cancellationToken = default)
    {
        var chunker =
            new SemanticTextChunker(
                embeddingGenerator);

        var chunks =
            await chunker.ChunkAsync(
                text,
                threshold,
                cancellationToken);

        return Ok(new
        {
            threshold,
            chunkCount = chunks.Count,
            chunks
        });
    }

    [HttpPost("chunk/semantic/debug")]
    public async Task<IActionResult> SemanticChunkDebug(
    [FromBody] string text,
    [FromQuery] double threshold = 0.75,
    CancellationToken cancellationToken = default)
    {
        var paragraphs = text.Split(
            "\n\n",
            StringSplitOptions.RemoveEmptyEntries |
            StringSplitOptions.TrimEntries);

        var embeddings = new List<Embedding<float>>();

        foreach (var paragraph in paragraphs)
        {
            var embedding =
                await embeddingGenerator.GenerateAsync(
                    paragraph,
                    cancellationToken: cancellationToken);

            embeddings.Add(embedding);
        }

        var boundaries =
            new List<object>();

        for (var i = 1; i < paragraphs.Length; i++)
        {
            var similarity =
                VectorSimilarity.CosineSimilarity(
                    embeddings[i - 1].Vector.Span,
                    embeddings[i].Vector.Span);

            boundaries.Add(new
            {
                left = i - 1,
                right = i,
                similarity,
                split = similarity < threshold
            });
        }

        return Ok(new
        {
            threshold,
            paragraphs,
            boundaries
        });
    }

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
            if (conversation.Messages.Count == 0)
            {
                conversation.Messages.Add(
                   new ChatMessage(
    ChatRole.System,
    """
    Você é um assistente técnico especializado em .NET e AI Engineering.
    Responda de forma objetiva, clara e tecnicamente correta.
    Quando houver incerteza, explicite a limitação.
    """));
            }


            conversation.Messages.Add(
                new ChatMessage(
                    ChatRole.User,
                    request.Message));

            var chatOptions = new ChatOptions
            {
                Temperature = 0.2f,
                TopP = 0.95f,
                Tools = aiTools.Create()
            };

            var response = await chatClient.GetResponseAsync(
                conversation.Messages,
                chatOptions,
                cancellationToken);

            var usage = response.Usage;

            var inputTokens = usage?.InputTokenCount ?? 0;
            var outputTokens = usage?.OutputTokenCount ?? 0;
            var totalTokens = usage?.TotalTokenCount ?? 0;

            var responseText = response.Text ?? string.Empty;

            conversation.Messages.Add(
                new ChatMessage(
                    ChatRole.Assistant,
                    responseText));

            return Ok(new
            {
                conversationId = request.ConversationId,
                response = responseText,

                usage = new
                {
                    inputTokens,
                    outputTokens,
                    totalTokens
                }
            });
        }
        finally
        {
            conversation.Gate.Release();
        }
    }

    [HttpPost("structured")]
    public async Task<IActionResult> AnalyzeStructuredAsync(
     [FromBody] string message,
     CancellationToken cancellationToken)
    {
        var messages = new List<ChatMessage>
    {
        new(
            ChatRole.System,
            """
            Você é um assistente técnico especializado em .NET e AI Engineering.
            Responda de forma objetiva, clara e tecnicamente correta.
            Quando houver incerteza, explicite a limitação.
            """),

        new(
            ChatRole.User,
            message)
    };

        var options = new ChatOptions
        {
            Temperature = 0.2f,

            ResponseFormat =
                ChatResponseFormat.ForJsonSchema<MessageAnalysis>()
        };

        var response = await chatClient.GetResponseAsync(
            messages,
            options,
            cancellationToken);

        var responseText = response.Text ?? string.Empty;

        try
        {
            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            serializerOptions.Converters.Add(
                new JsonStringEnumConverter());

            var analysis =
            JsonSerializer.Deserialize<MessageAnalysis>(
                responseText,
                serializerOptions);

            if (analysis is null)
            {
                return StatusCode(
                    StatusCodes.Status502BadGateway,
                    new
                    {
                        error = "O modelo retornou uma resposta inválida."
                    });
            }

            return Ok(analysis);
        }
        catch (JsonException)
        {
            return StatusCode(
                StatusCodes.Status502BadGateway,
                new
                {
                    error = "A resposta não corresponde ao schema esperado.",
                    raw = responseText
                });
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
                Temperature = 0.2f,
                TopP = 0.95f,
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

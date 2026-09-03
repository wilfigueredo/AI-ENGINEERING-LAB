using System.Text.Json;
using AiEngineeringLab.Core.Models.Retrieval;
using Microsoft.Extensions.AI;

namespace AiEngineeringLab.Core.AI.Retrieval;

public sealed class RerankingService(
    IChatClient chatClient)
{
    public async Task<IReadOnlyList<RerankResult>> RerankAsync(
        string query,
        IReadOnlyList<VectorSearchResult> candidates,
        int k,
        CancellationToken cancellationToken = default)
    {
        if (candidates.Count == 0)
        {
            return [];
        }

        var documents = candidates
            .Select((candidate, index) => new
            {
                index,
                candidate.Id,
                candidate.Text,
                candidate.Score
            })
            .ToList();

        var payload =
            JsonSerializer.Serialize(documents);

        var messages = new List<ChatMessage>
        {
           new(
    ChatRole.System,
    """
    Você é um re-ranker para um sistema RAG.

    Avalie individualmente a relevância de cada documento
    em relação à consulta do usuário.

    Use uma pontuação contínua entre 0 e 100:

    90-100 = responde diretamente à consulta
    70-89  = altamente relevante
    50-69  = relevante, mas parcial
    20-49  = relacionado apenas indiretamente
    1-19   = relação muito fraca
    0      = completamente irrelevante

    IMPORTANTE:
    - não trate a tarefa como classificação binária;
    - documentos parcialmente relevantes devem receber
      pontuações intermediárias;
    - avalie cada documento independentemente;
    - não responda à pergunta;
    - não invente informações;
    - retorne somente JSON.

    Formato obrigatório:

    {
      "results": [
        {
          "index": 0,
          "score": 85
        }
      ]
    }
    """),

            new(
                ChatRole.User,
                $"""
                Consulta:
                {query}

                Documentos:
                {payload}
                """)
        };

        var response =
            await chatClient.GetResponseAsync(
                messages,
                new ChatOptions
                {
                    Temperature = 0.1f
                },
                cancellationToken);

        var rerank =
            JsonSerializer.Deserialize<RerankResponse>(
                response.Text,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })
            ?? throw new InvalidOperationException(
                "Unable to deserialize rerank response.");

        var results = rerank.Results
            .Where(x =>
                x.Index >= 0 &&
                x.Index < candidates.Count)
            .Select(x =>
            {
                var candidate =
                    candidates[x.Index];

                return new RerankResult
                {
                    Id = candidate.Id,
                    Text = candidate.Text,
                    OriginalScore = candidate.Score,
                    RerankScore = x.Score / 100.0
                };
            })
            .OrderByDescending(x => x.RerankScore)
            .Take(k)
            .ToList();

        return results;
    }

    private sealed class RerankResponse
    {
        public List<RerankItem> Results { get; init; } = [];
    }

    private sealed class RerankItem
    {
        public int Index { get; init; }
        public double Score { get; init; }
    }
}

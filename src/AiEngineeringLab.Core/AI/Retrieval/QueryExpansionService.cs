using System.Text.Json;
using AiEngineeringLab.Core.Models.Retrieval;
using Microsoft.Extensions.AI;

namespace AiEngineeringLab.Core.AI.Retrieval;

public sealed class QueryExpansionService(
    IChatClient chatClient)
{
    public async Task<QueryExpansionResult> ExpandAsync(
        string query,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(query);

        var messages = new List<ChatMessage>
        {
            new(
                ChatRole.System,
                """
                Você é responsável por expandir consultas para um sistema RAG.

                Gere termos ou expressões semanticamente relacionados
                à consulta original.

                Regras:
                - preserve a intenção original;
                - não responda à pergunta;
                - gere no máximo 5 termos ou expressões;
                - inclua sinônimos, siglas ou terminologia técnica quando útil;
                - não repita a consulta original;
                - retorne somente JSON.
                Formato obrigatório:
                {
                  "terms": [
                    "termo ou expressão"
                  ]
                }
                """),

            new(
                ChatRole.User,
                query)
        };

        var options = new ChatOptions
        {
            Temperature = 0.2f
        };

        var response =
            await chatClient.GetResponseAsync(
                messages,
                options,
                cancellationToken);

        var expansion =
            JsonSerializer.Deserialize<ExpansionResponse>(
                response.Text,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })
            ?? throw new InvalidOperationException(
                "Unable to deserialize query expansion.");

        var expandedQuery =
            string.Join(
                " ",
                new[]
                {
                    query
                }.Concat(expansion.Terms));

        return new QueryExpansionResult
        {
            OriginalQuery = query,
            ExpandedTerms = expansion.Terms,
            ExpandedQuery = expandedQuery
        };
    }

    private sealed class ExpansionResponse
    {
        public List<string> Terms { get; init; } = [];
    }
}

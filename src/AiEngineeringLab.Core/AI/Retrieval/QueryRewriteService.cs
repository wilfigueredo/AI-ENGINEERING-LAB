using AiEngineeringLab.Core.Models.Retrieval;
using Microsoft.Extensions.AI;

namespace AiEngineeringLab.Core.AI.Retrieval;

public sealed class QueryRewriteService(
    IChatClient chatClient)
{
    public async Task<QueryRewriteResult> RewriteAsync(
        string query,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(query);

        var messages = new List<ChatMessage>
        {
            new(
                ChatRole.System,
                """
                Você reescreve consultas para um sistema RAG.

                Regras:
                - preserve exatamente a intenção da consulta original;
                - torne a consulta clara, objetiva e autocontida;
                - remova linguagem coloquial desnecessária;
                - resolva referências vagas quando possível;
                - não adicione fatos não presentes na consulta;
                - não responda à pergunta;
                - retorne somente a consulta reescrita.
                """),

            new(
                ChatRole.User,
                query)
        };

        var options = new ChatOptions
        {
            Temperature = 0.1f
        };

        var response =
            await chatClient.GetResponseAsync(
                messages,
                options,
                cancellationToken);

        return new QueryRewriteResult
        {
            OriginalQuery = query,
            RewrittenQuery = response.Text.Trim()
        };
    }
}

using AiEngineeringLab.Core.Models.Retrieval;
using Microsoft.Extensions.AI;

namespace AiEngineeringLab.Core.AI.Retrieval;

public sealed class ContextCompressionService(
    IChatClient chatClient)
{
    public async Task<IReadOnlyList<CompressedContextResult>> CompressAsync(
        string query,
        IReadOnlyList<VectorSearchResult> documents,
        CancellationToken cancellationToken = default)
    {
        var results = new List<CompressedContextResult>();

        foreach (var document in documents)
        {
            var messages = new List<ChatMessage>
            {new(
                ChatRole.System,
                """
                Você realiza compressão extrativa de contexto para um sistema RAG.

                Extraia somente informações explicitamente presentes
                no documento e relevantes para a consulta.

                Regras obrigatórias:
                - não responda à pergunta;
                - não faça inferências;
                - não adicione explicações;
                - não introduza informações que não estejam literalmente
                  suportadas pelo documento;
                - preserve apenas fatos relevantes já existentes no texto;
                - remova conteúdo irrelevante;
                - seja conciso;
                - se nenhuma informação do documento ajudar diretamente
                  a responder à consulta, retorne uma string vazia.

                É preferível retornar vazio do que inferir uma resposta.
                Se não houver informação relevante, retorne exatamente:
                EMPTY
                """),

                new(
                    ChatRole.User,
                    $"""
                    Consulta:
                    {query}

                    Documento:
                    {document.Text}
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

            var compressedText =
                response.Text.Trim();

            if (compressedText.Equals(
                "EMPTY",
                StringComparison.OrdinalIgnoreCase))
            {
                compressedText = string.Empty;
            }

            results.Add(new CompressedContextResult
            {
                Id = document.Id,
                OriginalText = document.Text,
                CompressedText = compressedText
            });
        }

        return results;
    }
}

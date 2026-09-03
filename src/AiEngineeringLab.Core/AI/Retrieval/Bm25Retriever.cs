using AiEngineeringLab.Core.Models.Ingestion;
using AiEngineeringLab.Core.Models.Retrieval;

namespace AiEngineeringLab.Core.AI.Retrieval;

public static class Bm25Retriever
{
    private const double K1 = 1.5;
    private const double B = 0.75;

    public static IReadOnlyList<LexicalSearchResult> Search(
        string query,
        IReadOnlyList<IndexedChunk> documents,
        int k)
    {
        if (documents.Count == 0)
        {
            return [];
        }

        var queryTerms = Tokenize(query);

        var tokenizedDocuments = documents
            .Select(document => new
            {
                Document = document,
                Terms = Tokenize(document.Text)
            })
            .ToList();

        var averageDocumentLength =
            tokenizedDocuments.Average(x => x.Terms.Count);

        var results = new List<LexicalSearchResult>();

        foreach (var item in tokenizedDocuments)
        {
            double score = 0;

            foreach (var term in queryTerms)
            {
                var termFrequency =
                    item.Terms.Count(x => x == term);

                if (termFrequency == 0)
                {
                    continue;
                }

                var documentsContainingTerm =
                    tokenizedDocuments.Count(
                        x => x.Terms.Contains(term));

                var idf = Math.Log(
                    1 +
                    (
                        documents.Count
                        - documentsContainingTerm
                        + 0.5
                    )
                    /
                    (
                        documentsContainingTerm
                        + 0.5
                    ));

                var documentLength =
                    item.Terms.Count;

                var numerator =
                    termFrequency * (K1 + 1);

                var denominator =
                    termFrequency
                    + K1 * (
                        1 - B
                        + B * (
                            documentLength
                            / averageDocumentLength
                        ));

                score +=
                    idf * (numerator / denominator);
            }

            if (score > 0)
            {
                results.Add(new LexicalSearchResult
                {
                    Id = item.Document.Id,
                    Text = item.Document.Text,
                    Score = score
                });
            }
        }

        return results
            .OrderByDescending(x => x.Score)
            .Take(k)
            .ToList();
    }

    private static List<string> Tokenize(string text)
    {
        return text
            .ToLowerInvariant()
            .Split(
                [' ', '.', ',', ';', ':', '!', '?', '\n', '\r'],
                StringSplitOptions.RemoveEmptyEntries)
            .ToList();
    }
}

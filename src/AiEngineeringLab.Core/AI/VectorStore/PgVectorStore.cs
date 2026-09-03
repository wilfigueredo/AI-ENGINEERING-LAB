using AiEngineeringLab.Core.AI.Interface;
using AiEngineeringLab.Core.Models.Ingestion;
using AiEngineeringLab.Core.Models.Retrieval;
using Npgsql;
using Pgvector;

namespace AiEngineeringLab.Core.AI.VectorStore;

public sealed class PgVectorStore(
    NpgsqlDataSource dataSource) : IVectorStore
{
    public async Task SaveAsync(
        IEnumerable<IndexedChunk> chunks,
        CancellationToken cancellationToken = default)
    {
        await using var connection =
            await dataSource.OpenConnectionAsync(cancellationToken);

        foreach (var chunk in chunks)
        {
            const string sql =
                """
                INSERT INTO chunks (
                    id,
                    document_id,
                    chunk_index,
                    title,
                    source,
                    version,
                    text,
                    embedding
                )
                VALUES (
                    @id,
                    @documentId,
                    @chunkIndex,
                    @title,
                    @source,
                    @version,
                    @text,
                    @embedding
                );
                """;

            await using var command =
                new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue(
                "id",
                Guid.Parse(chunk.Id));

            command.Parameters.AddWithValue(
                "documentId",
                chunk.DocumentId);

            command.Parameters.AddWithValue(
                "chunkIndex",
                chunk.ChunkIndex);

            command.Parameters.AddWithValue(
                "title",
                chunk.Title);

            command.Parameters.AddWithValue(
                "source",
                chunk.Source);

            command.Parameters.AddWithValue(
                "version",
                chunk.Version);

            command.Parameters.AddWithValue(
                "text",
                chunk.Text);

            command.Parameters.AddWithValue(
                "embedding",
                new Vector(chunk.Embedding));

            await command.ExecuteNonQueryAsync(
                cancellationToken);
        }
    }

    public async Task<IReadOnlyList<IndexedChunk>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var result = new List<IndexedChunk>();

        await using var connection =
            await dataSource.OpenConnectionAsync(cancellationToken);

        const string sql =
            """
            SELECT
                id,
                document_id,
                chunk_index,
                title,
                source,
                version,
                text,
                embedding
            FROM chunks
            ORDER BY document_id, chunk_index;
            """;

        await using var command =
            new NpgsqlCommand(sql, connection);

        await using var reader =
            await command.ExecuteReaderAsync(
                cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            var vector =
                reader.GetFieldValue<Vector>(
                    reader.GetOrdinal("embedding"));

            result.Add(new IndexedChunk
            {
                Id = reader
                    .GetGuid(reader.GetOrdinal("id"))
                    .ToString(),

                DocumentId =
                    reader.GetString(
                        reader.GetOrdinal("document_id")),

                ChunkIndex =
                    reader.GetInt32(
                        reader.GetOrdinal("chunk_index")),

                Title =
                    reader.GetString(
                        reader.GetOrdinal("title")),

                Source =
                    reader.GetString(
                        reader.GetOrdinal("source")),

                Version =
                    reader.GetString(
                        reader.GetOrdinal("version")),

                Text =
                    reader.GetString(
                        reader.GetOrdinal("text")),

                Embedding =
                    vector.ToArray()
            });
        }

        return result;
    }

    public async Task DeleteByDocumentIdAsync(
        string documentId,
        CancellationToken cancellationToken = default)
    {
        await using var connection =
            await dataSource.OpenConnectionAsync(cancellationToken);

        const string sql =
            """
            DELETE FROM chunks
            WHERE document_id = @documentId;
            """;

        await using var command =
            new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue(
            "documentId",
            documentId);

        await command.ExecuteNonQueryAsync(
            cancellationToken);
    }

    public async Task<IReadOnlyList<VectorSearchResult>> SearchAsync(
      ReadOnlyMemory<float> queryVector,
      int k,
      CancellationToken cancellationToken = default)
    {
        if (k <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(k));
        }

        var results = new List<VectorSearchResult>();

        await using var connection =
            await dataSource.OpenConnectionAsync(cancellationToken);

        const string sql =
            """
        SELECT
            id,
            text,
            1 - (embedding <=> @queryEmbedding) AS score
        FROM chunks
        ORDER BY embedding <=> @queryEmbedding
        LIMIT @k;
        """;

        await using var command =
            new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue(
            "queryEmbedding",
            new Vector(queryVector.ToArray()));

        command.Parameters.AddWithValue(
            "k",
            k);

        await using var reader =
            await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            results.Add(new VectorSearchResult
            {
                Id = reader
                    .GetGuid(reader.GetOrdinal("id"))
                    .ToString(),

                Text =
                    reader.GetString(
                        reader.GetOrdinal("text")),

                Score =
                    reader.GetDouble(
                        reader.GetOrdinal("score"))
            });
        }

        return results;
    }

    public async Task<IReadOnlyList<VectorSearchResult>> SearchAsync(
    ReadOnlyMemory<float> queryVector,
    int k,
    VectorSearchFilter? filter = null,
    CancellationToken cancellationToken = default)
    {
        if (k <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(k));
        }

        var results = new List<VectorSearchResult>();

        await using var connection =
            await dataSource.OpenConnectionAsync(cancellationToken);

        var sql =
            """
        SELECT
            id,
            text,
            1 - (embedding <=> @queryEmbedding) AS score
        FROM chunks
        WHERE 1 = 1
        """;

        if (!string.IsNullOrWhiteSpace(filter?.DocumentId))
        {
            sql += "\nAND document_id = @documentId";
        }

        if (!string.IsNullOrWhiteSpace(filter?.Source))
        {
            sql += "\nAND source = @source";
        }

        if (!string.IsNullOrWhiteSpace(filter?.Version))
        {
            sql += "\nAND version = @version";
        }

        sql +=
            """
        
        ORDER BY embedding <=> @queryEmbedding
        LIMIT @k;
        """;

        await using var command =
            new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue(
            "queryEmbedding",
            new Vector(queryVector.ToArray()));

        command.Parameters.AddWithValue("k", k);

        if (!string.IsNullOrWhiteSpace(filter?.DocumentId))
        {
            command.Parameters.AddWithValue(
                "documentId",
                filter.DocumentId);
        }

        if (!string.IsNullOrWhiteSpace(filter?.Source))
        {
            command.Parameters.AddWithValue(
                "source",
                filter.Source);
        }

        if (!string.IsNullOrWhiteSpace(filter?.Version))
        {
            command.Parameters.AddWithValue(
                "version",
                filter.Version);
        }

        await using var reader =
            await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            results.Add(new VectorSearchResult
            {
                Id = reader
                    .GetGuid(reader.GetOrdinal("id"))
                    .ToString(),

                Text =
                    reader.GetString(
                        reader.GetOrdinal("text")),

                Score =
                    reader.GetDouble(
                        reader.GetOrdinal("score"))
            });
        }

        return results;
    }

    public async Task<IReadOnlyList<LexicalSearchResult>> LexicalSearchAsync(
    string query,
    int k,
    CancellationToken cancellationToken = default)
    {
        var results = new List<LexicalSearchResult>();

        await using var connection =
            await dataSource.OpenConnectionAsync(cancellationToken);

        const string sql =
            """
        SELECT
            id,
            text,
            ts_rank(
                to_tsvector('portuguese', text),
                websearch_to_tsquery('portuguese', @query)
            ) AS score
        FROM chunks
        WHERE
            to_tsvector('portuguese', text)
            @@ websearch_to_tsquery('portuguese', @query)
        ORDER BY score DESC
        LIMIT @k;
        """;

        await using var command =
            new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("query", query);
        command.Parameters.AddWithValue("k", k);

        await using var reader =
            await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            results.Add(new LexicalSearchResult
            {
                Id = reader
                    .GetGuid(reader.GetOrdinal("id"))
                    .ToString(),

                Text = reader.GetString(
                    reader.GetOrdinal("text")),

                Score = reader.GetDouble(
                    reader.GetOrdinal("score"))
            });
        }

        return results;
    }
}

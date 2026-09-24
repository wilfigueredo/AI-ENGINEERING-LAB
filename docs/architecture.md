# AI Engineering Lab — Architecture

## 1. Overview

The AI Engineering Lab is a modular .NET solution used to implement, compare,
test and observe AI Engineering concepts through working experiments.

The repository currently contains practical implementations for:

- LLM integration;
- conversation history and streaming;
- tools and function calling;
- Semantic Kernel;
- Structured Outputs;
- embeddings and vector similarity;
- chunking and ingestion;
- PostgreSQL + pgvector;
- advanced retrieval;
- RAG evaluation and regression detection;
- MCP client/server communication;
- a local Transformer/BERT experiment with ONNX.

The laboratory is intentionally incremental. Individual capabilities are first
implemented and made observable in isolation, then composed into larger
pipelines when the roadmap reaches the appropriate phase.

---

## 2. Solution Structure

```text
AI-ENGINEERING-LAB
│
├── src/
│   ├── AiEngineeringLab.Api
│   ├── AiEngineeringLab.Core
│   ├── AiEngineeringLab.Plugins
│   ├── AiEngineeringLab.McpServer
│   ├── AiEngineeringLab.McpClient
│   └── AiEngineeringLab.Transformers
│
├── tests/
│   └── AiEngineeringLab.UnitTests
│
├── docs/
├── prompts/
├── docker-compose.yml
└── AiEngineeringLab.sln
```

---

## 3. Project Responsibilities

### AiEngineeringLab.Api

Executable ASP.NET Core entry point.

Current responsibilities include:

- application startup and dependency injection;
- configuration and secrets binding;
- OpenAI chat client registration;
- embedding generator registration;
- PostgreSQL/Npgsql configuration;
- pgvector type registration;
- HTTP experiment endpoints;
- conversation and streaming endpoints;
- endpoints for chunking, ingestion and retrieval experiments;
- Semantic Kernel integration.

The API layer is currently also the place where several advanced retrieval
experiments are composed. Reusable RAG orchestration should move out of the
controller during the production-ready phase.

### AiEngineeringLab.Core

Contains reusable AI/application behavior that is independent from HTTP.

Current areas include:

```text
AI/
├── Chunking/
├── Embeddings/
├── Evaluation/
│   ├── Generation/
│   ├── Grounding/
│   ├── Harness/
│   ├── Metrics/
│   └── Models/
├── Interface/
├── Retrieval/
└── VectorStore/

Models/
├── Chat/
├── Chunking/
├── Embedding/
├── Ingestion/
└── Retrieval/

Services/
├── Conversations/
└── Ingestion/
```

Core contains the main RAG building blocks, evaluation contracts and
deterministic algorithms used by the laboratory.

### AiEngineeringLab.Plugins

Contains deterministic capabilities exposed to AI systems.

Current approaches:

- Microsoft.Extensions.AI tools through `AIFunctionFactory`;
- Semantic Kernel native functions through `KernelFunction`.

Examples include date operations and deterministic text operations.

### AiEngineeringLab.McpServer

Standalone MCP server using stdio transport.

It currently exposes:

- a framework version tool;
- a framework information resource.

### AiEngineeringLab.McpClient

Standalone MCP client used to demonstrate:

- connection to the MCP server;
- tool discovery;
- resource discovery;
- tool invocation;
- resource reading.

### AiEngineeringLab.Transformers

Standalone local Transformer laboratory.

It uses:

- BERT tokenizer;
- ONNX Runtime;
- a BERT Tiny classification model;
- `input_ids`;
- `attention_mask`;
- logits and Softmax;
- toxicity classification.

This project exists to make Transformer fundamentals observable without
turning the main application into a machine-learning training project.

### AiEngineeringLab.UnitTests

Validates deterministic behavior and AI integration boundaries.

The suite currently covers, among other areas:

- vector similarity;
- retrieval metrics;
- evaluation metrics;
- generation evaluators;
- grounding evaluation;
- RAG Harness orchestration;
- RAG regression detection;
- dataset loading;
- plugin behavior.

Live-provider tests are isolated/skipped when external OpenAI access is
required.

---

## 4. Dependency Direction

The project-reference direction is:

```text
AiEngineeringLab.Api
       │
       ├────────► AiEngineeringLab.Core
       │
       └────────► AiEngineeringLab.Plugins
                         │
                         └────────► AiEngineeringLab.Core


AiEngineeringLab.UnitTests
       │
       ├────────► AiEngineeringLab.Core
       └────────► AiEngineeringLab.Plugins


AiEngineeringLab.McpClient     runtime MCP     AiEngineeringLab.McpServer
          │                  communication                │
          └───────────────────────►◄─────────────────────┘

AiEngineeringLab.Transformers
        standalone laboratory
```

The Core project must not depend on the API project.

---

## 5. LLM Integration

Chat communication uses `IChatClient` from Microsoft.Extensions.AI.

```text
HTTP Request
     ↓
ChatController
     ↓
IChatClient
     ↓
OpenAI
     ↓
ChatResponse
```

The API also demonstrates:

- System/User/Assistant roles;
- Structured Outputs;
- tools;
- streaming;
- usage/token inspection;
- cancellation.

---

## 6. Conversation State and Streaming

Conversation history is maintained per `conversationId`.

```text
conversationId
      ↓
ConversationHistoryService
      ↓
ConcurrentDictionary
      ↓
ConversationState
      ├── Messages
      └── SemaphoreSlim
```

The per-conversation gate prevents concurrent writes to the same history while
allowing independent conversations to execute concurrently.

Streaming uses Server-Sent Events and propagates cancellation. Interrupted
streaming operations can restore the previous conversation state instead of
persisting an incomplete assistant turn.

---

## 7. Embeddings

The primary application pipeline uses:

```text
IEmbeddingGenerator<string, Embedding<float>>
                 ↓
            OpenAI client
                 ↓
         Embedding<float>
```

registered through Microsoft.Extensions.AI.

There is also an older laboratory-specific `IEmbeddingGenerator` abstraction
used by `OpenAiEmbeddingGenerator` in isolated experiments. This is a
transitional duplication and should not be expanded in new production-ready
RAG code without an explicit reason.

---

## 8. Chunking and Ingestion

The repository contains multiple chunking strategies:

- `FixedTextChunker`;
- `RecursiveTextChunker`;
- `SemanticTextChunker`;
- `SlidingWindowChunker`;
- `TokenTextChunker`.

The current ingestion service uses fixed chunking and preserves metadata.

```text
DocumentInput
     ↓
IngestionService
     ↓
ChunkingContext
     ↓
FixedTextChunker
     ↓
IEmbeddingGenerator
     ↓
IndexedChunk
     ↓
DeleteByDocumentIdAsync
     ↓
IVectorStore.SaveAsync
```

Metadata currently includes:

- document ID;
- title;
- source;
- version;
- chunk index.

Deleting the previous chunks before saving the new set provides the current
reindexing behavior for a document.

---

## 9. Vector Storage and Search

`IVectorStore` is the main storage/search contract.

Implementations:

### InMemoryVectorStore

Used for local deterministic experiments and unit-test-friendly scenarios.

It supports:

- save;
- list;
- vector search;
- metadata filtering;
- deletion by document ID.

Lexical search is intentionally not supported by this implementation.

### PgVectorStore

Uses PostgreSQL, Npgsql and pgvector.

It supports:

- embedding persistence;
- vector retrieval;
- cosine-distance ordering with `<=>`;
- metadata filters;
- deletion by document ID;
- PostgreSQL Full Text Search for lexical retrieval.

Docker Compose provides a pgvector-enabled PostgreSQL container.

### Current infrastructure boundary

The repository currently assumes the `chunks` table exists. It does not yet
version the database schema, `CREATE EXTENSION vector`, or an ANN/HNSW index
definition. Production-ready database initialization/migrations therefore
remain an explicit evolution point.

---

## 10. Advanced Retrieval

Advanced retrieval capabilities live primarily under
`AiEngineeringLab.Core.AI.Retrieval`.

Current components include:

- `Bm25Retriever`;
- `QueryExpansionService`;
- `QueryRewriteService`;
- `RerankingService`;
- `MmrRetriever`;
- `ContextCompressionService`.

The API currently exposes separate experiment endpoints for:

- vector search;
- BM25;
- hybrid search;
- query expansion;
- query rewriting;
- re-ranking;
- MMR;
- context compression.

This separation is intentional for learning and comparison.

The production-ready phase will compose selected capabilities behind a reusable
RAG query pipeline instead of leaving orchestration in `ChatController`.

---

## 11. RAG Evaluation Architecture

The evaluation subsystem is separated from the concrete RAG implementation.

```text
RagEvaluationDataset
        ↓
RagEvaluationRunner
        ↓
RagEvaluationHarness
        │
        ├── IRagEvaluationPipeline
        ├── IGenerationEvaluator
        └── IGroundingEvaluator
        ↓
RagEvaluationExecutionResult
        ↓
RagEvaluationSummary
        ↓
RagRegressionDetector
```

### Retrieval evaluation

Current metrics include:

- Precision;
- Recall;
- Hit Rate;
- Reciprocal Rank / MRR.

### Generation evaluation

`IGenerationEvaluator` has:

- a rule-based implementation for deterministic experiments;
- an LLM-as-a-Judge implementation.

Generation criteria include:

- correctness;
- relevance;
- completeness;
- overall score.

### Grounding evaluation

`IGroundingEvaluator` evaluates:

- faithfulness;
- supported claims;
- unsupported claims.

### Harness boundary

`IRagEvaluationPipeline` deliberately isolates the Harness from a concrete RAG
pipeline.

Current Harness tests use fake pipeline implementations. A concrete end-to-end
RAG implementation is the bridge to be built in Phase 2.6.

---

## 12. Current RAG Architecture

The implemented pieces currently form two different maturity levels.

### Ingestion side — composed

```text
Document
   ↓
Chunking
   ↓
Embeddings
   ↓
IndexedChunk
   ↓
PgVectorStore
```

### Query side — capabilities exist, composition is pending

```text
Question
   │
   ├── Query Rewrite / Expansion
   ├── Embedding
   ├── Vector Search
   ├── Lexical Search / BM25
   ├── Hybrid Search
   ├── Re-ranking
   ├── MMR
   └── Context Compression
           ↓
     [end-to-end orchestration pending]
           ↓
        Generation
```

The query-side production pipeline is the main architectural target of
Phase 2.6.

---

## 13. MCP Architecture

MCP is isolated in dedicated console projects.

```text
AiEngineeringLab.McpClient
          ↓ stdio
AiEngineeringLab.McpServer
      ├── Tools
      └── Resources
```

This keeps protocol experimentation independent from the ASP.NET Core API.

---

## 14. Configuration and Secrets

Configuration uses standard .NET configuration/options.

Current configuration includes:

- AI provider;
- chat model ID;
- embedding model ID;
- OpenAI API key;
- PostgreSQL connection string.

Sensitive credentials must remain outside version control through environment
variables, user secrets or another secret-management mechanism.

---

## 15. Current Architectural Boundary

The repository is a learning laboratory, not a single production product.

Experiment endpoints are valid when they make a concept observable, even when
the same endpoint would not belong in a production public API.

The current production-readiness boundary is clear:

### Already implemented

- ingestion;
- chunking;
- embeddings;
- pgvector persistence/search;
- advanced retrieval capabilities;
- generation primitives;
- evaluation Harness;
- regression detection.

### Still to be consolidated in Phase 2.6

- a concrete end-to-end RAG query pipeline;
- retrieval provenance suitable for evaluation and source attribution;
- context construction as an explicit pipeline responsibility;
- integration of the real pipeline with `IRagEvaluationPipeline`;
- observability/tracing;
- latency/token/cost measurements across the RAG pipeline;
- timeout/retry/failure handling;
- production-ready database schema/index management;
- configuration and security hardening.

---

## 16. Evolution Rule

New frameworks, providers or infrastructure should not be added only to
increase technology count.

A capability should be introduced when it provides at least one of:

- a new AI Engineering concept to study;
- an observable technical experiment;
- a meaningful architectural comparison;
- a reusable implementation pattern;
- a measurable engineering trade-off.

Future architecture documentation should describe capabilities only after they
exist in the repository or explicitly identify them as planned.

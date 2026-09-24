# AI Engineering Lab — Learning Goals

## Objective

The AI Engineering Lab is a hands-on environment for developing practical
AI Engineering skills using .NET.

The goal is not only to learn how AI frameworks and APIs work, but to
understand the engineering decisions behind building reliable AI-powered
applications.

Each topic should combine:

1. conceptual understanding;
2. practical implementation;
3. observable experiments;
4. automated tests when applicable;
5. analysis of trade-offs and production use cases.

---

## Core Learning Areas

### LLM Integration

Understand how applications communicate with Large Language Models.

Topics include:

- chat clients;
- model configuration;
- prompts;
- conversation context;
- asynchronous execution;
- provider abstractions;
- token usage and cost awareness.

### LLM Generation Parameters

Understand how generation behavior can be controlled and evaluated.

Topics include:

- Temperature;
- Top P;
- determinism;
- creativity;
- interaction between sampling parameters;
- provider-specific capabilities.

### Structured Outputs

Understand how to obtain predictable machine-readable responses from LLMs.

Topics include:

- JSON outputs;
- schemas;
- typed responses;
- validation;
- deserialization;
- invalid-output handling.

### Tools and Function Calling

Understand how an LLM can interact with deterministic application capabilities.

Topics include:

- tool definitions;
- function schemas;
- automatic function selection;
- argument binding;
- execution;
- error handling;
- logging;
- tool orchestration.

### Semantic Kernel

Understand how Semantic Kernel can orchestrate AI services, plugins and
functions.

Topics include:

- Kernel;
- Kernel Plugins;
- Kernel Functions;
- direct invocation;
- automatic function calling;
- prompt invocation;
- integration with existing application pipelines.

### MCP

Understand how AI applications expose and consume interoperable tools and
resources.

Topics include:

- MCP client/server roles;
- stdio transport;
- tools;
- resources;
- capability discovery;
- invocation and resource reading.

### Transformer Fundamentals

Understand enough Transformer architecture to make better AI Engineering
decisions without turning the roadmap into a deep-learning research track.

Topics include:

- tokenization;
- attention and self-attention;
- Query, Key and Value;
- encoder x decoder;
- BERT x GPT;
- local ONNX inference;
- logits and Softmax;
- distinction between a base Transformer and a task-specific head.

### Embeddings

Understand how semantic information can be represented numerically.

Topics include:

- embedding models;
- vector dimensionality;
- semantic representation;
- cosine similarity;
- Dot Product;
- Euclidean Distance;
- comparison between similarity metrics.

### Chunking and Ingestion

Understand how source content becomes searchable RAG data.

Topics include:

- fixed chunking;
- recursive chunking;
- semantic chunking;
- token-based chunking;
- sliding windows;
- overlap;
- metadata;
- ingestion;
- reindexing;
- content versioning.

### Vector Search and Vector Databases

Understand how embeddings can be stored and retrieved efficiently.

Topics include:

- vector storage;
- PostgreSQL + pgvector;
- exact similarity search;
- Top-K retrieval;
- metadata filtering;
- lexical search;
- ANN concepts;
- HNSW concepts;
- operational trade-offs between local and managed vector databases.

### Retrieval-Augmented Generation

Understand how external knowledge can be retrieved and supplied to an LLM.

Topics include:

- document ingestion;
- chunking;
- embedding generation;
- vector storage;
- retrieval;
- context construction;
- grounded generation;
- source attribution;
- separation of ingestion and query pipelines.

### Advanced Retrieval

Understand how retrieval quality can be improved beyond plain vector Top-K.

Topics include:

- BM25;
- Hybrid Search;
- Query Expansion;
- Query Rewriting;
- re-ranking;
- MMR;
- Context Compression;
- metadata filtering;
- quality/latency/cost trade-offs.

### Retrieval Evaluation

Understand how retrieval quality can be measured.

Topics include:

- Precision;
- Recall;
- Hit Rate;
- Reciprocal Rank / MRR;
- relevance;
- ranking quality;
- retrieval trade-offs;
- aggregation across evaluation datasets.

### Generation and Grounding Evaluation

Understand how non-deterministic RAG answers can be evaluated systematically.

Topics include:

- correctness;
- answer relevance;
- completeness;
- overall generation score;
- groundedness / faithfulness;
- supported and unsupported claims;
- deterministic evaluators;
- LLM-as-a-Judge.

### AI Harness and Regression Testing

Understand how RAG quality can be evaluated repeatedly instead of by visual
inspection alone.

Topics include:

- evaluation datasets;
- pipeline contracts;
- automated execution;
- metric aggregation;
- baselines;
- tolerance;
- regression detection;
- comparison of pipeline changes.

### AI Orchestration

Understand how multiple AI capabilities cooperate in an application.

Topics include:

- LLM + Tools;
- LLM + RAG;
- RAG + Tools;
- Structured Outputs;
- multi-step execution;
- deterministic and probabilistic components.

### Observability and Production Readiness

Understand how AI systems can be operated and improved in production.

Topics include:

- structured logging;
- tracing;
- token usage;
- latency;
- cost;
- retrieval metrics;
- similarity scores;
- failure tracking;
- retries and timeouts;
- configuration;
- security;
- continuous evaluation.

---

## Current Progress

- ✅ Phase 1 — fundamentals, LLM integration, tools, Semantic Kernel, MCP and Transformers;
- ✅ Phase 2.1 — retrieval fundamentals;
- ✅ Phase 2.2 — chunking and ingestion;
- ✅ Phase 2.3 — practical PostgreSQL + pgvector vector-store implementation;
- ✅ Phase 2.4 — advanced retrieval capabilities;
- ✅ Phase 2.5 — evaluation Harness, generation evaluation, grounding and regression detection;
- 🟡 Phase 2.6 — next step: compose the existing capabilities into a production-ready end-to-end RAG pipeline.

---

## Expected Outcome

At the end of the roadmap, the developer should be capable of designing,
implementing, testing, evaluating, protecting and operating AI-enabled
applications while understanding the engineering trade-offs behind the chosen
architecture.

.NET remains the primary application stack, with other technologies introduced
only when they add a justified capability or meaningful comparison.

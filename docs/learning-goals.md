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

## Core Learning Areas

### LLM Integration

Understand how applications communicate with Large Language Models.

Topics include:

- chat clients;
- model configuration;
- prompts;
- conversation context;
- asynchronous execution;
- provider abstractions.

### LLM Generation Parameters

Understand how generation behavior can be controlled and evaluated.

Topics include:

- Temperature;
- Top P;
- determinism;
- creativity;
- interaction between sampling parameters;
- provider-specific capabilities.

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

### Embeddings

Understand how semantic information can be represented numerically.

Topics include:

- embedding models;
- vector dimensionality;
- semantic representation;
- cosine similarity;
- Euclidean distance;
- comparison between similarity metrics.

### Vector Search

Understand how embeddings can be indexed and retrieved efficiently.

Topics include:

- vector storage;
- indexing;
- similarity search;
- Top K retrieval;
- similarity thresholds;
- metadata;
- filtering;
- ranking.

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
- source attribution.

### Retrieval Evaluation

Understand how retrieval quality can be measured.

Topics include:

- Precision;
- Recall;
- Precision@K;
- Recall@K;
- relevance;
- ranking quality;
- retrieval trade-offs.

### Structured Outputs

Understand how to obtain predictable machine-readable responses from LLMs.

Topics include:

- JSON outputs;
- schemas;
- typed responses;
- validation;
- deserialization;
- invalid-output handling.

### AI Orchestration

Understand how multiple AI capabilities can cooperate in an application.

Topics include:

- LLM + Tools;
- LLM + RAG;
- RAG + Tools;
- Structured Outputs;
- multi-step execution;
- deterministic and probabilistic components.

### Observability

Understand how AI systems can be monitored and evaluated in production.

Topics include:

- structured logging;
- token usage;
- latency;
- tool execution duration;
- retrieval metrics;
- similarity scores;
- estimated request cost;
- failure tracking.

## Expected Outcome

At the end of the roadmap, the developer should be capable of designing,
implementing, testing and evaluating AI-enabled applications in .NET while
understanding the engineering trade-offs behind the chosen architecture.
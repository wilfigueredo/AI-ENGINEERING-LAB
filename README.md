# AI Engineering Lab

[![.NET](https://img.shields.io/badge/.NET-9-512BD4)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-13-239120)](https://learn.microsoft.com/dotnet/csharp/)
[![AI Engineering](https://img.shields.io/badge/focus-AI%20Engineering-blueviolet)](#learning-path)
[![Status](https://img.shields.io/badge/status-active%20learning-success)](#project-status)

A hands-on **AI Engineering laboratory built with .NET**, focused on implementing and validating modern LLM application concepts through working code rather than studying them only in theory.

This repository documents my progression from traditional software engineering into production-oriented AI Engineering using the .NET ecosystem.

## Purpose

The lab exists to answer a practical question:

> How do the main concepts behind modern AI applications behave when implemented as real .NET software?

Each topic is explored incrementally, with emphasis on understanding the engineering trade-offs behind the abstraction.

## Project status

This is an **active learning and experimentation repository**.

**Phase 1 is complete. Phase 2 — Professional RAG — is in progress, with retrieval, chunking, pgvector, advanced retrieval and the evaluation Harness already implemented. The next step is the production-ready end-to-end RAG pipeline.**

The repository currently contains practical experiments for:

- LLM integration;
- conversation history and streaming;
- tools and function calling;
- Semantic Kernel;
- Structured Outputs;
- embeddings and vector similarity;
- multiple chunking strategies;
- document ingestion and reindexing;
- PostgreSQL + pgvector;
- vector, lexical and hybrid retrieval;
- BM25, query expansion and query rewriting;
- re-ranking, MMR and context compression;
- RAG evaluation metrics;
- generation evaluation and LLM-as-a-Judge;
- grounding / faithfulness evaluation;
- RAG regression Harness;
- MCP client/server integration;
- local Transformer/BERT inference with ONNX.

## Learning path

The experiments follow an intentionally progressive path:

```text
LLM fundamentals
      ↓
Tools / function calling
      ↓
Semantic Kernel + MCP
      ↓
Embeddings + vector similarity
      ↓
Chunking + ingestion
      ↓
PostgreSQL + pgvector
      ↓
Advanced retrieval
      ↓
RAG evaluation / Harness
      ↓
Production-ready RAG
      ↓
Agentic AI
```

The goal is to understand each layer before relying on higher-level abstractions.

## Tech stack

- **.NET 9**
- **C#**
- **ASP.NET Core**
- **Microsoft.Extensions.AI**
- **Microsoft Semantic Kernel**
- **OpenAI integration**
- **PostgreSQL + pgvector**
- **Npgsql**
- **Model Context Protocol**
- **Microsoft.ML.Tokenizers**
- **ONNX Runtime**
- **xUnit**

## Repository structure

```text
src/
├── AiEngineeringLab.Api/          # API and experiment entry points
├── AiEngineeringLab.Core/         # RAG, evaluation, models and reusable AI logic
├── AiEngineeringLab.Plugins/      # Deterministic tools and Semantic Kernel plugins
├── AiEngineeringLab.McpServer/    # MCP tools/resources server
├── AiEngineeringLab.McpClient/    # MCP client laboratory
└── AiEngineeringLab.Transformers/ # BERT/ONNX Transformer fundamentals lab

tests/
└── AiEngineeringLab.UnitTests/    # Automated tests and RAG evaluation tests

docs/                              # Roadmap, learning goals and architecture
prompts/                           # Externalized prompts
docker-compose.yml                 # PostgreSQL + pgvector development service
```

## Engineering focus

The repository is intentionally more than a collection of API calls. Experiments are designed around recurring AI Engineering concerns such as:

- keeping deterministic logic outside the model when possible;
- separating reusable AI logic from HTTP concerns;
- understanding retrieval quality rather than only generating answers;
- structured outputs and validation;
- metadata and retrieval provenance;
- testability;
- explicit configuration and secret management;
- evaluating non-deterministic behavior through repeatable datasets and metrics;
- detecting quality regressions;
- evolving isolated experiments into production-ready pipelines.

## RAG learning track

Retrieval-Augmented Generation is treated as an engineering pipeline rather than a single feature:

```text
                     INGESTION

Source content
     ↓
Chunking
     ↓
Embeddings
     ↓
Vector storage


                       QUERY

Question
     ↓
Retrieval
     ↓
Advanced retrieval / ranking
     ↓
Context construction
     ↓
LLM generation
     ↓
Evaluation
```

The ingestion side is already composed through `IngestionService`.

The query-side capabilities are implemented as separate experiments. Phase 2.6 will consolidate them into the first reusable end-to-end RAG query pipeline and connect that real pipeline to the existing evaluation Harness.

## Evaluation track

The RAG Harness separates pipeline execution from evaluation:

```text
Evaluation Dataset
       ↓
RagEvaluationRunner
       ↓
RagEvaluationHarness
       │
       ├── Retrieval metrics
       ├── Generation evaluator
       └── Grounding evaluator
       ↓
Summary / Baseline
       ↓
Regression Detection
```

The pipeline boundary is represented by `IRagEvaluationPipeline`, allowing evaluation logic to remain independent from the concrete RAG implementation.

## Running locally

Clone the repository:

```bash
git clone https://github.com/wilfigueredo/AI-ENGINEERING-LAB.git
cd AI-ENGINEERING-LAB
```

Restore and build:

```bash
dotnet restore
dotnet build
```

Run tests:

```bash
dotnet test
```

Start the PostgreSQL + pgvector development container when running the database-backed retrieval experiments:

```bash
docker compose up -d
```

Experiments that call external AI providers require local credentials/configuration. API keys should be supplied through environment variables or .NET secret management and must never be committed to source control.

## What this repository is — and is not

This repository **is**:

- a hands-on engineering laboratory;
- a record of practical AI Engineering learning;
- a place to compare abstractions with their underlying concepts;
- a portfolio of progressively more advanced .NET AI implementations.

It is **not** intended to be a single production product. Individual experiments may later influence or be promoted into dedicated applications when they become mature enough.

## Documentation

- `docs/roadmap.md` — learning roadmap and implementation status;
- `docs/architecture.md` — architecture as currently implemented;
- `docs/learning-goals.md` — competencies and learning objectives.

## Portfolio context

My professional background is primarily in backend development, C#/.NET and software architecture. AI Engineering Lab documents the next stage of that trajectory: applying established software-engineering practices to LLMs, RAG, agents and other AI-powered systems.

**Software Engineering → AI Engineering**

---

**William Figueiredo**

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

This is an **active learning and experimentation repository**. Concepts are introduced progressively and may evolve as implementations become more robust.

The repository currently explores or is structured to explore:

- LLM integration
- conversation history
- streaming responses
- tools and function calling
- Semantic Kernel
- structured outputs
- embeddings
- vector similarity and vector search
- Retrieval-Augmented Generation (RAG)
- AI orchestration
- observability
- retrieval evaluation
- token usage and cost analysis

## Learning path

The experiments follow an intentionally progressive path:

```text
LLM fundamentals
      ↓
Conversation & streaming
      ↓
Tools / function calling
      ↓
Semantic Kernel
      ↓
Structured outputs
      ↓
Embeddings
      ↓
Vector search
      ↓
RAG
      ↓
Evaluation & observability
      ↓
Advanced retrieval and agents
```

The goal is to understand each layer before relying on higher-level abstractions.

## Tech stack

- **.NET 9**
- **C#**
- **ASP.NET Core**
- **Microsoft.Extensions.AI**
- **Microsoft Semantic Kernel**
- **OpenAI integration**
- **xUnit**

## Repository structure

```text
src/
├── AiEngineeringLab.Api/       # API and experiment entry points
├── AiEngineeringLab.Core/      # Core abstractions and AI application logic
└── AiEngineeringLab.Plugins/   # Tools/plugins used by AI workflows

tests/
└── AiEngineeringLab.UnitTests/ # Automated tests

docs/                           # Notes and technical documentation
prompts/                        # Externalized prompts
```

## Engineering focus

The repository is intentionally more than a collection of API calls. Experiments are designed around recurring AI Engineering concerns such as:

- keeping deterministic logic outside the model when possible
- separating AI orchestration from application code
- understanding retrieval quality rather than only generating answers
- structured outputs and validation
- observability and token/cost awareness
- testability
- explicit configuration and secret management
- evaluating behavior as systems become less deterministic

## RAG learning track

Retrieval-Augmented Generation is treated as an engineering pipeline rather than a single feature:

```text
Source content
     ↓
Chunking
     ↓
Embeddings
     ↓
Vector storage / search
     ↓
Retrieval
     ↓
Context construction
     ↓
LLM generation
     ↓
Evaluation
```

This makes it possible to reason separately about retrieval quality, generation quality and the interaction between both layers.

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

Experiments that call external AI providers require local credentials/configuration. API keys should be supplied through environment variables or .NET secret management and must never be committed to source control.

## What this repository is — and is not

This repository **is**:

- a hands-on engineering laboratory;
- a record of practical AI Engineering learning;
- a place to compare abstractions with their underlying concepts;
- a portfolio of progressively more advanced .NET AI implementations.

It is **not** intended to be a single production product. Individual experiments may later influence or be promoted into dedicated applications when they become mature enough.

## Portfolio context

My professional background is primarily in backend development, C#/.NET and software architecture. AI Engineering Lab documents the next stage of that trajectory: applying established software-engineering practices to LLMs, RAG, agents and other AI-powered systems.

**Software Engineering → AI Engineering**

---

**William Figueiredo**

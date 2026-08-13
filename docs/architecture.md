# AI Engineering Lab — Architecture

## 1. Overview

The AI Engineering Lab is structured as a modular .NET solution used to
implement and observe AI Engineering concepts through isolated experiments.

The current architecture separates:

- HTTP/API concerns;
- reusable application models and services;
- AI tools and plugins;
- automated tests.

The laboratory currently integrates:

- Microsoft.Extensions.AI;
- OpenAI;
- Semantic Kernel;
- conversational history;
- streaming;
- function calling;
- embeddings;
- vector similarity.

The architecture is intentionally modular so new AI experiments can be added
without coupling all concepts directly to the API layer.

---

## 2. Solution Structure

```text
AI-ENGINEERING-LAB
│
├── src/
│   ├── AiEngineeringLab.Api
│   ├── AiEngineeringLab.Core
│   └── AiEngineeringLab.Plugins
│
├── tests/
│   └── AiEngineeringLab.UnitTests
│
├── docs/
├── prompts/
└── AiEngineeringLab.sln
```

## 3. Project Responsibilities

### AiEngineeringLab.Api

The API project is the executable entry point of the laboratory.

Current responsibilities include:

ASP.NET Core application startup;
dependency injection;
external configuration;
OpenAI client configuration;
Microsoft.Extensions.AI registration;
Semantic Kernel registration;
embedding generator registration;
HTTP endpoints;
chat requests;
streamed responses through Server-Sent Events;
cancellation handling;
experiment endpoints.

The API layer coordinates the components but should avoid containing reusable
domain or mathematical logic.

### AiEngineeringLab.Core

The Core project contains reusable application concepts that do not depend on
HTTP concerns.

Current responsibilities include:

chat request and response models;
conversation state;
conversation history management;
AI configuration models;
embedding comparison models;
vector similarity algorithms.

Examples of components currently located in Core include:

ConversationHistoryService
ConversationState
ChatRequest
ChatResult
AiOptions
EmbeddingComparisonRequest
VectorSimilarity

The Core project is intended to contain logic that can be tested independently
from ASP.NET Core and external AI providers whenever possible.

### AiEngineeringLab.Plugins

The Plugins project contains deterministic capabilities exposed to AI systems.

The laboratory currently demonstrates two approaches.

Microsoft.Extensions.AI tools
DateTimePlugin
    ↓
AiTools
    ↓
AIFunctionFactory
    ↓
AITool

This approach demonstrates tool creation through
Microsoft.Extensions.AI.

Current deterministic capabilities include operations such as:

obtaining the current date;
calculating the number of days between dates.
Semantic Kernel native plugins
TextPlugin
    ↓
[KernelFunction]
    ↓
KernelPlugin
    ↓
Semantic Kernel

This approach demonstrates native Semantic Kernel functions.

Current functions include:

count_words
to_upper_case

Keeping both implementations in the laboratory is intentional.

The goal is to demonstrate and compare different AI integration approaches
rather than force every experiment through a single framework.

### AiEngineeringLab.UnitTests

The test project validates deterministic components independently from the LLM
whenever possible.

Current tests include:

DateTime plugin behavior;
vector similarity calculations.

Vector similarity tests currently cover scenarios such as:

identical vectors;
orthogonal vectors;
opposite vectors;
incompatible dimensions.

External LLM behavior is not treated as deterministic unit-test logic.

## 4. Dependency Direction

The current dependency direction is conceptually:
```text

                 AiEngineeringLab.Api
                      │       │
                      │       │
                      ▼       ▼
      AiEngineeringLab.Core   AiEngineeringLab.Plugins
                                      │
                                      │
                                      ▼
                             AI framework abstractions
							 
							 
```

Tests consume the projects containing the behavior being validated:

```text

AiEngineeringLab.UnitTests
        │
        ├──► AiEngineeringLab.Core
        │
        └──► AiEngineeringLab.Plugins
		
```

The Core project should remain independent from the API project.

## 5. LLM Integration

Chat communication is abstracted through:

IChatClient

from Microsoft.Extensions.AI.

Conceptually:

```text
HTTP Request
     ↓
ChatController
     ↓
IChatClient
     ↓
AI Provider
     ↓
LLM
     ↓
Response
```

The application therefore interacts with an abstraction rather than coupling
the controller directly to provider-specific chat APIs.

## 6. Conversation History

Conversation state is maintained independently for each conversation.

Conceptually:

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

Each conversation contains:

its own message history;
its own synchronization gate.

The per-conversation SemaphoreSlim prevents concurrent operations from
modifying the same conversation state simultaneously while still allowing
different conversations to execute independently.

## 7. Streaming Architecture

The laboratory exposes streamed responses using Server-Sent Events.

Conceptually:

```text
Client
  │
  │ POST /stream
  ▼
ChatController
  │
  ▼
IChatClient.GetStreamingResponseAsync
  │
  ├── chunk
  ├── chunk
  ├── chunk
  │
  └── completed
  ▼
Client

```

The streaming implementation currently includes:

text/event-stream;
chunk events;
completion events;
error events;
response flushing;
cancellation support;
conversation rollback when generation is interrupted.
## 8. Cancellation

ASP.NET Core provides a:

CancellationToken

for request operations.

The token is propagated through asynchronous AI calls.

Conceptually:

```text
Client disconnects or cancels
           ↓
ASP.NET Core
           ↓
CancellationToken
           ↓
AI operation
           ↓
operation interruption
```

Streaming also restores the conversation to its previous state when an
incomplete interaction must not become part of the history.

## 9. Tool Calling with Microsoft.Extensions.AI

Tools are exposed to the chat model through ChatOptions.

Conceptually:

```text

Application
     ↓
ChatOptions.Tools
     ↓
LLM
     ↓
Function selection
     ↓
C# Tool
     ↓
Tool result
     ↓
LLM response
```

The LLM decides when an available tool is useful, while deterministic business
logic remains implemented in C#.

## 10. Semantic Kernel

Semantic Kernel is included as a second orchestration approach.

The current Kernel contains registered native plugins.

Conceptually:

```text
Kernel
  │
  └── Plugin: Text
          │
          ├── count_words
          └── to_upper_case
```

The laboratory currently demonstrates two invocation modes.

Direct invocation

```text
Application
    ↓
Kernel.InvokeAsync
    ↓
Explicit plugin/function
    ↓
KernelFunction
```

The application selects the function.

Automatic function calling

```text
Prompt
   ↓
Kernel.InvokePromptAsync
   ↓
LLM
   ↓
FunctionChoiceBehavior.Auto
   ↓
KernelFunction
   ↓
Function result
   ↓
LLM
   ↓
Final response
```

In this mode, the LLM decides whether a registered function should be invoked.

## 11. Embeddings

Embeddings are abstracted through:

IEmbeddingGenerator<string, Embedding<float>>

Conceptually:

```text
Text
  ↓
IEmbeddingGenerator
  ↓
Embedding Model
  ↓
Embedding<float>
  ↓
Vector<float>

```

The current embedding experiment exposes:

the original text;
vector dimensionality;
a preview of vector values.

The embedding model and chat model are treated as separate AI capabilities.

## 12. Vector Similarity

Vector comparison logic is located in Core rather than the controller.

Current flow:

```text
Text A                  Text B
  ↓                       ↓
Embedding A            Embedding B
       \                 /
        \               /
         ▼             ▼
          VectorSimilarity
                ↓
        Cosine Similarity
```

This separation allows the mathematical behavior to be unit tested without
making calls to an external AI provider.

Euclidean distance will be added as another vector metric during the remaining
embedding experiments.

## 13. Dependency Injection

AI services and application components are registered through the standard
ASP.NET Core dependency injection container.

Current registrations conceptually include:

IChatClient
IEmbeddingGenerator
ConversationHistoryService
DateTimePlugin
AiTools
TextPlugin
Kernel

Dependency injection is used to:

avoid manual dependency creation in controllers;
isolate implementations;
support testability;
centralize configuration.

## 14. Configuration and Secrets

Application configuration is externalized through configuration files and
options.

Examples include:

AI Provider
Chat Model ID
Embedding Model ID
Logging configuration

Sensitive credentials such as API keys must not be committed to the
repository.

Development secrets are expected to be stored outside version-controlled
configuration.

## 15. Current Architectural Boundary

The laboratory is an experimental environment.

Some HTTP endpoints exist specifically to make concepts observable, for
example:

Kernel inspection;
direct Kernel Function invocation;
embedding inspection;
embedding similarity comparison.

These endpoints are valid in the laboratory even when they would not belong
in the public API of a production application.

This distinction is intentional:

```text
AI Engineering Lab
        ↓
experimentation
comparison
learning
technical validation

Production Application
        ↓
only capabilities justified
by product requirements
```

## 16. Current Architecture

At the current stage, the main component relationship can be summarized as:

```text

                         Client
                           │
                           ▼
                 AiEngineeringLab.Api
                           │
                     ChatController
                           │
          ┌────────────────┼────────────────┐
          │                │                │
          ▼                ▼                ▼
 ConversationHistory   IChatClient         Kernel
          │                │                │
          │                ▼                ▼
          │             OpenAI      Semantic Kernel Plugins
          │                                 │
          │                                 ▼
          │                              TextPlugin
          │
          ├───────────────────────────────────────┐
          │                                       │
          ▼                                       ▼
      Chat History                          AI Tools
                                              │
                                              ▼
                                         DateTimePlugin


                      Embedding Flow

                         Client
                           │
                           ▼
                    ChatController
                           │
                           ▼
               IEmbeddingGenerator
                           │
                           ▼
                    OpenAI Embeddings
                           │
                           ▼
                    Embedding<float>
                           │
                           ▼
                    VectorSimilarity
					
```

## 17. Evolution Rule

New technologies should not be added merely to increase the number of
frameworks used by the repository.

A capability should be introduced when it provides at least one of:

a new AI Engineering concept to study;
an observable technical experiment;
a meaningful architectural comparison;
a reusable implementation pattern;
a measurable engineering trade-off.

Future capabilities are documented in roadmap.md and should only be added to
this architecture document after they are actually implemented.
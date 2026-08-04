# BOS AI Copilot

## Project Charter

### Document Metadata

| Field | Value |
|---|---|
| Project Name | BOS AI Copilot |
| Document Type | Project Charter |
| Version | 0.1.0 |
| Status | Draft |
| Project Stage | Initiation |
| Initial Release | Version 1.0 |
| Primary Technology | .NET, C# and Semantic Kernel |
| Project Owner | William Figueredo |
| Human Approval Required | Yes |

---

## 1. Project Context

The BOS AI Copilot is an artificial intelligence application designed to support the creation, analysis and evolution of projects based on the BOS Framework.

The application will provide a conversational interface through which users can describe a business problem, submit a project idea, identify missing information, request preliminary artifact structures, perform deterministic calculations and receive structured outputs for human review.

A Large Language Model will be used for interpretation, natural-language interaction and tool selection. Native C# functions will be used for deterministic operations that must not depend exclusively on model-generated reasoning.

The project will also serve as a practical learning laboratory for validating the knowledge acquired during the first stage of the transition from .NET Developer to AI Engineer.

Version 1.0 will focus on:

- LLM APIs;
- prompts;
- conversation history;
- Semantic Kernel;
- native plugins;
- function calling;
- structured outputs;
- streaming;
- dependency injection;
- error handling;
- logging;
- secure configuration.

More advanced capabilities will be added progressively after the technical foundation has been validated.

---

## 2. Problem Statement

Projects based on the BOS Framework depend on structured information, explicit decisions and standardized artifacts.

During project initiation, users may provide incomplete, ambiguous or unstructured information. Transforming this information into organized project structures currently depends on manual interpretation and documentation.

The project owner also needs a practical application through which the studied concepts of LLMs and Semantic Kernel can be implemented, tested and demonstrated.

The BOS AI Copilot addresses these needs by providing an assistant capable of:

- interpreting natural-language requests;
- preserving conversational context;
- identifying missing information;
- selecting appropriate application functions;
- transforming information into structured objects;
- performing deterministic calculations;
- generating preliminary BOS artifact structures;
- presenting results in a comprehensible format;
- preserving human review and approval.

---

## 3. Project Vision

Create an AI copilot capable of understanding user requests and supporting the structured initiation and evolution of projects within the BOS ecosystem.

The long-term vision is for the BOS AI Copilot to become an intelligent operational interface for the BOS Framework, capable of consulting official knowledge, interacting with external tools and coordinating specialized agents.

Version 1.0 will establish the technical foundation for this evolution without prematurely introducing RAG, MCP or autonomous agents.

---

## 4. Project Objectives

### 4.1 Primary Objective

Develop a functional .NET application using Semantic Kernel that allows users to converse with an LLM and execute project-related operations through native C# plugins and automatic function calling.

### 4.2 Secondary Objectives

The project must:

1. demonstrate integration between a .NET application and an LLM provider;
2. preserve conversational context during the active application session;
3. separate the system prompt from the main orchestration code;
4. register native C# functions as Semantic Kernel plugins;
5. allow the model to select an appropriate function automatically;
6. execute deterministic calculations in C#;
7. generate structured and typed responses for selected operations;
8. present supported responses progressively through streaming;
9. handle expected configuration, provider, plugin and serialization failures;
10. register operational information about each interaction without exposing secrets;
11. maintain the API key outside the source code and repository;
12. produce an application suitable for professional portfolio demonstration;
13. establish a technical base for future versions involving RAG, MCP and agents.

---

## 5. Expected Value

### 5.1 Learning Value

The project will validate practical understanding of:

- model configuration;
- system and user prompts;
- tokens and context windows;
- generation parameters;
- chat completion;
- conversation history;
- Semantic Kernel configuration;
- plugin registration;
- function calling;
- structured output;
- JSON serialization;
- streaming;
- dependency injection;
- logging;
- error handling;
- secure configuration.

### 5.2 Portfolio Value

The project will demonstrate the transition from traditional .NET development to AI Engineering through a real application containing:

- LLM integration;
- tool use;
- deterministic C# functions;
- structured outputs;
- automated tests;
- documentation;
- observability;
- extensible technical foundations.

### 5.3 BOS Ecosystem Value

The project will provide the initial intelligent interface through which future users may operate the BOS Framework.

### 5.4 Strategic Value

The same application will evolve during subsequent learning stages, avoiding disconnected demonstration projects for each new AI capability.

---

## 6. Scope

### 6.1 In Scope for Version 1.0

Version 1.0 includes:

- ASP.NET Core Web API;
- simple console or minimal web interface;
- configurable LLM provider;
- Semantic Kernel configuration;
- chat completion service;
- session-based conversation history;
- external system prompt;
- streaming responses;
- native C# plugins;
- automatic function calling;
- typed response models;
- JSON serialization and deserialization;
- deterministic cost calculations;
- date and deadline calculations;
- initial project analysis;
- missing-information identification;
- project-stage classification;
- objective definition;
- success-criteria definition;
- preliminary Project Charter generation;
- preliminary requirement generation;
- preliminary acceptance-criteria generation;
- dependency injection;
- external configuration;
- secure API-key management;
- error handling;
- interaction logging;
- unit tests;
- integration tests for critical flows.

### 6.2 Initial Plugins

#### ProjectPlugin

Responsibilities:

- create an initial project draft;
- identify missing information;
- define project objectives;
- define success criteria;
- classify the current project stage.

Initial functions:

- `CreateProjectDraft`
- `IdentifyMissingInformation`
- `DefineProjectObjectives`
- `DefineSuccessCriteria`
- `ClassifyProjectStage`

#### CostEstimationPlugin

Responsibilities:

- perform deterministic cost calculations;
- prevent arithmetic from depending exclusively on the LLM.

Initial functions:

- `EstimateTokenCost`
- `EstimateMonthlyOperatingCost`
- `CalculateCostPerInteraction`

#### DateTimePlugin

Responsibilities:

- provide current date information;
- calculate deadlines and date intervals;
- validate simple tool-selection scenarios.

Initial functions:

- `GetCurrentDate`
- `CalculateDeadline`
- `CalculateDaysBetweenDates`

#### ArtifactPlugin

Responsibilities:

- generate preliminary structured artifact drafts;
- apply the minimum expected structure of selected BOS artifacts.

Initial functions:

- `CreateProjectCharterDraft`
- `CreateRequirementDraft`
- `CreateAcceptanceCriteria`

---

## 7. Out of Scope for Version 1.0

The following capabilities are outside the scope of the first version:

- Retrieval-Augmented Generation;
- embeddings;
- vector databases;
- semantic search over BOS documents;
- complete ingestion of the BOS repository;
- Model Context Protocol;
- autonomous agents;
- multi-agent orchestration;
- persistent conversational memory;
- WhatsApp integration;
- automatic generation of official files;
- automatic publication to GitHub;
- automatic modification of the BOS repository;
- autonomous approval of artifacts;
- production-scale infrastructure;
- advanced AI evaluations;
- distributed caching;
- enterprise access control.

These items may only enter the project through a future version or explicit scope revision.

---

## 8. Target Users

### 8.1 Primary User

The initial primary user is the project owner, who will use the application to:

- validate AI Engineering knowledge;
- operate demonstration scenarios;
- generate preliminary project structures;
- evaluate function calling;
- prepare the application for portfolio presentation.

### 8.2 Future Users

Future users may include:

- business analysts;
- product owners;
- software architects;
- project managers;
- developers;
- BOS Framework contributors;
- professionals initiating projects with incomplete information.

Future users are not part of the acceptance scope for Version 1.0.

---

## 9. Stakeholders

| Stakeholder | Responsibility |
|---|---|
| Project Owner | Defines priorities, approves scope and validates the result |
| Developer | Implements the application, plugins, tests and infrastructure |
| AI Engineer Learner | Validates the AI concepts represented by the implementation |
| BOS Framework Maintainer | Ensures conceptual alignment with the BOS ecosystem |
| Human Reviewer | Reviews preliminary artifacts before approval |
| LLM Provider | Provides chat-completion and function-calling capabilities |

During Version 1.0, multiple stakeholder roles may be performed by the same person.

---

## 10. High-Level Functional Capabilities

The application must allow the user to:

1. send natural-language messages;
2. receive responses from the configured model;
3. continue a contextual conversation during the active session;
4. request explanations about BOS concepts supported by the current prompt knowledge;
5. submit a project idea in unstructured form;
6. receive an initial structured project representation;
7. identify important missing project information;
8. request project objectives and success criteria;
9. classify a project according to its current stage;
10. request deterministic cost calculations;
11. request date and deadline calculations;
12. request preliminary artifact structures;
13. receive typed JSON results when required;
14. receive a comprehensible natural-language explanation of structured results;
15. receive controlled error responses when execution fails.

---

## 11. High-Level Non-Functional Requirements

### 11.1 Security

- The API key must not be committed to Git.
- The API key must not appear in logs.
- The API key must not be returned in application responses.
- Sensitive configuration must use environment variables, user secrets or another approved secret provider.
- Logs must not store confidential prompt content without an explicit decision.

### 11.2 Reliability

- Provider failures must be handled without unexpectedly terminating the application.
- Plugin failures must produce controlled error responses.
- Invalid structured responses must be detected.
- Operations must support cancellation and timeout controls.

### 11.3 Maintainability

- Prompts must be separated from the main orchestration code.
- Plugins must have clear and limited responsibilities.
- Domain models must not depend directly on the LLM provider.
- The architecture must remain understandable to a developer learning Semantic Kernel.
- Abstractions must only be introduced when they solve an identified problem.

### 11.4 Observability

Each interaction must register, when available:

- model used;
- operation duration;
- plugin or function invoked;
- success or failure;
- error category;
- token usage reported by the provider.

### 11.5 Testability

- Deterministic plugin functions must be unit-testable without an LLM.
- Critical orchestration flows must have integration tests.
- Tests must not require production credentials.
- External-provider behavior should be replaceable by test doubles where practical.

### 11.6 Usability

- Responses must be understandable to users who do not know the internal implementation.
- Structured results must be accompanied by a human-readable presentation.
- Missing information must be clearly distinguished from assumptions.
- Preliminary artifacts must be explicitly identified as drafts requiring human review.

---

## 12. Constraints

The project is subject to the following constraints:

- the primary programming language must be C#;
- the main application platform must be .NET;
- Semantic Kernel must be used for the initial LLM orchestration;
- Version 1.0 must not depend on a vector database;
- Version 1.0 must not implement autonomous agents;
- no persistent database is required for the first release;
- conversation history will exist only during the active session;
- the LLM provider must be externally configurable;
- the initial interface must remain simple;
- AI learning must take priority over visual-interface sophistication;
- generated artifacts must remain preliminary until reviewed;
- deterministic calculations must be performed by native code;
- the architecture must not hide Semantic Kernel behind unnecessary abstractions.

---

## 13. Assumptions

The project assumes that:

- the selected provider supports chat completion;
- the selected model supports tool or function calling;
- the provider may expose token-usage information;
- the development environment supports a maintained .NET version;
- the project owner has access to a valid provider credential;
- the application will initially run in a controlled development environment;
- one active user per execution is sufficient for Version 1.0;
- the user will review generated information before treating it as official;
- initial BOS artifact structures can be represented through typed C# models;
- future provider changes may require infrastructure adjustments.

---

## 14. Risks

| Risk | Impact | Initial Mitigation |
|---|---|---|
| Excessive architectural complexity | Semantic Kernel learning becomes hidden | Use the minimum architecture necessary |
| Scope expansion | Version 1.0 is delayed | Maintain an explicit out-of-scope list |
| Model hallucination | Incorrect information is presented as fact | Separate known facts, missing information and assumptions |
| Incorrect tool selection | The model invokes an inappropriate function | Use clear function descriptions and scenario tests |
| Invalid structured output | A response cannot be deserialized | Apply validation and controlled error handling |
| Provider dependency | The application becomes tightly coupled | Externalize configuration and isolate provider details |
| Unexpected API cost | Development consumes uncontrolled resources | Log usage and establish limits |
| API-key exposure | Credentials are compromised | Use secret management and log sanitization |
| Non-deterministic tests | Test results become unstable | Keep deterministic logic outside the model |
| Premature BOS automation | Drafts are treated as official artifacts | Require explicit human review |
| Prompt growth | Conversation exceeds the context window | Define session limits and later add summarization |
| Dependency changes | Framework or provider APIs change | Pin dependencies and document upgrades |

---

## 15. Success Criteria

### SC-001 — Contextual Conversation

Given that the user says:

> Quero criar um projeto para um restaurante.

And subsequently says:

> O objetivo é automatizar o WhatsApp.

The application must understand that both messages refer to the same project during the active session.

### SC-002 — Automatic Function Calling

Given the request:

> Qual seria o custo mensal de 10 mil interações a R$ 0,03 por interação?

The application must select a deterministic C# calculation function and return:

> R$ 300,00 por mês.

The arithmetic result must not be produced solely by the model.

### SC-003 — Correct Tool Selection

Given the request:

> Crie os objetivos desse projeto.

The application must select the project-objective function instead of a cost, date or unrelated artifact function.

### SC-004 — Structured Output

Given the request:

> Gere um rascunho do Project Charter.

The result must be converted into a typed C# object containing the mandatory Project Charter fields.

### SC-005 — Missing Information Detection

Given the request:

> Crie um projeto de inteligência artificial.

The application must identify important missing information instead of inventing a complete context.

### SC-006 — Streaming

The user must observe supported responses being delivered progressively.

### SC-007 — Error Handling

The application must return controlled errors for:

- missing API key;
- provider unavailability;
- timeout;
- invalid provider response;
- plugin execution failure;
- JSON deserialization failure.

### SC-008 — Secure Configuration

The provider API key must not:

- exist in source code;
- be committed to Git;
- appear in logs;
- be returned to the user;
- be sent unnecessarily to plugins.

### SC-009 — Interaction Logging

Each interaction must record:

- model identifier;
- duration;
- execution outcome;
- invoked function, when applicable;
- token usage, when available;
- sanitized error information.

### SC-010 — Automated Tests

All deterministic plugin functions must have automated unit tests, and critical function-calling and structured-output flows must have integration-test coverage.

---

## 16. Deliverables

Version 1.0 will produce:

1. Project Charter;
2. functional and non-functional requirements;
3. use cases;
4. acceptance criteria;
5. initial backlog;
6. architecture documentation;
7. source-code repository;
8. ASP.NET Core Web API;
9. simple user interface or console client;
10. Semantic Kernel configuration;
11. external prompt files;
12. ProjectPlugin;
13. CostEstimationPlugin;
14. DateTimePlugin;
15. ArtifactPlugin;
16. typed request and response models;
17. interaction logging;
18. error-handling strategy;
19. unit tests;
20. integration tests;
21. configuration and execution guide;
22. demonstration scenarios;
23. portfolio-oriented README.

---

## 17. Initial Technical Direction

The initial repository structure is:

```text
BOS-AI-Copilot
├── docs
├── prompts
├── src
└── tests
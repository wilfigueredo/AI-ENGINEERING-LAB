# Requirements

## Functional Requirements

### RF-001 — Chat with LLM

The application shall allow the user to send a message and receive a response from the configured LLM.

---

### RF-002 — Conversation History

The application shall preserve the conversation during the current session.

---

### RF-003 — External System Prompt

The system prompt shall be stored outside the application source code.

---

### RF-004 — Streaming

The application shall support streamed responses when available.

---

### RF-005 — Semantic Kernel Plugins

The application shall register native C# plugins.

---

### RF-006 — Automatic Function Calling

The configured model shall automatically select and invoke available functions.

---

### RF-007 — Structured Output

Selected operations shall return typed JSON objects.

---

### RF-008 — External Configuration

Model, endpoint and configuration shall be externalized.

---

### RF-009 — Error Handling

The application shall handle provider, plugin and serialization errors.

---

### RF-010 — Interaction Logging

Each interaction shall register execution information.

---

# Non Functional Requirements

### RNF-001

API Keys shall never be stored in source code.

### RNF-002

The application shall execute locally.

### RNF-003

Deterministic calculations shall be implemented in C#.

### RNF-004

Plugins shall be independently testable.

### RNF-005

The LLM provider shall be configurable.

---

# Out of Scope

- RAG
- Embeddings
- Vector Database
- MCP
- Agents
- Persistent Memory
- WhatsApp
- Authentication
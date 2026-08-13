# AI Engineering Lab — Roadmap

Este roadmap organiza a evolução prática do laboratório de AI Engineering em .NET.

O objetivo é estudar, implementar, testar e comparar conceitos de IA de forma progressiva antes de aplicá-los em projetos de produto.

## 2.1 Estrutura da aplicação

✅ Concluído

- solução .NET
- separação em projetos
- API
- Core
- Plugins
- Unit Tests
- configuração externa
- injeção de dependência

## 2.2 Integração com LLM / Microsoft.Extensions.AI

✅ Concluído

- integração com OpenAI
- `IChatClient`
- configuração do modelo
- abstração via `Microsoft.Extensions.AI`
- chamadas assíncronas
- gerenciamento seguro de API Key

## 2.3 Histórico contextual

✅ Concluído

- histórico por `conversationId`
- `ConcurrentDictionary`
- `ConversationState`
- `SemaphoreSlim` por conversa
- controle de concorrência
- limpeza de conversa

## 2.4 Streaming + SSE

✅ Concluído

- streaming de respostas
- Server-Sent Events
- eventos `chunk`
- evento `completed`
- evento `error`
- `CancellationToken`
- rollback de histórico em falha/cancelamento

## 2.5 Tools

✅ Concluído

- tools nativas em C#
- `AIFunctionFactory`
- `DateTimePlugin`
- funções determinísticas
- logging explícito das tools

## 2.6 Function Calling

✅ Concluído

- exposição de tools ao modelo
- seleção de função pelo LLM
- execução automática
- integração com fluxo de chat
- integração com streaming

## 2.7 Semantic Kernel

✅ Concluído

### 2.7.1 Kernel e configuração
✅

### 2.7.2 Plugins nativos
✅

### 2.7.3 KernelFunction
✅

### 2.7.4 Invocação direta com `Kernel.InvokeAsync`
✅

### 2.7.5 Integração com pipeline atual
✅

### 2.7.6 Function Calling automático
✅

- `InvokePromptAsync`
- `FunctionChoiceBehavior.Auto`
- seleção de `KernelFunction` pelo LLM
- execução automática pelo Kernel

## 2.8 Embeddings

🟡 Em andamento

### 2.8.1 Conceito e arquitetura
✅

### 2.8.2 Configuração do modelo de embeddings
✅

### 2.8.3 Geração de embeddings
✅

- `IEmbeddingGenerator`
- `Embedding<float>`
- `text-embedding-3-small`
- inspeção da dimensionalidade

### 2.8.4 Cosine Similarity
✅

- comparação entre vetores
- similaridade semântica
- testes com textos relacionados e não relacionados

### 2.8.5 Distância Euclidiana
⬜

### 2.8.6 Comparação entre métricas
⬜

- Cosine Similarity
- Euclidean Distance
- quando usar cada abordagem

### 2.8.7 Testes
🟡

- vetores idênticos
- vetores ortogonais
- vetores opostos
- dimensões incompatíveis

## 2.9 Vector Search

⬜

- armazenamento vetorial
- indexação
- busca por similaridade
- Top K
- similarity threshold
- metadados
- filtros
- ranking

## 2.10 RAG

⬜

- ingestão de documentos
- chunking
- embeddings dos chunks
- armazenamento vetorial
- retrieval
- Top K
- montagem de contexto
- resposta fundamentada
- referências às fontes
- avaliação de retrieval
- Precision
- Recall

## 2.11 Structured Outputs

⬜

- schemas
- respostas tipadas
- JSON estruturado
- desserialização segura
- validação
- tratamento de saída inválida

## 2.12 AI Orchestration Patterns

⬜

- combinação de LLM + Tools
- RAG + Tools
- Structured Outputs
- múltiplas etapas
- decisões de orquestração
- execução determinística vs probabilística

## 2.13 Qualidade e entrega

⬜

- testes unitários
- testes de integração
- avaliação de retrieval
- Precision
- Recall
- tratamento de erros
- segurança de configuração
- secrets
- documentação
- preparação do repositório para portfólio

## 2.14 Observabilidade

⬜

- logging estruturado
- uso de tokens
- latência
- duração das tools
- duração de Kernel Functions
- métricas de retrieval
- chunks recuperados
- similarity scores
- custo estimado por requisição
- telemetria essencial

## Parâmetros de geração

Também serão estudados e implementados durante o laboratório:

- Temperature
- Top P
- interação entre Temperature e Top P
- determinismo vs criatividade
- suporte do provider a Top K de geração
- distinção entre Top K de geração e Top K de retrieval

## Princípio do laboratório

Cada conceito deve passar por quatro etapas:

1. compreensão conceitual;
2. implementação prática;
3. teste ou experimento observável;
4. avaliação de quando usar em uma aplicação real.

O laboratório não tem como objetivo acumular tecnologias, mas demonstrar entendimento técnico e capacidade de aplicação.
# AI Engineering Lab — Roadmap

Este roadmap organiza a evolução prática do laboratório de AI Engineering em .NET.

O objetivo é estudar, implementar, testar, medir e comparar conceitos de IA de forma progressiva antes de aplicá-los em projetos de produto.

## Regra do laboratório

Cada competência deve seguir, sempre que fizer sentido, este ciclo:

1. compreensão conceitual;
2. implementação mínima funcional;
3. experimento observável;
4. testes;
5. documentação;
6. comparação de alternativas e trade-offs.

Quando várias tecnologias resolvem essencialmente o mesmo problema, a regra será:

> **uma implementação prática aprofundada + estudo conceitual/comparativo das principais alternativas.**

O objetivo não é acumular frameworks, bancos ou SDKs, mas demonstrar entendimento técnico, capacidade de implementação e critério de escolha.

---

# Fase 1 — Fundamentos de IA Generativa

**Objetivo:** entender como os principais componentes de aplicações com LLM funcionam e construir uma aplicação .NET capaz de conversar com um modelo.

## 1.1 Fundamentos conceituais

✅ Concluído

- LLMs;
- tokens;
- context window;
- Temperature;
- Top P;
- prompts e papéis `System`, `User` e `Assistant`;
- APIs da OpenAI;
- embeddings em nível conceitual;
- Function Calling em nível conceitual.

## 1.2 Estrutura da aplicação .NET

✅ Concluído

- solução .NET;
- separação em projetos;
- API;
- Core;
- Plugins;
- Unit Tests;
- configuração externa;
- injeção de dependência.

## 1.3 Integração com LLM / Microsoft.Extensions.AI

✅ Concluído

- integração com OpenAI;
- `IChatClient`;
- configuração do modelo;
- abstração via `Microsoft.Extensions.AI`;
- chamadas assíncronas;
- gerenciamento seguro de API Key;
- abstração entre aplicação e provider.

## 1.4 Histórico contextual

✅ Concluído

- histórico por `conversationId`;
- `ConcurrentDictionary`;
- `ConversationState`;
- `SemaphoreSlim` por conversa;
- controle de concorrência;
- limpeza de conversa.

## 1.5 Streaming + SSE

✅ Concluído

- streaming de respostas;
- Server-Sent Events;
- eventos `chunk`;
- evento `completed`;
- evento `error`;
- `CancellationToken`;
- rollback de histórico em falha ou cancelamento.

## 1.6 Tools

✅ Concluído

- tools nativas em C#;
- `AIFunctionFactory`;
- `DateTimePlugin`;
- funções determinísticas;
- logging explícito das tools.

## 1.7 Function Calling

✅ Concluído

- exposição de tools ao modelo;
- seleção de função pelo LLM;
- execução automática;
- integração com o fluxo de chat;
- integração com streaming;
- decisão entre executar código determinístico e usar o LLM.

## 1.8 Semantic Kernel

✅ Concluído

- Kernel e configuração;
- plugins nativos;
- `KernelFunction`;
- invocação direta com `Kernel.InvokeAsync`;
- integração com o pipeline atual;
- `InvokePromptAsync`;
- `FunctionChoiceBehavior.Auto`;
- seleção e execução automática de funções pelo Kernel.

**Entrega da fase:** aplicação em C# capaz de conversar com um LLM utilizando API, manter contexto, fazer streaming e executar ferramentas.

---

# Fase 2 — Retrieval, RAG e Evaluation

**Objetivo:** entender, implementar e avaliar sistemas que recuperam conhecimento externo para uso por LLMs.

## 2.1 Retrieval Fundamentals

🟡 Em andamento

### 2.1.1 Arquitetura RAG

✅ Conceito estudado

- fluxo de ingestão;
- embeddings;
- armazenamento;
- retrieval;
- construção de contexto;
- geração fundamentada.

### 2.1.2 Embeddings para Retrieval

✅ Implementação iniciada

- `IEmbeddingGenerator`;
- `Embedding<float>`;
- modelo `text-embedding-3-small`;
- dimensionalidade;
- inspeção de vetores;
- representação semântica.

### 2.1.3 Similaridade Vetorial

🟡 Em andamento

- ✅ Cosine Similarity;
- ⬜ Dot Product;
- ⬜ Euclidean Distance;
- ⬜ comparação prática entre as métricas;
- ⬜ quando usar cada abordagem.

### 2.1.4 Top-K Retrieval

⬜ Pendente

- conceito de Top K;
- ranking por similaridade;
- similarity threshold;
- distinção entre Top K de geração e Top K de retrieval.

### 2.1.5 Recall e Precision

⬜ Pendente

- Precision;
- Recall;
- relação entre relevância e cobertura;
- trade-off entre recuperar pouco conteúdo de alta precisão e recuperar mais conteúdo com maior recall.

### 2.1.6 Testes de Similaridade

🟡 Em andamento

- ✅ vetores idênticos;
- ✅ vetores ortogonais;
- ✅ vetores opostos;
- ✅ dimensões incompatíveis;
- ⬜ testes adicionais para Dot Product e Euclidean Distance.

**Projeto:** pipeline básico de recuperação sem depender ainda de uma infraestrutura vetorial complexa.

---

## 2.2 Chunking e Indexação

⬜ Pendente

**Objetivo:** preparar documentos corretamente antes da criação do índice vetorial.

### Aprender e implementar

- Fixed Chunking;
- Recursive Chunking;
- Semantic Chunking;
- Sliding Window;
- overlap;
- tamanho de chunk;
- relação entre chunk size, contexto e qualidade de retrieval;
- metadata;
- pipeline de ingestão;
- reindexação;
- versionamento de conteúdo quando aplicável.

**Projeto:** comparar estratégias de chunking usando o mesmo conjunto de documentos e observar diferenças de recuperação.

---

## 2.3 Vector Databases

⬜ Pendente

**Objetivo:** armazenar, indexar e recuperar embeddings utilizando infraestrutura vetorial real.

### Estratégia da fase

Não serão implementados vários bancos vetoriais em profundidade.

Será adotada a regra:

> **1 banco vetorial com implementação prática aprofundada + estudo conceitual/comparativo dos demais.**

### Implementação prática principal

⬜ PostgreSQL + `pgvector`

- schema para documentos e chunks;
- persistência de embeddings;
- indexação;
- consulta por similaridade;
- Top K;
- metadata filters;
- integração com a aplicação .NET.

### Estudo conceitual e comparativo

⬜

- ChromaDB;
- Pinecone;
- Weaviate;
- diferenças entre serviço gerenciado e banco operado pela aplicação;
- custo;
- escalabilidade;
- experiência de desenvolvimento;
- filtros e metadata;
- cenários de escolha.

### Conceitos de indexação vetorial

⬜

- Exact Search x Approximate Nearest Neighbor (ANN);
- HNSW;
- recall de busca aproximada;
- velocidade x qualidade;
- impacto de parâmetros do índice.

**Projeto:** indexação e busca vetorial real utilizando PostgreSQL + `pgvector`.

---

## 2.4 Retrieval Avançado

⬜ Pendente

**Objetivo:** melhorar a qualidade do retrieval além da busca vetorial simples.

### Aprender e experimentar

- Hybrid Search;
- BM25 em nível conceitual e prático quando adequado;
- Query Expansion;
- Query Rewriting;
- Re-ranking;
- MMR — Maximal Marginal Relevance;
- Context Compression;
- metadata filters avançados;
- combinação entre busca lexical e semântica;
- impacto de cada técnica na precisão e no recall.

**Projeto:** evoluir o pipeline de busca e comparar a qualidade antes e depois das técnicas avançadas.

---

## 2.5 AI Harness e Evaluation

⬜ Pendente

**Objetivo:** aprender a avaliar aplicações de IA de forma sistemática e reproduzível.

### Datasets de avaliação

- criação de conjuntos de perguntas e respostas esperadas;
- casos positivos e negativos;
- casos ambíguos;
- versionamento do dataset;
- dataset de regressão.

### Retrieval Evaluation

- Precision;
- Recall;
- Precision@K;
- Recall@K;
- MRR — Mean Reciprocal Rank;
- relevance;
- ranking quality;
- cobertura de conhecimento.

### Answer Evaluation

- groundedness;
- faithfulness;
- answer relevance;
- precisão factual quando mensurável;
- presença de informação não suportada pelo contexto recuperado.

### Harness de testes

- avaliação automática de respostas;
- testes de regressão para RAG;
- comparação entre versões de prompts;
- comparação entre estratégias de chunking;
- comparação entre pipelines de retrieval;
- comparação antes/depois de re-ranking ou query rewriting;
- métricas reproduzíveis.

**Projeto:** criar um conjunto de testes para o assistente RAG e medir quantitativamente a qualidade antes e depois das melhorias do pipeline.

---

# Fase 3 — Agentes e Automação

**Objetivo:** evoluir de aplicações que apenas respondem prompts para sistemas capazes de decidir, agir, manter estado, planejar e colaborar.

## 3.1 Fundamentos de AI Agents

⬜ Pendente

- o que caracteriza um agente;
- LLM x chatbot x workflow x agente;
- agent loop: observar → decidir → agir → avaliar;
- objetivos, estado e contexto;
- autonomia e limites;
- agente determinístico x comportamento probabilístico.

**Resultado:** compreender conceitualmente a arquitetura de um agente.

---

## 3.2 Tools e Function Calling para Agentes

🟡 Parcialmente concluído

Tools e Function Calling já foram introduzidos na Fase 1. Nesta fase serão aprofundados no contexto de agentes.

### Já implementado

- ✅ tools como capacidades executáveis;
- ✅ Function Calling;
- ✅ seleção automática de ferramentas;
- ✅ múltiplas abordagens com `Microsoft.Extensions.AI` e Semantic Kernel;
- ✅ logging de execução de tools.

### Aprofundar

- parâmetros e retorno estruturado;
- múltiplas tools em um mesmo fluxo;
- tratamento de falhas;
- retry de tools;
- autorização para execução;
- quando executar código C# versus usar o LLM;
- composição de tools em tarefas com múltiplos passos.

**Resultado:** agente capaz de escolher e executar ferramentas reais de forma controlada.

---

## 3.3 Estado, Memória e Planejamento

⬜ Pendente

- estado do agente;
- short-term memory;
- long-term memory;
- histórico x memória;
- RAG como memória/conhecimento;
- planejamento de tarefas;
- decomposição de objetivo em etapas;
- observação de resultados;
- replanejamento após resultados;
- persistência de estado quando necessária.

**Resultado:** agente capaz de executar tarefas com múltiplos passos mantendo contexto.

---

## 3.4 Agente Autônomo

⬜ Pendente

Fluxo de referência:

```text
Objetivo
   ↓
Agente
   ↓
Planejamento
   ↓
Escolha da Tool
   ↓
Execução
   ↓
Observação do resultado
   ↓
Próxima decisão
   ↓
Resultado final
```

### Estudar e implementar

- loop de execução;
- critérios de parada;
- limite de iterações;
- timeout;
- tratamento de erros;
- custo e tokens;
- human-in-the-loop;
- prevenção de execução infinita;
- idempotência quando aplicável.

**Resultado:** primeiro agente completo, com autonomia limitada e comportamento observável.

---

## 3.5 Multi-Agent Systems

⬜ Pendente

- especialização de agentes;
- responsabilidades;
- comunicação entre agentes;
- compartilhamento de contexto;
- isolamento de contexto;
- delegação;
- handoff;
- padrão supervisor/worker;
- conflitos entre agentes;
- consolidação de resultados.

Exemplo conceitual:

```text
                Orchestrator
              /      |      \
             ↓       ↓       ↓
        Research   Analyst   Writer
             \       |       /
              ↓      ↓      ↓
                 Resultado
```

**Resultado:** compreender e implementar colaboração entre agentes especializados.

---

## 3.6 Orquestração de Agentes

⬜ Pendente

**Objetivo:** transformar vários agentes independentes em uma arquitetura com fluxo de execução controlado.

- Sequential Orchestration;
- Concurrent Orchestration;
- Handoff;
- Supervisor;
- Routing;
- workflow baseado em estado;
- dependências entre tarefas;
- retry/fallback;
- controle de execução;
- cancelamento;
- propagação de erros;
- consolidação de saídas.

**Resultado:** sistema multiagente com fluxo controlado e decisões arquiteturais explícitas.

---

## 3.7 Reliability, Observability, Security e Guardrails

⬜ Pendente

**Objetivo:** sair de uma demonstração funcional e pensar como AI Engineer responsável por sistemas utilizáveis em produção.

### Observabilidade

- logging estruturado;
- tracing;
- execução e duração de tools;
- duração de Kernel Functions;
- consumo de tokens;
- latência;
- métricas de retrieval;
- chunks recuperados;
- similarity scores;
- custo estimado por requisição;
- falhas e retries;
- telemetria essencial.

### Structured Outputs e validação

- JSON estruturado;
- schemas;
- respostas tipadas;
- desserialização segura;
- validação;
- tratamento de saída inválida;
- contratos de entrada e saída para tools e agentes.

### Guardrails e controle de autonomia

- limites de autonomia;
- critérios de parada;
- limite de iterações;
- permissões;
- autorização de tools;
- human approval / human-in-the-loop;
- prevenção de loops;
- validação de resultados.

### Segurança de aplicações com IA

- prompt injection;
- indirect prompt injection;
- separação entre system instructions, user input e retrieved content;
- proteção do contexto RAG;
- prevenção de exposição indevida de conhecimento recuperado;
- secrets management;
- princípio do menor privilégio para tools;
- validação de argumentos antes da execução;
- rate limiting quando aplicável.

### Custo e resiliência

- custo por request;
- caching;
- redução de chamadas desnecessárias ao modelo;
- model/provider fallback;
- timeout;
- retries com política controlada;
- degradação graciosa.

**Resultado:** compreender como transformar agentes e aplicações com IA em sistemas seguros, observáveis, avaliáveis e operáveis.

---

# Competências transversais

Alguns assuntos atravessam várias fases e devem ser revisitados conforme o laboratório evolui.

## Parâmetros de geração

- Temperature;
- Top P;
- interação entre Temperature e Top P;
- determinismo x criatividade;
- capacidades específicas do provider;
- diferença entre Top K de geração e Top K de retrieval.

## Provider Abstraction

- abstrações do `Microsoft.Extensions.AI`;
- redução de acoplamento com providers;
- diferenças entre capacidades dos modelos;
- impacto arquitetural de trocar modelo ou provider.

## Testabilidade

- lógica determinística testada sem LLM quando possível;
- testes unitários;
- testes de integração;
- testes de regressão;
- datasets de avaliação;
- experimentos reproduzíveis.

## Documentação

A documentação deve registrar:

- conceito estudado;
- implementação realizada;
- experimento;
- resultados;
- limitações;
- trade-offs;
- decisão de quando utilizar a abordagem.

---

# Resultado esperado do AI Engineering Lab

Ao concluir o roadmap, o laboratório deverá demonstrar capacidade prática para:

- integrar aplicações .NET com LLMs;
- utilizar abstrações de providers;
- trabalhar com tools e Function Calling;
- utilizar Semantic Kernel com critério;
- gerar e comparar embeddings;
- construir busca vetorial;
- implementar ingestão e chunking;
- construir pipelines RAG;
- melhorar retrieval com técnicas avançadas;
- avaliar retrieval e respostas quantitativamente;
- implementar Structured Outputs;
- construir agentes com estado, memória e planejamento;
- implementar sistemas multiagentes;
- orquestrar execução;
- instrumentar custo, tokens e latência;
- aplicar guardrails e controles de segurança;
- avaliar trade-offs arquiteturais antes de levar uma técnica para um produto real.

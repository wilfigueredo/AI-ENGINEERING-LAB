# AI Engineering Lab — Roadmap

Roadmap prático para evolução de **AI Engineering com .NET como stack principal**, avançando de fundamentos de LLMs e Transformers para **RAG profissional, Agentic AI, arquitetura de sistemas de IA em produção, Python/Multimodal e um Capstone integrador**.

> **Objetivo final:** formar um AI Engineer capaz de construir sistemas de IA robustos, avaliáveis, escaláveis e seguros usando .NET como stack principal, com RAG, agentes, MCP, segurança, arquitetura production-ready, expansão prática para Python e IA multimodal e um projeto integrador final.

## Regra do laboratório

Cada competência deve seguir, sempre que fizer sentido:

1. compreensão conceitual;
2. implementação mínima funcional;
3. experimento observável;
4. testes;
5. documentação;
6. comparação de alternativas e trade-offs.

Quando várias tecnologias resolvem essencialmente o mesmo problema:

> **uma implementação prática aprofundada + estudo conceitual/comparativo das principais alternativas.**

O objetivo não é acumular frameworks, bancos ou SDKs, mas demonstrar entendimento técnico, capacidade de implementação e critério de escolha.

## Status geral

- ✅ **Fase 1 — Fundamentos + Desenvolvimento com LLMs:** concluída e auditada
- 🟡 **Fase 2 — RAG Profissional:** em andamento
- ⬜ **Fase 3 — Agentic AI:** pendente
- ⬜ **Fase 4 — Expansão do AI Engineer:** futura
- ⬜ **Fase 5 — Especializações Avançadas:** opcional/futura
- ⬜ **Fase 6 — Capstone Project:** futura

---

# Fase 1 — Fundamentos + Desenvolvimento com LLMs

**Status:** ✅ Concluída e auditada  
**Duração estimada:** 2–3 semanas

**Objetivo:** entender os conceitos centrais de aplicações com LLMs e Transformers e construir uma aplicação .NET funcional com experimentos observáveis.

## 1.1 LLMs, Tokens, Context Window e Custos

✅ Concluído

- funcionamento de LLMs;
- tokens;
- context window;
- limites e capacidades;
- contexto e custos;
- medição prática de `InputTokenCount`, `OutputTokenCount` e `TotalTokenCount`;
- cálculo do impacto de tokens de entrada e saída no custo de uma chamada real.

## 1.2 Prompts, Papéis e Parâmetros de Geração

✅ Concluído conceitualmente e na prática

- papéis `System`, `User` e `Assistant`;
- influência prática da mensagem `System` no comportamento do modelo;
- Prompt Engineering;
- comparação entre prompt simples e prompt estruturado;
- contexto, objetivo, público, restrições e formato;
- Temperature;
- Top-p;
- experimentos comparativos de previsibilidade x variedade;
- determinismo relativo x criatividade.

## 1.3 APIs da OpenAI e integração .NET

✅ Concluído

- modelos e endpoints;
- autenticação;
- integração em C#/.NET;
- `Microsoft.Extensions.AI`;
- `IChatClient`;
- chamadas assíncronas;
- gerenciamento seguro de API Key.

## 1.4 Structured Outputs / JSON Mode

✅ Concluído conceitualmente e na prática

- solicitação de JSON via prompt;
- diferença entre JSON solicitado por prompt e Structured Output;
- `ChatOptions.ResponseFormat`;
- `ChatResponseFormat.ForJsonSchema<T>()`;
- geração de schema a partir de tipos C#;
- DTO tipado;
- enums para restringir valores permitidos;
- desserialização com `System.Text.Json`;
- validação e tratamento de saída inválida;
- distinção entre garantia estrutural e correção semântica.

## 1.5 Streaming e Histórico

✅ Concluído

- streaming de respostas;
- Server-Sent Events (SSE);
- eventos `chunk`, `completed` e `error`;
- `CancellationToken`;
- histórico por `conversationId`;
- estado de conversa;
- controle de concorrência;
- rollback em falha/cancelamento.

## 1.6 Embeddings — Fundamentos

✅ Concluído conceitualmente e na prática

- conceito de embeddings;
- geração;
- representação vetorial;
- dimensionalidade;
- inspeção de vetores;
- interpretação semântica;
- relação entre embeddings, similaridade semântica e retrieval.

## 1.7 Transformers — Fundamentos

✅ Concluído conceitualmente e na prática

### Conceitos estudados

- redes neurais em nível necessário para AI Engineering;
- Attention e Self-Attention;
- Query, Key e Value (`Q`, `K`, `V`);
- Multi-Head Attention;
- arquitetura Transformer;
- Transformer Blocks;
- Feed Forward Network;
- residual connections e normalization em nível conceitual;
- Encoder x Decoder;
- Encoder-only, Decoder-only e Encoder-Decoder;
- BERT x GPT x Transformer original/T5;
- Causal/Masked Self-Attention;
- Positional Encoding;
- tokenização, token IDs e subwords;
- relação entre Transformers, embeddings e LLMs modernos.

### Laboratório prático

- projeto `AiEngineeringLab.Transformers`;
- `Microsoft.ML.Tokenizers`;
- comparação prática de tokenização em inglês, português e palavras incomuns;
- BERT WordPiece tokenizer;
- tokens especiais `[CLS]` e `[SEP]`;
- modelo BERT Tiny em ONNX;
- `Microsoft.ML.OnnxRuntime`;
- preparação de `input_ids` e `attention_mask`;
- inspeção do contrato de entrada/saída do modelo ONNX;
- execução local do Transformer em .NET;
- logits;
- Softmax;
- classificação de toxicidade (`not-toxic` x `toxic`);
- distinção entre Transformer base e classification head/fine-tuning.

## 1.8 Function Calling e Tools

✅ Concluído

- Function Calling;
- tools nativas em C#;
- parâmetros e retorno;
- `AIFunctionFactory`;
- execução automática;
- funções determinísticas;
- integração com chat e streaming;
- decisão entre código C# e LLM.

## 1.9 Semantic Kernel

✅ Concluído

- Kernel e configuração;
- plugins nativos;
- `KernelFunction`;
- `Kernel.InvokeAsync`;
- `InvokePromptAsync`;
- `FunctionChoiceBehavior.Auto`;
- seleção e execução automática de funções;
- integração com o pipeline .NET.

## 1.10 MCP — Model Context Protocol

✅ Concluído conceitualmente e na prática

- conceitos;
- hosts e clients;
- tools;
- resources;
- integração e papel do MCP na interoperabilidade de aplicações de IA;
- projeto `AiEngineeringLab.McpServer`;
- projeto `AiEngineeringLab.McpClient`;
- comunicação client/server usando o pacote `ModelContextProtocol`;
- descoberta e consumo de capacidades MCP no laboratório.

### Entrega da Fase 1

**Aplicação/assistente funcional em .NET** consumindo LLM via API, com contexto, histórico, streaming, medição de tokens, experimentos de Temperature/Top-p, Prompt Engineering, Structured Outputs tipados, Function Calling, Semantic Kernel, MCP e laboratório local de Transformer/BERT com ONNX.

---

# Fase 2 — RAG Profissional

**Status:** 🟡 Em andamento  
**Duração estimada:** 3–4 semanas

**Objetivo:** dominar recuperação de informação, vetores, RAG e avaliação de qualidade.

## 2.1 Retrieval Fundamentals

✅ Concluído

### 2.1.1 Arquitetura RAG

✅ Conceito estudado

- ingestão;
- embeddings;
- armazenamento;
- retrieval;
- construção de contexto;
- geração fundamentada.

### 2.1.2 Embeddings para Retrieval

✅ Implementado

- `IEmbeddingGenerator`;
- `Embedding<float>`;
- `text-embedding-3-small`;
- dimensionalidade;
- inspeção de vetores;
- representação semântica.

### 2.1.3 Similaridade Vetorial

✅ Concluído

- Cosine Similarity;
- Dot Product;
- Euclidean Distance;
- comparação prática entre as métricas;
- magnitude x direção;
- critérios de escolha conforme embedding model, índice/vector database e avaliação do caso de uso.

### 2.1.4 Top-K Retrieval

✅ Implementado

- conceito de Top-K;
- ranking por similaridade;
- seleção dos K documentos/chunks mais relevantes;
- implementação em `VectorRetriever.TopK`;
- ordenação por Cosine Similarity;
- distinção entre Top-K de geração e Top-K de retrieval.

> Similarity threshold será aprofundado junto da evolução do pipeline de retrieval e avaliação.

### 2.1.5 Recall e Precision

✅ Implementado

- Precision;
- Recall;
- relevância x cobertura;
- trade-offs de retrieval;
- implementação em `RetrievalMetrics`;
- base prática para evolução posterior para Precision@K e Recall@K no AI Harness.

### 2.1.6 Pipeline básico de recuperação

✅ Implementado em memória

- query → embedding;
- comparação/ranking;
- Top-K;
- retorno dos documentos/chunks relevantes;
- retrieval semântico;
- métricas básicas de avaliação;
- testes e experimentos observáveis.

---

## 2.2 Chunking e Indexação

✅ Concluído

### Estratégias implementadas

- Fixed Chunking em `FixedTextChunker`;
- Recursive Chunking em `RecursiveTextChunker`;
- Semantic Chunking em `SemanticTextChunker`;
- Sliding Window em `SlidingWindowChunker`;
- Token-based Chunking em `TokenTextChunker`;
- `chunkSize`, overlap e step;
- experimentos comparativos por endpoints da API.

### Metadata, ingestão e reindexação

- `ChunkingContext` com `DocumentId`, `Title`, `Source` e `Version`;
- `TextChunk` com posição, tamanho e índice do chunk;
- `DocumentInput` e `IndexedChunk`;
- `IngestionService` para chunking → embeddings → persistência;
- remoção dos chunks anteriores por `DocumentId` antes da nova gravação;
- versionamento de conteúdo preservado como metadata.

**Projeto:** pipeline de chunking e ingestão funcional, com múltiplas estratégias observáveis no laboratório.

---

## 2.3 Vector Databases

✅ Implementação prática principal concluída

### PostgreSQL + pgvector

- `IVectorStore` como contrato de armazenamento e busca;
- `InMemoryVectorStore` para experimentos determinísticos;
- `PgVectorStore` para persistência real;
- PostgreSQL + extensão pgvector via Docker Compose;
- persistência de embeddings e metadata;
- Top-K por similaridade vetorial;
- busca por distância cosseno usando o operador `<=>`;
- filtros por `DocumentId`, `Source` e `Version`;
- remoção/reindexação por documento;
- busca lexical com PostgreSQL Full Text Search.

### Limite atual versionado

O repositório contém a implementação de acesso ao pgvector, mas não contém migration/script versionado responsável por criar a tabela `chunks`, a extensão `vector` ou um índice ANN/HNSW. Esses itens devem ser tratados explicitamente na evolução production-ready.

### Alternativas e indexação

- ChromaDB, Pinecone e Weaviate permanecem como referências comparativas;
- Exact Search x Approximate Nearest Neighbor (ANN);
- HNSW;
- velocidade x recall;
- parâmetros de índice e custo operacional.

**Projeto:** indexação e busca vetorial real com PostgreSQL + pgvector.

---

## 2.4 Retrieval Avançado

✅ Concluído no laboratório

### Capacidades implementadas

- BM25 em `Bm25Retriever`;
- Hybrid Search combinando busca vetorial e lexical;
- Query Expansion em `QueryExpansionService`;
- Query Rewriting em `QueryRewriteService`;
- Re-ranking em `RerankingService`;
- MMR — Maximal Marginal Relevance em `MmrRetriever`;
- Context Compression em `ContextCompressionService`;
- metadata filters;
- experimentos comparativos expostos pela API.

As técnicas continuam separadas em componentes observáveis. A composição delas em um pipeline RAG end-to-end pertence à Fase 2.6.

**Projeto:** pipeline de retrieval avançado implementado como capacidades reutilizáveis e experimentáveis.

---

## 2.5 AI Harness / Avaliação

✅ Concluído

### Dataset de avaliação

- `RagEvaluationCase`;
- `RagEvaluationDataset`;
- `RagEvaluationDatasetLoader`;
- dataset JSON com casos positivos e caso sem resposta esperada.

### Retrieval Evaluation

- Precision;
- Recall;
- Hit Rate;
- Reciprocal Rank / MRR;
- agregação de métricas com `RetrievalMetricsAggregator`.

### Answer Evaluation

- `IGenerationEvaluator`;
- avaliação determinística básica com `RuleBasedGenerationEvaluator`;
- LLM-as-a-Judge com `LlmGenerationEvaluator`;
- correctness;
- relevance;
- completeness;
- overall score.

### Grounding / Faithfulness

- `IGroundingEvaluator`;
- `LlmGroundingEvaluator`;
- supported claims;
- unsupported claims;
- faithfulness score;
- agregação de grounding.

### Harness e regressão

- `IRagEvaluationPipeline`;
- `RagEvaluationHarness`;
- `RagEvaluationRunner`;
- `RagEvaluationBaseline`;
- `RagRegressionDetector`;
- agregação de retrieval, generation e grounding em uma execução;
- testes unitários com doubles/fakes para manter o Harness independente de um pipeline RAG concreto.

O contrato `IRagEvaluationPipeline` é a ponte planejada entre a suíte de avaliação e o pipeline production-ready da Fase 2.6.

**Projeto:** suíte de avaliação automatizada para retrieval, resposta, grounding e regressão de RAG.

---

## 2.6 RAG Production-Ready

🟡 Próximo passo / em início

### 2.6.1 Pipeline RAG end-to-end

- consolidar query → embedding → retrieval → contexto → geração;
- preservar proveniência do conteúdo recuperado, incluindo `DocumentId` e metadata necessária;
- construir uma implementação concreta consumível pelo `IRagEvaluationPipeline`;
- mover a orquestração reutilizável para fora do `ChatController`;
- conectar o pipeline real ao Harness criado na Fase 2.5.

### Evolução production-ready

- avaliação contínua;
- observabilidade e tracing;
- tokens, latência e custos;
- tratamento de falhas;
- cancellation, timeout e resiliência;
- configuração;
- segurança;
- otimização de qualidade x latência x custo;
- testes de qualidade e regressão;
- infraestrutura/versionamento do schema e índices necessários ao pgvector.

### Entrega da Fase 2

**Assistente Corporativo RAG em .NET** com pipeline end-to-end, testes de qualidade, Harness de avaliação e características production-ready.

---

# Fase 3 — Agentic AI

**Status:** ⬜ Pendente  
**Duração estimada:** 4–5 semanas

**Objetivo:** construir agentes confiáveis com ferramentas, memória, planejamento, orquestração, observabilidade, segurança e arquitetura de produção.

## 3.1 Fundamentos de AI Agents

- LLM x chatbot x workflow x agente;
- agent loop: observar → decidir → agir → avaliar;
- objetivos, estado e contexto;
- autonomia e limites;
- determinismo x comportamento probabilístico.

## 3.2 Tools e Function Calling para Agents

🟡 Base já construída na Fase 1; aprofundamento pendente

- seleção automática de ferramentas;
- múltiplas tools;
- parâmetros e retorno estruturado;
- tratamento de falhas;
- retry;
- autorização;
- composição de tools em tarefas multi-step;
- quando executar C# versus usar o LLM.

## 3.3 Estado, Memória e Planejamento

- short-term memory;
- long-term memory;
- histórico x memória;
- RAG como memória/conhecimento;
- decomposição de objetivos;
- observação e replanejamento;
- persistência de estado.

## 3.4 Agente Autônomo

- loop de execução;
- critérios de parada;
- limite de iterações;
- timeout;
- erros;
- custo/tokens;
- human-in-the-loop;
- prevenção de loops;
- idempotência quando aplicável.

## 3.5 Multi-Agent Systems

- especialização e responsabilidades;
- comunicação;
- compartilhamento e isolamento de contexto;
- delegação;
- handoff;
- supervisor/worker;
- conflitos;
- consolidação de resultados.

## 3.6 Orquestração de Agentes

- Sequential Orchestration;
- Concurrent Orchestration;
- Handoff;
- Supervisor;
- Routing;
- workflow baseado em estado;
- dependências;
- retry/fallback;
- controle de execução;
- cancelamento;
- propagação de erros.

## 3.7 Reliability, Observability e Guardrails

### Observabilidade

- logging estruturado;
- tracing;
- tokens;
- latência;
- falhas e retries;
- duração de tools e Kernel Functions;
- métricas de retrieval;
- similarity scores;
- custo estimado por requisição;
- telemetria essencial.

### Structured Outputs e validação

- schemas;
- respostas tipadas;
- desserialização segura;
- validação;
- tratamento de saída inválida;
- contratos de entrada/saída.

### Guardrails

- limites de autonomia;
- permissões;
- autorização de tools;
- human approval;
- prevenção de loops;
- validação de resultados.

## 3.8 AI Security & Robustez

- prompt injection;
- indirect prompt injection;
- data exfiltration;
- segurança de tools;
- least privilege;
- PII e secrets;
- separação entre instruções, input do usuário e retrieved content;
- proteção do contexto RAG;
- conteúdo não confiável;
- validação de argumentos;
- contenção/sandbox quando aplicável;
- threat modeling para aplicações com IA.

## 3.9 Arquitetura de Sistemas de IA em Produção

⬜ Nova seção

- separação entre componentes determinísticos e probabilísticos;
- model routing;
- fallback entre modelos/providers;
- cache de respostas e/ou resultados intermediários;
- filas e processamento assíncrono;
- resiliência;
- retry e circuit breaking quando aplicável;
- rate limits;
- idempotência;
- escalabilidade;
- custo x latência x qualidade;
- observabilidade end-to-end;
- isolamento de falhas;
- boas práticas de arquitetura para sistemas de IA production-ready.

### Entrega da Fase 3

**Agent Automation System em .NET** com múltiplos agentes, tools, memória, orquestração, observabilidade, segurança e arquitetura production-ready.

---

# Fase 4 — Expansão do AI Engineer

**Status:** ⬜ Nova / futura  
**Duração estimada:** 4–6 semanas

**Objetivo:** ampliar a stack além de .NET com Python, frameworks do ecossistema de IA e sistemas multimodais.

## 4.1 Python para AI Engineering

- async;
- typing;
- Pydantic;
- dataclasses;
- logging;
- HTTP/httpx;
- ambientes e dependências;
- testes com pytest.

## 4.2 Ecossistema Python de IA

- FastAPI;
- notebooks;
- SDKs de modelos;
- clientes de serviços de IA;
- integração e interoperabilidade com .NET.

## 4.3 LangChain / LangGraph

- Chains / Runnables;
- Agents;
- Tools;
- Memory;
- workflows de agentes baseados em grafo;
- integração com aplicações reais.

## 4.4 Multimodal AI

- modelos de texto + imagem + áudio;
- capacidades;
- limitações;
- casos de uso;
- desenho de pipelines multimodais.

## 4.5 Document AI / Vision

- compreensão de documentos e imagens;
- extração de texto;
- tabelas;
- layouts;
- classificação;
- OCR quando necessário.

## 4.6 Speech / Audio AI

- speech-to-text (STT);
- text-to-speech (TTS);
- transcrição;
- diarização;
- análise de áudio;
- aplicações de voz.

## 4.7 Projeto Multimodal End-to-End

- integração .NET + Python;
- pelo menos duas modalidades;
- upload/processamento de documentos, imagens e/ou áudio;
- avaliação;
- observabilidade;
- projeto demonstrável em portfólio.

### Entrega da Fase 4

Projeto multimodal demonstrável em portfólio e capacidade prática de atuar também no ecossistema Python de AI Engineering.

---

# Fase 5 — Especializações Avançadas

**Status:** ⬜ Opcional / futura  
**Duração estimada:** 3–4 semanas por trilha escolhida

**Objetivo:** aprofundar áreas de alto valor conforme demanda de mercado, projetos e posicionamento profissional.

## 5.1 GraphRAG e Knowledge Graphs

- grafos de conhecimento;
- graph retrieval;
- relações;
- consultas estruturadas;
- integração com RAG quando fizer sentido.

## 5.2 Fine-tuning / LoRA

- quando usar fine-tuning versus RAG;
- preparação de dados;
- avaliação;
- personalização;
- LoRA em nível adequado à especialização escolhida.

## 5.3 A2A e Protocolos de Integração

- agent-to-agent;
- protocolos A2A;
- OpenAPI e contratos de integração;
- interoperabilidade entre agentes e sistemas.

## 5.4 AI DevOps + MLOps/LLMOps Avançado

⬜ Nova ampliação

- CI/CD de aplicações de IA;
- testes e evals no pipeline;
- versionamento de modelos e prompts;
- infraestrutura e deploy;
- observabilidade;
- rollback;
- monitoramento contínuo;
- avaliação contínua;
- controle e otimização de custos;
- governança de releases de componentes de IA.

## 5.5 Ética, Compliance e Governança

- privacidade;
- vieses;
- explicabilidade;
- LGPD/GDPR quando aplicável;
- requisitos regulatórios;
- governança de IA.

### Entrega da Fase 5

Especialização escolhida conforme demanda de mercado, projetos e objetivos profissionais.

---

# Fase 6 — Capstone Project

**Status:** ⬜ Futura

**Objetivo:** integrar as competências principais do roadmap em um sistema completo, production-like, mensurável, seguro, documentado e demonstrável em portfólio.

## 6.1 Definição do Problema e Arquitetura

- escolher um problema real;
- definir requisitos;
- definir arquitetura;
- definir métricas;
- estabelecer critérios de sucesso;
- definir fronteiras entre componentes determinísticos e probabilísticos.

## 6.2 RAG Production-Ready

- ingestão;
- indexação;
- retrieval avançado;
- geração;
- evals;
- observabilidade;
- otimização de qualidade, latência e custo.

## 6.3 Agents e Orquestração

- agentes autônomos;
- tools;
- memória;
- roteamento;
- colaboração entre agentes;
- controle de execução;
- human-in-the-loop quando necessário.

## 6.4 MCP e Integrações

- expor tools/resources via MCP;
- consumir tools/resources via MCP;
- integrar serviços externos quando aplicável;
- manter contratos e limites de acesso claros.

## 6.5 Segurança e Governança

- guardrails;
- permissões;
- least privilege;
- proteção contra prompt injection;
- proteção contra data exfiltration;
- tratamento de PII e secrets;
- trilha de auditoria;
- políticas de acesso e aprovação.

## 6.6 Integração .NET + Python + Multimodal

- .NET como stack principal;
- Python quando fizer sentido arquitetural;
- integração entre serviços .NET e Python;
- ao menos duas modalidades quando o problema justificar;
- processamento de texto, imagem e/ou áudio.

## 6.7 Deploy, Documentação e Demonstração

- deploy production-like;
- CI/CD;
- observabilidade;
- documentação técnica;
- README completo;
- diagrama de arquitetura;
- instruções de execução;
- métricas e resultados de avaliação;
- demonstração para portfólio.

### Entrega da Fase 6

**Sistema completo e production-like**, documentado, avaliado e demonstrável em portfólio, reunindo RAG, Agents, MCP, segurança, observabilidade e, quando fizer sentido, integração .NET + Python + Multimodal.

---

# Competências esperadas ao final

- .NET/C# avançado aplicado a AI Engineering;
- LLMs e APIs;
- tokens, context window e custos;
- Prompt Engineering;
- Temperature e Top-p;
- Structured Outputs e contratos tipados;
- fundamentos de Transformers;
- embeddings e similaridade vetorial;
- RAG profissional;
- Vector Search e Vector Databases;
- avaliação e AI Harness;
- agentes autônomos e multiagentes;
- orquestração;
- MCP;
- observabilidade;
- segurança e guardrails;
- arquitetura de sistemas de IA em produção;
- Python para AI Engineering;
- LangChain/LangGraph;
- Multimodal AI;
- AI DevOps + MLOps/LLMOps;
- ética, compliance e governança;
- capacidade de entregar um sistema production-like demonstrável em portfólio.

---

# Posição atual

Neste momento:

- ✅ **Fase 1 concluída e auditada**;
- 🟡 **Fase 2 em andamento**;
- ✅ 2.1 Retrieval Fundamentals;
- ✅ 2.2 Chunking e Indexação;
- ✅ 2.3 implementação prática de Vector Database com PostgreSQL + pgvector;
- ✅ 2.4 Retrieval Avançado;
- ✅ 2.5 AI Harness / Avaliação;
- 🟡 próximo passo: **2.6 RAG Production-Ready**, começando pelo pipeline end-to-end;
- ⬜ Fase 3 Agentic AI após o fechamento da Fase 2.

A sequência imediata será:

**Pipeline RAG end-to-end → integração com o Harness → observabilidade → resiliência → custos/otimização → fechamento production-ready da Fase 2 → Agentic AI.**

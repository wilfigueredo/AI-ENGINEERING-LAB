# AI Engineering Lab — Roadmap

Roadmap prático para evolução de **AI Engineering com .NET como stack principal**, avançando de fundamentos de LLMs para **RAG profissional, Agentic AI, arquitetura de sistemas de IA em produção, Python/Multimodal e um Capstone integrador**.

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

- ✅ **Fase 1 — Fundamentos + Desenvolvimento com LLMs:** concluída
- 🟡 **Fase 2 — RAG Profissional:** em andamento
- ⬜ **Fase 3 — Agentic AI:** pendente
- ⬜ **Fase 4 — Expansão do AI Engineer:** futura
- ⬜ **Fase 5 — Especializações Avançadas:** opcional/futura
- ⬜ **Fase 6 — Capstone Project:** futura

---

# Fase 1 — Fundamentos + Desenvolvimento com LLMs

**Status:** ✅ Concluída  
**Duração estimada:** 2–3 semanas

**Objetivo:** entender os conceitos centrais de aplicações com LLMs e construir uma aplicação .NET funcional.

## 1.1 LLMs, Tokens e Context Window

✅ Concluído

- funcionamento de LLMs;
- tokens;
- context window;
- limites e capacidades;
- contexto e custos.

## 1.2 Prompts e Parâmetros

✅ Concluído

- System, User e Assistant;
- Prompt Engineering;
- Temperature;
- Top-p;
- determinismo x criatividade.

## 1.3 APIs da OpenAI e integração .NET

✅ Concluído

- modelos e endpoints;
- autenticação;
- integração em C#/.NET;
- `Microsoft.Extensions.AI`;
- `IChatClient`;
- chamadas assíncronas;
- gerenciamento seguro de API Key.

## 1.4 Embeddings — Fundamentos

✅ Concluído conceitualmente e iniciado na prática

- conceito de embeddings;
- geração;
- representação vetorial;
- interpretação semântica.

## 1.5 Function Calling e Tools

✅ Concluído

- Function Calling;
- tools nativas em C#;
- parâmetros e retorno;
- `AIFunctionFactory`;
- execução automática;
- funções determinísticas;
- integração com chat e streaming;
- decisão entre código C# e LLM.

## 1.6 Structured Outputs / JSON Mode

✅ Conceitos estudados; aprofundamento production-ready previsto na Fase 3

- JSON;
- schemas;
- saídas estruturadas;
- validação.

## 1.7 Streaming e Histórico

✅ Concluído

- streaming de respostas;
- Server-Sent Events (SSE);
- eventos `chunk`, `completed` e `error`;
- `CancellationToken`;
- histórico por `conversationId`;
- estado de conversa;
- controle de concorrência;
- rollback em falha/cancelamento.

## 1.8 Semantic Kernel

✅ Concluído

- Kernel e configuração;
- plugins nativos;
- `KernelFunction`;
- `Kernel.InvokeAsync`;
- `InvokePromptAsync`;
- `FunctionChoiceBehavior.Auto`;
- seleção e execução automática de funções;
- integração com o pipeline .NET.

## 1.9 MCP — Model Context Protocol

✅ Fundamentos estudados

- conceitos;
- hosts e clients;
- tools;
- resources;
- integração e papel do MCP na interoperabilidade de aplicações de IA.

### Entrega da Fase 1

Aplicação/assistente funcional em .NET consumindo LLM via API, com contexto, streaming, Structured Outputs em nível conceitual, Function Calling e Semantic Kernel.

---

# Fase 2 — RAG Profissional

**Status:** 🟡 Em andamento  
**Duração estimada:** 3–4 semanas

**Objetivo:** dominar recuperação de informação, vetores, RAG e avaliação de qualidade.

## 2.1 Retrieval Fundamentals

🟡 Em andamento

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

⬜ Próximo

- conceito de Top-K;
- ranking por similaridade;
- similarity threshold;
- seleção dos K documentos/chunks mais relevantes;
- distinção entre Top-K de geração e Top-K de retrieval.

### 2.1.5 Recall e Precision

⬜ Pendente

- Precision;
- Recall;
- Precision@K;
- Recall@K;
- relevância x cobertura;
- trade-offs de retrieval.

### 2.1.6 Pipeline básico de recuperação

⬜ Pendente

- query → embedding;
- comparação/ranking;
- Top-K;
- retorno dos documentos/chunks relevantes;
- testes do retriever.

---

## 2.2 Chunking e Indexação

⬜ Pendente

- Fixed Chunking;
- Recursive Chunking;
- Semantic Chunking;
- Sliding Window;
- overlap;
- chunk size;
- metadata;
- pipeline de ingestão;
- reindexação;
- versionamento de conteúdo;
- comparação prática entre estratégias de chunking.

**Projeto:** comparar diferentes estratégias de chunking sobre o mesmo conjunto de documentos.

---

## 2.3 Vector Databases

⬜ Pendente

**Estratégia:** implementar um banco em profundidade e estudar os demais conceitualmente.

### Implementação prática principal

- PostgreSQL + `pgvector`;
- schema para documentos/chunks;
- persistência de embeddings;
- indexação;
- consulta vetorial;
- Top-K;
- metadata filters;
- integração .NET.

### Estudo conceitual/comparativo

- ChromaDB;
- Pinecone;
- Weaviate;
- serviço gerenciado x infraestrutura própria;
- custo e escalabilidade;
- experiência de desenvolvimento;
- filtros e metadata.

### Indexação vetorial

- Exact Search x Approximate Nearest Neighbor (ANN);
- HNSW;
- recall de busca aproximada;
- velocidade x qualidade;
- parâmetros de índice.

**Projeto:** indexação e busca vetorial real com PostgreSQL + `pgvector`.

---

## 2.4 Retrieval Avançado

⬜ Pendente

- Hybrid Search;
- BM25;
- Query Expansion;
- Query Rewriting;
- Re-ranking;
- MMR — Maximal Marginal Relevance;
- Context Compression;
- metadata filters avançados;
- busca lexical + semântica;
- impacto das técnicas em Precision e Recall.

**Projeto:** evoluir o pipeline de busca e medir a melhoria de qualidade.

---

## 2.5 AI Harness / Avaliação

⬜ Pendente

### Datasets de avaliação

- perguntas e respostas esperadas;
- casos positivos, negativos e ambíguos;
- dataset de regressão;
- versionamento.

### Retrieval Evaluation

- Precision@K;
- Recall@K;
- MRR — Mean Reciprocal Rank;
- relevance;
- ranking quality;
- cobertura.

### Answer Evaluation

- groundedness;
- faithfulness;
- answer relevance;
- precisão factual quando mensurável;
- detecção de informação não suportada;
- LLM-as-a-Judge.

### Harness de testes

- avaliação automática;
- regressão para RAG;
- comparação de prompts;
- comparação de chunking;
- comparação de pipelines;
- comparação antes/depois de re-ranking e query rewriting;
- métricas reproduzíveis.

**Projeto:** suíte de avaliação do assistente RAG com métricas antes/depois das melhorias.

---

## 2.6 RAG Production-Ready

⬜ Pendente

- pipeline end-to-end: ingestão → indexação → retrieval → contexto → geração;
- avaliação contínua;
- observabilidade;
- otimização;
- custos;
- tratamento de falhas;
- configuração e segurança;
- testes de qualidade e regressão.

### Entrega da Fase 2

**Assistente Corporativo RAG em .NET** com testes de qualidade e pipeline production-ready.

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
- Prompt Engineering;
- embeddings e similaridade vetorial;
- RAG profissional;
- Vector Search e Vector Databases;
- avaliação e AI Harness;
- agentes autônomos e multiagentes;
- orquestração;
- MCP;
- Structured Outputs;
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

- ✅ Fase 1 concluída;
- 🟡 Fase 2 em andamento;
- ✅ arquitetura RAG estudada;
- ✅ embeddings gerados e inspecionados;
- ✅ Cosine Similarity implementada;
- ✅ Dot Product implementado;
- ✅ Euclidean Distance implementada;
- ✅ comparação prática das métricas concluída;
- ✅ testes automatizados das métricas;
- ⬜ próximo passo: **Top-K Retrieval**.

A sequência imediata será:

**Top-K → Recall/Precision → pipeline básico → Chunking/Indexação → Vector Database → Retrieval Avançado → AI Harness → RAG Production-Ready → Agentic AI → Arquitetura de IA em Produção → Python/Multimodal → Especializações → Capstone.**
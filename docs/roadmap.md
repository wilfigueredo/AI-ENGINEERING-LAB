# AI Engineering Lab — Roadmap

Roadmap prático para evolução de **AI Engineering com .NET como stack principal**, avançando de fundamentos de LLMs para **RAG profissional, Agentic AI, Python/Multimodal e especializações avançadas**.

> **Objetivo final:** construir sistemas de IA robustos, avaliáveis, escaláveis e seguros, levando conhecimento até produção.

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

---

# Fase 1 — Fundamentos + Desenvolvimento com LLMs

**Status:** ✅ Concluída  
**Duração estimada:** 2–3 semanas

**Objetivo:** entender os conceitos centrais de aplicações com LLMs e construir uma aplicação .NET funcional.

## 1.1 LLMs, conceitos e funcionamento

✅ Concluído

- funcionamento de LLMs;
- tokens;
- context window;
- limites e capacidades;
- custos.

## 1.2 APIs da OpenAI

✅ Concluído

- modelos e endpoints;
- autenticação;
- integração em C#/.NET;
- `Microsoft.Extensions.AI`;
- `IChatClient`;
- chamadas assíncronas;
- gerenciamento seguro de API Key.

## 1.3 Prompt Engineering

✅ Concluído

- System, User e Assistant;
- padrões de prompt;
- Temperature;
- Top-p;
- determinismo x criatividade.

## 1.4 Tokens e Custos

✅ Concluído conceitualmente

- consumo de tokens;
- contexto;
- estimativas de custo;
- trade-offs de modelo e uso.

## 1.5 Embeddings — Fundamentos

✅ Concluído conceitualmente e iniciado na prática

- conceito de embeddings;
- geração;
- representação vetorial;
- interpretação semântica.

## 1.6 Function Calling e Tools

✅ Concluído

- Function Calling;
- tools nativas em C#;
- parâmetros e retorno;
- `AIFunctionFactory`;
- execução automática;
- funções determinísticas;
- integração com chat e streaming;
- decisão entre código C# e LLM.

## 1.7 Structured Outputs / JSON Mode

✅ Conceitos estudados; aprofundamento production-ready previsto na Fase 3

- JSON;
- schemas;
- saídas estruturadas;
- validação.

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

### Entregas da Fase 1

- ✅ aplicação console/API .NET consumindo LLM;
- ✅ uso de prompts e parâmetros;
- ✅ saída estruturada em JSON em nível conceitual;
- ✅ chamada de função simples;
- ✅ histórico e streaming;
- ✅ Semantic Kernel integrado.

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

### Entregas da Fase 2

- ⬜ pipeline completo de RAG;
- ⬜ comparação de estratégias de chunking e busca;
- ⬜ suíte de testes (AI Harness);
- ⬜ melhoria mensurável da qualidade das respostas;
- ⬜ pipeline production-ready em .NET.

---

# Fase 3 — Agentic AI

**Status:** ⬜ Pendente  
**Duração estimada:** 4–5 semanas

**Objetivo:** construir agentes confiáveis com ferramentas, memória, planejamento, orquestração, observabilidade e segurança.

## 3.1 Agente Autônomo

- agent loop: observar → decidir → agir → avaliar;
- objetivos e autonomia controlada;
- critérios de parada;
- limites de execução.

## 3.2 Tools e Function Calling

🟡 Base já construída na Fase 1; aprofundamento pendente

- tools como capacidades do agente;
- seleção automática;
- múltiplas tools;
- parâmetros e retorno estruturado;
- tratamento de falhas;
- retry;
- autorização;
- composição de tools em tarefas multi-step;
- quando executar C# versus usar o LLM.

## 3.3 Memória e Estado

- estado do agente;
- short-term memory;
- long-term memory;
- histórico x memória;
- RAG como memória/conhecimento;
- persistência de estado.

## 3.4 Planejamento e Replanejamento

- decomposição de objetivos;
- planejamento de tarefas;
- observação de resultados;
- replanejamento;
- critérios de parada;
- retry;
- timeout;
- custo/tokens;
- human-in-the-loop.

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
- workflows baseados em estado;
- dependências;
- retry/fallback;
- controle de execução;
- cancelamento;
- propagação de erros.

## 3.7 Observabilidade e Guardrails

### Observabilidade

- logging estruturado;
- tracing;
- duração de tools e Kernel Functions;
- tokens;
- latência;
- falhas e retries;
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

⬜ Nova seção

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

### Entregas da Fase 3

- ⬜ agente funcional com tools e memória;
- ⬜ sistema multiagente orquestrado;
- ⬜ observabilidade completa;
- ⬜ guardrails e segurança aplicados;
- ⬜ projeto final: **Agent Automation System**.

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

## 4.4 Multimodal AI — Visão Geral

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

### Entregas da Fase 4

- ⬜ aplicações Python de IA funcionais;
- ⬜ pipeline multimodal com visão e/ou áudio;
- ⬜ integração .NET + Python;
- ⬜ projeto multimodal completo.

---

# Fase 5 — Especializações Avançadas

**Status:** ⬜ Opcional / futura  
**Duração estimada:** 3–4 semanas

**Objetivo:** aprofundar áreas de alto valor conforme demanda de mercado, projetos e posicionamento profissional.

## 5.1 GraphRAG e Knowledge Graphs

- grafos de conhecimento;
- graph retrieval;
- relações;
- consultas estruturadas;
- Neo4j ou tecnologia equivalente em nível prático quando fizer sentido.

## 5.2 Fine-tuning e Personalização

- quando usar fine-tuning versus RAG;
- preparação de dados;
- avaliação;
- LoRA em nível conceitual/prático quando aplicável;
- combinação RAG + fine-tuning.

## 5.3 A2A e Protocolos de Integração

- Agent-to-Agent;
- protocolos de interoperabilidade;
- A2A;
- OpenAPI;
- integração entre sistemas e agentes.

## 5.4 Deploy e MLOps/LLMOps de IA

- Docker;
- CI/CD;
- versionamento de modelos e prompts;
- monitoramento;
- avaliação contínua;
- rollout/fallback;
- observabilidade em produção.

## 5.5 Ética, Compliance e Governança

- privacidade;
- vieses;
- explicabilidade;
- conformidade;
- LGPD;
- GDPR;
- AI Act;
- governança de sistemas de IA.

### Entregas da Fase 5

- ⬜ projeto avançado de especialização;
- ⬜ documentação técnica;
- ⬜ avaliação e métricas;
- ⬜ deploy e monitoramento.

---

# Onde estamos agora

A Fase 1 está concluída e a Fase 2 está em andamento.

No momento, em **2.1 Retrieval Fundamentals**:

- ✅ arquitetura RAG estudada;
- ✅ embeddings gerados na prática;
- ✅ Cosine Similarity implementada;
- ✅ Dot Product implementado;
- ✅ Euclidean Distance implementada;
- ✅ comparação das três métricas;
- ✅ testes automatizados das métricas;
- ⬜ **próximo passo: Top-K Retrieval**.

A evolução seguinte será:

**Top-K → Recall/Precision → pipeline básico → Chunking/Indexação → Vector Database → Retrieval Avançado → AI Harness → RAG Production-Ready → Agentic AI → Python/Multimodal → especializações avançadas.**

# BOS AI Copilot

O BOS AI Copilot é uma aplicação de Inteligência Artificial desenvolvida em .NET com Semantic Kernel para apoiar a criação, análise e evolução de projetos baseados no BOS Framework.

O sistema permitirá que o usuário converse com um assistente de IA, descreva ideias de projetos, identifique informações ausentes, solicite estruturas iniciais de artefatos e execute operações por meio de funções nativas em C#.

## Objetivo

O objetivo da versão 1.0 é validar, na prática, os principais fundamentos necessários para a transição de desenvolvedor .NET para AI Engineer.

O projeto será utilizado como:

- laboratório de aprendizado;
- projeto de portfólio;
- aplicação real do ecossistema BOS;
- base evolutiva para o estudo de RAG, MCP e agentes.

## Problema

Durante a iniciação de projetos, as informações fornecidas pelos usuários podem ser incompletas, ambíguas ou não estruturadas.

O BOS AI Copilot deverá interpretar essas informações, identificar lacunas, selecionar ferramentas apropriadas e transformar a solicitação em estruturas organizadas para revisão humana.

## Exemplo de uso

Usuário:

> Quero iniciar um projeto para automatizar o atendimento de um restaurante pelo WhatsApp.

O sistema deverá:

1. interpretar a solicitação;
2. identificar que se trata de uma nova ideia;
3. detectar informações ausentes;
4. selecionar uma função apropriada;
5. gerar uma saída estruturada;
6. apresentar uma resposta compreensível ao usuário.

## Escopo da versão 1.0

A primeira versão incluirá:

- conversação com um Large Language Model;
- histórico da conversa durante a sessão;
- system prompt externo;
- streaming de respostas;
- Semantic Kernel;
- plugins nativos em C#;
- function calling;
- seleção automática de funções;
- respostas estruturadas;
- serialização e desserialização em JSON;
- injeção de dependência;
- tratamento de erros;
- logging;
- configuração externa;
- gerenciamento seguro da API key;
- testes unitários e de integração.

## Plugins previstos

### ProjectPlugin

Responsável por operações relacionadas à análise e estruturação de projetos.

Funções previstas:

- `CreateProjectDraft`
- `IdentifyMissingInformation`
- `DefineProjectObjectives`
- `DefineSuccessCriteria`
- `ClassifyProjectStage`

### CostEstimationPlugin

Responsável por cálculos determinísticos.

Funções previstas:

- `EstimateTokenCost`
- `EstimateMonthlyOperatingCost`
- `CalculateCostPerInteraction`

### DateTimePlugin

Responsável por operações relacionadas a datas.

Funções previstas:

- `GetCurrentDate`
- `CalculateDeadline`
- `CalculateDaysBetweenDates`

### ArtifactPlugin

Responsável pela criação inicial de estruturas de artefatos.

Funções previstas:

- `CreateProjectCharterDraft`
- `CreateRequirementDraft`
- `CreateAcceptanceCriteria`

## Fora do escopo da versão 1.0

A primeira versão não incluirá:

- RAG;
- embeddings;
- banco vetorial;
- leitura integral dos documentos do BOS;
- MCP;
- agentes autônomos;
- multiagentes;
- memória persistente;
- integração com WhatsApp;
- autenticação;
- publicação automática no GitHub;
- geração automática de documentos oficiais.

Essas capacidades serão adicionadas progressivamente em versões futuras.

## Arquitetura inicial

A estrutura inicial prevista é:

```text
BOS-AI-Copilot
├── docs
├── prompts
├── src
└── tests
using AiEngineeringLab.Core.AI.Evaluation.Generation;
using AiEngineeringLab.Core.AI.Evaluation.Grounding;
using AiEngineeringLab.Core.AI.Evaluation.Metrics;

namespace AiEngineeringLab.Core.AI.Evaluation.Harness;

public sealed record RagEvaluationExecutionResult(
    string CaseId,
    RetrievalMetricsResult Retrieval,
    GenerationEvaluationResult Generation,
    GroundingEvaluationResult Grounding);

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiEngineeringLab.Core.Models.Embedding;
public sealed class EmbeddingComparisonRequest
{
    public string FirstText { get; set; } = string.Empty;

    public string SecondText { get; set; } = string.Empty;
}

using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

namespace AiEngineeringLab.Plugins.SemanticKernel;

public sealed class TextPlugin(
    ILogger<TextPlugin> logger)
{
    [KernelFunction("count_words")]
    [Description("Conta a quantidade de palavras existentes em um texto.")]
    public int CountWords(
        [Description("Texto que será analisado")] string text)
    {
        logger.LogInformation(
            "Executando Kernel Function {FunctionName}.",
            nameof(CountWords));

        if (string.IsNullOrWhiteSpace(text))
        {
            return 0;
        }

        return text.Split(
            [' ', '\t', '\r', '\n'],
            StringSplitOptions.RemoveEmptyEntries)
            .Length;
    }

    [KernelFunction("to_upper_case")]
    [Description("Converte um texto para letras maiúsculas.")]
    public string ToUpperCase(
        [Description("Texto que será convertido")] string text)
    {
        logger.LogInformation(
            "Executando Kernel Function {FunctionName}.",
            nameof(ToUpperCase));

        return text.ToUpperInvariant();
    }
}

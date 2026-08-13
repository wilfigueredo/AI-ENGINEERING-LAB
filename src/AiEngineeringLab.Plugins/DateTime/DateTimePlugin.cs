using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace BosAiCopilot.Plugins.DateTimeTools;

public sealed class DateTimePlugin(
    ILogger<DateTimePlugin> logger)
{
    public string GetCurrentDate()
    {
        var stopwatch = Stopwatch.StartNew();

        logger.LogInformation(
            "Tool {ToolName} iniciada.",
            nameof(GetCurrentDate));

        try
        {
            var result = DateTimeOffset.Now.ToString("yyyy-MM-dd");

            logger.LogInformation(
                "Tool {ToolName} concluída. Resultado: {Result}. Duração: {ElapsedMs} ms.",
                nameof(GetCurrentDate),
                result,
                stopwatch.ElapsedMilliseconds);

            return result;
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Falha ao executar a tool {ToolName}.",
                nameof(GetCurrentDate));

            throw;
        }
    }

    public int CalculateDaysBetweenDates(
        DateTime startDate,
        DateTime endDate)
    {
        var stopwatch = Stopwatch.StartNew();

        logger.LogInformation(
            "Tool {ToolName} iniciada. StartDate: {StartDate}. EndDate: {EndDate}.",
            nameof(CalculateDaysBetweenDates),
            startDate,
            endDate);

        try
        {
            var result = Math.Abs(
                (endDate.Date - startDate.Date).Days);

            logger.LogInformation(
                "Tool {ToolName} concluída. Resultado: {Result}. Duração: {ElapsedMs} ms.",
                nameof(CalculateDaysBetweenDates),
                result,
                stopwatch.ElapsedMilliseconds);

            return result;
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Falha ao executar a tool {ToolName}.",
                nameof(CalculateDaysBetweenDates));

            throw;
        }
    }
}

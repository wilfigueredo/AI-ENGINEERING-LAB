using AiEngineeringLab.Plugins.DateTimeTools;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;

namespace AiEngineeringLab.Plugins;

public sealed class AiTools(
    IServiceProvider serviceProvider)
{
    public IList<AITool> Create()
    {
        var dateTimePlugin =
            serviceProvider.GetRequiredService<DateTimePlugin>();

        return
        [
            AIFunctionFactory.Create(
                dateTimePlugin.GetCurrentDate,
                name: "get_current_date",
                description: "Obtém a data atual do sistema."),

            AIFunctionFactory.Create(
                dateTimePlugin.CalculateDaysBetweenDates,
                name: "calculate_days_between_dates",
                description:
                    "Calcula a quantidade absoluta de dias entre duas datas.")
        ];
    }
}

using ModelContextProtocol.Server;
using System.ComponentModel;

namespace AiEngineeringLab.McpServer.Tools;

[McpServerToolType]
public static class FrameworkTools
{
    [McpServerTool]
    [Description("Returns the current version of the AI Engineering Lab framework.")]
    public static string GetCurrentFrameworkVersion()
    {
        return "1.0.0";
    }
}

using ModelContextProtocol.Server;
using System.ComponentModel;

namespace AiEngineeringLab.McpServer.Resources;

[McpServerResourceType]
public static class FrameworkResources
{
    [McpServerResource(
        UriTemplate = "framework://core",
        Name = "framework_core",
        MimeType = "text/plain")]
    [Description("Returns basic information about the AI Engineering Lab framework.")]
    public static string GetFrameworkCore()
    {
        return """
               AI Engineering Lab Framework

               Version: 1.0.0

               Purpose:
               Demonstrate AI Engineering concepts using .NET,
               including LLMs, embeddings, RAG, MCP and agents.
               """;
    }
}

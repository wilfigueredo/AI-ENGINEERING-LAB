using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

var serverProjectPath =
    Path.GetFullPath(
        "src/AiEngineeringLab.McpServer/AiEngineeringLab.McpServer.csproj");

var transport = new StdioClientTransport(
    new StdioClientTransportOptions
    {
        Name = "AI Engineering Lab MCP Server",
        Command = "dotnet",
        Arguments =
        [
            "run",
            "--project",
            serverProjectPath
        ]
    });

await using var client =
    await McpClient.CreateAsync(transport);

Console.WriteLine("Conectado ao MCP Server.");
Console.WriteLine();

var tools = await client.ListToolsAsync();

Console.WriteLine("Tools disponíveis:");

foreach (var tool in tools)
{
    Console.WriteLine(
        $"- {tool.Name}: {tool.Description}");
}

Console.WriteLine();

Console.WriteLine();
Console.WriteLine("Resources disponíveis:");

var resources = await client.ListResourcesAsync();

foreach (var resource in resources)
{
    Console.WriteLine(
        $"- {resource.Uri}: {resource.Name}");
}

var result = await client.CallToolAsync(
    "get_current_framework_version",
    cancellationToken: CancellationToken.None);

var textResult = result.Content
    .OfType<TextContentBlock>()
    .FirstOrDefault();

Console.WriteLine(
    $"Versão retornada pelo servidor: {textResult?.Text}");

Console.WriteLine();

var resourceResult =
    await client.ReadResourceAsync("framework://core");

foreach (var content in resourceResult.Contents)
{
    if (content is TextResourceContents text)
    {
        Console.WriteLine("Conteúdo do Resource:");
        Console.WriteLine(text.Text);
    }
}

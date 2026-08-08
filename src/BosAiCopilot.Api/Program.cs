using BosAiCopilot.Core.Options;
using BosAiCopilot.Core.Services.Conversations;
using BosAiCopilot.Plugins;
using BosAiCopilot.Plugins.DateTimeTools;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using OpenAI;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOptions<AiOptions>()
    .Bind(builder.Configuration
    .GetSection(AiOptions.SectionName))
    .ValidateDataAnnotations()
    .Validate(
        options => options.Provider.Equals(
            "OpenAI",
            StringComparison.OrdinalIgnoreCase),
        "O provedor configurado em AI:Provider ainda não é suportado.")
    .ValidateOnStart();

builder.Services.AddChatClient(serviceProvider =>
{
    var options = serviceProvider
        .GetRequiredService<IOptions<AiOptions>>()
        .Value;

    var openAIClient = new OpenAIClient(options.ApiKey);

    return openAIClient
        .GetChatClient(options.ModelId)
        .AsIChatClient()
        .AsBuilder()
        .UseFunctionInvocation(
            serviceProvider.GetRequiredService<ILoggerFactory>())
        .Build();
        });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSingleton<ConversationHistoryService>();
builder.Services.AddSingleton<DateTimePlugin>();
builder.Services.AddSingleton<AiTools>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();



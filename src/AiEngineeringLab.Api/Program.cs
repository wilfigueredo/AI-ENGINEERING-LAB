using AiEngineeringLab.Core.AI.Interface;
using AiEngineeringLab.Core.AI.Retrieval;
using AiEngineeringLab.Core.AI.VectorStore;
using AiEngineeringLab.Core.Options;
using AiEngineeringLab.Core.Services.Conversations;
using AiEngineeringLab.Core.Services.Ingestion;
using AiEngineeringLab.Plugins;
using AiEngineeringLab.Plugins.DateTimeTools;
using AiEngineeringLab.Plugins.SemanticKernel;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Npgsql;
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

builder.Services.AddOpenAIChatCompletion(
    modelId: builder.Configuration["AI:ModelId"]!,
    apiKey: builder.Configuration["AI:ApiKey"]!);

builder.Services.AddSingleton<
    IEmbeddingGenerator<string, Embedding<float>>>(
    serviceProvider =>
    {
        var options = serviceProvider
            .GetRequiredService<IOptions<AiOptions>>()
            .Value;

        var openAIClient =
            new OpenAIClient(options.ApiKey);

        return openAIClient
            .GetEmbeddingClient(options.EmbeddingModelId)
            .AsIEmbeddingGenerator();
    });

var connectionString =
    builder.Configuration.GetConnectionString("Postgres")
    ?? throw new InvalidOperationException(
        "Postgres connection string not configured.");

var dataSourceBuilder =
    new NpgsqlDataSourceBuilder(connectionString);

dataSourceBuilder.UseVector();

var dataSource =
    dataSourceBuilder.Build();

builder.Services.AddSingleton(dataSource);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSingleton<ConversationHistoryService>();
builder.Services.AddSingleton<DateTimePlugin>();
builder.Services.AddSingleton<AiTools>();
builder.Services.AddSingleton<TextPlugin>();
builder.Services.AddScoped<IngestionService>();
builder.Services.AddSingleton<IVectorStore, PgVectorStore>();
builder.Services.AddScoped<IngestionService>();
builder.Services.AddScoped<QueryExpansionService>();
builder.Services.AddScoped<QueryRewriteService>();
builder.Services.AddScoped<RerankingService>();
builder.Services.AddScoped<ContextCompressionService>();

builder.Services.AddTransient<Kernel>(serviceProvider =>
{
    var textPlugin =
        serviceProvider.GetRequiredService<TextPlugin>();

    var kernel =
        new Kernel(serviceProvider);

    kernel.Plugins.AddFromObject(
        textPlugin,
        "Text");

    return kernel;
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();



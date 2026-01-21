using TodoManager.Endpoints;
using Microsoft.Extensions.Options;
using TodoManager.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Dapr client
builder.Services.AddDaprClient();

// Configure TodoAccessor options (can be overridden via environment variable)
builder.Services.Configure<TodoAccessorOptions>(options =>
{
    builder.Configuration.GetSection(TodoAccessorOptions.SectionName).Bind(options);
    
    // Allow override via environment variable
    var envUrl = Environment.GetEnvironmentVariable("TODOACCESSOR_BASEURL");
    if (!string.IsNullOrWhiteSpace(envUrl))
    {
        options.BaseUrl = envUrl;
    }
});

// Register DaprTodoQueryClient (uses Dapr service invocation)
builder.Services.AddScoped<ITodoQueryClient, DaprTodoQueryClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Temporarily disabled for easier debugging
// app.UseHttpsRedirection();

//app.UseAuthorization();

// Map all todo endpoints using extension method
app.MapTodoEndpoints();

app.Run();

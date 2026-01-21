using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TodoManager.Contracts;

namespace TodoManager.Services;

public class HttpTodoQueryClient : ITodoQueryClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HttpTodoQueryClient> _logger;
    private readonly string _accessorBaseUrl;

    public HttpTodoQueryClient(
        HttpClient httpClient,
        IOptions<TodoAccessorOptions> options,
        ILogger<HttpTodoQueryClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _accessorBaseUrl = options.Value.BaseUrl.TrimEnd('/');
    }

    public async Task<TodoItemDto?> GetTodoAsync(Guid id)
    {
        // PUT BREAKPOINT HERE
        
        try
        {
            var url = $"{_accessorBaseUrl}/todos/{id}";
            _logger.LogDebug("Fetching todo from Accessor: {Url}", url);
            
            var response = await _httpClient.GetAsync(url);
            
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            
            response.EnsureSuccessStatusCode();
            
            var todo = await response.Content.ReadFromJsonAsync<TodoItemDto>();
            return todo;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching todo {Id} from Accessor", id);
            throw;
        }
    }
}

// Configuration class for TodoAccessor settings
public class TodoAccessorOptions
{
    public const string SectionName = "TodoAccessor";
    
    public string BaseUrl { get; set; } = "http://localhost:5292"; // Default to Accessor port
}
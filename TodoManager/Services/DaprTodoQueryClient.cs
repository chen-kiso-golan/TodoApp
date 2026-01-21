using Dapr.Client;
using Microsoft.Extensions.Logging;
using TodoManager.Contracts;
using System.Net;

namespace TodoManager.Services;

public class DaprTodoQueryClient : ITodoQueryClient
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<DaprTodoQueryClient> _logger;
    private const string AccessorAppId = "todoaccessor";

    public DaprTodoQueryClient(
        DaprClient daprClient,
        ILogger<DaprTodoQueryClient> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    public async Task<TodoItemDto?> GetTodoAsync(Guid id)
    {
        // PUT BREAKPOINT HERE - you can debug this!
        
        try
        {
            _logger.LogDebug("Fetching todo {Id} from Accessor via Dapr", id);
            
            // Use Dapr service invocation with HttpMethod.Get
            var todo = await _daprClient.InvokeMethodAsync<TodoItemDto>(
                HttpMethod.Get,
                AccessorAppId,
                $"todos/{id}");
            
            return todo;
        }
        catch (InvocationException ex)
        {
            // Check if it's a 404 (not found)
            if (ex.Response?.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogDebug("Todo {Id} not found", id);
                return null;
            }
            
            // For other errors, log and rethrow
            _logger.LogError(ex, "Error fetching todo {Id} from Accessor via Dapr", id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error fetching todo {Id} from Accessor via Dapr", id);
            throw;
        }
    }
}
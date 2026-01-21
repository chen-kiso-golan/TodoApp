using TodoManager.Contracts;
using TodoManager.Services;

namespace TodoManager.Endpoints;

public static class TodoEndpoints
{
    // Extension method to map all todo endpoints
    public static WebApplication MapTodoEndpoints(this WebApplication app)
    {
        // GET /todos/{id:guid} - Get a todo by ID
        app.MapGet("/todos/{id:guid}", GetTodo)
            .WithName("GetTodo");

     // POST /todos - Create a new todo
        app.MapPost("/todos", CreateTodo)
            .WithName("CreateTodo");

        return app;
    }

    // Handler for GET /todos/{id}
    static async Task<IResult> GetTodo(Guid id, ITodoQueryClient queryClient)
    {
        // PUT BREAKPOINT HERE - you can debug this!
        
        var todo = await queryClient.GetTodoAsync(id);
        
        if (todo == null)
        {
            return Results.NotFound();
        }
        
        return Results.Ok(todo);
    }


    // Handler for POST /todos
    static IResult CreateTodo(CreateTodoRequest request)
    {
        // PUT BREAKPOINT HERE
        
        // Validation: title must not be null/empty/whitespace
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return Results.BadRequest(new { error = "Title is required and cannot be empty or whitespace" });
        }

        // Trim title and description
        var title = request.Title.Trim();
        var description = request.Description?.Trim();

        // Generate new Guid id
        var id = Guid.NewGuid();

        // For now, just return the ID (we'll publish later)
        // TODO: Publish to queue later
        return Results.Accepted($"/todos/{id}", new { id });
    }
}
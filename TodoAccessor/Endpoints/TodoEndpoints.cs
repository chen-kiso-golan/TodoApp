using TodoAccessor.Models;
using Microsoft.EntityFrameworkCore;
using TodoAccessor.Data;

namespace TodoAccessor.Endpoints;

public static class TodoEndpoints
{
    // Extension method to map all todo endpoints
    public static WebApplication MapTodoEndpoints(this WebApplication app)
    {
        // GET /todos/{id}
        app.MapGet("/todos/{id:guid}", GetTodo)
            .WithName("GetTodo")
            .WithOpenApi();

        // POST /todos
        app.MapPost("/todos", CreateTodo)
            .WithName("CreateTodo")
            .WithOpenApi()
            .Accepts<CreateTodoRequest>("application/json");

        return app;
    }

    // Handler for GET /todos/{id}
   static async Task<IResult> GetTodo(Guid id, TodoDbContext db)
   {
    // PUT BREAKPOINT HERE - you can debug this!
    
    // Query database for the todo
      var todo = await db.TodoItems.FindAsync(id);
    
      if (todo == null)
      {
        return Results.NotFound();
      }
    
      return Results.Ok(todo);
    }

   // Handler for POST /todos
    static async Task<IResult> CreateTodo(CreateTodoRequest request, TodoDbContext db)
    {
        // PUT BREAKPOINT HERE - you can debug this!
        
        // Check if ID already exists
        var exists = await db.TodoItems.AnyAsync(t => t.Id == request.Id);
        if (exists)
        {
            return Results.Conflict(new { message = $"Todo with id {request.Id} already exists" });
        }

        // Create new todo item
        var todo = new TodoItem
        {
            Id = request.Id,
            Title = request.Title,
            Description = request.Description,
            CreatedAtUtc = DateTime.UtcNow
        };

        // Add to database
        db.TodoItems.Add(todo);
        await db.SaveChangesAsync();

        // Return 201 Created with the todo
        return Results.Created($"/todos/{todo.Id}", todo);
    }
}

// DataTransferObject for creating a todo
public record CreateTodoRequest
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
}


namespace TodoManager.Contracts;

public class CreateTodoRequest
{
    public required string Title { get; set; }
    
    public string? Description { get; set; }
}
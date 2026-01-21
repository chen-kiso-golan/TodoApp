namespace TodoManager.Contracts;

public class TodoItemDto
{
    public Guid Id { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public DateTime CreatedAtUtc { get; set; }
}
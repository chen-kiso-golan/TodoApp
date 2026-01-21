namespace TodoAccessor.Models;

public class TodoItem
{
    public Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public string? Description { get; set; }
    
    public DateTime CreatedAtUtc { get; set; }
}
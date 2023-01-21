using TodoApi.Models;

public class User
{
    public string Id { get; set; }
    public ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
}
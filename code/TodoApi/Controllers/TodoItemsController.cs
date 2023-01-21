using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers;

[Route("api/todoapi/[controller]")]
[ApiController]
public class TodoItemsController : ControllerBase
{
    private readonly TodoContext _context;

    public TodoItemsController(TodoContext context)
    {
        _context = context;
    }

    // GET: api/TodoItems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
    {
        var userId = GetUserIdFromJWT();
        return await _context.TodoItems
            .Where(x => x.UserId == userId)
            .Select(x => ItemToDTO(x))
            .ToListAsync();
    }

    // GET: api/TodoItems/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
    {
        var userId = GetUserIdFromJWT();
        var todoItem = await _context.TodoItems
                            .Where(x => x.UserId == userId && x.Id == id)
                            .FirstOrDefaultAsync();

        if (todoItem == null)
        {
            return NotFound();
        }

        return ItemToDTO(todoItem);
    }

    // PUT: api/TodoItems/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoDTO)
    {
        var userId = GetUserIdFromJWT();
        var todoItem = await _context.TodoItems
                            .Where(x => x.UserId == userId && x.Id == id)
                            .FirstOrDefaultAsync();

        if (todoItem == null)
        {
            return NotFound();
        }

        todoItem.Name = todoDTO.Name;
        todoItem.IsComplete = todoDTO.IsComplete;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    // POST: api/TodoItems
    [HttpPost]
    public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoDTO)
    {
        var userId = GetUserIdFromJWT();
        var todoItem = new TodoItem
        {
            IsComplete = todoDTO.IsComplete,
            Name = todoDTO.Name,
            UserId = userId
        };

        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTodoItem),
            new { id = todoItem.Id },
            ItemToDTO(todoItem));
    }

    // DELETE: api/TodoItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
        var userId = GetUserIdFromJWT();
        var todoItem = await _context.TodoItems
                            .Where(x => x.UserId == userId && x.Id == id)
                            .FirstOrDefaultAsync();
        if (todoItem == null)
        {
            return NotFound();
        }

        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TodoItemExists(long id)
    {
        var userId = GetUserIdFromJWT();
        return _context.TodoItems.Any(e => e.Id == id && e.UserId == userId);
    }

    private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
       new TodoItemDTO
       {
           Id = todoItem.Id,
           Name = todoItem.Name,
           IsComplete = todoItem.IsComplete
       };
    private string GetUserIdFromJWT()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            throw new Exception("User Id not found in JWT token");
        }
        return userId;
    }

}
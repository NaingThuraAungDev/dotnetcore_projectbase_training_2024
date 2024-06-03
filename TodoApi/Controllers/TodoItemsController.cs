using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

[ApiController]
[Route("api/[controller]")]
public class TodoItemsController : ControllerBase
{
    private readonly TodoContext _context;

    public TodoItemsController(TodoContext context)
    {
        _context = context;
    }

    // POST: api/AddTodoItem
    [HttpPost("AddTodoItem", Name = "AddTodoItem")]
    public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
    {
        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        return Created("", todoItem);
    }

    // GET: api/TodoItems
    [HttpGet("GetTodoItems", Name = "GetTodoItems")]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
    {
        return await _context.TodoItems.ToListAsync();
    }

    // PUT: api/UpdateTodoItem/5
    [HttpPut("UpdateTodoItem", Name = "UpdateTodoItem")]
    public async Task<IActionResult> PutTodoItem(TodoItem todoItem)
    {
        _context.Entry(todoItem).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return Ok(todoItem);
    }

    // DELETE: api/DeleteTodoItem/5
    [HttpDelete("DeleteTodoItem/{id}", Name = "DeleteTodoItem")]
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();
        return Ok();
    }

}
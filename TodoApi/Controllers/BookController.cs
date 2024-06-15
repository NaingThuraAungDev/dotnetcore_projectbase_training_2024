using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.DTO;
using TodoApi.Models;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly AppDB _context;

    public BookController(AppDB context)
    {
        _context = context;
    }

    // GET: api/allbooks
    [HttpGet("GetAllBooks", Name = "GetAllBooks")]
    public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
    {
        return await _context.GetAll<Book>("SELECT * FROM Book;", new DynamicParameters());
    }

    // POST: api/AddBook
    [HttpPost("AddBook", Name = "AddBook")]
    public async Task<ActionResult<Book>> AddBook(BookDTO book)
    {
        string query = "INSERT INTO book (title, author, description, price, category) VALUES (@Title, @Author, @Description, @Price, @Category);";
        await _context.EditData(query, book);
        return Created(nameof(GetAllBooks), book);
    }

    // PUT: api/UpdateBook
    [HttpPut("UpdateBook", Name = "UpdateBook")]
    public async Task<IActionResult> UpdateBook(UpdateBookRequestDTO book)
    {
        string query = "UPDATE book SET price = @Price WHERE book_id = @BookID;";
        await _context.EditData(query, book);
        return Ok(book);
    }

    // DELETE: api/DeleteBook/1
    [HttpDelete("DeleteBook/{bookID}", Name = "DeleteBook")]
    public async Task<IActionResult> DeleteBook(int bookID)
    {
        string query = "DELETE FROM book WHERE book_id = @id;";
        await _context.EditData(query, new { id = bookID });
        return Ok();
    }


}
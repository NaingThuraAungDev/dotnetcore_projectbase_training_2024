using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.DTO;
using TodoApi.Models;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly AppDB _context;

    public CategoryController(AppDB context)
    {
        _context = context;
    }

    // GET: api/Category
    [HttpGet("GetCategories", Name = "GetCategories")]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        return await _context.Category.ToListAsync();
    }

    // GET: api/Category/5
    [HttpGet("GetCategory/{id}", Name = "GetCategory")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var category = await _context.Category.FindAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        return category;
    }

    // POST: api/Category
    [HttpPost("AddCategory", Name = "AddCategory")]
    public async Task<ActionResult<Category>> AddCategory(CategoryDTO category)
    {
        Category newObj = new Category
        {
            category_name = category.CategoryName
        };
        _context.Category.Add(newObj);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryName }, category);
    }

    // PUT: api/Category/5
    [HttpPut("UpdateCategory/{id}", Name = "UpdateCategory")]
    public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryRequestDTO category)
    {
        if (id != category.CategoryID)
        {
            return BadRequest();
        }
        Category categoryObj = new Category
        {
            category_id = category.CategoryID,
            category_name = category.CategoryName
        };

        _context.Entry(categoryObj).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound();
        }

        return Ok(categoryObj);
    }

    // DELETE: api/Category/5
    [HttpDelete("DeleteCategory/{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Category.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        _context.Category.Remove(category);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("GetBooksCountByCategory", Name = "GetBooksCountByCategory")]
    public async Task<ActionResult<IEnumerable<GetBooksCountByCategoryResponseDTO>>> GetBooksCountByCategory()
    {
        var mainQuery = from c in _context.Category
                        join b in _context.Book on c.category_id equals b.category
                        group b by c.category_name into g
                        select new GetBooksCountByCategoryResponseDTO
                        {
                            CategoryName = g.Key,
                            BookCount = g.Count()
                        };
        return Ok(mainQuery);
    }
}
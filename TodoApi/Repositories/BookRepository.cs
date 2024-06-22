using Microsoft.EntityFrameworkCore;
using TodoApi.DTO;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(AppDB context) : base(context)
        {
        }

        public async Task<GetBookByCategoryNameResponseDTO?> GetBookByCategoryName(string categoryName)
        {
            var mainQuery = from b in _context.Book
                            join c in _context.Category on b.category equals c.category_id
                            where c.category_name == categoryName
                            select new GetBookByCategoryNameResponseDTO
                            {
                                Title = b.title,
                                Author = b.author,
                                Description = b.description,
                                Price = b.price,
                                Category = c.category_name
                            };
            return await mainQuery.FirstOrDefaultAsync();
        }
    }
}
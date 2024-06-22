using TodoApi.DTO;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface IBookRepository : IRepositoryBase<Book>
    {
        Task<GetBookByCategoryNameResponseDTO?> GetBookByCategoryName(string categoryName);
    }
}
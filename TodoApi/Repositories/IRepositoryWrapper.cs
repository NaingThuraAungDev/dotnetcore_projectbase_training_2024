namespace TodoApi.Repositories
{
    public interface IRepositoryWrapper
    {
        IBookRepository Book { get; }
    }
}
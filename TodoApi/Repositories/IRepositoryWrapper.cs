namespace TodoApi.Repositories
{
    public interface IRepositoryWrapper
    {
        IBookRepository Book { get; }
        IEventLogRepository EventLog { get; }
    }
}
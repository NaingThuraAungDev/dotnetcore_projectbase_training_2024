namespace TodoApi.Repositories
{
    public class RepositoryWrapper(AppDB context) : IRepositoryWrapper
    {
        readonly AppDB _context = context;
        private IBookRepository? _book;
        public IBookRepository Book
        {
            get
            {
                _book ??= new BookRepository(_context);
                return _book;
            }
        }

        private IEventLogRepository? _eventLog;
        public IEventLogRepository EventLog
        {
            get
            {
                _eventLog ??= new EventLogRepository(_context);
                return _eventLog;
            }
        }
    }
}
using TodoApi.DTO;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public class EventLogRepository : RepositoryBase<EventLog>, IEventLogRepository
    {
        public EventLogRepository(AppDB context) : base(context)
        {
        }

    }
}
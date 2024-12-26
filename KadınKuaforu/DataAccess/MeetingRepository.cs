using KadınKuaforu.Models;
using Microsoft.EntityFrameworkCore;

namespace KadınKuaforu.DataAccess
{
    public class MeetingRepository : Repository<Meeting>, IMeetingRepository
    {
        private readonly KuaforDbContext _dbContext;
        private readonly DbSet<Meeting> _dbset;
        public MeetingRepository(KuaforDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbset = _dbContext.Set<Meeting>();
        }
    }
}

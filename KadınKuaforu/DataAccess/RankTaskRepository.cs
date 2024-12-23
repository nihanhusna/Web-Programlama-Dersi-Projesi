using KadınKuaforu.Models;
using Microsoft.EntityFrameworkCore;

namespace KadınKuaforu.DataAccess
{
    public class RankTaskRepository : Repository<RankTask>, IRankTaskRepository
    {
        private readonly KuaforDbContext _dbContext;
        private readonly DbSet<RankTask> _dbset;
        public RankTaskRepository(KuaforDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbset = _dbContext.Set<RankTask>();
        }
    }
}

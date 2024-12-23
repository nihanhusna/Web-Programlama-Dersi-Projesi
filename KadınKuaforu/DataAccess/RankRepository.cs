using KadınKuaforu.Models;
using Microsoft.EntityFrameworkCore;

namespace KadınKuaforu.DataAccess
{
    public class RankRepository : Repository<Rank>, IRankRepository
    {
        private readonly KuaforDbContext _dbContext;
        private readonly DbSet<Rank> _dbset;
        public RankRepository(KuaforDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbset = _dbContext.Set<Rank>();
        }
    }
}

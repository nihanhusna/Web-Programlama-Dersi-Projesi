using KadınKuaforu.Models;
using Microsoft.EntityFrameworkCore;

namespace KadınKuaforu.DataAccess
{
    public class ExpertOfTaskRepository : Repository<ExpertOfTask>, IExpertOfTaskRepository
    {
        private readonly KuaforDbContext _dbContext;
        private readonly DbSet<ExpertOfTask> _dbset;
        public ExpertOfTaskRepository(KuaforDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbset = _dbContext.Set<ExpertOfTask>();
        }
    }
}

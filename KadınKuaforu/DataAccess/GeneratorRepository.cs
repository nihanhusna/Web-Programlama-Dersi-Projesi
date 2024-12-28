using KadınKuaforu.Models;
using Microsoft.EntityFrameworkCore;

namespace KadınKuaforu.DataAccess
{
    public class GeneratorRepository : Repository<Generator>, IGeneratorRepository
    {
        private readonly KuaforDbContext _dbContext;
        private readonly DbSet<Generator> _dbset;
        public GeneratorRepository(KuaforDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbset = _dbContext.Set<Generator>();
        }
    }
}

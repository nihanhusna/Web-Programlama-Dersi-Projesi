using KadınKuaforu.Models;
using Microsoft.EntityFrameworkCore;

namespace KadınKuaforu.DataAccess
{
    public class PersonnelRepository : Repository<Personnel>, IPersonnelRepository
    {
        private readonly KuaforDbContext _dbContext;
        private readonly DbSet<Personnel> _dbset;
        public PersonnelRepository(KuaforDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbset = _dbContext.Set<Personnel>();
        }
    }
}

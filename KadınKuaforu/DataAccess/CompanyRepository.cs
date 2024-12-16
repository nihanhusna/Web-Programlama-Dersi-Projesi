using KadınKuaforu.Models;
using Microsoft.EntityFrameworkCore;

namespace KadınKuaforu.DataAccess
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly KuaforDbContext _dbContext;
        private readonly DbSet<Company> _dbset;
        public CompanyRepository(KuaforDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbset = _dbContext.Set<Company>();
        }
    }
}

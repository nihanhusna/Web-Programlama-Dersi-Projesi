using KadınKuaforu.Models;
using Microsoft.EntityFrameworkCore;

namespace KadınKuaforu.DataAccess
{
    public class PersonnelShiftRepository : Repository<PersonnelShift>, IPersonnelShiftRepository
    {
        private readonly KuaforDbContext _dbContext;
        private readonly DbSet<PersonnelShift> _dbset;
        public PersonnelShiftRepository(KuaforDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbset = _dbContext.Set<PersonnelShift>();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KadınKuaforu.DataAccess
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly KuaforDbContext _dbContext;
        internal DbSet<T> dbSet;

        public Repository(KuaforDbContext dbContext)
        {
            _dbContext = dbContext;
            this.dbSet = _dbContext.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public IEnumerable<T> GetAll(string? includeProps = null)
        {
            IQueryable<T> request = dbSet;
            if (!string.IsNullOrEmpty(includeProps))
            {
                foreach (var prop in includeProps.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    request = request.Include(prop);
                }
            }
            return request.ToList();
        }

        public IEnumerable<T> GetAllByCondition(Expression<Func<T, bool>> filter, string? includeProps = null)
        {
            IQueryable<T> request = dbSet;
            request = request.Where(filter);
            if (!string.IsNullOrEmpty(includeProps))
            {
                foreach (var prop in includeProps.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    request = request.Include(prop);
                }
            }
            return request.ToList();
        }

        public T GetById(int id)
        {
            return dbSet.Find(id);
        }

        public T GetByIdWithProps(Expression<Func<T, bool>> filter, string? includeProps = null)
        {
            IQueryable<T> request = dbSet;
            request = request.Where(filter);
            if (!string.IsNullOrEmpty(includeProps))
            {
                foreach (var prop in includeProps.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    request = request.Include(prop);
                }
            }
            return request.SingleOrDefault();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Update(T entity)
        {
            dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}

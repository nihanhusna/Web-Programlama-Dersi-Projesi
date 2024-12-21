﻿using System.Linq.Expressions;

namespace KadınKuaforu.DataAccess
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(string? includeProps = null);
        T GetByIdWithProps(Expression<Func<T, bool>> filter, string? includeProps = null);
        T GetById(int id);
        IEnumerable<T> GetAllByCondition(Expression<Func<T, bool>> filter, string? includeProps = null);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();
    }
}

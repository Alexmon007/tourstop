using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repositories.Contracts
{
    public interface IBaseRepository<T>
    {
        T GetbyKey(object key);
        ICollection<T> Search(Func<T, bool> predicate);
        IEnumerable<T> GetAll();
        IQueryable<T> GetQueryable();
        T AddOrUpdate(T entity);
        void Remove(int id);
        void SaveChanges();
    }
}
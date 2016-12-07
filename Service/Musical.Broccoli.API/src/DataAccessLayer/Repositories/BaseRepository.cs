using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// All Data manipulation logic.
        /// </summary>
        protected readonly TourStopContext Context;
        protected DbSet<T> DbSet;

        /// <summary>
        /// Gets the context
        /// </summary>
        /// <param name="context">Database Instance</param>
        public BaseRepository(TourStopContext context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }

        #region Read
        /// <summary>
        /// Gets table of the Database with its records.
        /// </summary>
        /// <returns>List of an Database entity</returns>
        public IQueryable<T> GetQueryable()
        {
            return DbSet.AsNoTracking();
        }

        /// <summary>
        /// Gets all records of a table
        /// </summary>
        /// <returns>List of an Database entity</returns>
        public IEnumerable<T> GetAll()
        {
            return DbSet.AsNoTracking();
        }

        /// <summary>
        /// Find a matching object from the Database
        /// </summary>
        /// <param name="key">Identitier condition</param>
        /// <returns>Single entity</returns>
        public T GetbyKey(object key)
        {
            return DbSet.Find(key);
        }

        /// <summary>
        /// Find matching objects from Database.
        /// </summary>
        /// <param name="predicate">Conditions</param>
        /// <returns>List of an Database entity</returns>
        public ICollection<T> Search(Func<T, bool> predicate)
        {
            return DbSet.Where(predicate).ToList();
        }

        #endregion

        #region Write

        /// <summary>
        /// Add or updates a record from database
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Same entity</returns>
        public T AddOrUpdate(T entity)
        {
            return entity.Id == 0 ? DbSet.Add(entity).Entity : DbSet.Update(entity).Entity;
        }

        /// <summary>
        /// Removes a matching record from database
        /// </summary>
        /// <param name="id">Entity's Identifier</param>
        public void Remove(int id)
        {
            var entity = DbSet.Find(id);
            Context.Entry(entity).State = EntityState.Deleted;
        }

        /// <summary>
        /// Context save changes
        /// </summary>
        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        #endregion
    }
}
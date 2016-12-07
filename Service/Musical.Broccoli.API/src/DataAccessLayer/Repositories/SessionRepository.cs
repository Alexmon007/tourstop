using System.Linq;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class SessionRepository : BaseRepository<Session>, ISessionRepository
    {
        /// <summary>
        /// All logic on base repository
        /// </summary>
        /// <param name="context">Instance of Database</param>
        public SessionRepository(TourStopContext context) : base(context)
        {
        }
        /// <summary>
        /// Retrieves Sessions records from Databse including its matching user.
        /// </summary>
        /// <returns>List of Sessions</returns>
        public new IQueryable<Session> GetQueryable()
        {
            return DbSet.AsNoTracking().Include(x => x.User);
        }
    }
}
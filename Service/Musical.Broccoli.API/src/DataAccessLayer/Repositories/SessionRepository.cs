using System.Linq;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class SessionRepository : BaseRepository<Session>, ISessionRepository
    {
        public SessionRepository(TourStopContext context) : base(context)
        {
        }

        public new IQueryable<Session> GetQueryable()
        {
            return DbSet.AsNoTracking().Include(x => x.User);
        }
    }
}
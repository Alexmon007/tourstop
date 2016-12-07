using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Contracts;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        /// <summary>
        /// All logic on base repository
        /// </summary>
        /// <param name="context">Instance of Database</param>
        public UserRepository(TourStopContext context) : base(context)
        {
        }
    }
}
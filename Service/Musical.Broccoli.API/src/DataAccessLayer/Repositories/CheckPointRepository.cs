using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Contracts;

namespace DataAccessLayer.Repositories
{
    public class CheckPointRepository : BaseRepository<CheckPoint>, ICheckPointRepository
    {
        /// <summary>
        /// All logic on base repository
        /// </summary>
        /// <param name="context">Instance of Database</param>
        public CheckPointRepository(TourStopContext context) : base(context)
        {
        }
    }
}
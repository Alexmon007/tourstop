using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Contracts;

namespace DataAccessLayer.Repositories
{
    public class RatingRepository : BaseRepository<Rating>, IRatingRepository
    {
        /// <summary>
        /// All logic on base repository
        /// </summary>
        /// <param name="context">Instance of Database</param>
        public RatingRepository(TourStopContext context) : base(context)
        {
        }
    }
}
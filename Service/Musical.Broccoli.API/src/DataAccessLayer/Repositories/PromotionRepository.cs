using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Contracts;

namespace DataAccessLayer.Repositories
{
    public class PromotionRepository : BaseRepository<Promotion>, IPromotionRepository
    {
        /// <summary>
        /// All logic on base repository
        /// </summary>
        /// <param name="context">Instance of Database</param>
        public PromotionRepository(TourStopContext context) : base(context)
        {
        }
    }
}
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Contracts;

namespace DataAccessLayer.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
         /// <summary>
       /// All logic on base repository
       /// </summary>
       /// <param name="context">Instance of Database</param>
        public OrderRepository(TourStopContext context) : base(context)
        {
        }
    }
}
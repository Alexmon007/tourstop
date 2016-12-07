using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Contracts;

namespace DataAccessLayer.Repositories
{
    public class PaymentInfoRepository : BaseRepository<PaymentInfo>, IPaymentInfoRepository
    {
        /// <summary>
        /// All logic on base repository
        /// </summary>
        /// <param name="context">Instance of Database</param>
        public PaymentInfoRepository(TourStopContext context) : base(context)
        {
        }
    }
}
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Contracts;

namespace DataAccessLayer.Repositories
{
    public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
    {
        /// <summary>
        /// All logic on base repository
        /// </summary>
        /// <param name="context">Instance of Database</param>
        public ReservationRepository(TourStopContext context) : base(context)
        {
        }
    }
}
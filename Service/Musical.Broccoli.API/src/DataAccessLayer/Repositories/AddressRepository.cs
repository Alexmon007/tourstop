using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Contracts;

namespace DataAccessLayer.Repositories
{
    public class AddressRepository : BaseRepository<Address>, IAddressRepository
    {
       /// <summary>
       /// All logic on base repository
       /// </summary>
       /// <param name="context">Instance of Database</param>
        public AddressRepository(TourStopContext context) : base(context)
        {
        }
    }
}
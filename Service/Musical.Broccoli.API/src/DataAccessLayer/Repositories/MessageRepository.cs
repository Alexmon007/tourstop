using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Contracts;

namespace DataAccessLayer.Repositories
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        /// <summary>
        /// All logic on base repository
        /// </summary>
        /// <param name="context">Instance of Database</param>
        public MessageRepository(TourStopContext context) : base(context)
        {
        }
    }
}
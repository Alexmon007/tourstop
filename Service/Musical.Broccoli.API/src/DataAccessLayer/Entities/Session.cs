using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities
{
    public class Session : BaseEntity
    {
        public override int Id { get; set; }
        public Guid AuthToken { get; set; }
        public int UserID { get; set; }
        public DateTime DateCreated { get; set; }

        public User User { get; set; }
    }
}
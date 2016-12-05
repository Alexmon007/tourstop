using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entities
{
    public class Tour : BaseEntity
    {
        public override int Id { get; set; }
        public string Title { get; set; }
        public int MaxReservation { get; set; }
        public float ReservationPrice { get; set; }
        public bool Status { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Rating> Ratings { get; set; }
    }
}
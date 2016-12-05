using System;
using System.Collections.Generic;

namespace Common.DTOs
{
    public class TourDTO : BaseDTO
    {
        public override int Id { get; set; }
        public string Title { get; set; }
        public int MaxReservation { get; set; }
        public float ReservationPrice { get; set; }
        public bool Status { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public int UserId { get; set; }

        public UserDTO User { get; set; }
        public ICollection<ReservationDTO> Reservations { get; set; }
        public ICollection<RatingDTO> Ratings { get; set; }
    }
}
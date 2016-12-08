using System.ComponentModel.DataAnnotations;
using Common.Enums;

namespace DataAccessLayer.Entities
{
    public class Promotion : BaseEntity
    {
        public override int Id { get; set; }

        [Required]
        public float Value { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public PromoType PromoType { get; set; }

        [Required]
        public string PromoCode { get; set; }

        [Required]
        public int TourId { get; set; }

        public Tour Tour { get; set; }
    }
}
using System;
using System.Linq;
using Common.DTOs;

namespace Business.Handlers.Validation.Dto
{
    public sealed class TourValidator : BaseValidator<TourDTO>
    {
        public override Func<TourDTO, ValidationResult> Validate { get; internal set; }

        public TourValidator And(TourValidator other)
        {
            return new TourValidator
            {
                Validate = x => Validate(x) + other.Validate(x)
            };
        }

        public static TourValidator Holds(Predicate<TourDTO> predicate, string message)
        {
            return new TourValidator
            {
                Validate = tour => predicate.Invoke(tour) ? ValidationResult.Valid() : ValidationResult.Invalid(message)
            };
        }

        #region Validators

        public static TourValidator HasId()
        {
            return Holds(x => x.Id > 0, "Invalid Id");
        }

        public static TourValidator NameNotEmpty()
        {
            return Holds(tour => !string.IsNullOrEmpty(tour.Title), "Title is null or empty");
        }

        public static TourValidator MaxReservationIsNotZeroOrLess()
        {
            return Holds(tour => tour.MaxReservation > 0, "Max Reservation is zero or less");
        }

        public static TourValidator ReservationPriceIsNotNegative()
        {
            return Holds(tour => tour.ReservationPrice >= 0, "Reservation price is negative");
        }

        #endregion

        public static TourValidator All(params TourValidator[] validators)
        {
            return validators.Aggregate((x, y) => x.And(y));
        }

        public static TourValidator All()
        {
            return All(NameNotEmpty(), MaxReservationIsNotZeroOrLess(), ReservationPriceIsNotNegative());
        }
    }
}
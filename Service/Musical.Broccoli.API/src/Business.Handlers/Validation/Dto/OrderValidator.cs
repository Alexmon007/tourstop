using System;
using System.Linq;
using Common.DTOs;

namespace Business.Handlers.Validation.Dto
{
    public class OrderValidator : BaseValidator<OrderDTO>
    {
        public override Func<OrderDTO, ValidationResult> Validate { get; internal set; }

        public OrderValidator And(OrderValidator other)
        {
            return new OrderValidator
            {
                Validate = x => Validate(x) + other.Validate(x)
            };
        }

        public static OrderValidator Holds(Predicate<OrderDTO> predicate, string message)
        {
            return new OrderValidator
            {
                Validate = x => predicate.Invoke(x) ? ValidationResult.Valid() : ValidationResult.Invalid(message)
            };
        }

        #region Validator

        public static OrderValidator HasId()
        {
            return Holds(x => x.Id > 0, "Invalid Id");
        }

        public static OrderValidator UserIsValid()
        {
            return Holds(x => x.UserId == 0, "Invalid User");
        }

        public static OrderValidator PaymentTypeIsValid()
        {
            return Holds(x => x.PaymentType == 0, "Invalid PaymentType");
        }

        public static OrderValidator TotalAmountIsValid()
        {
            return Holds(x => x.TotalAmount < 0, "Invalid TotalAmount");
        }

        public static OrderValidator ReservationsAreNotEmpty()
        {
            return Holds(x => x.Reservations.Count == 0, "Invalid TotalAmount");
        }

        #endregion

        public static OrderValidator All()
        {
            return All(UserIsValid(), PaymentTypeIsValid(), TotalAmountIsValid(), ReservationsAreNotEmpty());
        }

        public static OrderValidator All(params OrderValidator[] validators)
        {
            return validators.Aggregate((x, y) => x.And(y));
        }
    }
}
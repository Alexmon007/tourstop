using System;
using System.Linq;
using Common.DTOs;

namespace Business.Handlers.Validation.Dto
{
    public class OrderValidator : BaseValidator<OrderDTO>
    {
        /// <summary>
        /// CheckPoint Data Validation
        /// </summary>
        public override Func<OrderDTO, ValidationResult> Validate { get; internal set; }

        /// <summary>
        /// Validate more than one validation
        /// </summary>
        /// <param name="other">Validator</param>
        /// <returns></returns>
        public OrderValidator And(OrderValidator other)
        {
            return new OrderValidator
            {
                Validate = x => Validate(x) + other.Validate(x)
            };
        }

        /// <summary>
        /// Performs the validation
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <param name="message">Error Message</param>
        /// <returns></returns>
        public static OrderValidator Holds(Predicate<OrderDTO> predicate, string message)
        {
            return new OrderValidator
            {
                Validate = x => predicate.Invoke(x) ? ValidationResult.Valid() : ValidationResult.Invalid(message)
            };
        }

        #region Validator

        /// <summary>
        /// Id Validation
        /// </summary>
        /// <returns>Result</returns>
        public static OrderValidator HasId()
        {
            return Holds(x => x.Id > 0, "Invalid Id");
        }

        /// <summary>
        /// User Validation
        /// </summary>
        /// <returns>Result</returns>
        public static OrderValidator UserIsValid()
        {
            return Holds(x => x.UserId == 0, "Invalid User");
        }

        /// <summary>
        /// Payment Validation
        /// </summary>
        /// <returns>Result</returns>
        public static OrderValidator PaymentTypeIsValid()
        {
            return Holds(x => x.PaymentType == 0, "Invalid PaymentType");
        }

        /// <summary>
        /// Invalid Validation
        /// </summary>
        /// <returns>Result</returns>
        public static OrderValidator TotalAmountIsValid()
        {
            return Holds(x => x.TotalAmount < 0, "Invalid TotalAmount");
        }

        /// <summary>
        /// TotalAmount Validation
        /// </summary>
        /// <returns>Result</returns>
        public static OrderValidator ReservationsAreNotEmpty()
        {
            return Holds(x => x.Reservations.Count == 0, "Invalid TotalAmount");
        }

        #endregion

        /// <summary>
        /// Perform all validations
        /// </summary>
        /// <param name="validators">Validations</param>
        /// <returns>Validation Result</returns>
        public static OrderValidator All()
        {
            return All(UserIsValid(), PaymentTypeIsValid(), TotalAmountIsValid(), ReservationsAreNotEmpty());
        }

        /// <summary>
        /// Perform all validations
        /// </summary>
        /// <param name="validators">Validations</param>
        /// <returns>Validation Result</returns>
        public static OrderValidator All(params OrderValidator[] validators)
        {
            return validators.Aggregate((x, y) => x.And(y));
        }
    }
}
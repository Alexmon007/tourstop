using System;
using System.Linq;
using Common.DTOs;

namespace Business.Handlers.Validation.Dto
{
    /// <summary>
    /// Movement Data Validation
    /// </summary>
    public class MovementValidator : BaseValidator<MovementDTO>
    {
        public override Func<MovementDTO, ValidationResult> Validate { get; internal set; }
        
        /// <summary>
        /// Validate more than one validation
        /// </summary>
        /// <param name="other">Validator</param>
        /// <returns></returns>
        public MovementValidator And(MovementValidator other)
        {
            return new MovementValidator
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
        public static MovementValidator Holds(Predicate<MovementDTO> predicate, string message)
        {
            return new MovementValidator
            {
                Validate = x => predicate.Invoke(x) ? ValidationResult.Valid() : ValidationResult.Invalid(message)
            };
        }

        #region Validators

        /// <summary>
        /// MovementType Validation
        /// </summary>
        /// <returns>Result</returns>
        public static MovementValidator MovementTypeIsValid()
        {
            return Holds(x => x.MovementType == 0, "Invalid MovementType");
        }

        /// <summary>
        /// Reservation Validation
        /// </summary>
        /// <returns>Result</returns>
        public static MovementValidator ReservationIdIsValid()
        {
            return Holds(x => x.ReservationId == 0, "Invalid Reservation");
        }

        /// <summary>
        /// Order Validation
        /// </summary>
        /// <returns>Result</returns>
        public static MovementValidator OrderIdIsValid()
        {
            return Holds(x => x.OrderId == 0, "Invalid Order");
        }

        #endregion
        /// <summary>
        /// Perform all validations
        /// </summary>
        /// <param name="validators">Validations</param>
        /// <returns>Validation Result</returns>
        public static MovementValidator All()
        {
            return All(MovementTypeIsValid(), ReservationIdIsValid(), OrderIdIsValid());
        }

        /// <summary>
        /// Perform all validations
        /// </summary>
        /// <param name="validators">Validations</param>
        /// <returns>Validation Result</returns>
        public static MovementValidator All(params MovementValidator[] validators)
        {
            return validators.Aggregate((x, y) => x.And(y));
        }
    }
}
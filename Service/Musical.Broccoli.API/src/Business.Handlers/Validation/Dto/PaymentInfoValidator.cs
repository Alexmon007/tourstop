using System;
using System.Linq;
using Common.DTOs;

namespace Business.Handlers.Validation.Dto
{
    /// <summary>
    /// CheckPoint Data Validation
    /// </summary>
    public class PaymentInfoValidator : BaseValidator<PaymentInfoDTO>
    {
        public override Func<PaymentInfoDTO, ValidationResult> Validate { get; internal set; }

        /// <summary>
        /// Validate more than one validation
        /// </summary>
        /// <param name="other">Validator</param>
        /// <returns></returns>
        public PaymentInfoValidator And(PaymentInfoValidator other)
        {
            return new PaymentInfoValidator
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
        public static PaymentInfoValidator Holds(Predicate<PaymentInfoDTO> predicate, string message)
        {
            return new PaymentInfoValidator
            {
                Validate = x => predicate.Invoke(x) ? ValidationResult.Valid() : ValidationResult.Invalid(message)
            };
        }

        /// <summary>
        /// Perform all validations
        /// </summary>
        /// <param name="validators">Validations</param>
        /// <returns>Validation Result</returns>
        public static PaymentInfoValidator All()
        {
            return All();
        }

        /// <summary>
        /// Perform all validations
        /// </summary>
        /// <param name="validators">Validations</param>
        /// <returns>Validation Result</returns>
        public static PaymentInfoValidator All(params PaymentInfoValidator[] validators)
        {
            var validatorsList = validators.ToList();
            return validators.Aggregate((x, y) => x.And(y));
        }
    }
}
using System;
using System.Linq;
using Common.DTOs;

namespace Business.Handlers.Validation.Dto
{
    /// <summary>
    /// CheckPoint Data Validation
    /// </summary>
    public class CheckPointValidator : BaseValidator<CheckPointDTO>
    {
        public override Func<CheckPointDTO, ValidationResult> Validate { get; internal set; }

        /// <summary>
        /// Validate more than one validation
        /// </summary>
        /// <param name="other">Validator</param>
        /// <returns></returns>
        public CheckPointValidator And(CheckPointValidator other)
        {
            return new CheckPointValidator
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
        public static CheckPointValidator Holds(Predicate<CheckPointDTO> predicate, string message)
        {
            return new CheckPointValidator
            {
                Validate = x => predicate.Invoke(x) ? ValidationResult.Valid() : ValidationResult.Invalid(message)
            };
        }

        #region Validators

        /// <summary>
        /// Address Validation
        /// </summary>
        /// <returns>Result</returns>
        public static CheckPointValidator AddressIdIsValid()
        {
            return Holds(x => x.AddressId == 0, "Invalid Address");
        }

        /// <summary>
        /// Tour Validation
        /// </summary>
        /// <returns>Result</returns>
        public static CheckPointValidator TourIdIsValid()
        {
            return Holds(x => x.TourId == 0, "Invalid Tour");
        }

        #endregion

        /// <summary>
        /// Perform all validations
        /// </summary>
        /// <param name="validators">Validations</param>
        /// <returns>Validation Result</returns>
        public static CheckPointValidator All()
        {
            return All(AddressIdIsValid(), TourIdIsValid());
        }

        /// <summary>
        /// Perform all validations
        /// </summary>
        /// <param name="validators">Validations</param>
        /// <returns>Validation Result</returns>
        public static CheckPointValidator All(params CheckPointValidator[] validators)
        {
            return validators.Aggregate((x, y) => x.And(y));
        }
    }
}
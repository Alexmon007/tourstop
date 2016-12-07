using System;
using System.Linq;
using Common.DTOs;

namespace Business.Handlers.Validation.Dto
{
    /// <summary>
    /// Message Data Validation
    /// </summary>
    public class MessageValidator : BaseValidator<MessageDTO>
    {
        public override Func<MessageDTO, ValidationResult> Validate { get; internal set; }

        /// <summary>
        /// Validate more than one validation
        /// </summary>
        /// <param name="other">Validator</param>
        /// <returns></returns>
        public MessageValidator And(MessageValidator other)
        {
            return new MessageValidator
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
        public static MessageValidator Holds(Predicate<MessageDTO> predicate, string message)
        {
            return new MessageValidator
            {
                Validate = x => predicate.Invoke(x) ? ValidationResult.Valid() : ValidationResult.Invalid(message)
            };
        }

        #region Validators
        /// <summary>
        /// Id Validation
        /// </summary>
        /// <returns>Result</returns>
        public static MessageValidator HasId()
        {
            return Holds(x => x.Id > 0, "Invalid Id");
        }

        /// <summary>
        /// Content Validation
        /// </summary>
        /// <returns>Result</returns>
        public static MessageValidator ContentIsNotEmpty()
        {
            return Holds(x => string.IsNullOrEmpty(x.Content), "Content is null or empty");
        }

        /// <summary>
        /// User Validation
        /// </summary>
        /// <returns>Result</returns>
        public static MessageValidator UserIsValid()
        {
            return Holds(x => x.SenderId == 0, "User is invalid");
        }

        #endregion

        /// <summary>
        /// Perform all validations
        /// </summary>
        /// <param name="validators">Validations</param>
        /// <returns>Validation Result</returns>
        public static MessageValidator All()
        {
            return All(ContentIsNotEmpty(), UserIsValid());
        }

        /// <summary>
        /// Perform all validations
        /// </summary>
        /// <returns>Validation Result</returns>
        public static MessageValidator All(params MessageValidator[] validators)
        {
            return validators.Aggregate((x, y) => x.And(y));
        }
    }
}
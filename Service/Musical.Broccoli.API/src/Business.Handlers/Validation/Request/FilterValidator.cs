using System;
using System.Linq;
using Business.Handlers.Request;

namespace Business.Handlers.Validation.Request
{
    /// <summary>
    /// Filter Data Validation
    /// </summary>
    public class FilterValidator : BaseValidator<Filter>
    {
        public override Func<Filter, ValidationResult> Validate { get; internal set; }

        /// <summary>
        /// Validate more than one validation
        /// </summary>
        /// <param name="other">Validator</param>
        /// <returns></returns>
        public FilterValidator And(FilterValidator other)
        {
            return new FilterValidator
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
        public static FilterValidator Holds(Predicate<Filter> predicate, string message)
        {
            return new FilterValidator
            {
                Validate = x => predicate.Invoke(x) ? ValidationResult.Valid() : ValidationResult.Invalid(message)
            };
        }

        #region Validators

        /// <summary>
        /// Property Name Validation
        /// </summary>
        /// <returns>Result</returns>
        public static FilterValidator PropertyNameIsNotNullOrEmpty()
        {
            return Holds(x => !string.IsNullOrEmpty(x.PropertyName), "Property Name is null or empty");
        }

        /// <summary>
        /// Value Validation
        /// </summary>
        /// <returns>Result</returns>
        public static FilterValidator ValueIsNotNull()
        {
            return Holds(x => x.Value != null, "Value is null");
        }

        /// <summary>
        /// RelationShip Validation
        /// </summary>
        /// <returns>Result</returns>
        public static FilterValidator RelationshipIsNotNullOrEmpty()
        {
            return Holds(x => !string.IsNullOrEmpty(x.Relationship), "Relationship is not valid");
        }

        #endregion

        /// <summary>
        /// Perform all validations
        /// </summary>
        /// <param name="validators">Validations</param>
        /// <returns>Validation Result</returns>
        public static FilterValidator All(params FilterValidator[] validators)
        {
            return validators.Aggregate((x, y) => x.And(y));
        }

        /// <summary>
        /// Perform all validations
        /// </summary>
        /// <returns>Validation Result</returns>
        public static FilterValidator All()
        {
            return All(PropertyNameIsNotNullOrEmpty(), RelationshipIsNotNullOrEmpty(), ValueIsNotNull());
        }
    }
}
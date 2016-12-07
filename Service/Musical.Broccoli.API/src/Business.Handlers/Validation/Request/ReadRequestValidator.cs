using System;
using System.Collections.Generic;
using System.Linq;
using Business.Handlers.Request;

namespace Business.Handlers.Validation.Request
{
    /// <summary>
    /// Read Request Validation Validation
    /// </summary>
    public class ReadRequestValidator : BaseValidator<ReadRequest>
    {
        /// <summary>
        /// Validates Data
        /// </summary>
        public override Func<ReadRequest, ValidationResult> Validate
        {
            get { return x => ValidateFilters(x.Filters); }
            internal set { }
        }

        /// <summary>
        /// Validate if filters are valid
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        private static ValidationResult ValidateFilters(IReadOnlyCollection<Filter> filters)
        {
            return filters == null || filters.Count <= 0
                ? ValidationResult.Valid()
                : filters.Select(x => FilterValidator.All().Validate(x))
                    .Aggregate((x, y) => x + y);
        }

        /// <summary>
        /// Request Validators
        /// </summary>
        /// <returns>ReadRequestValidator Instance </returns>
        public static ReadRequestValidator GetValidator()
        {
            return new ReadRequestValidator();
        }
    }
}
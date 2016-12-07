using System;

namespace Business.Handlers.Validation
{
    /// <summary>
    /// Base Validator
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseValidator<T>
    {
        /// <summary>
        /// Validates Data
        /// Not implemented in base
        /// </summary>
        public abstract Func<T, ValidationResult> Validate { get; internal set; }
    }
}
using System;
using System.Linq;
using Business.Handlers.Request;
using Common.DTOs;

namespace Business.Handlers.Validation.Request
{
    /// <summary>
    /// Read/Write Request Validation
    /// </summary>
    public class ReadWriteRequestValidator<T> : BaseValidator<ReadWriteRequest<T>> where T : BaseDTO
    {
        /// <summary>
        /// Validates Data
        /// </summary>
        public override Func<ReadWriteRequest<T>, ValidationResult> Validate { get; internal set; }
        
        /// <summary>
        /// Performs the validation
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <param name="message">Error Message</param>
        /// <returns></returns>
        public static ReadWriteRequestValidator<TDto> Holds<TDto>(Predicate<ReadWriteRequest<TDto>> predicate,
            string message) where TDto : BaseDTO
        {
            return new ReadWriteRequestValidator<TDto>
            {
                Validate = x => predicate.Invoke(x)
                    ? ValidationResult.Valid()
                    : ValidationResult.Invalid(message)
            };
        }

        /// <summary>
        /// Create an ReadWriteRequestValidator intance
        /// </summary>
        /// <typeparam name="TDto">DTO</typeparam>
        /// <param name="validator">Validators</param>
        /// <returns>ReadWriteRequestValidator Instance</returns>
        public static ReadWriteRequestValidator<TDto> Build<TDto>(BaseValidator<TDto> validator) where TDto : BaseDTO
        {
            return new ReadWriteRequestValidator<TDto>
            {
                Validate = request =>   request.Data
                    .Select(dto => validator.Validate(dto))
                    .Aggregate(ValidationResult.Valid(), (x, y) => x + y)
            };
        }
    }
}
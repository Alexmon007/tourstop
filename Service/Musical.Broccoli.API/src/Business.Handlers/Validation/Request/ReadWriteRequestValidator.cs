using System;
using System.Linq;
using Business.Handlers.Request;
using Common.DTOs;

namespace Business.Handlers.Validation.Request
{
    public class ReadWriteRequestValidator<T> : BaseValidator<ReadWriteRequest<T>> where T : BaseDTO
    {
        public override Func<ReadWriteRequest<T>, ValidationResult> Validate { get; internal set; }

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
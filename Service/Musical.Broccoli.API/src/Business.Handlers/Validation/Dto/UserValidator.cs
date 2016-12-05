using System;
using System.Linq;
using Common.DTOs;

namespace Business.Handlers.Validation.Dto
{
    public class UserValidator : BaseValidator<UserDTO>
    {
        public override Func<UserDTO, ValidationResult> Validate { get; internal set; }

        public UserValidator And(UserValidator other)
        {
            return new UserValidator
            {
                Validate = x => Validate(x) + other.Validate(x)
            };
        }

        public static UserValidator Holds(Predicate<UserDTO> predicate, string message)
        {
            return new UserValidator
            {
                Validate = x => predicate.Invoke(x) ? ValidationResult.Valid() : ValidationResult.Invalid(message)
            };
        }

        #region Validators

        public static UserValidator HasId()
        {
            return Holds(x => x.Id > 0, "Invalid Id");
        }

        public static UserValidator NameNotEmty()
        {
            return Holds(x => !string.IsNullOrEmpty(x.FirstName) && !string.IsNullOrEmpty(x.LastName),
                "Name is null or empty");
        }

        public static UserValidator EmailNotEmpty()
        {
            return Holds(x => !string.IsNullOrEmpty(x.Email), "Email is null or empty");
        }

        public static UserValidator EmailFormat()
        {
            //TODO: Use Regex
            return Holds(x => !string.IsNullOrEmpty(x.Email) && x.Email.Contains("@"), "Email invalid format");
        }

        public static UserValidator PasswordNotEmpty()
        {
            return Holds(x => !string.IsNullOrEmpty(x.Password), "Password is Empty");
        }

        public static UserValidator PhoneNotEmty()
        {
            return Holds(x => !string.IsNullOrEmpty(x.Phone), "Phone is null or empty");
        }

        #endregion

        public static UserValidator All()
        {
            return All(NameNotEmty(), EmailNotEmpty(), EmailFormat(), PasswordNotEmpty(), PhoneNotEmty());
        }

        public static UserValidator All(params UserValidator[] validators)
        {
            return validators.Aggregate((x, y) => x.And(y));
        }
    }
}
namespace Business.Handlers.Validation
{
    public class ValidationResult
    {
        public string Reason { get; set; }
        public bool IsValid { get; private set; }

        public static ValidationResult Valid()
        {
            return new ValidationResult
            {
                IsValid = true
            };
        }

        public static ValidationResult Invalid(string reason)
        {
            return new ValidationResult
            {
                IsValid = false,
                Reason = reason
            };
        }

        public static ValidationResult operator +(ValidationResult r1, ValidationResult r2)
        {
            return new ValidationResult
            {
                IsValid = r1.IsValid && r2.IsValid,
                Reason = r1.IsValid && r2.IsValid ? string.Empty : string.Format(@"{0}, {1}", r1.Reason, r2.Reason)
            };
        }
    }
}
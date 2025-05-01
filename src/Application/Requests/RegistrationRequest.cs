using FluentValidation;

namespace Application.Requests
{
    public class RegistrationRequest
    {
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
    public class RegistrationRequestValidator : AbstractValidator<RegistrationRequest>
    {
        public RegistrationRequestValidator()
        {
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(ValidationConstants.MaxUserNameLength);
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(ValidationConstants.MaxUserNameLength);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(ValidationConstants.MaxEmailLength);
            RuleFor(x => x.Password)
                    .NotEmpty()
                    .MinimumLength(ValidationConstants.MinPasswordLength)
                    .MaximumLength(ValidationConstants.MaxPasswordLength)
                    .Must(PasswordValidationCheck.PasswordContainsDigit)
                    .Must(PasswordValidationCheck.PasswordContainsUpperAndLower)
                    .Must(PasswordValidationCheck.PasswordNotContainsSpaces);
        }
    }
}

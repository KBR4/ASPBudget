using FluentValidation;

namespace Application.Requests
{
    public class CreateUserRequest
    {
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Email { get; set; }
    }

    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(ValidationConstants.MaxUserNameLength);
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(ValidationConstants.MaxUserNameLength);
            RuleFor(x => x.Email).MaximumLength(ValidationConstants.MaxEmailLength);
        }
    }
}

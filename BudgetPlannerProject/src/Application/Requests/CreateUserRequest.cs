using FluentValidation;

namespace Application.Requests
{
    public class CreateUserRequest
    {
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Email { get; set; }
    }

    public class CreateQuizRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateQuizRequestValidator()
        {
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(40).WithMessage("{PropertyName} has max length of 40.");
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(40).WithMessage("{PropertyName} has max length of 40.");
            RuleFor(x => x.Email).MaximumLength(100).WithMessage("{PropertyName} has max length of 100.");
        }
    }
}

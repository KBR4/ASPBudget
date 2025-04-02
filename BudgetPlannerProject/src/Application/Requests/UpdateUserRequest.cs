using FluentValidation;

namespace Application.Requests
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Email { get; set; }
    }
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0).WithMessage("Id must be greater than 0.")
                .LessThan(int.MaxValue).WithMessage("Id is too big.");
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(40).WithMessage("{PropertyName} has max length of 40.");
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(40).WithMessage("{PropertyName} has max length of 40.");
            RuleFor(x => x.Email).MaximumLength(100).WithMessage("{PropertyName} has max length of 100.");
        }
    }
}

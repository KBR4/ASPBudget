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
            RuleFor(x => x.Id).NotEmpty().ExclusiveBetween(0, int.MaxValue);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(ValidationConstants.MaxUserNameLength);
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(ValidationConstants.MaxUserNameLength);
            RuleFor(x => x.Email).MaximumLength(ValidationConstants.MaxEmailLength);
        }
    }
}

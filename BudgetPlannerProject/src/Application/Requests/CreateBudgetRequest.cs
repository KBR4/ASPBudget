using FluentValidation;

namespace Application.Requests
{ 
    public class CreateBudgetRequest
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public string Description { get; set; }
        public int CreatorId { get; set; }
    }
    
    public class CreateBudgetRequestValidator : AbstractValidator<CreateBudgetRequest>
    {
        public CreateBudgetRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100).WithMessage("{PropertyName} has max length of 100.");
            RuleFor(x => x.StartDate).GreaterThanOrEqualTo(new DateTime(1970, 1, 1));
            RuleFor(x => x.FinishDate).LessThanOrEqualTo(new DateTime(2038, 1, 1));
            RuleFor(x => x.Description).MaximumLength(255).WithMessage("{PropertyName} has max length of 255.");
            RuleFor(x => x.CreatorId).NotEmpty().GreaterThan(0).WithMessage("CreatorId must be greater than 0.")
                .LessThan(int.MaxValue).WithMessage("CreatorId is too big.");
        }
    }
}

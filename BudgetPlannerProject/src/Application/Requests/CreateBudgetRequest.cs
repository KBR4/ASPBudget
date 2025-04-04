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
            RuleFor(x => x.Name).NotEmpty().MaximumLength(ValidationConstants.MaxBudgetNameLength);
            RuleFor(x => x.StartDate).GreaterThanOrEqualTo(new DateTime(1970, 1, 1));
            RuleFor(x => x.FinishDate).GreaterThanOrEqualTo(new DateTime(1970, 1, 1));
            RuleFor(x => x.Description).MaximumLength(ValidationConstants.MaxDescriptionLength);
            RuleFor(x => x.CreatorId).NotEmpty().GreaterThan(0).WithMessage("CreatorId must be greater than 0.")
                .LessThan(int.MaxValue);
        }
    }
}

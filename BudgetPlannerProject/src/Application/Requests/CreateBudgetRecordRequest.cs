using FluentValidation;

namespace Application.Requests
{
    public class CreateBudgetRecordRequest
    {
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime SpendingDate { get; set; }
        public int BudgetId { get; set; }
        public double Total { get; set; }
        public string Comment { get; set; }
    }
    
    public class CreateBudgetRecordRequestValidator : AbstractValidator<CreateBudgetRecordRequest>
    {
        public CreateBudgetRecordRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100).WithMessage("{PropertyName} has max length of 100.");
            RuleFor(x => x.CreationDate).GreaterThanOrEqualTo(new DateTime(1970, 1, 1));
            RuleFor(x => x.SpendingDate).LessThanOrEqualTo(new DateTime(2038, 1, 1));
            RuleFor(x => x.BudgetId).NotEmpty().GreaterThan(0).WithMessage("BudgetId must be greater than 0.")
                .LessThan(int.MaxValue).WithMessage("BudgetId is too big.");
            RuleFor(x => x.Comment).MaximumLength(511).WithMessage("{PropertyName} has max length of 511.");
        }
    }
}

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
            RuleFor(x => x.Name).NotEmpty().MaximumLength(ValidationConstants.MaxBudgetNameLength);
            RuleFor(x => x.CreationDate).GreaterThanOrEqualTo(new DateTime(1970, 1, 1));
            RuleFor(x => x.SpendingDate).GreaterThanOrEqualTo(new DateTime(1970, 1, 1));
            RuleFor(x => x.BudgetId).NotEmpty().ExclusiveBetween(0, int.MaxValue);
            RuleFor(x => x.Total).ExclusiveBetween(double.MinValue, double.MaxValue);
            RuleFor(x => x.Comment).MaximumLength(ValidationConstants.MaxCommentLength);
        }
    }
}

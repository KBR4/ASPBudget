using FluentValidation;

namespace Application.Requests
{
    public class CreateBudgetResultRequest
    {
        public int BudgetId { get; set; }
        public double TotalProfit { get; set; }
    }
    
    public class CreateBudgetResultRequestValidator : AbstractValidator<CreateBudgetResultRequest>
    {
        public CreateBudgetResultRequestValidator()
        {
            RuleFor(x => x.BudgetId).NotEmpty().GreaterThan(0).WithMessage("BudgetId must be greater than 0.")
                .LessThan(int.MaxValue).WithMessage("BudgetId is too big.");
            RuleFor(x => x.TotalProfit).GreaterThan(double.MinValue)
                .LessThan(double.MaxValue);
        }
    }
}

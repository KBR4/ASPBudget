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
            RuleFor(x => x.BudgetId).NotEmpty().ExclusiveBetween(0, int.MaxValue);
            RuleFor(x => x.TotalProfit).ExclusiveBetween(double.MinValue, double.MaxValue);
        }
    }
}

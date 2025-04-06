using FluentValidation;

namespace Application.Requests
{
    public class UpdateBudgetResultRequest
    {
        public int Id { get; set; }
        public int BudgetId { get; set; }
        public double TotalProfit { get; set; }
    }

    public class UpdateBudgetResultRequestValidator : AbstractValidator<UpdateBudgetResultRequest>
    {
        public UpdateBudgetResultRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().ExclusiveBetween(0, int.MaxValue);
            RuleFor(x => x.BudgetId).NotEmpty().ExclusiveBetween(0, int.MaxValue);
            RuleFor(x => x.TotalProfit).ExclusiveBetween(double.MinValue, double.MaxValue);
        }
    }
}

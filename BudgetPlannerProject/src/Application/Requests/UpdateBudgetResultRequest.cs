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
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0).WithMessage("Id must be greater than 0.")
                .LessThan(int.MaxValue).WithMessage("Id is too big.");
            RuleFor(x => x.BudgetId).NotEmpty().GreaterThan(0).WithMessage("BudgetId must be greater than 0.")
                .LessThan(int.MaxValue).WithMessage("BudgetId is too big.");
            RuleFor(x => x.TotalProfit).GreaterThan(double.MinValue)
                .LessThan(double.MaxValue);
        }
    }
}

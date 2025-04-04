﻿using FluentValidation;

namespace Application.Requests
{
    public class UpdateBudgetRecordRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime SpendingDate { get; set; }
        public int BudgetId { get; set; }
        public double Total { get; set; }
        public string Comment { get; set; }
    }
    
    public class UpdateBudgetRecordRequestValidator : AbstractValidator<UpdateBudgetRecordRequest>
    {
        public UpdateBudgetRecordRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0).WithMessage("Id must be greater than 0.")
                .LessThan(int.MaxValue).WithMessage("Id is too big.");
            RuleFor(x => x.Name).NotEmpty().MaximumLength(ValidationConstants.MaxBudgetNameLength);
            RuleFor(x => x.CreationDate).GreaterThanOrEqualTo(new DateTime(1970, 1, 1));
            RuleFor(x => x.SpendingDate).GreaterThanOrEqualTo(new DateTime(1970, 1, 1));
            RuleFor(x => x.BudgetId).NotEmpty().GreaterThan(0).WithMessage("BudgetId must be greater than 0.")
                .LessThan(int.MaxValue);
            RuleFor(x => x.Total).GreaterThan(double.MinValue)
                .LessThan(double.MaxValue);
            RuleFor(x => x.Comment).MaximumLength(ValidationConstants.MaxCommentLength);
        }
    }
}

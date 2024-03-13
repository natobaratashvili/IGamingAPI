using FluentValidation;
using IGaming.Core.Bets.RequestModels;



namespace IGaming.Core.Bets.Validators
{
    public class PlaceBetRequestValidator: AbstractValidator<PlaceBetRequest>
    {
        public PlaceBetRequestValidator()
        {

            RuleFor(request => request.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0");

            RuleFor(request => request.Details)
                .MaximumLength(100).WithMessage("Details cannot exceed 100 characters");
        }
    }
}

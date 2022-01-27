using FluentValidation;

namespace OrkadWeb.Logic.Shares.Queries.GetMonthlyTotalExpenses
{
    public class GetMonthlyTotalExpensesValidator : AbstractValidator<GetMonthlyTotalExpensesQuery>
    {
        public GetMonthlyTotalExpensesValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(query => query.Month).NotEmpty().WithMessage("Le mois est requis");
        }
    }
}

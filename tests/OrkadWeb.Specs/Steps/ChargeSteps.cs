using OrkadWeb.Application.Charges.Commands;

namespace OrkadWeb.Specs.Steps
{
    [Binding]
    public class ChargeSteps
    {
        private readonly ISender sender;

        public ChargeSteps(ISender sender)
        {
            this.sender = sender;
        }

        [When(@"j'ajoute une charge ""(.*)"" d'un montant de (.*)€")]
        public async Task WhenJajouteUneChargeDunMontantDe(string name, decimal amount)
        {
            await sender.Send(new AddChargeCommand
            {
                Name = name,
                Amount = amount,
            });
        }


    }
}

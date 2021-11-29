using NFluent;
using System;
using TechTalk.SpecFlow;

namespace OrkadWeb.Tests.Steps
{
    [Binding]
    public class CalculatriceStepDefinitions
    {
        private int? firstNumber, secondNumber, result;

        [Given(@"le premier nombre est (.*)")]
        public void GivenLePremierNombreEst(int number)
        {
            firstNumber = number;
        }

        [Given(@"le deuxième nombre est (.*)")]
        public void GivenLeDeuxiemeNombreEst(int number)
        {
            secondNumber = number;
        }

        [When(@"les deux nombre sont ajoutés")]
        public void WhenLesDeuxNombreSontAjoutes()
        {
            result = firstNumber + secondNumber;
        }

        [Then(@"le résultat devrait être (.*)")]
        public void ThenLeResultatDevraitEtre(int expectedResult)
        {
            Check.That(result).IsEqualTo(expectedResult);
        }
    }
}

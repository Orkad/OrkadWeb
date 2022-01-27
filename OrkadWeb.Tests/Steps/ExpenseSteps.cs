using System;
using TechTalk.SpecFlow;

namespace OrkadWeb.Tests.Steps
{
    public class ExpenseSteps
    {
        [When(@"j'ajoute la dépense de (.*)€ à la date du (.*) que j'appelle (.*)")]
        public void WhenJAjouteLaDepenseDeALaDateDuQueJAppelle(decimal amount, DateTime date, string name)
        {
            
        }

    }
}

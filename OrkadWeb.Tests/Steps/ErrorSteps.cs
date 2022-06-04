using NFluent;
using OrkadWeb.Tests.Drivers;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace OrkadWeb.Tests.Steps
{
    [Binding]
    public class ErrorSteps
    {
        private readonly ExecutionDriver executionDriver;

        public ErrorSteps(ExecutionDriver executionDriver)
        {
            this.executionDriver = executionDriver;
        }

        [Then(@"je n'obtiens pas d'erreurs")]
        [Then(@"il n'y a pas d'erreurs")]
        public void ThenThereIsNoErrors()
        {
            Check.WithCustomMessage("Une erreur a été déclenchée alors qu'il ne devrait pas y avoir d'erreurs.")
                .That(executionDriver.Exception)
                .IsNull();
        }

        [Then(@"j'obtiens l'erreur avec le message suivant : (.*)")]
        [Then(@"il y a une erreur avec le message suivant : (.*)")]
        public void ThenThereIsErrorWithMessage(string expectedMessage)
        {
            Check.WithCustomMessage("Aucune erreur n'a été déclenchée alors qu'il devrait y en avoir une")
                .That(executionDriver.Exception).IsNotNull();

            Check.WithCustomMessage($"Le message d'erreur '{executionDriver.Exception.Message}' ne correspond pas avec le message attendu '{expectedMessage}'")
                .That(executionDriver.Exception.Message).IsEqualTo(expectedMessage);
        }
    }
}

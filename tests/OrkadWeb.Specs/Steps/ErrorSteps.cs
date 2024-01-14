using FluentValidation;
using FluentValidation.Results;
using NFluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OrkadWeb.Specs.Contexts;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace OrkadWeb.Specs.Steps
{
    [Binding]
    public class ErrorSteps
    {
        private readonly ExceptionContext executionDriver;

        public ErrorSteps(ExceptionContext executionDriver)
        {
            this.executionDriver = executionDriver;
        }

        [Then(@"je n'obtiens pas d'erreurs")]
        [Then(@"il n'y a pas d'erreurs")]
        public void ThenThereIsNoErrors()
        {
            var exception = executionDriver.HandleException();
            Check.WithCustomMessage("Une erreur a été déclenchée alors qu'il ne devrait pas y avoir d'erreurs.")
                .That(exception)
                .IsNull();
        }

        [Then(@"il y a une erreur")]
        public void ThenThereIsErrors()
        {
            var exception = executionDriver.HandleException();
            Check.WithCustomMessage("Aucune erreur n'a été déclenchée alors qu'il devrait y en avoir une")
                .That(exception).IsNotNull();
        }

        [Then(@"j'obtiens l'erreur avec le message suivant : (.*)")]
        [Then(@"il y a une erreur avec le message suivant : (.*)")]
        public void ThenThereIsErrorWithMessage(string expectedMessage)
        {
            var exception = executionDriver.HandleException();
            Check.WithCustomMessage("Aucune erreur n'a été déclenchée alors qu'il devrait y en avoir une")
                .That(exception).IsNotNull();
            if (exception is ValidationException validationException)
            {
                Check.That(validationException.Errors.Select(e => e.ErrorMessage)).Contains(expectedMessage);
            }
            else
            {
                Check.WithCustomMessage($"Le message d'erreur '{exception?.Message}' ne correspond pas avec le message attendu '{expectedMessage}'")
                    .That(exception?.Message).IsEqualTo(expectedMessage);
            }
        }

        [AfterScenario]
        public void CheckForUnexpectedExceptionsAfterEachScenario()
        {
            var exception = executionDriver.HandleException();
            Check.WithCustomMessage($"Une erreur inatendue s'est produite durant le scénario : {exception}")
                .That(exception).IsNull();
        }
    }
}

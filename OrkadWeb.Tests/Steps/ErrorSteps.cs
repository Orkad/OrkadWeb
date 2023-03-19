using FluentValidation;
using FluentValidation.Results;
using NFluent;
using OrkadWeb.Tests.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace OrkadWeb.Tests.Steps
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

            Check.WithCustomMessage($"Le message d'erreur '{exception?.Message}' ne correspond pas avec le message attendu '{expectedMessage}'")
                .That(exception?.Message).IsEqualTo(expectedMessage);
        }

        [Then(@"il y a les erreurs de validation suivantes")]
        public void ThenIlYalesErreursDeValidationSuivantes(Table table)
        {
            var exception = executionDriver.HandleException();
            Check.WithCustomMessage("Aucune erreur n'a été déclenchée alors qu'il devrait y en avoir une")
                .That(exception).IsNotNull();
            Check.WithCustomMessage("Il n'a pas d'erreur de validation")
                .That(exception).IsInstanceOf<ValidationException>();
            var validationException = (ValidationException)exception;
            Check.WithCustomMessage("Il n'y a pas le même nombre d'erreurs de validation attendues")
                .That(validationException.Errors).HasSize(table.RowCount);
            var expectedFalidationFailure = table.Rows.Select(r => r[0]);
            Check.That(validationException.Errors.Select(e => e.ErrorMessage)).Contains(expectedFalidationFailure);
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

using FluentValidation;
using FluentValidation.Results;
using NFluent;
using OrkadWeb.Tests.Drivers;
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

        [Then(@"il y a une erreur")]
        public void ThenThereIsErrors()
        {
            Check.WithCustomMessage("Aucune erreur n'a été déclenchée alors qu'il devrait y en avoir une")
                .That(executionDriver.Exception).IsNotNull();
        }

        [Then(@"j'obtiens l'erreur avec le message suivant : (.*)")]
        [Then(@"il y a une erreur avec le message suivant : (.*)")]
        public void ThenThereIsErrorWithMessage(string expectedMessage)
        {
            Check.WithCustomMessage("Aucune erreur n'a été déclenchée alors qu'il devrait y en avoir une")
                .That(executionDriver.Exception).IsNotNull();

            Check.WithCustomMessage($"Le message d'erreur '{executionDriver.Exception?.Message}' ne correspond pas avec le message attendu '{expectedMessage}'")
                .That(executionDriver.Exception?.Message).IsEqualTo(expectedMessage);
        }

        [Then(@"il y a les erreurs de validation suivantes")]
        public void ThenIlYalesErreursDeValidationSuivantes(Table table)
        {
            Check.WithCustomMessage("Aucune erreur n'a été déclenchée alors qu'il devrait y en avoir une")
                .That(executionDriver.Exception).IsNotNull();
            Check.WithCustomMessage("Il n'a pas d'erreur de validation")
                .That(executionDriver.Exception).IsInstanceOf<ValidationException>();
            var validationException = (ValidationException)executionDriver.Exception;
            Check.WithCustomMessage("Il n'y a pas le même nombre d'erreurs de validation attendues")
                .That(validationException.Errors).HasSize(table.RowCount);
            var expectedFalidationFailure = table.Rows.Select(r => r[0]);
            Check.That(validationException.Errors.Select(e => e.ErrorMessage)).Contains(expectedFalidationFailure);
        }
    }
}

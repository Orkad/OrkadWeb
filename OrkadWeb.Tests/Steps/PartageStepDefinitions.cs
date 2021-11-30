using OrkadWeb.Data;
using OrkadWeb.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace OrkadWeb.Tests.Steps
{
    /// <summary>
    /// Contexte des objets nommés
    /// </summary>
    public class NamedContext
    {
        public Dictionary<string, User> Users { get; } = new Dictionary<string, User>();
    }

    public class LastContext
    {

    }

    [Binding]
    public class PartageStepDefinitions
    {
        private readonly NamedContext namedContext;

        public PartageStepDefinitions(IDataService dataService, NamedContext namedContext)
        {
            this.namedContext = namedContext;
        }

        [Given(@"il existe les utilisateurs suivants :")]
        public void GivenExistsUsers(Table table)
        {
            throw new PendingStepException();
        }

        [Given(@"il existe un partage entre (.*) et (.*)")]
        public void GivenExistsShare(string user1, string user2)
        {
            throw new PendingStepException();
        }

        [When(@"(.*) ajoute la dépense de (.*)€ nommée ""([^""]*)"" au partage")]
        public void WhenUserAddExpense(string user, decimal amount, string name)
        {
            throw new PendingStepException();
        }

        [Then(@"le partage affiche que les dépenses totales s'élèvent à hauteur de (.*)€")]
        public void ThenShareTotalAmountIs(decimal amount)
        {
            throw new PendingStepException();
        }

        [Then(@"le partage affiche qu'il y a un écart de (.*)€")]
        public void ThenShareGapIs(decimal amout)
        {
            throw new PendingStepException();
        }

        [Then(@"(.*) doit (.*)€ à (.*) sur le partage")]
        public void ThenUserOweToOtherOnShare(string user1, decimal amount, string user2)
        {
            throw new PendingStepException();
        }

    }
}

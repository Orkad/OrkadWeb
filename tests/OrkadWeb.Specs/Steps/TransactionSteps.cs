﻿using NHibernate.Linq;
using OrkadWeb.Application.Expenses.Commands;
using System;
using System.Collections.Generic;
using OrkadWeb.Application.Transactions.Commands;
using OrkadWeb.Application.Transactions.Models;
using OrkadWeb.Application.Transactions.Queries;
using TechTalk.SpecFlow;
using OrkadWeb.Application.Users;
using OrkadWeb.Angular.Controllers;
using System.Threading;
using System.Net.Http;

namespace OrkadWeb.Specs.Steps
{
    [Binding]
    public class TransactionSteps
    {
        private readonly ISender sender;
        private readonly IDataService dataService;
        private readonly TransactionController transactionController;
        private List<TransactionVM> transactions;

        public TransactionSteps(ISender sender, IDataService dataService, TransactionController transactionController)
        {
            this.sender = sender;
            this.dataService = dataService;
            this.transactionController = transactionController;
        }

        [When(@"j'ajoute la dépense de (.*)€ à la date du (.*) que j'appelle (.*)")]
        public async Task WhenJAjouteLaDepenseDeALaDateDuQueJAppelle(decimal amount, DateTime date, string name)
        {
            await sender.Send(new AddTransactionExpenseCommand
            {
                Amount = amount,
                Date = date,
                Name = name,
            });
        }

        [Given(@"les transactions suivantes")]
        public void GivenLesDepensesSuivantes(Table table)
        {
            table.CreateSet<Transaction>(row =>
            {
                var proprietaire = row.GetString("propriétaire");
                var tx = new Transaction
                {
                    Amount = row.GetDecimal("montant"),
                    Date = row.GetDateTime("date"),
                    Name = row.GetString("nom"),
                };
                if (!string.IsNullOrEmpty(proprietaire))
                {
                    tx.Owner = dataService.Query<User>()
                        .Single(u => u.Username == proprietaire);
                }
                dataService.Insert(tx);
                return tx;
            });
        }

        [When(@"j'affiche la liste de mes dépenses sur le mois de (.*)")]
        public async Task WhenJafficheLaListeDeMesDepensesSurLeMoisDeJuillet(DateTime month)
        {
            transactions = await transactionController.GetMonthly(month, CancellationToken.None);
        }

        [Then(@"mes transactions sont les suivantes")]
        public void ThenMesTransactionsSontLesSuivantes(Table table)
        {
            var expecteds = table.CreateSet<Transaction>(row => new Transaction
            {
                Amount = row.GetDecimal("montant"),
                Date = row.GetDateTime("date"),
                Name = row.GetString("nom"),
            }).ToList();
            var displayedRows = transactions;
            Check.That(displayedRows).HasSize(expecteds.Count);
            foreach (var expected in expecteds)
            {
                var actual = displayedRows.Single(r => r.Name == expected.Name);
                Check.That(actual.Amount).IsEqualTo(expected.Amount);
                Check.That(actual.Date).IsEqualTo(expected.Date);
            }
        }

        [Then(@"il n'y aucune dépense affichée")]
        public void ThenIlNyAucuneDepenseAffichee()
        {
            Check.That(transactions).IsEmpty();
        }

        [When(@"je modifie la dépense ""(.*)"" par")]
        public void WhenJeModifieLaDepensePar(string name, Table table)
        {
            var dto = table.CreateSet<Transaction>(row => new Transaction
            {
                Amount = row.GetDecimal("montant"),
                Date = row.GetDateTime("date"),
                Name = row.GetString("nom"),
            }).Single();
            var id = dataService.Query<Transaction>().Single(t => t.Name == name).Id;
            sender.Send(new UpdateTransactionExpenseCommand
            {
                Id = id,
                Amount = dto.Amount,
                Date = dto.Date,
                Name = dto.Name,
            });
        }

        [When(@"je supprime la dépense ""(.*)""")]
        public void WhenJeSupprimeLaDepense(string name)
        {
            var id = dataService.Query<Transaction>().Single(t => t.Name == name).Id;
            sender.Send(new DeleteTransactionCommand()
            {
                Id = id,
            });
        }
    }
}

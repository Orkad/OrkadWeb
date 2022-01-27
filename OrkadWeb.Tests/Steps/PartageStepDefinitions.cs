using OrkadWeb.Data;
using OrkadWeb.Data.Models;
using OrkadWeb.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace OrkadWeb.Tests.Steps
{
    /// <summary>
    /// Contexte des objets nommés
    /// </summary>
    [Binding]
    public class NamedContext
    {
        public Dictionary<string, User> Users { get; } = new Dictionary<string, User>();
        public Dictionary<string, Share> Shares { get; } = new Dictionary<string, Share>();
    }

    [Binding]
    public class LastContext
    {
        public Share? LastShare { get; set; }
    }

    [Binding]
    public class PartageStepDefinitions
    {
        private readonly IDataService dataService;
        private readonly NamedContext namedContext;
        private readonly LastContext lastContext;

        public PartageStepDefinitions(IDataService dataService, 
            NamedContext namedContext,
            LastContext lastContext)
        {
            this.dataService = dataService;
            this.namedContext = namedContext;
            this.lastContext = lastContext;
        }

        [Given(@"il existe les utilisateurs suivants :")]
        public void GivenExistsUsers(Table table)
        {
            foreach (var row in table.Rows)
            {
                var username = row["Nom d'utilisateur"];
                var user = new User
                {
                    Username = username,
                    Password = "empty",
                    Email = $"{username}@gmail.com",
                };
                dataService.Insert(user);
                namedContext.Users.Add(username, user);
            }
        }

        [Given(@"il existe un partage entre (.*) et (.*)")]
        public void GivenExistsShare(string user1, string user2)
        {
            var share = new Share
            {
                Name = "Any",
                Owner = namedContext.Users[user1], // le premier utilisateur est le propriétaire
                Rule = ShareRule.Free,
                UserShares = new HashSet<UserShare> {
                    new UserShare
                    {
                        User = namedContext.Users[user1],
                    },
                    new UserShare
                    {
                        User = namedContext.Users[user2],
                    },
                },
            };
            foreach (var userShare in share.UserShares)
            {
                userShare.Share = share;
            }
            dataService.Insert(share);
            lastContext.LastShare = share;

        }


        [When(@"(.*) ajoute la dépense de (.*)€ nommée ""([^""]*)"" au partage")]
        public void WhenUserAddExpense(string user, decimal amount, string name)
        {
           
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

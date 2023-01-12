﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using OrkadWeb.Data;
using OrkadWeb.Data.Models;
using OrkadWeb.Logic.Users;
using OrkadWeb.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace OrkadWeb.Tests.Steps
{
    [Binding]
    public class UserSteps
    {
        private readonly IDataService service;

        public IAuthenticatedUser? AuthenticatedUser { get; private set; }

        public UserSteps(IDataService service)
        {
            this.service = service;
        }

        [Given(@"l'utilisateur (.*) existe")]
        public void GivenLutilisateurExiste(string name)
        {
            service.Insert(new User
            {
                Email = "test@test.test",
                Username = name,
                Password = "none",
            });
        }

        [Given(@"je suis connecté en tant que (.*)")]
        public void GivenJeSuisConnecteEnTantQue(string name)
        {
            var user = service.Query<User>().Where(u => u.Username == name).Single();
            AuthenticatedUser = new TestUser(user.Id, user.Username, user.Email);
        }

        [When(@"je tente de m'enregistrer avec les informations suivantes")]
        public void WhenJeTenteDeMenregistrerAvecLesInformationsSuivantes(Table table)
        {

        }

    }
}

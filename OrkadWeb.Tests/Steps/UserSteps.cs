using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using OrkadWeb.Domain;
using OrkadWeb.Domain.Entities;
using OrkadWeb.Application.Users;
using OrkadWeb.Application.Users.Commands;
using OrkadWeb.Tests.Drivers;
using OrkadWeb.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using OrkadWeb.Application.Common.Interfaces;

namespace OrkadWeb.Tests.Steps
{
    [Binding]
    public class UserSteps
    {
        private readonly IDataService service;
        private readonly ExecutionDriver executionDriver;

        public IAuthenticatedUser? AuthenticatedUser { get; private set; }

        public UserSteps(IDataService service, ExecutionDriver executionDriver)
        {
            this.service = service;
            this.executionDriver = executionDriver;
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

        [When(@"je tente de m'inscrire avec les informations suivantes")]
        public async Task WhenJeTenteDeMenregistrerAvecLesInformationsSuivantes(Table table)
        {
            var command = table.CreateInstance<RegisterCommand>();
            await executionDriver.Send(command);
        }

        [When(@"je tente de m'inscrire avec le nom d'utilisateur (.*)")]
        public async Task WhenJeTenteDeMenregistrerAvecLeNomDutilisateur(string name)
        {
            await executionDriver.Send(new RegisterCommand
            {
                UserName = name,
                Password = "Default@123",
                Email = "test@test.fr",
            });
        }

        [When(@"je tente de m'inscrire avec le mot de passe (.*)")]
        public async Task WhenJeTenteDeMenregistrerAvecLeMotDePasse(string password)
        {
            await executionDriver.Send(new RegisterCommand
            {
                UserName = "Orkad",
                Password = password,
                Email = "test@test.fr",
            });
        }

        [Then(@"l'utilisateur (.*) existe")]
        public void ThenLutilisateurExiste(string name)
        {
            Check.That(service.Exists<User>(u => u.Username == name)).IsTrue();
        }
    }
}

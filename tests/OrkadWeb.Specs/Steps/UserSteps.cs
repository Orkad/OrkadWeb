﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using OrkadWeb.Domain;
using OrkadWeb.Domain.Entities;
using OrkadWeb.Application.Users;
using OrkadWeb.Application.Users.Commands;
using OrkadWeb.Specs.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using OrkadWeb.Application.Common.Interfaces;
using OrkadWeb.Application.Config;
using System.Text.RegularExpressions;
using System.Web;
using OrkadWeb.Domain.Consts;
using MediatR;
using OrkadWeb.Angular.Controllers;
using OrkadWeb.Application.Security;
using OrkadWeb.Specs.Contexts;
using OrkadWeb.Specs.Models;

namespace OrkadWeb.Specs.Steps
{
    [Binding]
    public class UserSteps
    {
        private static readonly Regex URL_REGEX = new(@"(http|ftp|https):\/\/([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:\/~+#-]*[\w@?^=%&\/~+#-])");
        private readonly IDataService service;
        private readonly LastContext lastContext;
        private readonly IEmailService emailService;
        private readonly ISender sender;
        private readonly ITimeProvider timeProvider;
        private readonly UserContext userContext;

        public UserSteps(IDataService service, LastContext lastContext, IEmailService emailService,
            ISender sender, ITimeProvider timeProvider, UserContext userContext)
        {
            this.service = service;
            this.lastContext = lastContext;
            this.emailService = emailService;
            this.sender = sender;
            this.timeProvider = timeProvider;
            this.userContext = userContext;
        }

        [Given(@"l'utilisateur (.*) existe")]
        public void GivenLutilisateurExiste(string name)
        {
            var user = new User
            {
                Email = "test@test.test",
                Username = name,
                Password = "none",
                Role = UserRoles.User,
            };
            service.Insert(user);
            lastContext.Mention(user);
        }

        [Given(@"l'utilisateur a été supprimé")]
        public void GivenDeletedUser()
        {
            var user = lastContext.Last<User>();
            service.Delete(user);
        }

        [Given(@"son adresse email est (.*)")]
        public void GivenSonAdresseEmailEst(string email)
        {
            var user = lastContext.Last<User>();
            user.Email = email;
        }

        [Given(@"je suis connecté en tant que (.*)")]
        public void GivenJeSuisConnecteEnTantQue(string name)
        {
            var user = service.Find<User>(u => u.Username == name);
            lastContext.Mention(user);
            userContext.SetAuthenticatedUser(user);
        }

        [Given(@"l'email de confirmation a déjà été envoyé")]
        public void GivenLemailDeConfirmationADejaEteEnvoye()
        {
            var user = lastContext.Last<User>();
            var hash = Hash.Create(user.Email);
            var html = @$"Hello {user.Username},

You just register using this email adress.
Please follow the link to validate your email : 
<a href=""http://orkad.fr/auth/confirm?email={user.Email}&hash={hash}"">confirm your email</a>";
            emailService.Send(user.Email, "Confirm your email adress", html);
        }

        [Given(@"l'utilisateur (.*) existe avec le mot de passe (.*)")]
        public void GivenLutilisateurTestExisteAvecLeMotDePasse(string username, string password)
        {
            var user = new User
            {
                Email = "test@test.test",
                Username = username,
                Password = Hash.Create(password),
                Role = UserRoles.User,
            };
            service.Insert(user);
            lastContext.Mention(user);
        }

        [Given(@"son adresse email (.*) est déjà confirmée")]
        public void GivenEmailAlreadyConfirmed(string email)
        {
            var user = lastContext.Last<User>();
            user.Email = email;
            user.Confirmation = timeProvider.Now;
        }


        [When(@"je tente de m'inscrire avec les informations suivantes")]
        public async Task WhenJeTenteDeMenregistrerAvecLesInformationsSuivantes(Table table)
        {
            var command = table.CreateInstance<RegisterCommand>();
            await sender.Send(command);
        }

        [When(@"je tente de m'inscrire avec le nom d'utilisateur (.*)")]
        public async Task WhenJeTenteDeMenregistrerAvecLeNomDutilisateur(string name)
        {
            await sender.Send(new RegisterCommand
            {
                UserName = name,
                Password = "Default@123",
                Email = "test@test.fr",
            });
        }

        [When(@"je tente de m'inscrire avec le mot de passe (.*)")]
        public async Task WhenJeTenteDeMenregistrerAvecLeMotDePasse(string password)
        {
            await sender.Send(new RegisterCommand
            {
                UserName = "Orkad",
                Password = password,
                Email = "test@test.fr",
            });
        }

        [When(@"j'utilise le lien de confirmation dans l'email envoyé")]
        public async Task WhenJutiliseLeLienDeConfirmationDansLeemailEnvoye()
        {
            var html = lastContext.Last<SendedEmail>().Html;
            var uri = new Uri(URL_REGEX.Match(html).Value);
            var parameters = HttpUtility.ParseQueryString(uri.Query);
            var email = parameters["email"];
            var hash = parameters["hash"];
            await sender.Send(new EmailConfirmCommand
            {
                Email = email,
                Hash = hash,
            });
        }

        [Then(@"l'utilisateur (.*) existe")]
        public void ThenLutilisateurExiste(string name)
        {
            Check.That(service.Exists<User>(u => u.Username == name)).IsTrue();
        }

        [Then(@"mon email est validé")]
        public void ThenMonEmailEstValide()
        {
            var user = service.Get<User>(lastContext.Last<User>().Id);
            Check.That(user.Confirmation).IsNotNull();
        }

    }
}

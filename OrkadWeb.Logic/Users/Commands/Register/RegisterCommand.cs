using FluentValidation.Validators;
using MediatR;
using OrkadWeb.Logic.Common;
using System.Collections.Generic;
using System.Text;

namespace OrkadWeb.Logic.Users.Commands.Register
{
    public class RegisterCommand : IRequest
    {
        /// <summary>
        /// (required) username 5 to 32 characters
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// (required) valid email adress
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// (required) password with at least 8 characters, one lower, one upper, and one special character
        /// </summary>
        public string Password { get; set; }

        

        
    }
}

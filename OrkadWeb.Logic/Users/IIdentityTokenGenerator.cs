using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace OrkadWeb.Logic.Users
{
    public interface IIdentityTokenGenerator
    {
        /// <summary>
        /// Generate a identity token
        /// </summary>
        string GenerateToken(IEnumerable<Claim> claims);
    }
}

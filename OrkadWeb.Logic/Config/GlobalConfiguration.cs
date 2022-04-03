using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace OrkadWeb.Logic.Config
{
    public static class GlobalConfiguration
    {
        public const int USERNAME_MIN_LENGHT = 5;
        public const int USERNAME_MAX_LENGHT = 20;
        public static readonly Regex USERNAME_REGEX = new Regex("^[a-zA-Z0-9]*$");

        public const int PASSWORD_MIN_LENGHT = 8;
        public const int PASSWORD_MAX_LENGHT = 32;
        public static readonly Regex PASSWORD_REGEX = new Regex("(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[*.!@$%^&(){}[\\]:;<>,.?/~_+-=|])$");
    }
}

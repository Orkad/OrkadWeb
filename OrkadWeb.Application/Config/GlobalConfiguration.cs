﻿using System.Text.RegularExpressions;

namespace OrkadWeb.Application.Config;

public static class GlobalConfiguration
{
    public const int USERNAME_MIN_LENGHT = 5;
    public const int USERNAME_MAX_LENGHT = 20;
    public static readonly Regex USERNAME_REGEX = new("^[a-zA-Z0-9]*$");

    public const int PASSWORD_MIN_LENGHT = 8;
    public const int PASSWORD_MAX_LENGHT = 32;
    public static readonly Regex PASSWORD_REGEX = new(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[^(A-Za-z0-9)]).{8,32}$");

    public static readonly Regex EMAIL_REGEX = new(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

    public const int EMAIL_CONFIRMATION_HASH_LENGHT = 8;
}
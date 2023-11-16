using System;

namespace OrkadWeb.Application.Users.Exceptions;

public class NotOwnedException(string message) : Exception(message)
{
}
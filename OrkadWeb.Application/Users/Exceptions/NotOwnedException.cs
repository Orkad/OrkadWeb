﻿using System;

namespace OrkadWeb.Application.Users.Exceptions;

[Serializable]
public class NotOwnedException : Exception
{
    public NotOwnedException() { }
    public NotOwnedException(string message) : base(message) { }
    public NotOwnedException(string message, Exception inner) : base(message, inner) { }
    protected NotOwnedException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
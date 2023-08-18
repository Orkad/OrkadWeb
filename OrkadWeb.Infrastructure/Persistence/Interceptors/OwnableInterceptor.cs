using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using NHibernate;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using OrkadWeb.Application.Users;
using OrkadWeb.Application.Users.Exceptions;
using OrkadWeb.Domain.Entities;
using OrkadWeb.Domain.Primitives;

namespace OrkadWeb.Infrastructure.Persistence.Interceptors;

public class OwnableInterceptor : EmptyInterceptor
{
    private readonly IAppUser user;

    public OwnableInterceptor(IAppUser user)
    {
        this.user = user;
    }

    public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
    {
        if (entity is not IOwnable) return false;
        var actualOwner = GetValue(state, propertyNames, nameof(IOwnable.Owner));
        if (actualOwner is not null)
        {
            return false;
        }
        SetValue(state, propertyNames, nameof(IOwnable.Owner), new User { Id = user.Id });
        return true;
    }

    public override void OnDelete(object entity, object id, object[] state, string[] propertyNames, IType[] types)
    {
        if (entity is not IOwnable { Owner: not null } ownable) return;
        if (ownable.Owner.Id != user.Id)
        {
            throw new NotOwnedException("The actual user is trying to delete a not owned ownable entity");
        }
    }

    public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames,
        IType[] types)
    {
        if (entity is not IOwnable { Owner: not null } ownable) return false;
        if (ownable.Owner.Id != user.Id)
        {
            throw new NotOwnedException("The actual user is trying to update a not owned ownable entity");
        }

        return false;
    }

    private void SetValue(object[] state, string[] propertyNames, string propertyName, object value)
    {
        var index = Array.IndexOf(propertyNames, propertyName);
        state[index] = value;
    }

    private object GetValue(object[] state, string[] propertyNames, string propertyName)
    {
        var index = Array.IndexOf(propertyNames, propertyName);
        return state[index];
    }
}
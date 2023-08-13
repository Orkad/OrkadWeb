using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using OrkadWeb.Application.Users;
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

    public override bool OnLoad(object entity, object id, object[] state, string[] propertyNames, IType[] types)
    {
        return base.OnLoad(entity, id, state, propertyNames, types);
    }

    public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
    {
        if (entity is not IOwnable) return false;
        SetValue(state, propertyNames, nameof(IOwnable.Owner), new User { Id = user.Id });
        return true;
    }

    public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState,
        string[] propertyNames,
        IType[] types)
    {
        return base.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types);
    }

    private void SetValue(object[] state, string[] propertyNames, string propertyName, object value)
    {
        var index = Array.IndexOf(propertyNames, propertyName);
        ;
        state[index] = value;
    }
}
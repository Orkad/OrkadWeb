using NHibernate;
using NHibernate.Type;
using OrkadWeb.Application.Users;

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
        return base.OnSave(entity, id, state, propertyNames, types);
    }

    public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames,
        IType[] types)
    {
        return base.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types);
    }
}
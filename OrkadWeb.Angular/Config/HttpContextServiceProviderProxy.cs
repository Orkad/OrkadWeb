using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OrkadWeb.Infrastructure.Injection;
using System;

namespace OrkadWeb.Angular.Config
{
    public class HttpContextServiceProviderProxy : IServiceProviderProxy
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public HttpContextServiceProviderProxy(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public T GetService<T>()
        {
            return httpContextAccessor.HttpContext.RequestServices.GetService<T>();
        }

        public object GetService(Type type)
        {
            return httpContextAccessor.HttpContext.RequestServices.GetService(type);
        }
    }
}

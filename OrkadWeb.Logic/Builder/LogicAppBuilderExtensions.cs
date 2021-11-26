using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OrkadWeb.Logic.Builder
{
    public static class LogicAppBuilderExtensions
    {
        public static void AddLogic(this IServiceCollection services)
        {
            var asm = Assembly.GetExecutingAssembly();
            services.AddMediatR(asm);
            services.AddAutoMapper(asm);
        }
    }
}

using MediatR;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OrkadWeb.Logic.Abstractions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using OrkadWeb.Common;
using OrkadWeb.Logic.Services;

namespace OrkadWeb.Logic
{
    public static class LogicAppBuilderExtensions
    {
        /// <summary>
        /// Ajoute le pattern CQRS du projet (Mediator / Fluent Validation)
        /// </summary>
        /// <param name="services"></param>
        public static void AddLogic(this IServiceCollection services)
        {
            var asm = Assembly.GetExecutingAssembly();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(NHibernateTransactionalBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddMediatR(asm);
            services.AddValidatorsFromAssembly(asm);
            services.AddAutoMapper(asm);
            services.AddSingleton<ITimeProvider, RealTimeProvider>();
        }
    }
}

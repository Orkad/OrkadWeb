using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OrkadWeb.Logic.Abstractions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

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
            services.AddMediatR(asm);
            services.AddAutoMapper(asm);
        }

        /// <summary>
        /// Détermine le contexte de temps en temps que réél
        /// </summary>
        public static void AddRealTime(this IServiceCollection services)
        {
            services.AddSingleton<ITimeProvider, RealTimeProvider>();
        }
    }
}

using OrkadWeb.Application.Common.Interfaces;
using System;

namespace OrkadWeb.Application.Abstractions
{
    /// <summary>
    /// Implémentation réélle du fournisseur d'unité de temps basé sur <see cref="DateTime.Now"/>
    /// </summary>
    internal class RealTimeProvider : ITimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}

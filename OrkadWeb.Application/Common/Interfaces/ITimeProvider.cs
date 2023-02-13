using System;
using System.Collections.Generic;
using System.Text;

namespace OrkadWeb.Application.Common.Interfaces
{
    /// <summary>
    /// Permet de fournir des unités de temps dans un contexte donné (à préférer aux propriétés statiques de <see cref="DateTime"/>
    /// </summary>
    public interface ITimeProvider
    {
        public DateTime Now { get; }
    }
}

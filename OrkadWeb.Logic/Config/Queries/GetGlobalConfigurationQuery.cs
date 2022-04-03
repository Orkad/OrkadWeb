using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrkadWeb.Logic.Config.Queries
{
    /// <summary>
    /// Get the global configuration of the application.
    /// </summary>
    public class GetGlobalConfigurationQuery : IRequest<GlobalConfigurationResult>
    {

    }
}

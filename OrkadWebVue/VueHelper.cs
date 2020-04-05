using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace OrkadWebVue
{
    public static class VueHelper
    {
        private const int DEV_PORT = 4000;
        public static void UseVueDevelopmentServer(this ISpaBuilder spa)
        {
            spa.UseProxyToSpaDevelopmentServer(async () =>
            {
                if (IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners().Select(x => x.Port).Contains(DEV_PORT))
                {
                    return new Uri($"http://localhost:{DEV_PORT}");
                }
                throw new InvalidOperationException($"Please serve the port {DEV_PORT}");
            });

        }
    }
}
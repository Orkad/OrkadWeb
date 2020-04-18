using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace OrkadWeb.Services
{
    public class SupervisionService : ISupervisionService
    {
        private const string THERMAL_ZONE_PATH = "/sys/class/thermal/thermal_zone0/temp";
        private readonly bool isLinux;
        private readonly bool isThermalZoneOk;
        public SupervisionService()
        {
            isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            isThermalZoneOk = File.Exists(THERMAL_ZONE_PATH);
        }

        public double GetCpuTemperature()
        {
            if (!isLinux || !isThermalZoneOk)
            {
                return double.NaN;
            }
            using (StreamReader reader = new StreamReader(new FileStream(THERMAL_ZONE_PATH, FileMode.Open, FileAccess.Read)))
            {
                string data = reader.ReadLine();
                if (int.TryParse(data, out int temp))
                {
                    return temp / 1000d;
                }
                return double.NaN;
            }
        }
    }
}

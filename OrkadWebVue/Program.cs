using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace OrkadWebVue
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://*:80");
                    //webBuilder.UseSetting("https_port", "443");
                });
    }
}

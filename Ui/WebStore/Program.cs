using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;
using System.Xml;

namespace WebStore
{
    public class Program
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(Program));

        public static void Main(string[] args)
        {
            var log4NetConfig = new XmlDocument();
            log4NetConfig.Load(File.OpenRead("log4net.config"));

            var repo = log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

            log4net.Config.XmlConfigurator.Configure(repo, log4NetConfig["log4net"]);

            Log.Info("Application - Main is invoked");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            //.ConfigureLogging(builder =>
            //    {
                    //builder.SetMinimumLevel(LogLevel.Trace);
                   // builder.AddLog4Net("log4net.config")
                    //.AddConsole(c => c.IncludeScopes = true)
                    //.AddDebug()
                    //.AddEventLog()
                    //.AddFilter("Microsoft", LogLevel.Information)
                //    ;
                //})
            ;
    }
}

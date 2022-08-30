using ds.pms.bl.logger.LoggerTypeSettings.SeqConfigSetting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace ds.pms.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var configuration = new ConfigurationBuilder()
            //   .AddJsonFile("appsettings.json").Build();
            CreateHostBuilder(args).Build().Run();

            //string loggerType = configuration.GetValue<string>("LoggingType");
            //if (SeqConfiguration.IsSeqLogging(loggerType))
            //{
            //    SeqConfiguration.ConfigureSeq();
            //    LoadSeqLogConfig(args);
            //}
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void LoadSeqLogConfig(string[] args)
        {
            try
            {
                Log.Information("Application starting up using SEQ BL Logger.");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The Application failed to start correctly using SEQ BL Logger.");
            }
            finally { Log.CloseAndFlush(); }
        }
    }
}

using Microsoft.Extensions.Configuration;
using Serilog;
using System.IO;

namespace ds.pms.bl.logger.LoggerTypeSettings.SeqConfigSetting
{
    public class SeqConfiguration
    {
        public static bool IsSeqLogging(string loggingType)
        {
            if (!string.IsNullOrEmpty(loggingType))
            {
                if (loggingType == LoggingTypes.SeqLogging.ToString())
                    return true;
            }
            return false;
        }

        public static void ConfigureSeq()
        {
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            string seqConfigJsonPath = Path.Combine(path, @"ds.pms.bl.logger\LoggerTypeSettings\SeqConfigSetting\seqconfigsettings.json");

            var configuration = new ConfigurationBuilder()
                   .AddJsonFile(seqConfigJsonPath).Build();

            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
    }
}

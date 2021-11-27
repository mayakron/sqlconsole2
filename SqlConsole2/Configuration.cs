using System.Configuration;

namespace SqlConsole2
{
    internal static class Configuration
    {
        public static readonly string CommonFilesDirectoryPath = ConfigurationManager.AppSettings["CommonFilesDirectoryPath"];
    }
}
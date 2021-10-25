using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;

namespace WebStore.Logger
{
    public static class Log4NetExtensions
    {
        private static string CheckFile(string FilePath)
        {
            if (FilePath is not { Length : > 0 })
            {
                throw new ArgumentException("Не указан путь к файлу");
            }

            if (Path.IsPathRooted(FilePath))
            {
                return FilePath;
            }

            var assembly = Assembly.GetEntryAssembly();
            var dir = Path.GetDirectoryName(assembly!.Location);
            return Path.Combine(dir!, FilePath);
        }

        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, string log4NetConfigFile)
        {
            factory.AddProvider(new Log4NetProvider(CheckFile(log4NetConfigFile)));
            return factory;
        }

        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory)
        {
            factory.AddProvider(new Log4NetProvider("log4net.config"));
            return factory;
        }
    }
}

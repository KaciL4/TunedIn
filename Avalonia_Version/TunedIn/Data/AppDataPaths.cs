using System;
using System.IO;

namespace TunedIn.Utilities
{
    public static class AppDataPaths
    {
        private const string AppFolderName = "TunedIn";

        public static string GetDatabaseFilePath()
        {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appPath = Path.Combine(basePath, AppFolderName);
            Directory.CreateDirectory(appPath);
            return Path.Combine(appPath, "tunedin.db");
        }

        public static string GetAppDataFolder()
        {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appPath = Path.Combine(basePath, AppFolderName);
            Directory.CreateDirectory(appPath);
            return appPath;
        }
    }
}
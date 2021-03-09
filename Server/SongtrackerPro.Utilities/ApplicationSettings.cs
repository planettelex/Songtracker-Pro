using Microsoft.Extensions.Configuration;
using System.IO;

namespace SongtrackerPro.Utilities
{
    public static class ApplicationSettings
    {
        private const string AppDirectory = "SongtrackerPro.Api";
        private const string AppSettingsJson = "appsettings.json";
        private static bool _hasLoaded;

        public static string ConnectionString
        {
            get
            {
                if (!_hasLoaded)
                    LoadSettings();

                return _connectionString;
            }
        }
        private static string _connectionString;

        public static bool MinifyJson
        {
            get
            {
                if (!_hasLoaded)
                    LoadSettings();

                return _minifyJson;
            }
        }
        private static bool _minifyJson;

        private static void LoadSettings()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var settingsFilePath = FindApplicationSettingsFile(Directory.GetCurrentDirectory());

            configurationBuilder.AddJsonFile(settingsFilePath, false);

            var configuration = configurationBuilder.Build();

            _connectionString = configuration.GetSection("ConnectionStrings").GetSection("ApplicationDatabase").Value;
            _minifyJson = bool.Parse(configuration.GetSection("ApiOptions").GetSection("MinifyJson").Value);
            _hasLoaded = true;
        }

        private static string FindApplicationSettingsFile(string currentDirectory)
        {
            // 1: See if the settings file is in the currentDirectory. If so, return it.
            var path = Path.Combine(currentDirectory, AppSettingsJson);
            if (File.Exists(path))
                return path;

            // 2: See if the app directory is in the currentDirectory. If so, recurse on it.
            var directoryPath = Path.Combine(currentDirectory, AppDirectory);
            if (Directory.Exists(directoryPath))
                return FindApplicationSettingsFile(directoryPath);

            // 3: Move up a directory and try again if possible.
            var lastSlash = currentDirectory.LastIndexOf('\\');
            var upDirectoryPath = currentDirectory.Substring(0, lastSlash);
            if (Directory.Exists(upDirectoryPath))
                return FindApplicationSettingsFile(upDirectoryPath);

            throw new FileNotFoundException(path);
        }
    }
}

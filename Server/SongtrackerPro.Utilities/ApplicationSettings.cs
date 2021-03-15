﻿using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace SongtrackerPro.Utilities
{
    public static class ApplicationSettings
    {
        private const string AppDirectory = "SongtrackerPro.Api";
        private const string AppSettingsJson = "appsettings.json";
        private static bool _hasLoaded;

        public static class Database
        {
            public static string ConnectionString
            {
                get
                {
                    if (!_hasLoaded) LoadSettings();
                    return _connectionString;
                }
                set => _connectionString = value;
            }
            private static string _connectionString;

            public static string HostingConsole
            {
                get
                {
                    if (!_hasLoaded) LoadSettings();
                    return _hostingConsole;
                }
                set => _hostingConsole = value;
            }
            private static string _hostingConsole;
        }

        public static class Api
        {
            public static bool MinifyJson
            {
                get
                {
                    if (!_hasLoaded) LoadSettings();
                    return _minifyJson;
                }
                set => _minifyJson = value;
            }
            private static bool _minifyJson;

            public static string HostingConsole
            {
                get
                {
                    if (!_hasLoaded) LoadSettings();
                    return _hostingConsole;
                }
                set => _hostingConsole = value;
            }
            private static string _hostingConsole;
        }

        public static class Web
        {
            public static string OAuthId
            {
                get
                {
                    if (!_hasLoaded) LoadSettings();
                    return _oauthId;
                }
                set => _oauthId = value;
            }
            private static string _oauthId;

            public static string OAuthConsole
            {
                get
                {
                    if (!_hasLoaded) LoadSettings();
                    return _oauthConsole;
                }
                set => _oauthConsole = value;
            }
            private static string _oauthConsole;

            public static string HostingConsole
            {
                get
                {
                    if (!_hasLoaded) LoadSettings();
                    return _hostingConsole;
                }
                set => _hostingConsole = value;
            }
            private static string _hostingConsole;
        }

        public static string Version
        {
            get
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                return version == null ? null : $"{version.Major}.{version.Minor}{version.Build}";
            }
        }

        private static void LoadSettings()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var settingsFilePath = FindApplicationSettingsFile(Directory.GetCurrentDirectory());

            configurationBuilder.AddJsonFile(settingsFilePath, false);

            var configuration = configurationBuilder.Build();

            Database.ConnectionString = configuration.GetSection("ConnectionStrings").GetSection("ApplicationDatabase").Value;
            Database.HostingConsole = configuration.GetSection("Database")?.GetSection("HostingConsole")?.Value;
            Api.MinifyJson = bool.Parse(configuration.GetSection("Api")?.GetSection("MinifyJson")?.Value ?? bool.TrueString);
            Api.HostingConsole = configuration.GetSection("Api")?.GetSection("HostingConsole")?.Value;
            Web.OAuthId = configuration.GetSection("Web")?.GetSection("OAuthId")?.Value;
            Web.OAuthConsole = configuration.GetSection("Web")?.GetSection("OAuthConsole")?.Value;
            Web.HostingConsole = configuration.GetSection("Web")?.GetSection("HostingConsole")?.Value;

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

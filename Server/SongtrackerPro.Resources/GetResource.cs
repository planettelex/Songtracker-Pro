using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace SongtrackerPro.Resources
{
    public static class GetResource
    {
        private const string SystemMessagesFile = "SystemMessages.json";
        private const string SeedDataFile = "SeedData.json";
        private const string EmailTemplatesFolder = "EmailTemplates";

        public static string SeedData(string culture, string key)
        {
            if (_seedDataTranslations == null)
                LoadSeedDataTranslations(culture);

            if (_seedDataTranslations == null)
                throw new NullReferenceException();

            return _seedDataTranslations[key];
        }
        private static Dictionary<string, string> _seedDataTranslations;

        private static void LoadSeedDataTranslations(string culture)
        {
            var _ = AssemblyInfo.SupportedCultures.Single(c => c == culture);
            culture = culture.Replace('-', '_');

            var embeddedFilePath = $"{AssemblyInfo.Name}.{culture}.{SeedDataFile}";
            var assembly = Assembly.GetExecutingAssembly();

            using var stream = assembly.GetManifestResourceStream(embeddedFilePath);
            using var reader = new StreamReader(stream ?? throw new FileNotFoundException(null, embeddedFilePath));
            
            var translationsJson = reader.ReadToEnd();
            var seedData = JsonSerializer.Deserialize<CultureTranslations>(translationsJson);

            _seedDataTranslations = new Dictionary<string, string>();
            foreach (var translation in seedData.Translations)
                _seedDataTranslations.Add(translation.Key, translation.Value);
        }

        public static string SystemMessage(string culture, string key)
        {
            if (_systemMessageTranslations == null)
                LoadSystemMessageTranslations(culture);

            if (_systemMessageTranslations == null)
                throw new NullReferenceException();

            return _systemMessageTranslations[key];
        }
        private static Dictionary<string, string> _systemMessageTranslations;

        private static void LoadSystemMessageTranslations(string culture)
        {
            var _ = AssemblyInfo.SupportedCultures.Single(c => c == culture);
            culture = culture.Replace('-', '_');

            var embeddedFilePath = $"{AssemblyInfo.Name}.{culture}.{SystemMessagesFile}";
            var assembly = Assembly.GetExecutingAssembly();

            using var stream = assembly.GetManifestResourceStream(embeddedFilePath);
            using var reader = new StreamReader(stream ?? throw new FileNotFoundException(null, embeddedFilePath));
            
            var translationsJson = reader.ReadToEnd();
            var systemMessages = JsonSerializer.Deserialize<CultureTranslations>(translationsJson);

            _systemMessageTranslations = new Dictionary<string, string>();
            foreach (var translation in systemMessages.Translations)
                _systemMessageTranslations.Add(translation.Key, translation.Value);
        }

        public static string EmailTemplate(string culture, string filename)
        {
            var _ = AssemblyInfo.SupportedCultures.Single(c => c == culture);
            culture = culture.Replace('-', '_');

            var embeddedFilePath = $"{AssemblyInfo.Name}.{culture}.{EmailTemplatesFolder}.{filename}";
            var assembly = Assembly.GetExecutingAssembly();

            using var stream = assembly.GetManifestResourceStream(embeddedFilePath);
            using var reader = new StreamReader(stream ?? throw new FileNotFoundException(null, embeddedFilePath));

            return reader.ReadToEnd();
        }
    }
}

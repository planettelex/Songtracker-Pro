using System.Text.RegularExpressions;

namespace SongtrackerPro.Utilities.Services
{
    public interface IHtmlService
    {
        public string GetTitle(string htmlDocument);
    }

    public class HtmlService : IHtmlService
    {
        public string GetTitle(string htmlDocument)
        {
            var match = Regex.Match(htmlDocument, @"<title>\s*(.+?)\s*</title>");
            return match.Success ? match.Groups[1].Value : string.Empty;
        }
    }
}

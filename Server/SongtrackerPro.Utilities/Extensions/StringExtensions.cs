using System.Text.RegularExpressions;

namespace SongtrackerPro.Utilities.Extensions
{
    public static class StringExtensions
    {
        public static string SpaceUniformly(this string s)
        {
            return Regex.Replace(s, @"\s+", " ").Trim();
        }
    }
}

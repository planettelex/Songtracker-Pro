using System.Text.RegularExpressions;

namespace SongtrackerPro.Data.Services
{
    public interface IFormattingService
    {
        public string FormatTaxId(string taxId);

        public string FormatPhoneNumber(string phoneNumber);
    }

    public class FormattingService : IFormattingService
    {
        public string FormatTaxId(string taxId)
        {
            if (string.IsNullOrEmpty(taxId))
                return null;

            var stripped = StripNonNumeric(taxId);

            if (stripped.Length <= 2)
                return stripped;

            return $"{stripped[..2]}-{stripped[2..]}";
        }

        public string FormatPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return null;

            var stripped = StripNonNumeric(phoneNumber);

            if (stripped.Length <= 3)
                return stripped;

            if (stripped.Length > 3 && stripped.Length <= 7)
                return $"{stripped[..3]}-{stripped[3..]}";

            if (stripped.Length > 7 && stripped.Length <= 10)
                return $"({stripped[..3]}) {stripped[3..6]}-{stripped[6..]}";

            if (stripped.Length == 11)
                return $"+{stripped[..1]} ({stripped[1..4]}) {stripped[4..7]}-{stripped[7..]}";

            return $"+{stripped[..1]} ({stripped[1..4]}) {stripped[4..7]}-{stripped[7..11]} x{stripped[11..]}";
        }

        private static string StripNonNumeric(string text)
        {
            return Regex.Replace(text, "[^0-9]", "");
        }
    }
}

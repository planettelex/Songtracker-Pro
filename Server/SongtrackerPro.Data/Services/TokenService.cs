using System;
using System.Reflection;

namespace SongtrackerPro.Data.Services
{
    public interface ITokenService
    {
        public string ReplaceTokens(string textWithTokens, object objectWithValues);
    }

    public class TokenService : ITokenService
    {
        public string ReplaceTokens(string textWithTokens, object objectWithValues)
        {
            if (textWithTokens == null)
                return null;

            if (objectWithValues == null)
                return textWithTokens;

            var className = objectWithValues.GetType().Name;
            var objectProperties = objectWithValues.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propertyInfo in objectProperties)
            {
                if (!IsReplaceableType(propertyInfo.PropertyType)) 
                    continue;
                
                var tokenName = "{" + className + "." + propertyInfo.Name + "}";
                var propertyValue = $"{ propertyInfo.GetValue(objectWithValues) }";
                textWithTokens = textWithTokens.Replace(tokenName, propertyValue);
            }

            return textWithTokens;
        }

        private static bool IsReplaceableType(Type type)
        {
            return type == typeof(string) ||
                   type == typeof(int) ||
                   type == typeof(int?) ||
                   type == typeof(bool) ||
                   type == typeof(bool?) ||
                   type == typeof(decimal) ||
                   type == typeof(decimal?) ||
                   type == typeof(double) ||
                   type == typeof(double?) ||
                   type == typeof(DateTime) ||
                   type == typeof(DateTime?) ||
                   type.BaseType == typeof(Enum);
        }
    }
}

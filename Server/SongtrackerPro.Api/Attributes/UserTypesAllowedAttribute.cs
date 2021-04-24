using System;
using SongtrackerPro.Data.Enums;

namespace SongtrackerPro.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UserTypesAllowedAttribute : Attribute
    {
        public UserTypesAllowedAttribute(params UserType[] userTypes)
        {
            UserTypes = userTypes;
        }

        public virtual UserType[] UserTypes { get; }
    }
}

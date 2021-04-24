using System;
using SongtrackerPro.Data.Enums;

namespace SongtrackerPro.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SystemUserRolesAllowedAttribute : Attribute
    {
        public SystemUserRolesAllowedAttribute(SystemUserRoles systemUserRoles)
        {
            SystemUserRoles = systemUserRoles;
        }

        public virtual SystemUserRoles SystemUserRoles { get; }
    }
}

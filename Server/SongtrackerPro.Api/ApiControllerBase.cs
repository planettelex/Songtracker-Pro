using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SongtrackerPro.Api.Attributes;
using SongtrackerPro.Api.ViewModels;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Resources;
using SongtrackerPro.Tasks;
using SongtrackerPro.Tasks.UserTasks;
using SongtrackerPro.Utilities;

namespace SongtrackerPro.Api
{
    public class ApiControllerBase : ControllerBase
    {
        public ApiControllerBase(IGetLoginTask getLoginTask)
        {
            _getLoginTask = getLoginTask;
        }
        private readonly IGetLoginTask _getLoginTask;

        protected JsonSerializerOptions SerializerOptions =>
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = !ApplicationSettings.Api.MinifyJson
            };

        protected string JsonSerialize<T>(TaskResult<T> taskResults)
        {
            return taskResults.Success ?
                JsonSerializer.Serialize(taskResults.Data, SerializerOptions) : 
                JsonSerializer.Serialize(new ServerError(taskResults.Exception), SerializerOptions);
        }

        protected string JsonSerialize<T>(T toSerialize)
        {
            return JsonSerializer.Serialize(toSerialize, SerializerOptions);
        }

        protected IActionResult Json<T>(TaskResult<T> taskResults)
        {
            return Content(JsonSerialize(taskResults), "application/json");
        }

        protected IActionResult Json<T>(T toSerialize)
        {
            return Content(JsonSerialize(toSerialize), "application/json");
        }

        protected IActionResult Error(Exception exception)
        {
            // Add exception logging here.
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        protected string AuthenticationToken => Request.Headers["AuthenticationToken"];

        protected Login Login
        {
            get
            {
                if (_login != null) 
                    return _login;

                var results = _getLoginTask.DoTask(AuthenticationToken);
                if (results.Success && results.Data.LogoutAt == null)
                    _login = results.Data;

                return _login;
            }
        }
        private Login _login;

        protected User AuthenticatedUser => Login?.User;

        protected bool UserIsAuthorized(MethodBase callingMethod)
        {
            if (AuthenticationToken == null)
                return false;

            if (Login == null)
                return false;

            if (AuthenticatedUser == null)
                return false;

            if (Login.LogoutAt != null)
                return false;

            var userTypesAllowedAttributes = (UserTypesAllowedAttribute[]) callingMethod.GetCustomAttributes(typeof(UserTypesAllowedAttribute), true);
            if (!userTypesAllowedAttributes.Any())
                return false;

            var userTypesAllowedAttribute = userTypesAllowedAttributes.First();
            var userTypesAllowed = userTypesAllowedAttribute.UserTypes;
            if (userTypesAllowed.Contains(UserType.Unassigned))
                return true;

            if (AuthenticatedUser.Type == UserType.Unassigned)
                return false;

            var userIsAnAllowedType = userTypesAllowed.Any(userType => AuthenticatedUser.Type == userType);
            if (!userIsAnAllowedType)
                return false;

            if (AuthenticatedUser.Type != UserType.SystemUser)
                return true;

            var userRolesAllowedAttributes = (SystemUserRolesAllowedAttribute[]) callingMethod.GetCustomAttributes(typeof(SystemUserRolesAllowedAttribute), true);
            if (!userRolesAllowedAttributes.Any())
                return false;

            var userRolesAllowedAttribute = userRolesAllowedAttributes.First();
            var userRolesAllowed = userRolesAllowedAttribute.SystemUserRoles;
            
            return Enum.GetValues(typeof(SystemUserRoles)).Cast<SystemUserRoles>().Any(roleFlag => userRolesAllowed.HasFlag(roleFlag));
        }

        protected string SystemMessage(string key)
        {
            return GetResource.SystemMessage(ApplicationSettings.Api.Culture, key);
        }
    }
}

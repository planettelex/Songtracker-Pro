using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    public abstract class ApiControllerBase : ControllerBase
    {
        protected ApiControllerBase(IGetLoginTask getLoginTask, ILogger<ApiControllerBase> logger)
        {
            _getLoginTask = getLoginTask;
            _logger = logger;
        }
        private readonly IGetLoginTask _getLoginTask;
        private readonly ILogger<ApiControllerBase> _logger;

        protected const string JsonContentType = "application/json";

        protected string ClientKey => Request.Headers["ClientKey"];

        protected string AuthenticationToken => Request.Headers["AuthenticationToken"];

        protected bool ClientKeyIsValid()
        {
            if (ClientKey == null)
                return false;

            return ClientKey == ApplicationSettings.Api.ClientKey;
        }

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
            return Content(JsonSerialize(taskResults), JsonContentType);
        }

        protected IActionResult Json<T>(T toSerialize)
        {
            return Content(JsonSerialize(toSerialize), JsonContentType);
        }

        protected IActionResult Error(Exception exception)
        {
            var innerException = exception.InnerException;
            if (innerException != null)
                _logger.LogError(innerException, innerException.Message);

            _logger.LogError(exception, exception.Message);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        protected string SystemMessage(string key)
        {
            return GetResource.SystemMessage(ApplicationSettings.Culture, key);
        }

        protected string SeedData(string key)
        {
            return GetResource.SeedData(ApplicationSettings.Culture, key);
        }

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

        protected bool UserIsAuthenticatedAndAuthorized(MethodBase callingMethod)
        {
            var userTypesAllowedAttributes = (UserTypesAllowedAttribute[]) callingMethod.GetCustomAttributes(typeof(UserTypesAllowedAttribute), true);
            if (!userTypesAllowedAttributes.Any())
                return false;

            var userTypesAllowedAttribute = userTypesAllowedAttributes.First();
            var userTypesAllowed = userTypesAllowedAttribute.UserTypes;
            if (userTypesAllowed.Contains(UserType.Unassigned)) // Unassigned user types allowed means allow unauthenticated access.
                return true;

            return UserIsAuthenticated() && UserIsAuthorized(callingMethod, userTypesAllowed);
        }

        protected bool UserIsAuthenticated()
        {
            if (AuthenticationToken == null)
                return false;

            if (Login == null)
                return false;

            if (Login.LogoutAt != null)
                return false;

            if (Login.TokenExpiration < DateTime.UtcNow)
                return false;

            if (AuthenticatedUser == null)
                return false;

            return true;
        }

        private bool UserIsAuthorized(ICustomAttributeProvider callingMethod, IEnumerable<UserType> userTypesAllowed)
        {
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
    }
}

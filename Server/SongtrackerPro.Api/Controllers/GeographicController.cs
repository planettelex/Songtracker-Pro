using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using SongtrackerPro.Api.Attributes;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Tasks.GeographicTasks;
using SongtrackerPro.Tasks.UserTasks;

namespace SongtrackerPro.Api.Controllers
{
    [ApiController]
    public class GeographicController : ApiControllerBase
    {
        #region Constructor

        public GeographicController(IGetLoginTask getLoginTask,
            IListCountriesTask listCountriesTask) : 
            base(getLoginTask)
        {
            _listCountriesTask = listCountriesTask;
        }
        private readonly IListCountriesTask _listCountriesTask;

        #endregion

        [Route(Routes.Countries)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult ListCountries()
        {
            try
            {
                if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var taskResults = _listCountriesTask.DoTask(null);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }
    }
}

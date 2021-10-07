using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SongtrackerPro.Tasks.GeographicTasks;
using SongtrackerPro.Tasks.UserTasks;

namespace SongtrackerPro.Api.Controllers
{
    [ApiController]
    public class GeographicController : ApiControllerBase
    {
        #region Constructor

        public GeographicController(IGetLoginTask getLoginTask,
                                    IListCountriesTask listCountriesTask,
                                    ILogger<GeographicController> logger) : 
        base(getLoginTask, logger)
        {
            _listCountriesTask = listCountriesTask;
        }
        private readonly IListCountriesTask _listCountriesTask;

        #endregion

        [Route(Routes.Countries)]
        [HttpGet]
        public IActionResult ListCountries()
        {
            try
            {
                if (!ClientKeyIsValid())
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

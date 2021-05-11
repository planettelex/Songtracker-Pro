using System.Reflection;
using Microsoft.AspNetCore.Http;
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
        public GeographicController(IGetLoginTask getLoginTask,
                                    IListCountriesTask listCountriesTask) : 
        base(getLoginTask)
        {
            _listCountriesTask = listCountriesTask;
        }
        private readonly IListCountriesTask _listCountriesTask;

        [Route(Routes.Countries)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult ListCountries()
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            var taskResults = _listCountriesTask.DoTask(null);

            if (taskResults.Success)
                return Ok(JsonSerialize(taskResults));

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}

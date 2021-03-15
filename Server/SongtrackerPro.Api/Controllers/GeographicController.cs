using Microsoft.AspNetCore.Mvc;
using SongtrackerPro.Tasks.GeographicTasks;

namespace SongtrackerPro.Api.Controllers
{
    [ApiController]
    public class GeographicController : ApiControllerBase
    {
        public GeographicController(IListCountriesTask listCountriesTask)
        {
            _listCountriesTask = listCountriesTask;
        }
        private readonly IListCountriesTask _listCountriesTask;

        [Route(Routes.Countries)]
        [HttpGet]
        public string ListCountries()
        {
            var taskResults = _listCountriesTask.DoTask(null);

            return JsonSerialize(taskResults);
        }
    }
}

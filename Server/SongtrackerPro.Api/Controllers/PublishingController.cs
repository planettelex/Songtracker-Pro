using Microsoft.AspNetCore.Mvc;
using SongtrackerPro.Tasks.PublishingTasks;

namespace SongtrackerPro.Api.Controllers
{
    [ApiController]
    public class PublishingController : ApiControllerBase
    {
        public PublishingController(IListPerformingRightsOrganizationsTask listPerformingRightsOrganizationsTask)
        {
            _listPerformingRightsOrganizationsTask = listPerformingRightsOrganizationsTask;
        }
        private readonly IListPerformingRightsOrganizationsTask _listPerformingRightsOrganizationsTask;

        [Route(Routes.PerformingRightsOrganizations)]
        [HttpGet]
        public string ListPerformingRightsOrganizations()
        {
            var taskResults = _listPerformingRightsOrganizationsTask.DoTask(null);

            return JsonSerialize(taskResults);
        }
    }
}

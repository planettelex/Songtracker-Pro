using Microsoft.AspNetCore.Mvc;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.PublishingTasks;

namespace SongtrackerPro.Api.Controllers
{
    [ApiController]
    public class PublishingController : ApiControllerBase
    {
        public PublishingController(IListPerformingRightsOrganizationsTask listPerformingRightsOrganizationsTask, 
                                    IListPublishersTask listPublishersTask,
                                    IGetPublisherTask getPublisherTask,
                                    IAddPublisherTask addPublisherTask,
                                    IUpdatePublisherTask updatePublisherTask)
        {
            _listPerformingRightsOrganizationsTask = listPerformingRightsOrganizationsTask;
            _listPublishersTask = listPublishersTask;
            _getPublisherTask = getPublisherTask;
            _addPublisherTask = addPublisherTask;
            _updatePublisherTask = updatePublisherTask;
        }
        private readonly IListPerformingRightsOrganizationsTask _listPerformingRightsOrganizationsTask;
        private readonly IListPublishersTask _listPublishersTask;
        private readonly IGetPublisherTask _getPublisherTask;
        private readonly IAddPublisherTask _addPublisherTask;
        private readonly IUpdatePublisherTask _updatePublisherTask;

        [Route(Routes.PerformingRightsOrganizations)]
        [HttpGet]
        public string ListPerformingRightsOrganizations()
        {
            var taskResults = _listPerformingRightsOrganizationsTask.DoTask(null);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.Publishers)]
        [HttpPost]
        public string AddPublisher(Publisher publisher)
        {
            var taskResults = _addPublisherTask.DoTask(publisher);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.Publishers)]
        [HttpGet]
        public string ListPublishers()
        {
            var taskResults = _listPublishersTask.DoTask(null);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.Publisher)]
        [HttpGet]
        public string GetPublisher(int id)
        {
            var taskResults = _getPublisherTask.DoTask(id);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.Publisher)]
        [HttpPut]
        public void UpdatePublisher(int id, Publisher publisher)
        {
            publisher.Id = id;
            _updatePublisherTask.DoTask(publisher);
        }
    }
}

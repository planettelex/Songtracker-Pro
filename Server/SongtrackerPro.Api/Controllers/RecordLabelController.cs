using Microsoft.AspNetCore.Mvc;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.RecordLabelTasks;

namespace SongtrackerPro.Api.Controllers
{
    [ApiController]
    public class RecordLabelController : ApiControllerBase
    {
        public RecordLabelController(IListRecordLabelsTask listRecordLabelsTask,
                                     IGetRecordLabelTask getRecordLabelTask,
                                     IAddRecordLabelTask addRecordLabelTask,
                                     IUpdateRecordLabelTask updateRecordLabelTask
        )
        {
            _listRecordLabelsTask = listRecordLabelsTask;
            _getRecordLabelTask = getRecordLabelTask;
            _addRecordLabelTask = addRecordLabelTask;
            _updateRecordLabelTask = updateRecordLabelTask;
        }
        private readonly IListRecordLabelsTask _listRecordLabelsTask;
        private readonly IGetRecordLabelTask _getRecordLabelTask;
        private readonly IAddRecordLabelTask _addRecordLabelTask;
        private readonly IUpdateRecordLabelTask _updateRecordLabelTask;

        [Route(Routes.RecordLabels)]
        [HttpPost]
        public string AddRecordLabel(RecordLabel recordLabel)
        {
            var taskResults = _addRecordLabelTask.DoTask(recordLabel);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.RecordLabels)]
        [HttpGet]
        public string ListRecordLabels()
        {
            var taskResults = _listRecordLabelsTask.DoTask(null);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.RecordLabel)]
        [HttpGet]
        public string GetRecordLabel(int id)
        {
            var taskResults = _getRecordLabelTask.DoTask(id);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.RecordLabel)]
        [HttpPut]
        public void UpdateRecordLabel(int id, RecordLabel recordLabel)
        {
            recordLabel.Id = id;
            _updateRecordLabelTask.DoTask(recordLabel);
        }
    }
}

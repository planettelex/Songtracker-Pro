using Microsoft.AspNetCore.Mvc;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.ArtistTasks;

namespace SongtrackerPro.Api.Controllers
{
    [ApiController]
    public class ArtistController : ApiControllerBase
    {
        public ArtistController(IListArtistsTask listArtistsTask,
            IGetArtistTask getArtistTask,
            IAddArtistTask addArtistTask,
            IUpdateArtistTask updateArtistTask
        )
        {
            _listArtistsTask = listArtistsTask;
            _getArtistTask = getArtistTask;
            _addArtistTask = addArtistTask;
            _updateArtistTask = updateArtistTask;
        }
        private readonly IListArtistsTask _listArtistsTask;
        private readonly IGetArtistTask _getArtistTask;
        private readonly IAddArtistTask _addArtistTask;
        private readonly IUpdateArtistTask _updateArtistTask;

        [Route(Routes.Artists)]
        [HttpPost]
        public string AddArtist(Artist artist)
        {
            var taskResults = _addArtistTask.DoTask(artist);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.Artists)]
        [HttpGet]
        public string ListArtists()
        {
            var taskResults = _listArtistsTask.DoTask(null);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.Artist)]
        [HttpGet]
        public string GetArtist(int id)
        {
            var taskResults = _getArtistTask.DoTask(id);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.Artist)]
        [HttpPut]
        public void UpdateArtist(int id, Artist artist)
        {
            artist.Id = id;
            _updateArtistTask.DoTask(artist);
        }
    }
}

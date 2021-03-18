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
                                IUpdateArtistTask updateArtistTask,
                                IListArtistAccountsTask listArtistAccountsTask,
                                IGetArtistAccountTask getArtistAccountTask,
                                IAddArtistAccountTask addArtistAccountTask,
                                IUpdateArtistAccountTask updateArtistAccountTask,
                                IRemoveArtistAccountTask removeArtistAccountTask,
                                IListArtistLinksTask listArtistLinksTask,
                                IGetArtistLinkTask getArtistLinkTask,
                                IAddArtistLinkTask addArtistLinkTask,
                                IRemoveArtistLinkTask removeArtistLinkTask,
                                IListArtistMembersTask listArtistMembersTask,
                                IGetArtistMemberTask getArtistMemberTask,
                                IAddArtistMemberTask addArtistMemberTask,
                                IUpdateArtistMemberTask updateArtistMemberTask
        )
        {
            _listArtistsTask = listArtistsTask;
            _getArtistTask = getArtistTask;
            _addArtistTask = addArtistTask;
            _updateArtistTask = updateArtistTask;
            _listArtistAccountsTask = listArtistAccountsTask;
            _getArtistAccountTask = getArtistAccountTask;
            _addArtistAccountTask = addArtistAccountTask;
            _updateArtistAccountTask = updateArtistAccountTask;
            _removeArtistAccountTask = removeArtistAccountTask;
            _listArtistLinksTask = listArtistLinksTask;
            _getArtistLinkTask = getArtistLinkTask;
            _addArtistLinkTask = addArtistLinkTask;
            _removeArtistLinkTask = removeArtistLinkTask;
            _listArtistMembersTask = listArtistMembersTask;
            _getArtistMemberTask = getArtistMemberTask;
            _addArtistMemberTask = addArtistMemberTask;
            _updateArtistMemberTask = updateArtistMemberTask;
        }
        private readonly IListArtistsTask _listArtistsTask;
        private readonly IGetArtistTask _getArtistTask;
        private readonly IAddArtistTask _addArtistTask;
        private readonly IUpdateArtistTask _updateArtistTask;
        private readonly IListArtistAccountsTask _listArtistAccountsTask;
        private readonly IGetArtistAccountTask _getArtistAccountTask;
        private readonly IAddArtistAccountTask _addArtistAccountTask;
        private readonly IUpdateArtistAccountTask _updateArtistAccountTask;
        private readonly IRemoveArtistAccountTask _removeArtistAccountTask;
        private readonly IListArtistLinksTask _listArtistLinksTask;
        private readonly IGetArtistLinkTask _getArtistLinkTask;
        private readonly IAddArtistLinkTask _addArtistLinkTask;
        private readonly IRemoveArtistLinkTask _removeArtistLinkTask;
        private readonly IListArtistMembersTask _listArtistMembersTask;
        private readonly IGetArtistMemberTask _getArtistMemberTask;
        private readonly IAddArtistMemberTask _addArtistMemberTask;
        private readonly IUpdateArtistMemberTask _updateArtistMemberTask;

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

        [Route(Routes.ArtistMembers)]
        [HttpPost]
        public string AddArtistMember(int artistId, ArtistMember artistMember)
        {
            artistMember.ArtistId = artistId;
            var taskResults = _addArtistMemberTask.DoTask(artistMember);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.ArtistMembers)]
        [HttpGet]
        public string ListArtistMembers(int artistId)
        {
            var artist = _getArtistTask.DoTask(artistId).Data;
            var taskResults = _listArtistMembersTask.DoTask(artist);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.ArtistMember)]
        [HttpPut]
        public void UpdateArtistMember(int artistId, int artistMemberId, ArtistMember artistMember)
        {
            artistMember.ArtistId = artistId;
            artistMember.Id = artistMemberId;
            _updateArtistMemberTask.DoTask(artistMember);
        }

        [Route(Routes.ArtistAccounts)]
        [HttpPost]
        public string AddArtistAccount(int artistId, ArtistAccount artistAccount)
        {
            artistAccount.ArtistId = artistId;
            var taskResults = _addArtistAccountTask.DoTask(artistAccount);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.ArtistAccounts)]
        [HttpGet]
        public string ListArtistAccounts(int artistId)
        {
            var artist = _getArtistTask.DoTask(artistId).Data;
            var taskResults = _listArtistAccountsTask.DoTask(artist);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.ArtistAccount)]
        [HttpPut]
        public void UpdateArtistAccount(int artistId, int artistAccountId, ArtistAccount artistAccount)
        {
            artistAccount.ArtistId = artistId;
            artistAccount.Id = artistAccountId;
            _updateArtistAccountTask.DoTask(artistAccount);
        }

        [Route(Routes.ArtistAccount)]
        [HttpDelete]
        public void RemoveArtistAccount(int artistId, int artistAccountId)
        {
            var toRemove = _getArtistAccountTask.DoTask(artistAccountId).Data;
            if (toRemove == null)
                return;

            if (toRemove.ArtistId == artistId)
                _removeArtistAccountTask.DoTask(toRemove);
        }

        [Route(Routes.ArtistLinks)]
        [HttpPost]
        public string AddArtistLink(int artistId, ArtistLink artistLink)
        {
            artistLink.ArtistId = artistId;
            var taskResults = _addArtistLinkTask.DoTask(artistLink);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.ArtistLinks)]
        [HttpGet]
        public string ListArtistLinks(int artistId)
        {
            var artist = _getArtistTask.DoTask(artistId).Data;
            var taskResults = _listArtistLinksTask.DoTask(artist);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.ArtistLink)]
        [HttpDelete]
        public void RemoveArtistLink(int artistId, int artistLinkId)
        {
            var toRemove = _getArtistLinkTask.DoTask(artistLinkId).Data;
            if (toRemove == null)
                return;

            if (toRemove.ArtistId == artistId)
                _removeArtistLinkTask.DoTask(toRemove);
        }
    }
}

using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SongtrackerPro.Api.Attributes;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.UserTasks;

namespace SongtrackerPro.Api.Controllers
{
    [ApiController]
    public class ArtistController : ApiControllerBase
    {
        public ArtistController(IGetUserByAuthenticationTokenTask getUserByAuthenticationTokenTask,
                                IListArtistsTask listArtistsTask,
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
                                IAddArtistMemberTask addArtistMemberTask,
                                IUpdateArtistMemberTask updateArtistMemberTask,
                                IListArtistManagersTask listArtistManagersTask,
                                IAddArtistManagerTask addArtistManagerTask,
                                IUpdateArtistManagerTask updateArtistManagerTask) : 
        base(getUserByAuthenticationTokenTask)
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
            _addArtistMemberTask = addArtistMemberTask;
            _updateArtistMemberTask = updateArtistMemberTask;
            _listArtistManagersTask = listArtistManagersTask;
            _addArtistManagerTask = addArtistManagerTask;
            _updateArtistManagerTask = updateArtistManagerTask;
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
        private readonly IAddArtistMemberTask _addArtistMemberTask;
        private readonly IUpdateArtistMemberTask _updateArtistMemberTask;
        private readonly IListArtistManagersTask _listArtistManagersTask;
        private readonly IAddArtistManagerTask _addArtistManagerTask;
        private readonly IUpdateArtistManagerTask _updateArtistManagerTask;

        [Route(Routes.Artists)]
        [HttpPost]
        [UserTypesAllowed(UserType.SystemAdministrator, UserType.LabelAdministrator)]
        public IActionResult AddArtist(Artist artist)
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            if (AuthenticatedUser.Type == UserType.LabelAdministrator && AuthenticatedUser.RecordLabelId != artist.RecordLabelId)
                return Unauthorized();

            var taskResults = _addArtistTask.DoTask(artist);

            if (taskResults.Success)
                return Ok(JsonSerialize(taskResults));

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Route(Routes.Artists)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult ListArtists()
        {
            try
            {
                if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var taskResults = _listArtistsTask.DoTask(null);

                if (!taskResults.Success) 
                    return StatusCode(StatusCodes.Status500InternalServerError);

                var artists = taskResults.Data;
                foreach (var artist in artists)
                    RedactArtistData(artist);
            
                return Ok(JsonSerialize(artists));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route(Routes.Artist)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult GetArtist(int id)
        {
            try
            {
                if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var taskResults = _getArtistTask.DoTask(id);

                if (!taskResults.Success) 
                    return StatusCode(StatusCodes.Status500InternalServerError);

                var artist = taskResults.Data;
                RedactArtistData(artist);

                return Ok(JsonSerialize(artist));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route(Routes.Artist)]
        [HttpPut]
        [UserTypesAllowed(UserType.SystemAdministrator, UserType.LabelAdministrator, UserType.SystemUser)]
        [SystemUserRolesAllowed(SystemUserRoles.ArtistMember | SystemUserRoles.ArtistManager)]
        public IActionResult UpdateArtist(int id, Artist artist)
        {
            try
            {
                if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (!UserIsAuthorizedForArtist(id))
                    return Unauthorized();

                artist.Id = id;
                var taskResults = _updateArtistTask.DoTask(artist);

                return taskResults.Success ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route(Routes.ArtistMembers)]
        [HttpPost]
        [UserTypesAllowed(UserType.SystemAdministrator, UserType.LabelAdministrator, UserType.SystemUser)]
        [SystemUserRolesAllowed(SystemUserRoles.ArtistMember | SystemUserRoles.ArtistManager)]
        public IActionResult AddArtistMember(int artistId, ArtistMember artistMember)
        {
            try
            {
                if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (!UserIsAuthorizedForArtist(artistId))
                    return Unauthorized();

                artistMember.ArtistId = artistId;
                var taskResults = _addArtistMemberTask.DoTask(artistMember);

                if (taskResults.Success)
                    return Ok(JsonSerialize(taskResults));

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route(Routes.ArtistMembers)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult ListArtistMembers(int artistId)
        {
            try
            {
                if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var artist = _getArtistTask.DoTask(artistId).Data;
                var taskResults = _listArtistMembersTask.DoTask(artist);

                if (!taskResults.Success)
                    return StatusCode(StatusCodes.Status500InternalServerError);

                var artistMembers = taskResults.Data;
                foreach (var artistMember in artistMembers)
                    RedactPersonData(artistMember.Member, artist);

                return Ok(JsonSerialize(artistMembers));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route(Routes.ArtistMember)]
        [HttpPut]
        [UserTypesAllowed(UserType.SystemAdministrator, UserType.LabelAdministrator, UserType.SystemUser)]
        [SystemUserRolesAllowed(SystemUserRoles.ArtistMember | SystemUserRoles.ArtistManager)]
        public IActionResult UpdateArtistMember(int artistId, int artistMemberId, ArtistMember artistMember)
        {
            try
            {
                if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (!UserIsAuthorizedForArtist(artistId))
                    return Unauthorized();

                artistMember.ArtistId = artistId;
                artistMember.Id = artistMemberId;
                var taskResults = _updateArtistMemberTask.DoTask(artistMember);

                return taskResults.Success ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route(Routes.ArtistManagers)]
        [HttpPost]
        [UserTypesAllowed(UserType.SystemAdministrator, UserType.LabelAdministrator, UserType.SystemUser)]
        [SystemUserRolesAllowed(SystemUserRoles.ArtistMember | SystemUserRoles.ArtistManager)]
        public IActionResult AddArtistManager(int artistId, ArtistManager artistManager)
        {
            try
            {
                if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (!UserIsAuthorizedForArtist(artistId))
                    return Unauthorized();

                artistManager.ArtistId = artistId;
                var taskResults = _addArtistManagerTask.DoTask(artistManager);

                if (taskResults.Success)
                    return Ok(JsonSerialize(taskResults));

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route(Routes.ArtistManagers)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult ListArtistManagers(int artistId)
        {
            try
            {
                if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var artist = _getArtistTask.DoTask(artistId).Data;
                var taskResults = _listArtistManagersTask.DoTask(artist);

                if (!taskResults.Success)
                    return StatusCode(StatusCodes.Status500InternalServerError);

                var artistManagers = taskResults.Data;
                foreach (var artistManager in artistManagers)
                    RedactPersonData(artistManager.Manager, artist);

                return Ok(JsonSerialize(artistManagers));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route(Routes.ArtistManager)]
        [HttpPut]
        [UserTypesAllowed(UserType.SystemAdministrator, UserType.LabelAdministrator, UserType.SystemUser)]
        [SystemUserRolesAllowed(SystemUserRoles.ArtistMember | SystemUserRoles.ArtistManager)]
        public IActionResult UpdateArtistManager(int artistId, int artistManagerId, ArtistManager artistManager)
        {
            try
            {
                if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (!UserIsAuthorizedForArtist(artistId))
                    return Unauthorized();

                artistManager.ArtistId = artistId;
                artistManager.Id = artistManagerId;
                var taskResults = _updateArtistManagerTask.DoTask(artistManager);

                return taskResults.Success ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route(Routes.ArtistAccounts)]
        [HttpPost]
        [UserTypesAllowed(UserType.SystemAdministrator, UserType.LabelAdministrator, UserType.SystemUser)]
        [SystemUserRolesAllowed(SystemUserRoles.ArtistMember | SystemUserRoles.ArtistManager)]
        public IActionResult AddArtistAccount(int artistId, ArtistAccount artistAccount)
        {
            try
            {
                if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (!UserIsAuthorizedForArtist(artistId))
                    return Unauthorized();

                artistAccount.ArtistId = artistId;
                var taskResults = _addArtistAccountTask.DoTask(artistAccount);

                if (taskResults.Success)
                    return Ok(JsonSerialize(taskResults));

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route(Routes.ArtistAccounts)]
        [HttpGet]
        [UserTypesAllowed(UserType.SystemAdministrator, UserType.LabelAdministrator, UserType.SystemUser)]
        [SystemUserRolesAllowed(SystemUserRoles.ArtistMember | SystemUserRoles.ArtistManager)]
        public IActionResult ListArtistAccounts(int artistId)
        {
            try
            {
                if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (!UserIsAuthorizedForArtist(artistId))
                    return Unauthorized();

                var artist = _getArtistTask.DoTask(artistId).Data;
                var taskResults = _listArtistAccountsTask.DoTask(artist);

                if (taskResults.Success)
                    return Ok(JsonSerialize(taskResults));

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route(Routes.ArtistAccount)]
        [HttpPut]
        [UserTypesAllowed(UserType.SystemAdministrator, UserType.LabelAdministrator, UserType.SystemUser)]
        [SystemUserRolesAllowed(SystemUserRoles.ArtistMember | SystemUserRoles.ArtistManager)]
        public IActionResult UpdateArtistAccount(int artistId, int artistAccountId, ArtistAccount artistAccount)
        {
            try
            {
                if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (!UserIsAuthorizedForArtist(artistId))
                    return Unauthorized();

                artistAccount.ArtistId = artistId;
                artistAccount.Id = artistAccountId;
                var taskResults = _updateArtistAccountTask.DoTask(artistAccount);

                return taskResults.Success ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route(Routes.ArtistAccount)]
        [HttpDelete]
        [UserTypesAllowed(UserType.SystemAdministrator, UserType.LabelAdministrator, UserType.SystemUser)]
        [SystemUserRolesAllowed(SystemUserRoles.ArtistMember | SystemUserRoles.ArtistManager)]
        public IActionResult RemoveArtistAccount(int artistId, int artistAccountId)
        {
            try
            {
                if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (!UserIsAuthorizedForArtist(artistId))
                    return Unauthorized();

                var toRemove = _getArtistAccountTask.DoTask(artistAccountId).Data;
                if (toRemove == null)
                    return Ok(JsonSerialize(false));

                if (toRemove.ArtistId == artistId)
                {
                    var taskResults = _removeArtistAccountTask.DoTask(toRemove);
                    if (taskResults.Success)
                        return Ok(JsonSerialize(true));
                }
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }

        [Route(Routes.ArtistLinks)]
        [HttpPost]
        [UserTypesAllowed(UserType.SystemAdministrator, UserType.LabelAdministrator, UserType.SystemUser)]
        [SystemUserRolesAllowed(SystemUserRoles.ArtistMember | SystemUserRoles.ArtistManager)]
        public IActionResult AddArtistLink(int artistId, ArtistLink artistLink)
        {
            try
            {
                if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (!UserIsAuthorizedForArtist(artistId))
                    return Unauthorized();

                artistLink.ArtistId = artistId;
                var taskResults = _addArtistLinkTask.DoTask(artistLink);

                if (taskResults.Success)
                    return Ok(JsonSerialize(taskResults));

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route(Routes.ArtistLinks)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult ListArtistLinks(int artistId)
        {
            try
            {
                if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var artist = _getArtistTask.DoTask(artistId).Data;
                var taskResults = _listArtistLinksTask.DoTask(artist);

                if (taskResults.Success)
                    return Ok(JsonSerialize(taskResults));

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route(Routes.ArtistLink)]
        [HttpDelete]
        [UserTypesAllowed(UserType.SystemAdministrator, UserType.LabelAdministrator, UserType.SystemUser)]
        [SystemUserRolesAllowed(SystemUserRoles.ArtistMember | SystemUserRoles.ArtistManager)]
        public IActionResult RemoveArtistLink(int artistId, int artistLinkId)
        {
            try
            {
                if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (!UserIsAuthorizedForArtist(artistId))
                    return Unauthorized();

                var toRemove = _getArtistLinkTask.DoTask(artistLinkId).Data;
                if (toRemove == null)
                    return Ok(JsonSerialize(false));

                if (toRemove.ArtistId == artistId)
                {
                    var taskResults = _removeArtistLinkTask.DoTask(toRemove);
                    if (taskResults.Success)
                        return Ok(JsonSerialize(true));
                }
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }

        private bool UserIsAuthorizedForArtist(int artistId)
        {
            switch (AuthenticatedUser.Type)
            {
                case UserType.SystemAdministrator:
                    return true;
                case UserType.PublisherAdministrator:
                case UserType.Unassigned:
                    return false;
            }

            var artistResult = _getArtistTask.DoTask(artistId);
            if (!artistResult.Success)
                throw artistResult.Exception;

            var artist = artistResult.Data;

            if (AuthenticatedUser.Type == UserType.LabelAdministrator && artist.RecordLabelId == AuthenticatedUser.RecordLabelId)
                return true;
            
            if (AuthenticatedUser.Type == UserType.SystemUser && AuthenticatedUser.Roles.HasFlag(SystemUserRoles.ArtistMember))
            {
                var artistMemberResults = _listArtistMembersTask.DoTask(artist);

                if (!artistMemberResults.Success) 
                    throw artistMemberResults.Exception;

                if (artistMemberResults.Data.Any(am => am.PersonId == AuthenticatedUser.PersonId))
                    return true;
            }

            if (AuthenticatedUser.Type == UserType.SystemUser && AuthenticatedUser.Roles.HasFlag(SystemUserRoles.ArtistManager))
            {
                var artistManagerResults = _listArtistManagersTask.DoTask(artist);

                if (!artistManagerResults.Success) 
                    throw artistManagerResults.Exception;

                if (artistManagerResults.Data.Any(am => am.PersonId == AuthenticatedUser.PersonId))
                    return true;
            }

            return false;
        }

        private void RedactArtistData(Artist artist)
        {
            switch (AuthenticatedUser.Type)
            {
                case UserType.SystemUser:
                    if (!AuthenticatedUser.Roles.HasFlag(SystemUserRoles.ArtistMember) ||
                        !AuthenticatedUser.Roles.HasFlag(SystemUserRoles.ArtistManager))
                    {
                        artist.TaxId = null;
                        break;
                    }
                    
                    if (AuthenticatedUser.Roles.HasFlag(SystemUserRoles.ArtistMember))
                    {
                        var artistMemberResults = _listArtistMembersTask.DoTask(artist);

                        if (!artistMemberResults.Success) 
                            throw artistMemberResults.Exception;

                        if (artistMemberResults.Data.All(am => am.PersonId != AuthenticatedUser.PersonId))
                            artist.TaxId = null;
                    }

                    if (AuthenticatedUser.Roles.HasFlag(SystemUserRoles.ArtistManager))
                    {
                        var artistManagerResults = _listArtistManagersTask.DoTask(artist);

                        if (!artistManagerResults.Success) 
                            throw artistManagerResults.Exception;

                        if (artistManagerResults.Data.All(am => am.PersonId != AuthenticatedUser.PersonId))
                            artist.TaxId = null;
                    }
                    break;
                case UserType.Unassigned:
                case UserType.PublisherAdministrator:
                case UserType.LabelAdministrator when artist.RecordLabelId != AuthenticatedUser.RecordLabelId:
                    artist.TaxId = null;
                    break;
            }
        }

        private void RedactPersonData(Person person, Artist artist)
        {
            if (AuthenticatedUser.PersonId == person.Id)
                return;
            
            switch (AuthenticatedUser.Type)
            {
                case UserType.SystemUser:
                    if (!AuthenticatedUser.Roles.HasFlag(SystemUserRoles.ArtistMember) ||
                        !AuthenticatedUser.Roles.HasFlag(SystemUserRoles.ArtistManager))
                    {
                        RemoveSensitivePersonData(person);
                        break;
                    }

                    if (AuthenticatedUser.Roles.HasFlag(SystemUserRoles.ArtistMember))
                    {
                        var artistMemberResults = _listArtistMembersTask.DoTask(artist);

                        if (!artistMemberResults.Success) 
                            throw artistMemberResults.Exception;

                        if (artistMemberResults.Data.All(am => am.PersonId != person.Id))
                            RemoveSensitivePersonData(person);
                    }

                    if (AuthenticatedUser.Roles.HasFlag(SystemUserRoles.ArtistManager))
                    {
                        var artistManagerResults = _listArtistManagersTask.DoTask(artist);

                        if (!artistManagerResults.Success) 
                            throw artistManagerResults.Exception;

                        if (artistManagerResults.Data.All(am => am.PersonId != person.Id))
                            RemoveSensitivePersonData(person);
                    }
                    break;
                case UserType.Unassigned:
                    RemoveSensitivePersonData(person);
                    break;
            }
        }

        private static void RemoveSensitivePersonData(Person person)
        {
            person.Email = null;
            person.Phone = null;
            person.Address = null;
            person.AddressId = null;
        }
    }
}

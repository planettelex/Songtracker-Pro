﻿using System;
using System.Linq;
using System.Reflection;
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
        #region Constructor

        public ArtistController(IGetLoginTask getLoginTask,
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
                                IUpdateArtistLinkTask updateArtistLinkTask,
                                IRemoveArtistLinkTask removeArtistLinkTask,
                                IListArtistMembersTask listArtistMembersTask,
                                IGetArtistMemberTask getArtistMemberTask,
                                IAddArtistMemberTask addArtistMemberTask,
                                IUpdateArtistMemberTask updateArtistMemberTask,
                                IListArtistManagersTask listArtistManagersTask,
                                IGetArtistManagerTask getArtistManagerTask,
                                IAddArtistManagerTask addArtistManagerTask,
                                IUpdateArtistManagerTask updateArtistManagerTask) : 
        base(getLoginTask)
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
            _updateArtistLinkTask = updateArtistLinkTask;
            _removeArtistLinkTask = removeArtistLinkTask;
            _listArtistMembersTask = listArtistMembersTask;
            _getArtistMemberTask = getArtistMemberTask;
            _addArtistMemberTask = addArtistMemberTask;
            _updateArtistMemberTask = updateArtistMemberTask;
            _listArtistManagersTask = listArtistManagersTask;
            _getArtistManagerTask = getArtistManagerTask;
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
        private readonly IUpdateArtistLinkTask _updateArtistLinkTask;
        private readonly IRemoveArtistLinkTask _removeArtistLinkTask;
        private readonly IListArtistMembersTask _listArtistMembersTask;
        private readonly IGetArtistMemberTask _getArtistMemberTask;
        private readonly IAddArtistMemberTask _addArtistMemberTask;
        private readonly IUpdateArtistMemberTask _updateArtistMemberTask;
        private readonly IListArtistManagersTask _listArtistManagersTask;
        private readonly IGetArtistManagerTask _getArtistManagerTask;
        private readonly IAddArtistManagerTask _addArtistManagerTask;
        private readonly IUpdateArtistManagerTask _updateArtistManagerTask;

        #endregion

        [Route(Routes.Artists)]
        [HttpPost]
        [UserTypesAllowed(UserType.SystemAdministrator, UserType.LabelAdministrator)]
        public IActionResult AddArtist(Artist artist)
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (AuthenticatedUser.Type == UserType.LabelAdministrator && AuthenticatedUser.RecordLabelId != artist.RecordLabelId)
                    return Unauthorized();

                var taskResults = _addArtistTask.DoTask(artist);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.Artists)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult ListArtists()
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var taskResults = _listArtistsTask.DoTask(null);

                if (!taskResults.Success) 
                    return Error(taskResults.Exception);

                var artists = taskResults.Data;
                foreach (var artist in artists)
                    RedactArtistData(artist);
            
                return Json(artists);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.Artist)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult GetArtist(int id)
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var taskResults = _getArtistTask.DoTask(id);

                if (taskResults.HasException)
                    return Error(taskResults.Exception);

                if (taskResults.HasNoData)
                    return NotFound();
                
                var artist = taskResults.Data;
                RedactArtistData(artist);

                return Json(artist);
            }
            catch (Exception e)
            {
                return Error(e);
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
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (!UserIsAuthorizedForArtist(id))
                    return Unauthorized();

                var invalidArtistPathResult = InvalidArtistPathResult(id);
                if (invalidArtistPathResult != null)
                    return invalidArtistPathResult;

                artist.Id = id;
                var taskResults = _updateArtistTask.DoTask(artist);

                return taskResults.Success ? 
                    Ok() : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
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
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (!UserIsAuthorizedForArtist(artistId))
                    return Unauthorized();

                var invalidArtistPathResult = InvalidArtistPathResult(artistId);
                if (invalidArtistPathResult != null)
                    return invalidArtistPathResult;

                artistMember.ArtistId = artistId;
                var taskResults = _addArtistMemberTask.DoTask(artistMember);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.ArtistMembers)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult ListArtistMembers(int artistId)
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var getArtistResults = _getArtistTask.DoTask(artistId);

                if (getArtistResults.HasException)
                    return Error(getArtistResults.Exception);

                if (getArtistResults.HasNoData)
                    return NotFound();

                var artist = getArtistResults.Data;
                var taskResults = _listArtistMembersTask.DoTask(artist);

                if (!taskResults.Success)
                    return Error(taskResults.Exception);

                var artistMembers = taskResults.Data;
                foreach (var artistMember in artistMembers)
                    RedactPersonData(artistMember.Member, artist);

                return Json(artistMembers);
            }
            catch (Exception e)
            {
                return Error(e);
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
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (!UserIsAuthorizedForArtist(artistId))
                    return Unauthorized();

                var invalidArtistPathResult = InvalidArtistPathResult(artistId);
                if (invalidArtistPathResult != null)
                    return invalidArtistPathResult;

                var getArtistMemberResult = _getArtistMemberTask.DoTask(artistMemberId);

                if (getArtistMemberResult.HasException)
                    return Error(getArtistMemberResult.Exception);

                if (getArtistMemberResult.HasNoData)
                    return NotFound();

                if (getArtistMemberResult.Data.ArtistId != artistId)
                    return BadRequest();

                artistMember.ArtistId = artistId;
                artistMember.Id = artistMemberId;
                var taskResults = _updateArtistMemberTask.DoTask(artistMember);

                return taskResults.Success ? 
                    Ok() : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
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
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (!UserIsAuthorizedForArtist(artistId))
                    return Unauthorized();

                var invalidArtistPathResult = InvalidArtistPathResult(artistId);
                if (invalidArtistPathResult != null)
                    return invalidArtistPathResult;

                artistManager.ArtistId = artistId;
                var taskResults = _addArtistManagerTask.DoTask(artistManager);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.ArtistManagers)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult ListArtistManagers(int artistId)
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var getArtistResults = _getArtistTask.DoTask(artistId);

                if (getArtistResults.HasException)
                    return Error(getArtistResults.Exception);

                if (getArtistResults.HasNoData)
                    return NotFound();

                var artist = getArtistResults.Data;
                var taskResults = _listArtistManagersTask.DoTask(artist);

                if (!taskResults.Success)
                    return Error(taskResults.Exception);

                var artistManagers = taskResults.Data;
                foreach (var artistManager in artistManagers)
                    RedactPersonData(artistManager.Manager, artist);

                return Json(artistManagers);
            }
            catch (Exception e)
            {
                return Error(e);
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
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (!UserIsAuthorizedForArtist(artistId))
                    return Unauthorized();

                var invalidArtistPathResult = InvalidArtistPathResult(artistId);
                if (invalidArtistPathResult != null)
                    return invalidArtistPathResult;

                var getArtistManagerResult = _getArtistManagerTask.DoTask(artistManagerId);

                if (getArtistManagerResult.HasException)
                    return Error(getArtistManagerResult.Exception);

                if (getArtistManagerResult.HasNoData)
                    return NotFound();

                if (getArtistManagerResult.Data.ArtistId != artistId)
                    return BadRequest();

                artistManager.ArtistId = artistId;
                artistManager.Id = artistManagerId;
                var taskResults = _updateArtistManagerTask.DoTask(artistManager);

                return taskResults.Success ? 
                    Ok() : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
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
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (!UserIsAuthorizedForArtist(artistId))
                    return Unauthorized();

                var invalidArtistPathResult = InvalidArtistPathResult(artistId);
                if (invalidArtistPathResult != null)
                    return invalidArtistPathResult;

                artistAccount.ArtistId = artistId;
                var taskResults = _addArtistAccountTask.DoTask(artistAccount);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
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
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (!UserIsAuthorizedForArtist(artistId))
                    return Unauthorized();

                var getArtistResults = _getArtistTask.DoTask(artistId);

                if (getArtistResults.HasException)
                    return Error(getArtistResults.Exception);

                if (getArtistResults.HasNoData)
                    return NotFound();

                var artist = getArtistResults.Data;
                var taskResults = _listArtistAccountsTask.DoTask(artist);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
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
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (!UserIsAuthorizedForArtist(artistId))
                    return Unauthorized();

                var invalidArtistPathResult = InvalidArtistPathResult(artistId);
                if (invalidArtistPathResult != null)
                    return invalidArtistPathResult;

                var getArtistAccountResult = _getArtistAccountTask.DoTask(artistAccountId);

                if (getArtistAccountResult.HasException)
                    return Error(getArtistAccountResult.Exception);

                if (getArtistAccountResult.HasNoData)
                    return NotFound();

                if (getArtistAccountResult.Data.ArtistId != artistId)
                    return BadRequest();

                artistAccount.ArtistId = artistId;
                artistAccount.Id = artistAccountId;
                var taskResults = _updateArtistAccountTask.DoTask(artistAccount);

                return taskResults.Success ? 
                    Ok() : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
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
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (!UserIsAuthorizedForArtist(artistId))
                    return Unauthorized();

                var invalidArtistPathResult = InvalidArtistPathResult(artistId);
                if (invalidArtistPathResult != null)
                    return invalidArtistPathResult;

                var getArtistAccountResult = _getArtistAccountTask.DoTask(artistAccountId);

                if (getArtistAccountResult.HasException)
                    return Error(getArtistAccountResult.Exception);

                if (getArtistAccountResult.HasNoData)
                    return NotFound();

                if (getArtistAccountResult.Data.ArtistId != artistId)
                    return BadRequest();

                var toRemove = getArtistAccountResult.Data;
                if (toRemove == null)
                    return Json(false);

                var taskResults = _removeArtistAccountTask.DoTask(toRemove);
                
                return taskResults.Success ? 
                    Json(true) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
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
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (!UserIsAuthorizedForArtist(artistId))
                    return Unauthorized();

                var invalidArtistPathResult = InvalidArtistPathResult(artistId);
                if (invalidArtistPathResult != null)
                    return invalidArtistPathResult;

                artistLink.ArtistId = artistId;
                var taskResults = _addArtistLinkTask.DoTask(artistLink);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.ArtistLinks)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult ListArtistLinks(int artistId)
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var getArtistResults = _getArtistTask.DoTask(artistId);

                if (getArtistResults.HasException)
                    return Error(getArtistResults.Exception);

                if (getArtistResults.HasNoData)
                    return NotFound();

                var artist = getArtistResults.Data;
                var taskResults = _listArtistLinksTask.DoTask(artist);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.ArtistLink)]
        [HttpPut]
        [UserTypesAllowed(UserType.SystemAdministrator, UserType.LabelAdministrator, UserType.SystemUser)]
        [SystemUserRolesAllowed(SystemUserRoles.ArtistMember | SystemUserRoles.ArtistManager)]
        public IActionResult UpdateArtistLink(int artistId, int artistLinkId, ArtistLink artistLink)
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (!UserIsAuthorizedForArtist(artistId))
                    return Unauthorized();

                var invalidArtistPathResult = InvalidArtistPathResult(artistId);
                if (invalidArtistPathResult != null)
                    return invalidArtistPathResult;

                var getArtistLinkResult = _getArtistLinkTask.DoTask(artistLinkId);

                if (getArtistLinkResult.HasException)
                    return Error(getArtistLinkResult.Exception);

                if (getArtistLinkResult.HasNoData)
                    return NotFound();

                if (getArtistLinkResult.Data.ArtistId != artistId)
                    return BadRequest();

                artistLink.ArtistId = artistId;
                artistLink.Id = artistLinkId;
                var taskResults = _updateArtistLinkTask.DoTask(artistLink);

                return taskResults.Success ? 
                    Ok() : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
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
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (!UserIsAuthorizedForArtist(artistId))
                    return Unauthorized();

                var invalidArtistPathResult = InvalidArtistPathResult(artistId);
                if (invalidArtistPathResult != null)
                    return invalidArtistPathResult;

                var getArtistLinkResult = _getArtistLinkTask.DoTask(artistLinkId);

                if (getArtistLinkResult.HasException)
                    return Error(getArtistLinkResult.Exception);

                if (getArtistLinkResult.HasNoData)
                    return NotFound();

                if (getArtistLinkResult.Data.ArtistId != artistId)
                    return BadRequest();

                var toRemove = getArtistLinkResult.Data;
                if (toRemove == null)
                    return Json(false);

                var taskResults = _removeArtistLinkTask.DoTask(toRemove);

                return taskResults.Success ? 
                    Json(true) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        #region Private

        private IActionResult InvalidArtistPathResult(int artistId)
        {
            var getArtistResult = _getArtistTask.DoTask(artistId);

            if (getArtistResult.HasException)
                return Error(getArtistResult.Exception);

            if (getArtistResult.HasNoData)
                return NotFound();

            return null;
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

        #endregion
    }
}

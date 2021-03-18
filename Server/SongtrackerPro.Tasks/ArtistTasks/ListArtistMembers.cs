using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IListArtistMembersTask : ITask<Artist, List<ArtistMember>> { }

    public class ListArtistMembers : IListArtistMembersTask
    {
        public ListArtistMembers(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<ArtistMember>> DoTask(Artist artist)
        {
            try
            {
                var artistMembers = _dbContext.ArtistMembers.Where(am => am.ArtistId == artist.Id)
                    .Include(am => am.Member).ThenInclude(m => m.Address).ThenInclude(a => a.Country)
                    .ToList();

                return new TaskResult<List<ArtistMember>>(artistMembers);
            }
            catch (Exception e)
            {
                return new TaskResult<List<ArtistMember>>(new TaskException(e));
            }
        }
    }
}
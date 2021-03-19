using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IGetArtistMemberTask : ITask<int, ArtistMember> { }

    public class GetArtistMember : TaskBase, IGetArtistMemberTask
    {
        public GetArtistMember(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<ArtistMember> DoTask(int artistMemberId)
        {
            try
            {
                var artistMember = _dbContext.ArtistMembers.Where(am => am.Id == artistMemberId)
                    .Include(am => am.Member).ThenInclude(m => m.Address).ThenInclude(a => a.Country)
                    .SingleOrDefault();

                return new TaskResult<ArtistMember>(artistMember);
            }
            catch (Exception e)
            {
                return new TaskResult<ArtistMember>(new TaskException(e));
            }
        }
    }
}

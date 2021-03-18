using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IGetArtistMemberTask : ITask<int, ArtistMember> { }

    public class GetArtistMember : IGetArtistMemberTask
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
                var artistMember = _dbContext.ArtistMembers.SingleOrDefault(am => am.Id == artistMemberId);

                return new TaskResult<ArtistMember>(artistMember);
            }
            catch (Exception e)
            {
                return new TaskResult<ArtistMember>(new TaskException(e));
            }
        }
    }
}

using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IUpdateArtistMemberTask : ITask<ArtistMember, Nothing> { }

    public class UpdateArtistMember : TaskBase, IUpdateArtistMemberTask
    {
        public UpdateArtistMember(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Nothing> DoTask(ArtistMember update)
        {
            try
            {
                var artistMember = _dbContext.ArtistMembers.SingleOrDefault(am => am.Id == update.Id);
                if (artistMember == null)
                    throw new TaskException(SystemMessage("ARTIST_MEMBER_NOT_FOUND"));

                var hasEnded = update.EndedOn.HasValue && update.EndedOn.Value <= DateTime.Today;
                artistMember.StartedOn = update.StartedOn;
                artistMember.EndedOn = update.EndedOn;
                artistMember.IsActive = !hasEnded;
                _dbContext.SaveChanges();

                return new TaskResult<Nothing>(null as Nothing);
            }
            catch (Exception e)
            {
                return new TaskResult<Nothing>(new TaskException(e));
            }
        }
    }
}

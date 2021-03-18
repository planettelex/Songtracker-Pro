using System;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IAddArtistMemberTask : ITask<ArtistMember, int?> { }

    public class AddArtistMember : IAddArtistMemberTask
    {
        public AddArtistMember(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(ArtistMember artistMember)
        {
            try
            {
                var artistId = artistMember.Artist?.Id ?? artistMember.ArtistId;
                var personId = artistMember.Member?.Id ?? artistMember.PersonId;
                var hasEnded = artistMember.EndedOn.HasValue && artistMember.EndedOn.Value <= DateTime.Today;

                artistMember.Artist = null;
                artistMember.ArtistId = artistId;
                artistMember.Member = null;
                artistMember.PersonId = personId;
                artistMember.IsActive = !hasEnded;

                _dbContext.ArtistMembers.Add(artistMember);
                _dbContext.SaveChanges();

                return new TaskResult<int?>(artistMember.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}

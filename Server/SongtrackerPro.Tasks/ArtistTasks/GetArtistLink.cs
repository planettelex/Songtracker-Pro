using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IGetArtistLinkTask : ITask<int, ArtistLink> { }

    public class GetArtistLink : IGetArtistLinkTask
    {
        public GetArtistLink(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<ArtistLink> DoTask(int artistLinkId)
        {
            try
            {
                var artistLink = _dbContext.ArtistLinks.SingleOrDefault(al => al.Id == artistLinkId);

                return new TaskResult<ArtistLink>(artistLink);
            }
            catch (Exception e)
            {
                return new TaskResult<ArtistLink>(new TaskException(e));
            }
        }
    }
}

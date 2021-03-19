using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IGetArtistTask : ITask<int, Artist> { }

    public class GetArtist : TaskBase, IGetArtistTask
    {
        public GetArtist(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Artist> DoTask(int artistId)
        {
            try
            {
                var artist = _dbContext.Artists.Where(a => a.Id == artistId)
                    .Include(a => a.RecordLabel).ThenInclude(l => l.Address).ThenInclude(a => a.Country)
                    .SingleOrDefault();

                return new TaskResult<Artist>(artist);
            }
            catch (Exception e)
            {
                return new TaskResult<Artist>(new TaskException(e));
            }
        }
    }
}

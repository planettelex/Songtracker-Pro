using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IGetArtistManagerTask : ITask<int, ArtistManager> { }

    public class GetArtistManager : TaskBase,  IGetArtistManagerTask
    {
        public GetArtistManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<ArtistManager> DoTask(int artistManagerId)
        {
            try
            {
                var artistManager = _dbContext.ArtistManagers.Where(am => am.Id == artistManagerId)
                    .Include(am => am.Manager).ThenInclude(m => m.Address).ThenInclude(a => a.Country)
                    .SingleOrDefault();

                return new TaskResult<ArtistManager>(artistManager);
            }
            catch (Exception e)
            {
                return new TaskResult<ArtistManager>(new TaskException(e));
            }
        }
    }
}

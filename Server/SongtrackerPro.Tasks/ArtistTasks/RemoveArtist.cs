using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IRemoveArtistTask : ITask<Artist, bool> { }

    public class RemoveArtist : TaskBase, IRemoveArtistTask
    {
        public RemoveArtist(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(Artist artist)
        {
            try
            {
                var toRemove = _dbContext.Artists.SingleOrDefault(a => a.Id == artist.Id);
                if (toRemove == null)
                    return new TaskResult<bool>(false);

                var members = _dbContext.ArtistMembers.Where(am => am.ArtistId == artist.Id).ToList();
                _dbContext.ArtistMembers.RemoveRange(members);
                _dbContext.SaveChanges();

                var managers = _dbContext.ArtistManagers.Where(am => am.ArtistId == artist.Id).ToList();
                _dbContext.ArtistManagers.RemoveRange(managers);
                _dbContext.SaveChanges();

                var addressId = toRemove.AddressId;
                _dbContext.Artists.Remove(toRemove);
                _dbContext.SaveChanges();

                if (addressId.HasValue)
                {
                    var addressToRemove = _dbContext.Addresses.SingleOrDefault(a => a.Id == addressId.Value);
                    if (addressToRemove != null)
                    {
                        _dbContext.Addresses.Remove(addressToRemove);
                        _dbContext.SaveChanges();
                    }
                }

                return new TaskResult<bool>(true);
            }
            catch (Exception e)
            {
                return new TaskResult<bool>(new TaskException(e));
            }
        }
    }
}

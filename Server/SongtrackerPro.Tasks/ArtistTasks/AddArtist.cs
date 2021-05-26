using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IAddArtistTask : ITask<Artist, int?> { }

    public class AddArtist : TaskBase, IAddArtistTask
    {
        public AddArtist(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(Artist artist)
        {
            try
            {
                var address = artist.Address;
                var countryId = address.Country?.Id ?? address.CountryId;
                var country = _dbContext.Countries.SingleOrDefault(c => c.Id == countryId);
                address.Country = country;
                _dbContext.Addresses.Add(address);
                _dbContext.SaveChanges();

                var recordLabelId = artist.RecordLabel?.Id ?? artist.RecordLabelId;

                artist.Address = null;
                artist.AddressId = address.Id;
                artist.RecordLabel = null;
                artist.RecordLabelId = recordLabelId;

                _dbContext.Artists.Add(artist);
                _dbContext.SaveChanges();

                artist.Address = address;
                artist.RecordLabel = recordLabelId > 0 ? 
                    _dbContext.RecordLabels.Where(r => r.Id == recordLabelId)
                    .Include(r => r.Address).ThenInclude(a => a.Country)
                    .SingleOrDefault() : null;

                return new TaskResult<int?>(artist.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}

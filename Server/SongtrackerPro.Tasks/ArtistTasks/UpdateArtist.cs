using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IUpdateArtistTask : ITask<Artist, Nothing> { }

    public class UpdateArtist : TaskBase, IUpdateArtistTask
    {
        public UpdateArtist(ApplicationDbContext dbContext, IFormattingService formattingService)
        {
            _dbContext = dbContext;
            _formattingService = formattingService;
        }
        private readonly ApplicationDbContext _dbContext;
        private readonly IFormattingService _formattingService;

        public TaskResult<Nothing> DoTask(Artist update)
        {
            try
            {
                var artist = _dbContext.Artists
                    .SingleOrDefault(a => a.Id == update.Id);

                if (artist == null)
                    throw new TaskException(SystemMessage("ARTIST_NOT_FOUND"));

                artist.Name = update.Name;
                artist.TaxId = _formattingService.FormatTaxId(update.TaxId);
                artist.Email = string.IsNullOrWhiteSpace(update.Email) ? null : update.Email;
                artist.HasServiceMark = update.HasServiceMark;
                artist.WebsiteUrl = string.IsNullOrWhiteSpace(update.WebsiteUrl) ? null : update.WebsiteUrl;
                artist.PressKitUrl = string.IsNullOrWhiteSpace(update.PressKitUrl) ? null : update.PressKitUrl;

                if (update.Address != null)
                {
                    if (artist.Address == null)
                    {
                        var address = update.Address;
                        var countryId = address.Country?.Id ?? address.CountryId;
                        var country = _dbContext.Countries.SingleOrDefault(c => c.Id == countryId);
                        address.Country = country;
                        _dbContext.Addresses.Add(address);
                        _dbContext.SaveChanges();
                        artist.Address = address;
                    }
                    artist.Address.Street = update.Address.Street;
                    artist.Address.City = update.Address.City;
                    artist.Address.Region = update.Address.Region;
                    artist.Address.PostalCode = update.Address.PostalCode;

                    artist.Address.CountryId = update.Address.Country?.Id;
                    if (artist.Address.CountryId.HasValue)
                    {
                        var country = _dbContext.Countries.SingleOrDefault(c => c.Id == artist.Address.CountryId);
                        artist.Address.Country = country ?? throw new TaskException(SystemMessage("COUNTRY_NOT_FOUND"));
                    }
                }

                artist.RecordLabelId = update.RecordLabel?.Id;
                if (artist.RecordLabelId.HasValue)
                {
                    var recordLabel = _dbContext.RecordLabels.SingleOrDefault(l => l.Id == artist.RecordLabelId);
                    artist.RecordLabel = recordLabel ?? throw new TaskException(SystemMessage("RECORD_LABEL_NOT_FOUND"));
                }

                _dbContext.SaveChanges();

                return new TaskResult<Nothing>(true);
            }
            catch (Exception e)
            {
                return new TaskResult<Nothing>(new TaskException(e));
            }
        }
    }
}

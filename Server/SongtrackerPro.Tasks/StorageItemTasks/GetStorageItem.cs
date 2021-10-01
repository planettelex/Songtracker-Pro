using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.StorageItemTasks
{
    public interface IGetStorageItemTask : ITask<Guid, StorageItem> { }

    public class GetStorageItem : TaskBase, IGetStorageItemTask
    {
        public GetStorageItem(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<StorageItem> DoTask(Guid storageItemId)
        {
            try
            {
                var recordLabelContract = _dbContext.RecordLabelContracts.Where(rlc => rlc.Uuid == storageItemId)
                    .Include(rlc => rlc.Release).ThenInclude(r => r.Artist)
                    .Include(rlc => rlc.Release).ThenInclude(r => r.Genre)
                    .Include(rlc => rlc.Release).ThenInclude(r => r.RecordLabel)
                    .Include(rlc => rlc.Recording).ThenInclude(r => r.Composition).ThenInclude(c => c.Publisher)
                    .Include(rlc => rlc.Recording).ThenInclude(r => r.Composition).ThenInclude(c => c.ExternalPublisher)
                    .Include(rlc => rlc.Recording).ThenInclude(r => r.Artist)
                    .Include(rlc => rlc.Recording).ThenInclude(r => r.Genre)
                    .Include(rlc => rlc.Recording).ThenInclude(r => r.RecordLabel)
                    .Include(rlc => rlc.Recording).ThenInclude(r => r.OriginalRecording)
                    .Include(rlc => rlc.Artist)
                    .Include(rlc => rlc.RecordLabel)
                    .Include(rlc => rlc.Template)
                    .SingleOrDefault();

                if (recordLabelContract != null)
                    return new TaskResult<StorageItem>(recordLabelContract);

                var publisherContract = _dbContext.PublisherContracts.Where(pc => pc.Uuid == storageItemId)
                    .Include(pc => pc.Publication).ThenInclude(p => p.Publisher).ThenInclude(p => p.PerformingRightsOrganization)
                    .Include(pc => pc.Composition).ThenInclude(c => c.Publisher).ThenInclude(p => p.PerformingRightsOrganization)
                    .Include(pc => pc.Composition).ThenInclude(c => c.ExternalPublisher)
                    .Include(pc => pc.Artist)
                    .Include(pc => pc.Publisher).ThenInclude(p => p.PerformingRightsOrganization)
                    .Include(pc => pc.Template)
                    .SingleOrDefault();

                if (publisherContract != null)
                    return new TaskResult<StorageItem>(publisherContract);

                var contract = _dbContext.Contracts.Where(pc => pc.Uuid == storageItemId)
                    .Include(c => c.Artist)
                    .Include(c => c.Publisher).ThenInclude(p => p.PerformingRightsOrganization)
                    .Include(c => c.RecordLabel)
                    .Include(c => c.Template)
                    .SingleOrDefault();

                if (contract != null)
                    return new TaskResult<StorageItem>(contract);

                var document = _dbContext.Documents.Where(d => d.Uuid == storageItemId)
                    .Include(d => d.Artist)
                    .Include(d => d.Publisher).ThenInclude(p => p.PerformingRightsOrganization)
                    .Include(d => d.RecordLabel)
                    .SingleOrDefault();

                if (document != null)
                    return new TaskResult<StorageItem>(document);

                var digitalMedia = _dbContext.DigitalMedia.Where(d => d.Uuid == storageItemId)
                    .Include(d => d.Artist)
                    .Include(d => d.Publisher).ThenInclude(p => p.PerformingRightsOrganization)
                    .Include(d => d.RecordLabel)
                    .SingleOrDefault();

                if (digitalMedia != null)
                    return new TaskResult<StorageItem>(digitalMedia);

                var storageItem = _dbContext.StorageItems.Where(si => si.Uuid == storageItemId)
                    .Include(si => si.Artist)
                    .Include(si => si.Publisher).ThenInclude(p => p.PerformingRightsOrganization)
                    .Include(si => si.RecordLabel)
                    .SingleOrDefault();

                return new TaskResult<StorageItem>(storageItem);
            }
            catch (Exception e)
            {
                return new TaskResult<StorageItem>(new TaskException(e));
            }
        }
    }
}

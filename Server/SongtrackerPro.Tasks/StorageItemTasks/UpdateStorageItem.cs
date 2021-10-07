using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.StorageItemTasks
{
    public interface IUpdateStorageItemTask : ITask<StorageItem, Nothing> { }

    public class UpdateStorageItem : TaskBase, IUpdateStorageItemTask
    {
        public UpdateStorageItem(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Nothing> DoTask(StorageItem update)
        {
            try
            {
                var storageItem = _dbContext.StorageItems.Single(si => si.Uuid == update.Uuid);

                storageItem.Name = update.Name;
                storageItem.Container = update.Container;
                storageItem.FileName = update.FileName;
                storageItem.FolderPath = update.FolderPath;
                storageItem.UpdatedOn = DateTime.UtcNow;
                _dbContext.SaveChanges();

                if (update is RecordLabelContract recordLabelContractUpdate)
                {
                    var recordLabelContract = _dbContext.RecordLabelContracts.Single(rlc => rlc.Uuid == recordLabelContractUpdate.Uuid);
                    recordLabelContract.RecordingId = recordLabelContractUpdate.Recording?.Id ?? recordLabelContractUpdate.RecordingId;
                    recordLabelContract.Recording = _dbContext.Recordings.SingleOrDefault(r => r.Id == recordLabelContract.RecordingId);
                    recordLabelContract.ReleaseId = recordLabelContractUpdate.Release?.Id ?? recordLabelContractUpdate.ReleaseId;
                    recordLabelContract.Release = _dbContext.Releases.SingleOrDefault(r => r.Id == recordLabelContract.ReleaseId);
                    recordLabelContract.ProvidedById = recordLabelContractUpdate.ProvidedBy?.Id ?? recordLabelContractUpdate.ProvidedById;
                    recordLabelContract.ProvidedBy = _dbContext.LegalEntities.SingleOrDefault(le => le.Id == recordLabelContract.ProvidedById);
                    recordLabelContract.TemplateId = recordLabelContractUpdate.Template?.TemplateId ?? recordLabelContractUpdate.TemplateId;
                    recordLabelContract.Template = _dbContext.Contracts.SingleOrDefault(c => c.Uuid == recordLabelContract.TemplateId);
                    recordLabelContract.ContractStatus = recordLabelContractUpdate.ContractStatus;
                    recordLabelContract.DraftedOn = recordLabelContractUpdate.DraftedOn;
                    recordLabelContract.ProvidedOn = recordLabelContractUpdate.ProvidedOn;
                    recordLabelContract.ProposedOn = recordLabelContractUpdate.ProposedOn;
                    recordLabelContract.ExecutedOn = recordLabelContractUpdate.ExecutedOn;
                    recordLabelContract.RejectedOn = recordLabelContractUpdate.RejectedOn;
                    recordLabelContract.ExpiredOn = recordLabelContractUpdate.ExpiredOn;
                    recordLabelContract.PromiseePartyType = recordLabelContractUpdate.PromiseePartyType;
                    recordLabelContract.PromisorPartyType = recordLabelContractUpdate.PromisorPartyType;
                    recordLabelContract.IsTemplate = recordLabelContractUpdate.IsTemplate;
                    recordLabelContract.Version = recordLabelContractUpdate.Version;
                    _dbContext.SaveChanges();
                }
                else if (update is PublisherContract publisherContractUpdate)
                {
                    var publisherContract = _dbContext.PublisherContracts.Single(rlc => rlc.Uuid == publisherContractUpdate.Uuid);
                    publisherContract.PublicationId = publisherContractUpdate.Publication?.Id ?? publisherContractUpdate.PublicationId;
                    publisherContract.Publication = _dbContext.Publications.SingleOrDefault(p => p.Id == publisherContract.PublicationId);
                    publisherContract.CompositionId = publisherContractUpdate.Composition?.Id ?? publisherContractUpdate.CompositionId;
                    publisherContract.Composition = _dbContext.Compositions.SingleOrDefault(c => c.Id == publisherContract.CompositionId);
                    publisherContract.ProvidedById = publisherContractUpdate.ProvidedBy?.Id ?? publisherContractUpdate.ProvidedById;
                    publisherContract.ProvidedBy = _dbContext.LegalEntities.SingleOrDefault(le => le.Id == publisherContract.ProvidedById);
                    publisherContract.TemplateId = publisherContractUpdate.Template?.TemplateId ?? publisherContractUpdate.TemplateId;
                    publisherContract.Template = _dbContext.Contracts.SingleOrDefault(c => c.Uuid == publisherContract.TemplateId);
                    publisherContract.ContractStatus = publisherContractUpdate.ContractStatus;
                    publisherContract.DraftedOn = publisherContractUpdate.DraftedOn;
                    publisherContract.ProvidedOn = publisherContractUpdate.ProvidedOn;
                    publisherContract.ProposedOn = publisherContractUpdate.ProposedOn;
                    publisherContract.ExecutedOn = publisherContractUpdate.ExecutedOn;
                    publisherContract.RejectedOn = publisherContractUpdate.RejectedOn;
                    publisherContract.ExpiredOn = publisherContractUpdate.ExpiredOn;
                    publisherContract.PromiseePartyType = publisherContractUpdate.PromiseePartyType;
                    publisherContract.PromisorPartyType = publisherContractUpdate.PromisorPartyType;
                    publisherContract.IsTemplate = publisherContractUpdate.IsTemplate;
                    publisherContract.Version = publisherContractUpdate.Version;
                    _dbContext.SaveChanges();
                }
                else if (update is Contract contractUpdate)
                {
                    var contract = _dbContext.Contracts.Single(c => c.Uuid == contractUpdate.Uuid);
                    contract.ProvidedById = contractUpdate.ProvidedBy?.Id ?? contractUpdate.ProvidedById;
                    contract.ProvidedBy = _dbContext.LegalEntities.SingleOrDefault(le => le.Id == contract.ProvidedById);
                    contract.TemplateId = contractUpdate.Template?.TemplateId ?? contractUpdate.TemplateId;
                    contract.Template = _dbContext.Contracts.SingleOrDefault(c => c.Uuid == contract.TemplateId);
                    contract.ContractStatus = contractUpdate.ContractStatus;
                    contract.DraftedOn = contractUpdate.DraftedOn;
                    contract.ProvidedOn = contractUpdate.ProvidedOn;
                    contract.ProposedOn = contractUpdate.ProposedOn;
                    contract.ExecutedOn = contractUpdate.ExecutedOn;
                    contract.RejectedOn = contractUpdate.RejectedOn;
                    contract.ExpiredOn = contractUpdate.ExpiredOn;
                    contract.PromiseePartyType = contractUpdate.PromiseePartyType;
                    contract.PromisorPartyType = contractUpdate.PromisorPartyType;
                    contract.IsTemplate = contractUpdate.IsTemplate;
                    contract.Version = contractUpdate.Version;
                    _dbContext.SaveChanges();
                }
                else if (update is Document documentUpdate)
                {
                    var document = _dbContext.Documents.Single(d => d.Uuid == documentUpdate.Uuid);
                    document.DocumentType = documentUpdate.DocumentType;
                    document.Version = documentUpdate.Version;
                    _dbContext.SaveChanges();
                }
                else if (update is DigitalMedia digitalMediaUpdate)
                {
                    var digitalMedia = _dbContext.DigitalMedia.Single(dm => dm.Uuid == digitalMediaUpdate.Uuid);
                    digitalMedia.IsCompressed = digitalMediaUpdate.IsCompressed;
                    digitalMedia.MediaCategory = digitalMediaUpdate.MediaCategory;
                    _dbContext.SaveChanges();
                }
            
                return new TaskResult<Nothing>(true);
            }
            catch (Exception e)
            {
                return new TaskResult<Nothing>(new TaskException(e));
            }
        }
    }
}

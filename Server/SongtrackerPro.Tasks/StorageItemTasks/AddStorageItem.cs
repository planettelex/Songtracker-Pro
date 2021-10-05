using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.StorageItemTasks
{
    public interface IAddStorageItemTask : ITask<StorageItem, Guid?> { }

    public class AddStorageItem : TaskBase, IAddStorageItemTask
    {
        public AddStorageItem(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Guid?> DoTask(StorageItem storageItem)
        {
            try
            {
                var artistId = storageItem.Artist?.Id ?? storageItem.ArtistId;
                var artist = _dbContext.Artists.SingleOrDefault(a => a.Id == artistId);
                var recordLabelId = storageItem.RecordLabel?.Id ?? storageItem.RecordLabelId;
                var recordLabel = _dbContext.RecordLabels.SingleOrDefault(a => a.Id == artistId);
                var publisherId = storageItem.Publisher?.Id ?? storageItem.PublisherId;
                var publisher = _dbContext.Publishers.SingleOrDefault(p => p.Id == publisherId);

                storageItem.Artist = null;
                storageItem.ArtistId = artistId;
                storageItem.RecordLabel = null;
                storageItem.RecordLabelId = recordLabelId;
                storageItem.Publisher = null;
                storageItem.PublisherId = publisherId;
                storageItem.Uuid = Guid.NewGuid();
                storageItem.CreatedOn = DateTime.UtcNow;
                
                if (storageItem is RecordLabelContract recordLabelContract)
                {
                    var legalEntityId = recordLabelContract.ProvidedById;
                    var legalEntity = _dbContext.LegalEntities.SingleOrDefault(le => le.Id == legalEntityId);
                    var templateId = recordLabelContract.Template?.Uuid ?? recordLabelContract.TemplateId;
                    var template = _dbContext.Contracts.SingleOrDefault(c => c.Uuid == templateId);
                    var releaseId = recordLabelContract.Release?.Id ?? recordLabelContract.ReleaseId;
                    var release = _dbContext.Releases.SingleOrDefault(r => r.Id == releaseId);
                    var recordingId = recordLabelContract.Recording?.Id ?? recordLabelContract.RecordingId;
                    var recording = _dbContext.Recordings.SingleOrDefault(r => r.Id == recordingId);

                    recordLabelContract.ProvidedBy = null;
                    recordLabelContract.ProvidedById = legalEntityId;
                    recordLabelContract.Template = null;
                    recordLabelContract.TemplateId = templateId;
                    recordLabelContract.Release = null;
                    recordLabelContract.ReleaseId = releaseId;
                    recordLabelContract.Recording = null;
                    recordLabelContract.RecordingId = recordingId;

                    _dbContext.RecordLabelContracts.Add(recordLabelContract);
                    _dbContext.SaveChanges();

                    recordLabelContract.ProvidedBy = legalEntity;
                    recordLabelContract.Template = template;
                    recordLabelContract.Release = release;
                    recordLabelContract.Recording = recording;
                }
                else if (storageItem is PublisherContract publisherContract)
                {
                    var legalEntityId = publisherContract.ProvidedById;
                    var legalEntity = _dbContext.LegalEntities.SingleOrDefault(le => le.Id == legalEntityId);
                    var templateId = publisherContract.Template?.Uuid ?? publisherContract.TemplateId;
                    var template = _dbContext.Contracts.SingleOrDefault(c => c.Uuid == templateId);
                    var compositionId = publisherContract.Composition?.Id ?? publisherContract.CompositionId;
                    var composition = _dbContext.Compositions.SingleOrDefault(c => c.Id == compositionId);
                    var publicationId = publisherContract.Publication?.Id ?? publisherContract.PublicationId;
                    var publication = _dbContext.Publications.SingleOrDefault(p => p.Id == publicationId);

                    publisherContract.ProvidedBy = null;
                    publisherContract.ProvidedById = legalEntityId;
                    publisherContract.Template = null;
                    publisherContract.TemplateId = templateId;
                    publisherContract.Composition = null;
                    publisherContract.CompositionId = compositionId;
                    publisherContract.Publication = null;
                    publisherContract.PublicationId = publicationId;

                    _dbContext.PublisherContracts.Add(publisherContract);
                    _dbContext.SaveChanges();

                    publisherContract.ProvidedBy = legalEntity;
                    publisherContract.Template = template;
                    publisherContract.Composition = composition;
                    publisherContract.Publication = publication;
                }
                else if (storageItem is Contract contract)
                {
                    var legalEntityId = contract.ProvidedBy?.Id ?? contract.ProvidedById;
                    var legalEntity = _dbContext.LegalEntities.SingleOrDefault(le => le.Id == legalEntityId);
                    var templateId = contract.Template?.Uuid ?? contract.TemplateId;
                    var template = _dbContext.Contracts.SingleOrDefault(c => c.Uuid == templateId);
                    var parties = contract.Parties;

                    contract.ProvidedBy = null;
                    contract.ProvidedById = legalEntityId;
                    contract.Template = null;
                    contract.TemplateId = templateId;
                    contract.Parties = null;

                    _dbContext.Contracts.Add(contract);
                    _dbContext.SaveChanges();

                    if (parties != null && parties.Any())
                    {
                        foreach (var contractParty in parties)
                        {
                            var contractPartyLegalEntityId = contractParty.LegalEntity?.Id ?? contractParty.LegalEntityId;
                            var contractPartyLegalEntity = _dbContext.LegalEntities.SingleOrDefault(le => le.Id == legalEntityId);

                            contractParty.ContractId = contract.Uuid;
                            contractParty.Contract = null;
                            contractParty.LegalEntityId = contractPartyLegalEntityId;
                            contractParty.LegalEntity = null;

                            _dbContext.ContractParties.Add(contractParty);
                            _dbContext.SaveChanges();

                            contractParty.LegalEntity = contractPartyLegalEntity;
                        }
                    }

                    contract.ProvidedBy = legalEntity;
                    contract.Template = template;
                }
                else if (storageItem is Document document)
                {
                    _dbContext.Documents.Add(document);
                    _dbContext.SaveChanges();
                }
                else if (storageItem is DigitalMedia digitalMedia)
                {
                    _dbContext.DigitalMedia.Add(digitalMedia);
                    _dbContext.SaveChanges();
                }
                else
                {
                    _dbContext.StorageItems.Add(storageItem);
                    _dbContext.SaveChanges();
                }

                storageItem.Artist = artist;
                storageItem.RecordLabel = recordLabel;
                storageItem.Publisher = publisher;

                return new TaskResult<Guid?>(storageItem.Uuid);
            }
            catch (Exception e)
            {
                return new TaskResult<Guid?>(new TaskException(e));
            }
        }
    }
}

using System;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.LegalEntityTasks
{
    public interface IAddLegalEntityContactTask : ITask<LegalEntityContact, int?> { }

    public class AddLegalEntityContact : TaskBase, IAddLegalEntityContactTask
    {
        public AddLegalEntityContact(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(LegalEntityContact legalEntityContact)
        {
            try
            {
                var legalEntityId = legalEntityContact.LegalEntity?.Id ?? legalEntityContact.LegalEntityId;
                var contactId = legalEntityContact.Contact?.Id ?? legalEntityContact.ContactId;

                legalEntityContact.LegalEntity = null;
                legalEntityContact.LegalEntityId = legalEntityId;
                legalEntityContact.Contact = null;
                legalEntityContact.ContactId = contactId;

                _dbContext.LegalEntityContacts.Add(legalEntityContact);
                _dbContext.SaveChanges();

                return new TaskResult<int?>(legalEntityContact.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}

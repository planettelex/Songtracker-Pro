using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.LegalEntityTasks
{
    public interface IUpdateLegalEntityContactTask : ITask<LegalEntityContact, Nothing> { }

    public class UpdateLegalEntityContact : TaskBase, IUpdateLegalEntityContactTask
    {
        public UpdateLegalEntityContact(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Nothing> DoTask(LegalEntityContact update)
        {
            try
            {
                var contact = _dbContext.LegalEntityContacts.SingleOrDefault(lec => lec.Id == update.Id);

                if (contact == null)
                    throw new TaskException(SystemMessage("LEGAL_ENTITY_CONTACT_NOT_FOUND"));

                contact.ContactId = update.Contact?.Id ?? update.ContactId;
                contact.Contact = _dbContext.People.Single(p => p.Id == contact.ContactId);
                contact.Position = update.Position;

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

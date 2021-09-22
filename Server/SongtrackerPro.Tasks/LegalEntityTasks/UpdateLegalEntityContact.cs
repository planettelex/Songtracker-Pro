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
            var contact = _dbContext.LegalEntityContacts.SingleOrDefault(lec => lec.Id == update.Id);

            if (contact == null)
                throw new TaskException(SystemMessage("LEGAL_ENTITY_CONTACT_NOT_FOUND"));

            contact.PersonId = update.Person?.Id ?? update.PersonId;
            contact.Person = _dbContext.People.Single(p => p.Id == contact.PersonId);
            contact.Position = update.Position;

            _dbContext.SaveChanges();

            return new TaskResult<Nothing>(true);
        }
    }
}

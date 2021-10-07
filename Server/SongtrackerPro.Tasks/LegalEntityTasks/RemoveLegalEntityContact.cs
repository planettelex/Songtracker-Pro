using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.LegalEntityTasks
{
    public interface IRemoveLegalEntityContactTask : ITask<LegalEntityContact, bool> { }

    public class RemoveLegalEntityContact : TaskBase, IRemoveLegalEntityContactTask
    {
        public RemoveLegalEntityContact(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(LegalEntityContact legalEntityContact)
        {
            try
            {
                var toRemove = _dbContext.LegalEntityContacts.SingleOrDefault(lc => lc.Id == legalEntityContact.Id);
                if (toRemove == null)
                    return new TaskResult<bool>(false);

                _dbContext.LegalEntityContacts.Remove(toRemove);
                _dbContext.SaveChanges();

                return new TaskResult<bool>(true);
            }
            catch (Exception e)
            {
                return new TaskResult<bool>(new TaskException(e));
            }
        }
    }
}

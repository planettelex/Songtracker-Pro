using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.LegalEntityTasks
{
    public interface IGetLegalEntityContactTask : ITask<int, LegalEntityContact> { }

    public class GetLegalEntityContact : TaskBase,  IGetLegalEntityContactTask
    {
        public GetLegalEntityContact(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<LegalEntityContact> DoTask(int legalEntityContactId)
        {
            try
            {
                var legalEntityContact = _dbContext.LegalEntityContacts.SingleOrDefault(lc => lc.Id == legalEntityContactId);

                return new TaskResult<LegalEntityContact>(legalEntityContact);
            }
            catch (Exception e)
            {
                return new TaskResult<LegalEntityContact>(new TaskException(e));
            }
        }
    }
}

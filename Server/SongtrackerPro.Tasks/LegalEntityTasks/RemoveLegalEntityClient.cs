using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.LegalEntityTasks
{
    public interface IRemoveLegalEntityClientTask : ITask<LegalEntityClient, bool> { }

    public class RemoveLegalEntityClient : TaskBase, IRemoveLegalEntityClientTask
    {
        public RemoveLegalEntityClient(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(LegalEntityClient legalEntityClient)
        {
            try
            {
                var toRemove = _dbContext.LegalEntityClients.SingleOrDefault(lc => lc.Id == legalEntityClient.Id);
                if (toRemove == null)
                    return new TaskResult<bool>(false);

                _dbContext.LegalEntityClients.Remove(toRemove);
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

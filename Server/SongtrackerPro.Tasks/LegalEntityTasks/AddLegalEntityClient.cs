using System;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.LegalEntityTasks
{
    public interface IAddLegalEntityClientTask : ITask<LegalEntityClient, int?> { }

    public class AddLegalEntityClient : TaskBase, IAddLegalEntityClientTask
    {
        public AddLegalEntityClient(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(LegalEntityClient legalEntityClient)
        {
            try
            {
                var legalEntityId = legalEntityClient.LegalEntity?.Id ?? legalEntityClient.LegalEntityId;
                var clientId = legalEntityClient.Client?.Id ?? legalEntityClient.ClientId;

                legalEntityClient.LegalEntity = null;
                legalEntityClient.LegalEntityId = legalEntityId;
                legalEntityClient.Client = null;
                legalEntityClient.ClientId = clientId;

                _dbContext.LegalEntityClients.Add(legalEntityClient);
                _dbContext.SaveChanges();

                return new TaskResult<int?>(legalEntityClient.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}

using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.LegalEntityTasks
{
    public interface IGetLegalEntityClientTask : ITask<int, LegalEntityClient> { }

    public class GetLegalEntityClient : TaskBase,  IGetLegalEntityClientTask
    {
        public GetLegalEntityClient(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<LegalEntityClient> DoTask(int legalEntityClientId)
        {
            try
            {
                var legalEntityClient = _dbContext.LegalEntityClients.SingleOrDefault(lc => lc.Id == legalEntityClientId);

                return new TaskResult<LegalEntityClient>(legalEntityClient);
            }
            catch (Exception e)
            {
                return new TaskResult<LegalEntityClient>(new TaskException(e));
            }
        }
    }
}

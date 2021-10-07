using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IGetCompositionAuthorTask : ITask<int, CompositionAuthor> { }

    public class GetCompositionAuthor : TaskBase,  IGetCompositionAuthorTask
    {
        public GetCompositionAuthor(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<CompositionAuthor> DoTask(int compositionAuthorId)
        {
            try
            {
                var compositionAuthor = _dbContext.CompositionAuthors.Where(pa => pa.Id == compositionAuthorId)
                    .Include(pa => pa.Author).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                    .SingleOrDefault();

                return new TaskResult<CompositionAuthor>(compositionAuthor);
            }
            catch (Exception e)
            {
                return new TaskResult<CompositionAuthor>(new TaskException(e));
            }
        }
    }
}

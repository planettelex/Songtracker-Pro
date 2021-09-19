using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IGetPublicationAuthorTask : ITask<int, PublicationAuthor> { }

    public class GetPublicationAuthor : TaskBase,  IGetPublicationAuthorTask
    {
        public GetPublicationAuthor(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<PublicationAuthor> DoTask(int publicationAuthorId)
        {
            try
            {
                var publicationAuthor = _dbContext.PublicationAuthors.Where(pa => pa.Id == publicationAuthorId)
                    .Include(pa => pa.Author).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                    .SingleOrDefault();

                return new TaskResult<PublicationAuthor>(publicationAuthor);
            }
            catch (Exception e)
            {
                return new TaskResult<PublicationAuthor>(new TaskException(e));
            }
        }
    }
}

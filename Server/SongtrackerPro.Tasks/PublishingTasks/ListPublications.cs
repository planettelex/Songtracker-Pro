using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IListPublicationsTask : ITask<Publisher, List<Publication>> { }

    public class ListPublications : TaskBase, IListPublicationsTask
    {
        public ListPublications(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<Publication>> DoTask(Publisher publisher)
        {
            try
            {
                var publications = _dbContext.Publications.Where(p => p.PublisherId == publisher.Id)
                    .ToList();

                return new TaskResult<List<Publication>>(publications);
            }
            catch (Exception e)
            {
                return new TaskResult<List<Publication>>(new TaskException(e));
            }
        }
    }
}

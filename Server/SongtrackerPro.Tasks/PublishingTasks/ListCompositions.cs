using System;
using System.Collections.Generic;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IListCompositionsTask : ITask<Publisher, List<Composition>> { }

    public class ListCompositions : TaskBase, IListCompositionsTask
    {
        public ListCompositions(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<Composition>> DoTask(Publisher publisher)
        {
            try
            {
                var compositions = _dbContext.Compositions.Where(p => p.PublisherId == publisher.Id)
                    .ToList();

                return new TaskResult<List<Composition>>(compositions);
            }
            catch (Exception e)
            {
                return new TaskResult<List<Composition>>(new TaskException(e));
            }
        }
    }
}

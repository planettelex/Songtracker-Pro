using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IListCompositionAuthorsTask : ITask<Composition, List<CompositionAuthor>> { }

    public class ListCompositionAuthors : TaskBase, IListCompositionAuthorsTask
    {
        public ListCompositionAuthors(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<CompositionAuthor>> DoTask(Composition composition)
        {
            try
            {
                var compositionAuthors = _dbContext.CompositionAuthors.Where(ca => ca.CompositionId == composition.Id)
                    .Include(ca => ca.Author).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                    .OrderByDescending(pa => pa.OwnershipPercentage)
                    .ToList();

                return new TaskResult<List<CompositionAuthor>>(compositionAuthors);
            }
            catch (Exception e)
            {
                return new TaskResult<List<CompositionAuthor>>(new TaskException(e));
            }
        }
    }
}

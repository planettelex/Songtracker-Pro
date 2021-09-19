using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IUpdateCompositionTask : ITask<Composition, Nothing> { }

    public class UpdateComposition : TaskBase, IUpdateCompositionTask
    {
        public UpdateComposition(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Nothing> DoTask(Composition update)
        {
            try
            {
                var composition = _dbContext.Compositions
                    .SingleOrDefault(p => p.Id == update.Id);

                if (composition == null)
                    throw new TaskException(SystemMessage("COMPOSITION_NOT_FOUND"));

                composition.Title = update.Title;
                composition.CatalogNumber = update.CatalogNumber;
                composition.Iswc = update.Iswc;
                composition.CopyrightedOn = update.CopyrightedOn;

                _dbContext.SaveChanges();

                return new TaskResult<Nothing>(true);
            }
            catch (Exception e)
            {
                return new TaskResult<Nothing>(new TaskException(e));
            }
        }
    }
}

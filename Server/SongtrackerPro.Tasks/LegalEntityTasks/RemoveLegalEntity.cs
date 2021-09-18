﻿using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.LegalEntityTasks
{
    public interface IRemoveLegalEntityTask : ITask<LegalEntity, bool> { }

    public class RemoveLegalEntity : TaskBase, IRemoveLegalEntityTask
    {
        public RemoveLegalEntity(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(LegalEntity legalEntity)
        {
            try
            {
                var toRemove = _dbContext.LegalEntities.SingleOrDefault(p => p.Id == legalEntity.Id);
                if (toRemove == null)
                    return new TaskResult<bool>(false);

                _dbContext.LegalEntities.Remove(toRemove);
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

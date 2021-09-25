using System;
using System.Collections.Generic;
using System.Text;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.MerchandiseTasks
{
    public interface IAddMerchandiseItemTask : ITask<MerchandiseItem, int?> { }

    public class AddMerchandiseItem : TaskBase, IAddMerchandiseItemTask
    {
        public AddMerchandiseItem(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(MerchandiseItem merchandiseItem)
        {
            throw new NotImplementedException();
        }
    }
}

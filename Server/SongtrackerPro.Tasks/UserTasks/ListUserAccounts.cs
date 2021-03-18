﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface IListUserAccountsTask : ITask<User, List<UserAccount>> { }

    public class ListUserAccounts : IListUserAccountsTask
    {
        public ListUserAccounts(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<UserAccount>> DoTask(User user)
        {
            try
            {
                var userAccounts = _dbContext.UserAccounts.Where(ua => ua.UserId == user.Id)
                    .Include(ua => ua.Platform)
                    .ToList();

                foreach (var _ in userAccounts.Select(userAccount => _dbContext.PlatformServices
                    .Where(ps => ps.PlatformId == userAccount.PlatformId)
                    .Include(ps => ps.Service)
                    .ToList())) { }

                return new TaskResult<List<UserAccount>>(userAccounts);
            }
            catch (Exception e)
            {
                return new TaskResult<List<UserAccount>>(new TaskException(e));
            }
        }
    }
}
﻿using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IUpdateArtistAccountTask : ITask<ArtistAccount, Nothing> { }

    public class UpdateArtistAccount : IUpdateArtistAccountTask
    {
        public UpdateArtistAccount(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Nothing> DoTask(ArtistAccount update)
        {
            try
            {
                var artistAccount = _dbContext.ArtistAccounts.SingleOrDefault(aa => aa.Id == update.Id);

                if (artistAccount == null)
                    throw new TaskException("Artist account not found.");

                if (update.IsPreferred)
                {
                    var allArtistAccounts = _dbContext.ArtistAccounts.ToList();
                    foreach (var account in allArtistAccounts)
                        account.IsPreferred = false;

                    _dbContext.SaveChanges();
                }

                artistAccount.IsPreferred = update.IsPreferred;
                artistAccount.Username = update.Username;
                _dbContext.SaveChanges();

                return new TaskResult<Nothing>(null as Nothing);
            }
            catch (Exception e)
            {
                return new TaskResult<Nothing>(new TaskException(e));
            }
        }
    }
}

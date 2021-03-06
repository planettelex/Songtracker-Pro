﻿using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IAddArtistTask : ITask<Artist, int?> { }

    public class AddArtist : TaskBase, IAddArtistTask
    {
        public AddArtist(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(Artist artist)
        {
            try
            {
                var recordLabelId = artist.RecordLabel?.Id ?? artist.RecordLabelId;

                artist.RecordLabel = null;
                artist.RecordLabelId = recordLabelId;

                _dbContext.Artists.Add(artist);
                _dbContext.SaveChanges();

                artist.RecordLabel = recordLabelId > 0 ? 
                    _dbContext.RecordLabels.Where(r => r.Id == recordLabelId)
                    .Include(r => r.Address).ThenInclude(a => a.Country)
                    .SingleOrDefault() : null;

                return new TaskResult<int?>(artist.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}

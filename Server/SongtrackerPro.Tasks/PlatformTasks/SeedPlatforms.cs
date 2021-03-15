using System;
using System.Collections.Generic;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PlatformTasks
{
    public interface ISeedPlatformsTask : ITask<Nothing, bool> { }

    public class SeedPlatforms : ISeedPlatformsTask
    {
        public SeedPlatforms(ApplicationDbContext dbContext, IListServicesTask listServicesTask, IAddPlatformTask addPlatformTask)
        {
            _dbContext = dbContext;
            _listServicesTask = listServicesTask;
            _addPlatformTask = addPlatformTask;
        }
        private readonly ApplicationDbContext _dbContext;
        private readonly IListServicesTask _listServicesTask;
        private readonly IAddPlatformTask _addPlatformTask;

        public TaskResult<bool> DoTask(Nothing nothing)
        {
            try
            {
                var platforms = _dbContext.Platforms.ToList();
                if (platforms.Any())
                    return new TaskResult<bool>(false);

                var services = _listServicesTask.DoTask(nothing);
                var interactiveStreaming = services.Data.Single(s => s.Name.ToLower() == "interactive streaming");
                var nonInteractiveStreaming = services.Data.Single(s => s.Name.ToLower() == "non-interactive streaming");
                var liveStreaming = services.Data.Single(s => s.Name.ToLower() == "live streaming");
                var videoHosting = services.Data.Single(s => s.Name.ToLower() == "video hosting");
                var videoClips = services.Data.Single(s => s.Name.ToLower() == "video clips");
                var socialMedia = services.Data.Single(s => s.Name.ToLower() == "social media");
                var digitalSales = services.Data.Single(s => s.Name.ToLower() == "digital sales");
                var physicalSales = services.Data.Single(s => s.Name.ToLower() == "physical sales");
                var payment = services.Data.Single(s => s.Name.ToLower() == "payment");
                var musicIdentification = services.Data.Single(s => s.Name.ToLower() == "music identification");
                var eventManagement = services.Data.Single(s => s.Name.ToLower() == "event management");

                _addPlatformTask.DoTask(new Platform { Name = "Amazon", Website = "https://www.amazon.com", 
                    Services = new List<Service>{ physicalSales }});
                _addPlatformTask.DoTask(new Platform { Name = "Amazon Music", Website = "https://music.amazon.com", 
                    Services = new List<Service>{ interactiveStreaming, digitalSales }});
                _addPlatformTask.DoTask(new Platform { Name = "Apple Music", Website = "https://music.apple.com", 
                    Services = new List<Service>{ interactiveStreaming }});
                _addPlatformTask.DoTask(new Platform { Name = "Bandcamp", Website = "https://bandcamp.com", 
                    Services = new List<Service>{ nonInteractiveStreaming, digitalSales, physicalSales }});
                _addPlatformTask.DoTask(new Platform { Name = "Beatport", Website = "https://www.beatport.com", 
                    Services = new List<Service>{ nonInteractiveStreaming, digitalSales, physicalSales }});
                _addPlatformTask.DoTask(new Platform { Name = "Cash App", Website = "https://cash.app", 
                    Services = new List<Service>{ payment }});
                _addPlatformTask.DoTask(new Platform { Name = "Drooble", Website = "https://drooble.com", 
                    Services = new List<Service>{ interactiveStreaming, socialMedia }});
                _addPlatformTask.DoTask(new Platform { Name = "Facebook", Website = "https://facebook.com", 
                    Services = new List<Service>{ socialMedia, liveStreaming, videoHosting, eventManagement }});
                _addPlatformTask.DoTask(new Platform { Name = "Instagram", Website = "https://www.instagram.com", 
                    Services = new List<Service>{ socialMedia, videoClips }});
                _addPlatformTask.DoTask(new Platform { Name = "Pandora", Website = "https://www.pandora.com", 
                    Services = new List<Service>{ nonInteractiveStreaming }});
                _addPlatformTask.DoTask(new Platform { Name = "Paypal", Website = "https://www.paypal.com/us/home", 
                    Services = new List<Service>{ payment }});
                _addPlatformTask.DoTask(new Platform { Name = "Shazam", Website = "https://www.shazam.com", 
                    Services = new List<Service>{ musicIdentification }});
                _addPlatformTask.DoTask(new Platform { Name = "Songkick", Website = "https://www.songkick.com", 
                    Services = new List<Service>{ eventManagement }});
                _addPlatformTask.DoTask(new Platform { Name = "Soundcloud", Website = "https://soundcloud.com", 
                    Services = new List<Service>{ interactiveStreaming }});
                _addPlatformTask.DoTask(new Platform { Name = "Spotify", Website = "https://www.spotify.com/us/home", 
                    Services = new List<Service>{ interactiveStreaming }});
                _addPlatformTask.DoTask(new Platform { Name = "Square", Website = "https://squareup.com/us/en", 
                    Services = new List<Service>{ payment, physicalSales }});
                _addPlatformTask.DoTask(new Platform { Name = "TikTok", Website = "https://www.tiktok.com/en", 
                    Services = new List<Service>{ socialMedia, videoClips }});
                _addPlatformTask.DoTask(new Platform { Name = "Twitch", Website = "https://www.twitch.tv", 
                    Services = new List<Service>{ liveStreaming }});
                _addPlatformTask.DoTask(new Platform { Name = "Twitter", Website = "https://twitter.com", 
                    Services = new List<Service>{ socialMedia }});
                _addPlatformTask.DoTask(new Platform { Name = "Venmo", Website = "https://venmo.com", 
                    Services = new List<Service>{ payment }});
                _addPlatformTask.DoTask(new Platform { Name = "YouTube", Website = "https://www.youtube.com", 
                    Services = new List<Service>{ videoHosting, liveStreaming }});
                _addPlatformTask.DoTask(new Platform { Name = "Zelle", Website = "https://www.zellepay.com", 
                    Services = new List<Service>{ payment }});

                return new TaskResult<bool>(true);
            }
            catch (Exception e)
            {
                return new TaskResult<bool>(new TaskException(e));
            }
        }
    }
}
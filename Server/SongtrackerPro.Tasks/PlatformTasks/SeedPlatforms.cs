using System;
using System.Collections.Generic;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.InstallationTasks;

namespace SongtrackerPro.Tasks.PlatformTasks
{
    public interface ISeedPlatformsTask : ITask<Nothing, bool> { }

    public class SeedPlatforms : TaskBase, ISeedPlatformsTask
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

                var services = _listServicesTask.DoTask(ServiceType.Platform);
                var interactiveStreaming = services.Data.Single(s => s.Name == SeedData("SERVICE_INTERACTIVE_STREAMING") && s.Type == ServiceType.Platform);
                var nonInteractiveStreaming = services.Data.Single(s => s.Name == SeedData("SERVICE_NON_INTERACTIVE_STREAMING") && s.Type == ServiceType.Platform);
                var liveStreaming = services.Data.Single(s => s.Name == SeedData("SERVICE_LIVE_STREAMING") && s.Type == ServiceType.Platform);
                var videoHosting = services.Data.Single(s => s.Name == SeedData("SERVICE_VIDEO_HOSTING") && s.Type == ServiceType.Platform);
                var videoClips = services.Data.Single(s => s.Name == SeedData("SERVICE_VIDEO_CLIPS") && s.Type == ServiceType.Platform);
                var socialMedia = services.Data.Single(s => s.Name == SeedData("SERVICE_SOCIAL_MEDIA") && s.Type == ServiceType.Platform);
                var digitalSales = services.Data.Single(s => s.Name == SeedData("SERVICE_DIGITAL_SALES") && s.Type == ServiceType.Platform);
                var physicalSales = services.Data.Single(s => s.Name == SeedData("SERVICE_PHYSICAL_SALES") && s.Type == ServiceType.Platform);
                var payment = services.Data.Single(s => s.Name == SeedData("SERVICE_PAYMENT") && s.Type == ServiceType.Platform);
                var musicIdentification = services.Data.Single(s => s.Name == SeedData("SERVICE_MUSIC_IDENTIFICATION") && s.Type == ServiceType.Platform);
                var eventManagement = services.Data.Single(s => s.Name == SeedData("SERVICE_EVENT_MANAGEMENT") && s.Type == ServiceType.Platform);

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
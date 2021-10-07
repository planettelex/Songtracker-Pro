using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.InstallationTasks
{
    public interface ISeedServicesTask : ITask<Nothing, bool> { }

    public class SeedServices : TaskBase, ISeedServicesTask
    {
        public SeedServices(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(Nothing nothing)
        {
            try
            {
                var services = _dbContext.Services.ToList();
                if (services.Any())
                    return new TaskResult<bool>(false);

                // Platform Services
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_INTERACTIVE_STREAMING"), Type = ServiceType.Platform });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_NON_INTERACTIVE_STREAMING"), Type = ServiceType.Platform });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_LIVE_STREAMING"), Type = ServiceType.Platform });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_VIDEO_HOSTING"), Type = ServiceType.Platform });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_VIDEO_CLIPS"), Type = ServiceType.Platform });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_SOCIAL_MEDIA"), Type = ServiceType.Platform });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_DIGITAL_SALES"), Type = ServiceType.Platform });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_PHYSICAL_SALES"), Type = ServiceType.Platform });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_PAYMENT"), Type = ServiceType.Platform });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_MUSIC_IDENTIFICATION"), Type = ServiceType.Platform });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_EVENT_MANAGEMENT"), Type = ServiceType.Platform });

                // LegalEntity Services
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_VENUE"), Type = ServiceType.LegalEntity });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_RESTAURANT_BAR"), Type = ServiceType.LegalEntity });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_BOOKING_AGENT"), Type = ServiceType.LegalEntity });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_RECORDING_STUDIO"), Type = ServiceType.LegalEntity });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_PRODUCTION_COMPANY"), Type = ServiceType.LegalEntity });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_RECORD_LABEL"), Type = ServiceType.LegalEntity });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_LEGAL_SERVICES"), Type = ServiceType.LegalEntity });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_TALENT_AGENCY"), Type = ServiceType.LegalEntity });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_PUBLISHING"), Type = ServiceType.LegalEntity });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_PROFESSIONAL_SERVICES"), Type = ServiceType.LegalEntity });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_RETAIL_SALES"), Type = ServiceType.LegalEntity });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_BROADCASTING"), Type = ServiceType.LegalEntity });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_EVENT_PLANNING"), Type = ServiceType.LegalEntity });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_ROYALTY_COLLECTIONS"), Type = ServiceType.LegalEntity });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_MANUFACTURING"), Type = ServiceType.LegalEntity });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_WHOLESALE_PROVIDER"), Type = ServiceType.LegalEntity });
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
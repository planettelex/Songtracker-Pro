using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SongtrackerPro.Data;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.GeographicTasks;
using SongtrackerPro.Tasks.InstallationTasks;
using SongtrackerPro.Tasks.PlatformTasks;
using SongtrackerPro.Tasks.PublishingTasks;
using SongtrackerPro.Tasks.RecordLabelTasks;

namespace SongtrackerPro.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer("name=ConnectionStrings:ApplicationDatabase"));

            RegisterTasks(services);

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void RegisterTasks(IServiceCollection services)
        {
            services.AddScoped<IGetInstallationInfoTask, GetInstallation>();
            services.AddScoped<IListCountriesTask, ListCountries>();
            services.AddScoped<IListServicesTask, ListServices>();
            services.AddScoped<IListPerformingRightsOrganizationsTask, ListPerformingRightsOrganizations>();

            services.AddScoped<IListPlatformsTask, ListPlatforms>();
            services.AddScoped<IGetPlatformTask, GetPlatform>();
            services.AddScoped<IAddPlatformTask, AddPlatform>();
            services.AddScoped<IUpdatePlatformTask, UpdatePlatform>();

            services.AddScoped<IListPublishersTask, ListPublishers>();
            services.AddScoped<IGetPublisherTask, GetPublisher>();
            services.AddScoped<IAddPublisherTask, AddPublisher>();
            services.AddScoped<IUpdatePublisherTask, UpdatePublisher>();

            services.AddScoped<IListRecordLabelsTask, ListRecordLabels>();
            services.AddScoped<IGetRecordLabelTask, GetRecordLabel>();
            services.AddScoped<IAddRecordLabelTask, AddRecordLabel>();
            services.AddScoped<IUpdateRecordLabelTask, UpdateRecordLabel>();

            services.AddScoped<IListArtistsTask, ListArtists>();
            services.AddScoped<IGetArtistTask, GetArtist>();
            services.AddScoped<IAddArtistTask, AddArtist>();
            services.AddScoped<IUpdateArtistTask, UpdateArtist>();
        }
    }
}

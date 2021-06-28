using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.GeographicTasks;
using SongtrackerPro.Tasks.InstallationTasks;
using SongtrackerPro.Tasks.PersonTasks;
using SongtrackerPro.Tasks.PlatformTasks;
using SongtrackerPro.Tasks.PublishingTasks;
using SongtrackerPro.Tasks.RecordLabelTasks;
using SongtrackerPro.Tasks.UserTasks;
using SongtrackerPro.Utilities;
using SongtrackerPro.Utilities.Services;

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

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder => { builder.WithOrigins(ApplicationSettings.Web.Domain)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });

            services.AddControllers();
        }
        //private const string RequestsAllowedFrom = "RequestsAllowedFrom";
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void RegisterTasks(IServiceCollection services)
        {
            services.AddScoped<IFormattingService, FormattingService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IHtmlService, HtmlService>();
            services.AddScoped<ITokenService, TokenService>();
            
            services.AddScoped<ISeedInstallationTask, SeedInstallation>();
            services.AddScoped<ISeedCountriesTask, SeedCountries>();
            services.AddScoped<ISeedPerformingRightsOrganizationsTask, SeedPerformingRightsOrganizations>();
            services.AddScoped<ISeedServicesTask, SeedServices>();
            services.AddScoped<ISeedPlatformsTask, SeedPlatforms>();
            services.AddScoped<ISeedSystemDataTask, SeedSystemData>();

            services.AddScoped<IGetInstallationTask, GetInstallation>();
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

            services.AddScoped<IListArtistMembersTask, ListArtistMembers>();
            services.AddScoped<IGetArtistMemberTask, GetArtistMember>();
            services.AddScoped<IAddArtistMemberTask, AddArtistMember>();
            services.AddScoped<IUpdateArtistMemberTask, UpdateArtistMember>();

            services.AddScoped<IListArtistManagersTask, ListArtistManagers>();
            services.AddScoped<IGetArtistManagerTask, GetArtistManager>();
            services.AddScoped<IAddArtistManagerTask, AddArtistManager>();
            services.AddScoped<IUpdateArtistManagerTask, UpdateArtistManager>();

            services.AddScoped<IListArtistAccountsTask, ListArtistAccounts>();
            services.AddScoped<IGetArtistAccountTask, GetArtistAccount>();
            services.AddScoped<IAddArtistAccountTask, AddArtistAccount>();
            services.AddScoped<IUpdateArtistAccountTask, UpdateArtistAccount>();
            services.AddScoped<IRemoveArtistAccountTask, RemoveArtistAccount>();

            services.AddScoped<IListArtistLinksTask, ListArtistLinks>();
            services.AddScoped<IGetArtistLinkTask, GetArtistLink>();
            services.AddScoped<IAddArtistLinkTask, AddArtistLink>();
            services.AddScoped<IRemoveArtistLinkTask, RemoveArtistLink>();

            services.AddScoped<IGetPersonTask, GetPerson>();
            services.AddScoped<IAddPersonTask, AddPerson>();
            services.AddScoped<IUpdatePersonTask, UpdatePerson>();

            services.AddScoped<ISendUserInvitationTask, SendUserInvitation>();
            services.AddScoped<IGetUserInvitationTask, GetUserInvitation>();
            services.AddScoped<IAcceptUserInvitationTask, AcceptUserInvitation>();

            services.AddScoped<IListUsersTask, ListUsers>();
            services.AddScoped<IGetUserTask, GetUser>();
            services.AddScoped<IAddUserTask, AddUser>();
            services.AddScoped<IUpdateUserTask, UpdateUser>();
            services.AddScoped<ILoginUserTask, LoginUser>();
            services.AddScoped<IGetLoginTask, GetLogin>();
            services.AddScoped<ILogoutUserTask, LogoutUser>();

            services.AddScoped<IListUserAccountsTask, ListUserAccounts>();
            services.AddScoped<IGetUserAccountTask, GetUserAccount>();
            services.AddScoped<IAddUserAccountTask, AddUserAccount>();
            services.AddScoped<IUpdateUserAccountTask, UpdateUserAccount>();
            services.AddScoped<IRemoveUserAccountTask, RemoveUserAccount>();
        }
    }
}

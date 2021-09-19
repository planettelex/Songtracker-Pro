using Microsoft.EntityFrameworkCore.Design;
using SongtrackerPro.Utilities;

namespace SongtrackerPro.Data
{
    public class DesignTimeContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            return new ApplicationDbContext(ApplicationSettings.Database.ConnectionString);
        }

        /* PMC Commands (https://docs.microsoft.com/en-us/ef/core/cli/powershell):
        
           Add-Migration -project SongtrackerPro.Data -name v_0_06
            fix ReleaseTrack cascade deletes and discriminator casing.

           Remove-Migration -project SongtrackerPro.Data
        
           Script-Migration 0 v_0_06 -project SongtrackerPro.Data -output SongtrackerPro.Data/Migrations/SQL/0_to_v_0_06.sql

           Update-Database -project SongtrackerPro.Data
        
         */
    }
}

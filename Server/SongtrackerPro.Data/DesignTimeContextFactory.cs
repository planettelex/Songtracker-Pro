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

        /* Package Manager Console Commands (https://docs.microsoft.com/en-us/ef/core/cli/powershell)
         * ------------------------------------------------------------------------------------------
         *
         * Add-Migration -project SongtrackerPro.Data -name v_0_06
         *
         * Remove-Migration -project SongtrackerPro.Data
         *
         * Script-Migration 0 v_0_06 -project SongtrackerPro.Data -output SongtrackerPro.Data/Migrations/SQL/0_to_v_0_06.sql
         *
         * Update-Database -project SongtrackerPro.Data
         *
         */

        /* Manual Updates
         * --------------
         *
         * 0 to 0.06:
         *   1) Update ReleaseTrack (release_tracks) release and recording properties from cascade on delete to no action.
         *   2) Make 'Discriminator' database fields lowercase to follow conventions.
         *
         */
    }
}

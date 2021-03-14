﻿using Microsoft.EntityFrameworkCore.Design;
using SongtrackerPro.Utilities;

namespace SongtrackerPro.Data
{
    public class DesignTimeContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            return new ApplicationDbContext(ApplicationSettings.ConnectionString);
        }

        /* PMC Commands (https://docs.microsoft.com/en-us/ef/core/cli/powershell):
        
           Add-Migration -project SongtrackerPro.Data -name v_0_03

           Remove-Migration -project SongtrackerPro.Data
        
           Script-Migration v_0_02 v_0_03 -project SongtrackerPro.Data -output SongtrackerPro.Data/Migrations/SQL/v_0_02_to_v_0_03.sql
        
         */
    }
}
using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SongtrackerPro.Data.Migrations
{
    public partial class v_0_02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "installation",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    version = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    tagline = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    oauth_id = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_installation", x => x.uuid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "installation");
        }
    }
}

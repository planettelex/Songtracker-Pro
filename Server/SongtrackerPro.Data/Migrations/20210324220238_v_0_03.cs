using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SongtrackerPro.Data.Migrations
{
    public partial class v_0_03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    iso_code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_countries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "installation",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    version = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    tagline = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_installation", x => x.uuid);
                });

            migrationBuilder.CreateTable(
                name: "platforms",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    website = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_platforms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "services",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_services", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    region = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    postal_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    country_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => x.id);
                    table.ForeignKey(
                        name: "FK_addresses_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "performing_rights_organizations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    country_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_performing_rights_organizations", x => x.id);
                    table.ForeignKey(
                        name: "FK_performing_rights_organizations_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "platform_services",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    platform_id = table.Column<int>(type: "int", nullable: false),
                    service_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_platform_services", x => x.id);
                    table.ForeignKey(
                        name: "FK_platform_services_platforms_platform_id",
                        column: x => x.platform_id,
                        principalTable: "platforms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_platform_services_services_service_id",
                        column: x => x.service_id,
                        principalTable: "services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "people",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    middle_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name_suffix = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    address_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_people", x => x.id);
                    table.ForeignKey(
                        name: "FK_people_addresses_address_id",
                        column: x => x.address_id,
                        principalTable: "addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "record_labels",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tax_id = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    address_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_record_labels", x => x.id);
                    table.ForeignKey(
                        name: "FK_record_labels_addresses_address_id",
                        column: x => x.address_id,
                        principalTable: "addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publishers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tax_id = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    address_id = table.Column<int>(type: "int", nullable: true),
                    performing_rights_organization_id = table.Column<int>(type: "int", nullable: true),
                    performing_rights_organization_publisher_number = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publishers", x => x.id);
                    table.ForeignKey(
                        name: "FK_publishers_addresses_address_id",
                        column: x => x.address_id,
                        principalTable: "addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_publishers_performing_rights_organizations_performing_rights_organization_id",
                        column: x => x.performing_rights_organization_id,
                        principalTable: "performing_rights_organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "artists",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tax_id = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    has_service_mark = table.Column<bool>(type: "bit", nullable: false),
                    website_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    press_kit_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    record_label_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_artists", x => x.id);
                    table.ForeignKey(
                        name: "FK_artists_record_labels_record_label_id",
                        column: x => x.record_label_id,
                        principalTable: "record_labels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type = table.Column<int>(type: "int", nullable: false),
                    profile_image_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    authentication_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    authentication_token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    last_login = table.Column<DateTime>(type: "datetime2", nullable: true),
                    person_id = table.Column<int>(type: "int", nullable: true),
                    social_security_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    publisher_id = table.Column<int>(type: "int", nullable: true),
                    record_label_id = table.Column<int>(type: "int", nullable: true),
                    performing_rights_organization_id = table.Column<int>(type: "int", nullable: true),
                    performing_rights_organization_member_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sound_exchange_account_number = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_people_person_id",
                        column: x => x.person_id,
                        principalTable: "people",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_users_performing_rights_organizations_performing_rights_organization_id",
                        column: x => x.performing_rights_organization_id,
                        principalTable: "performing_rights_organizations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_users_publishers_publisher_id",
                        column: x => x.publisher_id,
                        principalTable: "publishers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_users_record_labels_record_label_id",
                        column: x => x.record_label_id,
                        principalTable: "record_labels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "artist_accounts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    artist_id = table.Column<int>(type: "int", nullable: false),
                    platform_id = table.Column<int>(type: "int", nullable: false),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_preferred = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_artist_accounts", x => x.id);
                    table.ForeignKey(
                        name: "FK_artist_accounts_artists_artist_id",
                        column: x => x.artist_id,
                        principalTable: "artists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_artist_accounts_platforms_platform_id",
                        column: x => x.platform_id,
                        principalTable: "platforms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "artist_links",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    artist_id = table.Column<int>(type: "int", nullable: false),
                    platform_id = table.Column<int>(type: "int", nullable: false),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_artist_links", x => x.id);
                    table.ForeignKey(
                        name: "FK_artist_links_artists_artist_id",
                        column: x => x.artist_id,
                        principalTable: "artists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_artist_links_platforms_platform_id",
                        column: x => x.platform_id,
                        principalTable: "platforms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "artist_managers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    artist_id = table.Column<int>(type: "int", nullable: false),
                    person_id = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    started_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ended_on = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_artist_managers", x => x.id);
                    table.ForeignKey(
                        name: "FK_artist_managers_artists_artist_id",
                        column: x => x.artist_id,
                        principalTable: "artists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_artist_managers_people_person_id",
                        column: x => x.person_id,
                        principalTable: "people",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "artist_members",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    artist_id = table.Column<int>(type: "int", nullable: false),
                    person_id = table.Column<int>(type: "int", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    started_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ended_on = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_artist_members", x => x.id);
                    table.ForeignKey(
                        name: "FK_artist_members_artists_artist_id",
                        column: x => x.artist_id,
                        principalTable: "artists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_artist_members_people_person_id",
                        column: x => x.person_id,
                        principalTable: "people",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_accounts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    platform_id = table.Column<int>(type: "int", nullable: false),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_preferred = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_accounts", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_accounts_platforms_platform_id",
                        column: x => x.platform_id,
                        principalTable: "platforms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_accounts_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_invitations",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    invited_by_user_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false),
                    publisher_id = table.Column<int>(type: "int", nullable: true),
                    record_label_id = table.Column<int>(type: "int", nullable: true),
                    artist_id = table.Column<int>(type: "int", nullable: true),
                    sent_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    accepted_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_user_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_invitations", x => x.uuid);
                    table.ForeignKey(
                        name: "FK_user_invitations_artists_artist_id",
                        column: x => x.artist_id,
                        principalTable: "artists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_invitations_publishers_publisher_id",
                        column: x => x.publisher_id,
                        principalTable: "publishers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_invitations_record_labels_record_label_id",
                        column: x => x.record_label_id,
                        principalTable: "record_labels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_invitations_users_created_user_id",
                        column: x => x.created_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_invitations_users_invited_by_user_id",
                        column: x => x.invited_by_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_addresses_country_id",
                table: "addresses",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_artist_accounts_artist_id",
                table: "artist_accounts",
                column: "artist_id");

            migrationBuilder.CreateIndex(
                name: "IX_artist_accounts_platform_id",
                table: "artist_accounts",
                column: "platform_id");

            migrationBuilder.CreateIndex(
                name: "IX_artist_links_artist_id",
                table: "artist_links",
                column: "artist_id");

            migrationBuilder.CreateIndex(
                name: "IX_artist_links_platform_id",
                table: "artist_links",
                column: "platform_id");

            migrationBuilder.CreateIndex(
                name: "IX_artist_managers_artist_id",
                table: "artist_managers",
                column: "artist_id");

            migrationBuilder.CreateIndex(
                name: "IX_artist_managers_person_id",
                table: "artist_managers",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "IX_artist_members_artist_id",
                table: "artist_members",
                column: "artist_id");

            migrationBuilder.CreateIndex(
                name: "IX_artist_members_person_id",
                table: "artist_members",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "IX_artists_record_label_id",
                table: "artists",
                column: "record_label_id");

            migrationBuilder.CreateIndex(
                name: "IX_people_address_id",
                table: "people",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_performing_rights_organizations_country_id",
                table: "performing_rights_organizations",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_platform_services_platform_id",
                table: "platform_services",
                column: "platform_id");

            migrationBuilder.CreateIndex(
                name: "IX_platform_services_service_id",
                table: "platform_services",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "IX_publishers_address_id",
                table: "publishers",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_publishers_performing_rights_organization_id",
                table: "publishers",
                column: "performing_rights_organization_id");

            migrationBuilder.CreateIndex(
                name: "IX_record_labels_address_id",
                table: "record_labels",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_accounts_platform_id",
                table: "user_accounts",
                column: "platform_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_accounts_user_id",
                table: "user_accounts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_invitations_artist_id",
                table: "user_invitations",
                column: "artist_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_invitations_created_user_id",
                table: "user_invitations",
                column: "created_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_invitations_invited_by_user_id",
                table: "user_invitations",
                column: "invited_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_invitations_publisher_id",
                table: "user_invitations",
                column: "publisher_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_invitations_record_label_id",
                table: "user_invitations",
                column: "record_label_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_performing_rights_organization_id",
                table: "users",
                column: "performing_rights_organization_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_person_id",
                table: "users",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_publisher_id",
                table: "users",
                column: "publisher_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_record_label_id",
                table: "users",
                column: "record_label_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "artist_accounts");

            migrationBuilder.DropTable(
                name: "artist_links");

            migrationBuilder.DropTable(
                name: "artist_managers");

            migrationBuilder.DropTable(
                name: "artist_members");

            migrationBuilder.DropTable(
                name: "installation");

            migrationBuilder.DropTable(
                name: "platform_services");

            migrationBuilder.DropTable(
                name: "user_accounts");

            migrationBuilder.DropTable(
                name: "user_invitations");

            migrationBuilder.DropTable(
                name: "services");

            migrationBuilder.DropTable(
                name: "platforms");

            migrationBuilder.DropTable(
                name: "artists");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "people");

            migrationBuilder.DropTable(
                name: "publishers");

            migrationBuilder.DropTable(
                name: "record_labels");

            migrationBuilder.DropTable(
                name: "performing_rights_organizations");

            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "countries");
        }
    }
}

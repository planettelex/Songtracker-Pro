using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SongtrackerPro.Data.Migrations
{
    public partial class v_0_06 : Migration
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
                name: "genres",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    parent_genre_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genres", x => x.id);
                    table.ForeignKey(
                        name: "FK_genres_genres_parent_genre_id",
                        column: x => x.parent_genre_id,
                        principalTable: "genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "merchandise_categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    parent_category_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_merchandise_categories", x => x.id);
                    table.ForeignKey(
                        name: "FK_merchandise_categories_merchandise_categories_parent_category_id",
                        column: x => x.parent_category_id,
                        principalTable: "merchandise_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "recording_roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recording_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "services",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false)
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
                name: "legal_entities",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    entity_type = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tax_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address_id = table.Column<int>(type: "int", nullable: true),
                    phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    press_kit_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    record_label_id = table.Column<int>(type: "int", nullable: true),
                    trade_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    website_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    has_servicemark = table.Column<bool>(type: "bit", nullable: true),
                    has_trademark = table.Column<bool>(type: "bit", nullable: true),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    middle_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name_suffix = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    authentication_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_type = table.Column<int>(type: "int", nullable: true),
                    roles = table.Column<int>(type: "int", nullable: true),
                    publisher_id = table.Column<int>(type: "int", nullable: true),
                    performing_rights_organization_id = table.Column<int>(type: "int", nullable: true),
                    performing_rights_organization_member_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sound_exchange_account_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    performing_rights_organization_publisher_number = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_legal_entities", x => x.id);
                    table.ForeignKey(
                        name: "FK_legal_entities_addresses_address_id",
                        column: x => x.address_id,
                        principalTable: "addresses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_legal_entities_legal_entities_publisher_id",
                        column: x => x.publisher_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_legal_entities_legal_entities_record_label_id",
                        column: x => x.record_label_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_legal_entities_performing_rights_organizations_performing_rights_organization_id",
                        column: x => x.performing_rights_organization_id,
                        principalTable: "performing_rights_organizations",
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
                        name: "FK_artist_accounts_legal_entities_artist_id",
                        column: x => x.artist_id,
                        principalTable: "legal_entities",
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
                        name: "FK_artist_links_legal_entities_artist_id",
                        column: x => x.artist_id,
                        principalTable: "legal_entities",
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
                        name: "FK_artist_managers_legal_entities_artist_id",
                        column: x => x.artist_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_artist_managers_legal_entities_person_id",
                        column: x => x.person_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
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
                        name: "FK_artist_members_legal_entities_artist_id",
                        column: x => x.artist_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_artist_members_legal_entities_person_id",
                        column: x => x.person_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "compositions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    publisher_id = table.Column<int>(type: "int", nullable: false),
                    legal_entity_id = table.Column<int>(type: "int", nullable: true),
                    iswc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    catalog_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    copyrighted_on = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_compositions", x => x.id);
                    table.ForeignKey(
                        name: "FK_compositions_legal_entities_legal_entity_id",
                        column: x => x.legal_entity_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_compositions_legal_entities_publisher_id",
                        column: x => x.publisher_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "legal_entity_clients",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    legal_entity_id = table.Column<int>(type: "int", nullable: false),
                    client_legal_entity_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_legal_entity_clients", x => x.id);
                    table.ForeignKey(
                        name: "FK_legal_entity_clients_legal_entities_client_legal_entity_id",
                        column: x => x.client_legal_entity_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_legal_entity_clients_legal_entities_legal_entity_id",
                        column: x => x.legal_entity_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "legal_entity_contacts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    legal_entity_id = table.Column<int>(type: "int", nullable: false),
                    contact_legal_entity_id = table.Column<int>(type: "int", nullable: false),
                    position = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_legal_entity_contacts", x => x.id);
                    table.ForeignKey(
                        name: "FK_legal_entity_contacts_legal_entities_contact_legal_entity_id",
                        column: x => x.contact_legal_entity_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_legal_entity_contacts_legal_entities_legal_entity_id",
                        column: x => x.legal_entity_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "legal_entity_services",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    legal_entity_id = table.Column<int>(type: "int", nullable: false),
                    service_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_legal_entity_services", x => x.id);
                    table.ForeignKey(
                        name: "FK_legal_entity_services_legal_entities_legal_entity_id",
                        column: x => x.legal_entity_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_legal_entity_services_services_service_id",
                        column: x => x.service_id,
                        principalTable: "services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "logins",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    authentication_token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    token_expiration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    login_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    logout_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logins", x => x.id);
                    table.ForeignKey(
                        name: "FK_logins_legal_entities_user_id",
                        column: x => x.user_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "merchandise",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    category_id = table.Column<int>(type: "int", nullable: true),
                    is_promotional = table.Column<bool>(type: "bit", nullable: false),
                    artist_id = table.Column<int>(type: "int", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    publisher_id = table.Column<int>(type: "int", nullable: true),
                    record_label_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_merchandise", x => x.id);
                    table.ForeignKey(
                        name: "FK_merchandise_legal_entities_artist_id",
                        column: x => x.artist_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_merchandise_legal_entities_publisher_id",
                        column: x => x.publisher_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_merchandise_legal_entities_record_label_id",
                        column: x => x.record_label_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_merchandise_merchandise_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "merchandise_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publications",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    publisher_id = table.Column<int>(type: "int", nullable: false),
                    isbn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    catalog_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    copyrighted_on = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publications", x => x.id);
                    table.ForeignKey(
                        name: "FK_publications_legal_entities_publisher_id",
                        column: x => x.publisher_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "releases",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    artist_id = table.Column<int>(type: "int", nullable: true),
                    record_label_id = table.Column<int>(type: "int", nullable: false),
                    genre_id = table.Column<int>(type: "int", nullable: true),
                    type = table.Column<int>(type: "int", nullable: false),
                    catalog_number = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_releases", x => x.id);
                    table.ForeignKey(
                        name: "FK_releases_genres_genre_id",
                        column: x => x.genre_id,
                        principalTable: "genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_releases_legal_entities_artist_id",
                        column: x => x.artist_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_releases_legal_entities_record_label_id",
                        column: x => x.record_label_id,
                        principalTable: "legal_entities",
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
                        name: "FK_user_accounts_legal_entities_user_id",
                        column: x => x.user_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_accounts_platforms_platform_id",
                        column: x => x.platform_id,
                        principalTable: "platforms",
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
                    roles = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_user_invitations_legal_entities_artist_id",
                        column: x => x.artist_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_invitations_legal_entities_created_user_id",
                        column: x => x.created_user_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_invitations_legal_entities_invited_by_user_id",
                        column: x => x.invited_by_user_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_user_invitations_legal_entities_publisher_id",
                        column: x => x.publisher_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_invitations_legal_entities_record_label_id",
                        column: x => x.record_label_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "composition_authors",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    composition_id = table.Column<int>(type: "int", nullable: false),
                    person_id = table.Column<int>(type: "int", nullable: false),
                    ownership_percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_composition_authors", x => x.id);
                    table.ForeignKey(
                        name: "FK_composition_authors_compositions_composition_id",
                        column: x => x.composition_id,
                        principalTable: "compositions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_composition_authors_legal_entities_person_id",
                        column: x => x.person_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "recordings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    artist_id = table.Column<int>(type: "int", nullable: false),
                    record_label_id = table.Column<int>(type: "int", nullable: false),
                    composition_id = table.Column<int>(type: "int", nullable: false),
                    genre_id = table.Column<int>(type: "int", nullable: true),
                    isrc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    seconds_long = table.Column<int>(type: "int", nullable: false),
                    is_live = table.Column<bool>(type: "bit", nullable: false),
                    is_cover = table.Column<bool>(type: "bit", nullable: false),
                    is_remix = table.Column<bool>(type: "bit", nullable: false),
                    original_recording_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recordings", x => x.id);
                    table.ForeignKey(
                        name: "FK_recordings_compositions_composition_id",
                        column: x => x.composition_id,
                        principalTable: "compositions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_recordings_genres_genre_id",
                        column: x => x.genre_id,
                        principalTable: "genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_recordings_legal_entities_artist_id",
                        column: x => x.artist_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_recordings_legal_entities_record_label_id",
                        column: x => x.record_label_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_recordings_recordings_original_recording_id",
                        column: x => x.original_recording_id,
                        principalTable: "recordings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "publication_authors",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    publication_id = table.Column<int>(type: "int", nullable: false),
                    person_id = table.Column<int>(type: "int", nullable: false),
                    ownership_percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publication_authors", x => x.id);
                    table.ForeignKey(
                        name: "FK_publication_authors_legal_entities_person_id",
                        column: x => x.person_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_publication_authors_publications_publication_id",
                        column: x => x.publication_id,
                        principalTable: "publications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "merchandise_products",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    merchandise_item_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sku = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    upc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    color_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    publication_id = table.Column<int>(type: "int", nullable: true),
                    issue_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    release_id = table.Column<int>(type: "int", nullable: true),
                    media_type = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_merchandise_products", x => x.id);
                    table.ForeignKey(
                        name: "FK_merchandise_products_merchandise_merchandise_item_id",
                        column: x => x.merchandise_item_id,
                        principalTable: "merchandise",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_merchandise_products_publications_publication_id",
                        column: x => x.publication_id,
                        principalTable: "publications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_merchandise_products_releases_release_id",
                        column: x => x.release_id,
                        principalTable: "releases",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "recording_credits",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    recording_id = table.Column<int>(type: "int", nullable: false),
                    person_id = table.Column<int>(type: "int", nullable: false),
                    is_featured = table.Column<bool>(type: "bit", nullable: false),
                    ownership_percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recording_credits", x => x.id);
                    table.ForeignKey(
                        name: "FK_recording_credits_legal_entities_person_id",
                        column: x => x.person_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_recording_credits_recordings_recording_id",
                        column: x => x.recording_id,
                        principalTable: "recordings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "release_tracks",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    release_id = table.Column<int>(type: "int", nullable: false),
                    recording_id = table.Column<int>(type: "int", nullable: false),
                    track_number = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_release_tracks", x => x.id);
                    table.ForeignKey(
                        name: "FK_release_tracks_recordings_recording_id",
                        column: x => x.recording_id,
                        principalTable: "recordings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_release_tracks_releases_release_id",
                        column: x => x.release_id,
                        principalTable: "releases",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "storage_items",
                columns: table => new
                {
                    uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    publisher_id = table.Column<int>(type: "int", nullable: true),
                    record_label_id = table.Column<int>(type: "int", nullable: true),
                    artist_id = table.Column<int>(type: "int", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    file_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    folder_path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    container = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    media_category = table.Column<int>(type: "int", nullable: true),
                    is_compressed = table.Column<bool>(type: "bit", nullable: true),
                    document_type = table.Column<int>(type: "int", nullable: true),
                    version = table.Column<int>(type: "int", nullable: true),
                    is_template = table.Column<bool>(type: "bit", nullable: true),
                    promisor_party_type = table.Column<int>(type: "int", nullable: true),
                    promisee_party_type = table.Column<int>(type: "int", nullable: true),
                    provided_by_id = table.Column<int>(type: "int", nullable: true),
                    template_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    contract_status = table.Column<int>(type: "int", nullable: true),
                    drafted_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    provided_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    proposed_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    rejected_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    executed_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    expired_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    composition_id = table.Column<int>(type: "int", nullable: true),
                    publication_id = table.Column<int>(type: "int", nullable: true),
                    recording_id = table.Column<int>(type: "int", nullable: true),
                    release_id = table.Column<int>(type: "int", nullable: true),
                    merchandise_item_id = table.Column<int>(type: "int", nullable: true),
                    product_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_storage_items", x => x.uuid);
                    table.ForeignKey(
                        name: "FK_storage_items_compositions_composition_id",
                        column: x => x.composition_id,
                        principalTable: "compositions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_storage_items_legal_entities_artist_id",
                        column: x => x.artist_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_storage_items_legal_entities_provided_by_id",
                        column: x => x.provided_by_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_storage_items_legal_entities_publisher_id",
                        column: x => x.publisher_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_storage_items_legal_entities_record_label_id",
                        column: x => x.record_label_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_storage_items_merchandise_merchandise_item_id",
                        column: x => x.merchandise_item_id,
                        principalTable: "merchandise",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_storage_items_merchandise_products_product_id",
                        column: x => x.product_id,
                        principalTable: "merchandise_products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_storage_items_publications_publication_id",
                        column: x => x.publication_id,
                        principalTable: "publications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_storage_items_recordings_recording_id",
                        column: x => x.recording_id,
                        principalTable: "recordings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_storage_items_releases_release_id",
                        column: x => x.release_id,
                        principalTable: "releases",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_storage_items_storage_items_template_id",
                        column: x => x.template_id,
                        principalTable: "storage_items",
                        principalColumn: "uuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "recording_credit_roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    recording_credit_id = table.Column<int>(type: "int", nullable: false),
                    recording_role_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recording_credit_roles", x => x.id);
                    table.ForeignKey(
                        name: "FK_recording_credit_roles_recording_credits_recording_credit_id",
                        column: x => x.recording_credit_id,
                        principalTable: "recording_credits",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_recording_credit_roles_recording_roles_recording_role_id",
                        column: x => x.recording_role_id,
                        principalTable: "recording_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contract_parties",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    storage_item_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    role = table.Column<int>(type: "int", nullable: false),
                    is_principal = table.Column<bool>(type: "bit", nullable: false),
                    legal_entity_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contract_parties", x => x.id);
                    table.ForeignKey(
                        name: "FK_contract_parties_legal_entities_legal_entity_id",
                        column: x => x.legal_entity_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_contract_parties_storage_items_storage_item_id",
                        column: x => x.storage_item_id,
                        principalTable: "storage_items",
                        principalColumn: "uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "digital_media_uploads",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    storage_item_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    uploaded_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    uploaded_by_user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_digital_media_uploads", x => x.id);
                    table.ForeignKey(
                        name: "FK_digital_media_uploads_legal_entities_uploaded_by_user_id",
                        column: x => x.uploaded_by_user_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_digital_media_uploads_storage_items_storage_item_id",
                        column: x => x.storage_item_id,
                        principalTable: "storage_items",
                        principalColumn: "uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "document_uploads",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    storage_item_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    uploaded_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    uploaded_by_user_id = table.Column<int>(type: "int", nullable: false),
                    from_version = table.Column<int>(type: "int", nullable: true),
                    to_version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_document_uploads", x => x.id);
                    table.ForeignKey(
                        name: "FK_document_uploads_legal_entities_uploaded_by_user_id",
                        column: x => x.uploaded_by_user_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_document_uploads_storage_items_storage_item_id",
                        column: x => x.storage_item_id,
                        principalTable: "storage_items",
                        principalColumn: "uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contract_signatories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    storage_item_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    person_id = table.Column<int>(type: "int", nullable: true),
                    contract_party_id = table.Column<int>(type: "int", nullable: true),
                    signatory_title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    signed_on = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contract_signatories", x => x.id);
                    table.ForeignKey(
                        name: "FK_contract_signatories_contract_parties_contract_party_id",
                        column: x => x.contract_party_id,
                        principalTable: "contract_parties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_contract_signatories_legal_entities_person_id",
                        column: x => x.person_id,
                        principalTable: "legal_entities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_contract_signatories_storage_items_storage_item_id",
                        column: x => x.storage_item_id,
                        principalTable: "storage_items",
                        principalColumn: "uuid",
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
                name: "IX_composition_authors_composition_id",
                table: "composition_authors",
                column: "composition_id");

            migrationBuilder.CreateIndex(
                name: "IX_composition_authors_person_id",
                table: "composition_authors",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "IX_compositions_legal_entity_id",
                table: "compositions",
                column: "legal_entity_id");

            migrationBuilder.CreateIndex(
                name: "IX_compositions_publisher_id",
                table: "compositions",
                column: "publisher_id");

            migrationBuilder.CreateIndex(
                name: "IX_contract_parties_legal_entity_id",
                table: "contract_parties",
                column: "legal_entity_id");

            migrationBuilder.CreateIndex(
                name: "IX_contract_parties_storage_item_id",
                table: "contract_parties",
                column: "storage_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_contract_signatories_contract_party_id",
                table: "contract_signatories",
                column: "contract_party_id");

            migrationBuilder.CreateIndex(
                name: "IX_contract_signatories_person_id",
                table: "contract_signatories",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "IX_contract_signatories_storage_item_id",
                table: "contract_signatories",
                column: "storage_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_digital_media_uploads_storage_item_id",
                table: "digital_media_uploads",
                column: "storage_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_digital_media_uploads_uploaded_by_user_id",
                table: "digital_media_uploads",
                column: "uploaded_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_document_uploads_storage_item_id",
                table: "document_uploads",
                column: "storage_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_document_uploads_uploaded_by_user_id",
                table: "document_uploads",
                column: "uploaded_by_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_genres_parent_genre_id",
                table: "genres",
                column: "parent_genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_legal_entities_address_id",
                table: "legal_entities",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_legal_entities_performing_rights_organization_id",
                table: "legal_entities",
                column: "performing_rights_organization_id");

            migrationBuilder.CreateIndex(
                name: "IX_legal_entities_publisher_id",
                table: "legal_entities",
                column: "publisher_id");

            migrationBuilder.CreateIndex(
                name: "IX_legal_entities_record_label_id",
                table: "legal_entities",
                column: "record_label_id");

            migrationBuilder.CreateIndex(
                name: "IX_legal_entity_clients_client_legal_entity_id",
                table: "legal_entity_clients",
                column: "client_legal_entity_id");

            migrationBuilder.CreateIndex(
                name: "IX_legal_entity_clients_legal_entity_id",
                table: "legal_entity_clients",
                column: "legal_entity_id");

            migrationBuilder.CreateIndex(
                name: "IX_legal_entity_contacts_contact_legal_entity_id",
                table: "legal_entity_contacts",
                column: "contact_legal_entity_id");

            migrationBuilder.CreateIndex(
                name: "IX_legal_entity_contacts_legal_entity_id",
                table: "legal_entity_contacts",
                column: "legal_entity_id");

            migrationBuilder.CreateIndex(
                name: "IX_legal_entity_services_legal_entity_id",
                table: "legal_entity_services",
                column: "legal_entity_id");

            migrationBuilder.CreateIndex(
                name: "IX_legal_entity_services_service_id",
                table: "legal_entity_services",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "IX_logins_user_id",
                table: "logins",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_merchandise_artist_id",
                table: "merchandise",
                column: "artist_id");

            migrationBuilder.CreateIndex(
                name: "IX_merchandise_category_id",
                table: "merchandise",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_merchandise_publisher_id",
                table: "merchandise",
                column: "publisher_id");

            migrationBuilder.CreateIndex(
                name: "IX_merchandise_record_label_id",
                table: "merchandise",
                column: "record_label_id");

            migrationBuilder.CreateIndex(
                name: "IX_merchandise_categories_parent_category_id",
                table: "merchandise_categories",
                column: "parent_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_merchandise_products_merchandise_item_id",
                table: "merchandise_products",
                column: "merchandise_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_merchandise_products_publication_id",
                table: "merchandise_products",
                column: "publication_id");

            migrationBuilder.CreateIndex(
                name: "IX_merchandise_products_release_id",
                table: "merchandise_products",
                column: "release_id");

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
                name: "IX_publication_authors_person_id",
                table: "publication_authors",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "IX_publication_authors_publication_id",
                table: "publication_authors",
                column: "publication_id");

            migrationBuilder.CreateIndex(
                name: "IX_publications_publisher_id",
                table: "publications",
                column: "publisher_id");

            migrationBuilder.CreateIndex(
                name: "IX_recording_credit_roles_recording_credit_id",
                table: "recording_credit_roles",
                column: "recording_credit_id");

            migrationBuilder.CreateIndex(
                name: "IX_recording_credit_roles_recording_role_id",
                table: "recording_credit_roles",
                column: "recording_role_id");

            migrationBuilder.CreateIndex(
                name: "IX_recording_credits_person_id",
                table: "recording_credits",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "IX_recording_credits_recording_id",
                table: "recording_credits",
                column: "recording_id");

            migrationBuilder.CreateIndex(
                name: "IX_recordings_artist_id",
                table: "recordings",
                column: "artist_id");

            migrationBuilder.CreateIndex(
                name: "IX_recordings_composition_id",
                table: "recordings",
                column: "composition_id");

            migrationBuilder.CreateIndex(
                name: "IX_recordings_genre_id",
                table: "recordings",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_recordings_original_recording_id",
                table: "recordings",
                column: "original_recording_id");

            migrationBuilder.CreateIndex(
                name: "IX_recordings_record_label_id",
                table: "recordings",
                column: "record_label_id");

            migrationBuilder.CreateIndex(
                name: "IX_release_tracks_recording_id",
                table: "release_tracks",
                column: "recording_id");

            migrationBuilder.CreateIndex(
                name: "IX_release_tracks_release_id",
                table: "release_tracks",
                column: "release_id");

            migrationBuilder.CreateIndex(
                name: "IX_releases_artist_id",
                table: "releases",
                column: "artist_id");

            migrationBuilder.CreateIndex(
                name: "IX_releases_genre_id",
                table: "releases",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_releases_record_label_id",
                table: "releases",
                column: "record_label_id");

            migrationBuilder.CreateIndex(
                name: "IX_storage_items_artist_id",
                table: "storage_items",
                column: "artist_id");

            migrationBuilder.CreateIndex(
                name: "IX_storage_items_composition_id",
                table: "storage_items",
                column: "composition_id");

            migrationBuilder.CreateIndex(
                name: "IX_storage_items_merchandise_item_id",
                table: "storage_items",
                column: "merchandise_item_id");

            migrationBuilder.CreateIndex(
                name: "IX_storage_items_product_id",
                table: "storage_items",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_storage_items_provided_by_id",
                table: "storage_items",
                column: "provided_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_storage_items_publication_id",
                table: "storage_items",
                column: "publication_id");

            migrationBuilder.CreateIndex(
                name: "IX_storage_items_publisher_id",
                table: "storage_items",
                column: "publisher_id");

            migrationBuilder.CreateIndex(
                name: "IX_storage_items_record_label_id",
                table: "storage_items",
                column: "record_label_id");

            migrationBuilder.CreateIndex(
                name: "IX_storage_items_recording_id",
                table: "storage_items",
                column: "recording_id");

            migrationBuilder.CreateIndex(
                name: "IX_storage_items_release_id",
                table: "storage_items",
                column: "release_id");

            migrationBuilder.CreateIndex(
                name: "IX_storage_items_template_id",
                table: "storage_items",
                column: "template_id");

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
                name: "composition_authors");

            migrationBuilder.DropTable(
                name: "contract_signatories");

            migrationBuilder.DropTable(
                name: "digital_media_uploads");

            migrationBuilder.DropTable(
                name: "document_uploads");

            migrationBuilder.DropTable(
                name: "installation");

            migrationBuilder.DropTable(
                name: "legal_entity_clients");

            migrationBuilder.DropTable(
                name: "legal_entity_contacts");

            migrationBuilder.DropTable(
                name: "legal_entity_services");

            migrationBuilder.DropTable(
                name: "logins");

            migrationBuilder.DropTable(
                name: "platform_services");

            migrationBuilder.DropTable(
                name: "publication_authors");

            migrationBuilder.DropTable(
                name: "recording_credit_roles");

            migrationBuilder.DropTable(
                name: "release_tracks");

            migrationBuilder.DropTable(
                name: "user_accounts");

            migrationBuilder.DropTable(
                name: "user_invitations");

            migrationBuilder.DropTable(
                name: "contract_parties");

            migrationBuilder.DropTable(
                name: "services");

            migrationBuilder.DropTable(
                name: "recording_credits");

            migrationBuilder.DropTable(
                name: "recording_roles");

            migrationBuilder.DropTable(
                name: "platforms");

            migrationBuilder.DropTable(
                name: "storage_items");

            migrationBuilder.DropTable(
                name: "merchandise_products");

            migrationBuilder.DropTable(
                name: "recordings");

            migrationBuilder.DropTable(
                name: "merchandise");

            migrationBuilder.DropTable(
                name: "publications");

            migrationBuilder.DropTable(
                name: "releases");

            migrationBuilder.DropTable(
                name: "compositions");

            migrationBuilder.DropTable(
                name: "merchandise_categories");

            migrationBuilder.DropTable(
                name: "genres");

            migrationBuilder.DropTable(
                name: "legal_entities");

            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "performing_rights_organizations");

            migrationBuilder.DropTable(
                name: "countries");
        }
    }
}

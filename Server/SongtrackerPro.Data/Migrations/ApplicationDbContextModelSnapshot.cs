﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SongtrackerPro.Data;

namespace SongtrackerPro.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SongtrackerPro.Data.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("city");

                    b.Property<int?>("CountryId")
                        .HasColumnType("int")
                        .HasColumnName("country_id");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("postal_code");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)")
                        .HasColumnName("region");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("street");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("addresses");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.Artist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("HasServiceMark")
                        .HasColumnType("bit")
                        .HasColumnName("has_service_mark");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<string>("PressKitUrl")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("press_kit_url");

                    b.Property<int?>("RecordLabelId")
                        .HasColumnType("int")
                        .HasColumnName("record_label_id");

                    b.Property<string>("TaxId")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("tax_id");

                    b.Property<string>("WebsiteUrl")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("website_url");

                    b.HasKey("Id");

                    b.HasIndex("RecordLabelId");

                    b.ToTable("artists");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.ArtistAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ArtistId")
                        .HasColumnType("int")
                        .HasColumnName("artist_id");

                    b.Property<bool>("IsPreferred")
                        .HasColumnType("bit")
                        .HasColumnName("is_preferred");

                    b.Property<int>("PlatformId")
                        .HasColumnType("int")
                        .HasColumnName("platform_id");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.HasIndex("PlatformId");

                    b.ToTable("artist_accounts");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.ArtistLink", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ArtistId")
                        .HasColumnType("int")
                        .HasColumnName("artist_id");

                    b.Property<int>("PlatformId")
                        .HasColumnType("int")
                        .HasColumnName("platform_id");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("url");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.HasIndex("PlatformId");

                    b.ToTable("artist_links");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.ArtistManager", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ArtistId")
                        .HasColumnType("int")
                        .HasColumnName("artist_id");

                    b.Property<DateTime?>("EndedOn")
                        .HasColumnType("datetime2")
                        .HasColumnName("ended_on");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit")
                        .HasColumnName("is_active");

                    b.Property<int>("PersonId")
                        .HasColumnType("int")
                        .HasColumnName("person_id");

                    b.Property<DateTime>("StartedOn")
                        .HasColumnType("datetime2")
                        .HasColumnName("started_on");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.HasIndex("PersonId");

                    b.ToTable("artist_managers");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.ArtistMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ArtistId")
                        .HasColumnType("int")
                        .HasColumnName("artist_id");

                    b.Property<DateTime?>("EndedOn")
                        .HasColumnType("datetime2")
                        .HasColumnName("ended_on");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit")
                        .HasColumnName("is_active");

                    b.Property<int>("PersonId")
                        .HasColumnType("int")
                        .HasColumnName("person_id");

                    b.Property<DateTime>("StartedOn")
                        .HasColumnType("datetime2")
                        .HasColumnName("started_on");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.HasIndex("PersonId");

                    b.ToTable("artist_members");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("IsoCode")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)")
                        .HasColumnName("iso_code");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("countries");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.Installation", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("name");

                    b.Property<string>("Tagline")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("tagline");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)")
                        .HasColumnName("version");

                    b.HasKey("Uuid");

                    b.ToTable("installation");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.PerformingRightsOrganization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CountryId")
                        .HasColumnType("int")
                        .HasColumnName("country_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("performing_rights_organizations");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AddressId")
                        .HasColumnType("int")
                        .HasColumnName("address_id");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("last_name");

                    b.Property<string>("MiddleName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("middle_name");

                    b.Property<string>("NameSuffix")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)")
                        .HasColumnName("name_suffix");

                    b.Property<string>("Phone")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("phone");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.ToTable("people");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.Platform", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<string>("Website")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("website");

                    b.HasKey("Id");

                    b.ToTable("platforms");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.PlatformService", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PlatformId")
                        .HasColumnType("int")
                        .HasColumnName("platform_id");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int")
                        .HasColumnName("service_id");

                    b.HasKey("Id");

                    b.HasIndex("PlatformId");

                    b.HasIndex("ServiceId");

                    b.ToTable("platform_services");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.Publisher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AddressId")
                        .HasColumnType("int")
                        .HasColumnName("address_id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<int?>("PerformingRightsOrganizationId")
                        .HasColumnType("int")
                        .HasColumnName("performing_rights_organization_id");

                    b.Property<string>("PerformingRightsOrganizationPublisherNumber")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("performing_rights_organization_publisher_number");

                    b.Property<string>("Phone")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("phone");

                    b.Property<string>("TaxId")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("tax_id");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("PerformingRightsOrganizationId");

                    b.ToTable("publishers");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.RecordLabel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AddressId")
                        .HasColumnType("int")
                        .HasColumnName("address_id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<string>("Phone")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("phone");

                    b.Property<string>("TaxId")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("tax_id");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.ToTable("record_labels");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("services");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AuthenticationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("authentication_id");

                    b.Property<string>("AuthenticationToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("authentication_token");

                    b.Property<DateTime?>("LastLogin")
                        .HasColumnType("datetime2")
                        .HasColumnName("last_login");

                    b.Property<int?>("PerformingRightsOrganizationId")
                        .HasColumnType("int")
                        .HasColumnName("performing_rights_organization_id");

                    b.Property<string>("PerformingRightsOrganizationMemberNumber")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("performing_rights_organization_member_number");

                    b.Property<int?>("PersonId")
                        .HasColumnType("int")
                        .HasColumnName("person_id");

                    b.Property<string>("ProfileImageUrl")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("profile_image_url");

                    b.Property<int?>("PublisherId")
                        .HasColumnType("int")
                        .HasColumnName("publisher_id");

                    b.Property<int?>("RecordLabelId")
                        .HasColumnType("int")
                        .HasColumnName("record_label_id");

                    b.Property<string>("SocialSecurityNumber")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("social_security_number");

                    b.Property<string>("SoundExchangeAccountNumber")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("sound_exchange_account_number");

                    b.Property<int>("Type")
                        .HasColumnType("int")
                        .HasColumnName("type");

                    b.HasKey("Id");

                    b.HasIndex("PerformingRightsOrganizationId");

                    b.HasIndex("PersonId");

                    b.HasIndex("PublisherId");

                    b.HasIndex("RecordLabelId");

                    b.ToTable("users");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.UserAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsPreferred")
                        .HasColumnType("bit")
                        .HasColumnName("is_preferred");

                    b.Property<int>("PlatformId")
                        .HasColumnType("int")
                        .HasColumnName("platform_id");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.HasIndex("PlatformId");

                    b.HasIndex("UserId");

                    b.ToTable("user_accounts");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.Address", b =>
                {
                    b.HasOne("SongtrackerPro.Data.Models.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.Artist", b =>
                {
                    b.HasOne("SongtrackerPro.Data.Models.RecordLabel", "RecordLabel")
                        .WithMany()
                        .HasForeignKey("RecordLabelId");

                    b.Navigation("RecordLabel");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.ArtistAccount", b =>
                {
                    b.HasOne("SongtrackerPro.Data.Models.Artist", "Artist")
                        .WithMany("Accounts")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SongtrackerPro.Data.Models.Platform", "Platform")
                        .WithMany()
                        .HasForeignKey("PlatformId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");

                    b.Navigation("Platform");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.ArtistLink", b =>
                {
                    b.HasOne("SongtrackerPro.Data.Models.Artist", "Artist")
                        .WithMany("Links")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SongtrackerPro.Data.Models.Platform", "Platform")
                        .WithMany()
                        .HasForeignKey("PlatformId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");

                    b.Navigation("Platform");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.ArtistManager", b =>
                {
                    b.HasOne("SongtrackerPro.Data.Models.Artist", "Artist")
                        .WithMany("Managers")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SongtrackerPro.Data.Models.Person", "Member")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.ArtistMember", b =>
                {
                    b.HasOne("SongtrackerPro.Data.Models.Artist", "Artist")
                        .WithMany("Members")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SongtrackerPro.Data.Models.Person", "Member")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.PerformingRightsOrganization", b =>
                {
                    b.HasOne("SongtrackerPro.Data.Models.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.Person", b =>
                {
                    b.HasOne("SongtrackerPro.Data.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.Navigation("Address");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.PlatformService", b =>
                {
                    b.HasOne("SongtrackerPro.Data.Models.Platform", "Platform")
                        .WithMany("PlatformServices")
                        .HasForeignKey("PlatformId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SongtrackerPro.Data.Models.Service", "Service")
                        .WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Platform");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.Publisher", b =>
                {
                    b.HasOne("SongtrackerPro.Data.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.HasOne("SongtrackerPro.Data.Models.PerformingRightsOrganization", "PerformingRightsOrganization")
                        .WithMany()
                        .HasForeignKey("PerformingRightsOrganizationId");

                    b.Navigation("Address");

                    b.Navigation("PerformingRightsOrganization");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.RecordLabel", b =>
                {
                    b.HasOne("SongtrackerPro.Data.Models.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.Navigation("Address");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.User", b =>
                {
                    b.HasOne("SongtrackerPro.Data.Models.PerformingRightsOrganization", "PerformingRightsOrganization")
                        .WithMany()
                        .HasForeignKey("PerformingRightsOrganizationId");

                    b.HasOne("SongtrackerPro.Data.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId");

                    b.HasOne("SongtrackerPro.Data.Models.Publisher", "Publisher")
                        .WithMany()
                        .HasForeignKey("PublisherId");

                    b.HasOne("SongtrackerPro.Data.Models.RecordLabel", "RecordLabel")
                        .WithMany()
                        .HasForeignKey("RecordLabelId");

                    b.Navigation("PerformingRightsOrganization");

                    b.Navigation("Person");

                    b.Navigation("Publisher");

                    b.Navigation("RecordLabel");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.UserAccount", b =>
                {
                    b.HasOne("SongtrackerPro.Data.Models.Platform", "Platform")
                        .WithMany()
                        .HasForeignKey("PlatformId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SongtrackerPro.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Platform");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.Artist", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Links");

                    b.Navigation("Managers");

                    b.Navigation("Members");
                });

            modelBuilder.Entity("SongtrackerPro.Data.Models.Platform", b =>
                {
                    b.Navigation("PlatformServices");
                });
#pragma warning restore 612, 618
        }
    }
}

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [countries] (
    [id] int NOT NULL IDENTITY,
    [name] nvarchar(max) NOT NULL,
    [iso_code] nvarchar(3) NOT NULL,
    CONSTRAINT [PK_countries] PRIMARY KEY ([id])
);
GO

CREATE TABLE [installation] (
    [uuid] uniqueidentifier NOT NULL,
    [version] nvarchar(25) NOT NULL,
    [name] nvarchar(50) NOT NULL,
    [tagline] nvarchar(200) NULL,
    CONSTRAINT [PK_installation] PRIMARY KEY ([uuid])
);
GO

CREATE TABLE [platforms] (
    [id] int NOT NULL IDENTITY,
    [name] nvarchar(max) NOT NULL,
    [website] nvarchar(max) NULL,
    CONSTRAINT [PK_platforms] PRIMARY KEY ([id])
);
GO

CREATE TABLE [services] (
    [id] int NOT NULL IDENTITY,
    [name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_services] PRIMARY KEY ([id])
);
GO

CREATE TABLE [addresses] (
    [id] int NOT NULL IDENTITY,
    [street] nvarchar(max) NOT NULL,
    [city] nvarchar(max) NOT NULL,
    [region] nvarchar(3) NOT NULL,
    [postal_code] nvarchar(max) NOT NULL,
    [country_id] int NULL,
    CONSTRAINT [PK_addresses] PRIMARY KEY ([id]),
    CONSTRAINT [FK_addresses_countries_country_id] FOREIGN KEY ([country_id]) REFERENCES [countries] ([id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [performing_rights_organizations] (
    [id] int NOT NULL IDENTITY,
    [name] nvarchar(max) NOT NULL,
    [country_id] int NULL,
    CONSTRAINT [PK_performing_rights_organizations] PRIMARY KEY ([id]),
    CONSTRAINT [FK_performing_rights_organizations_countries_country_id] FOREIGN KEY ([country_id]) REFERENCES [countries] ([id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [platform_services] (
    [id] int NOT NULL IDENTITY,
    [platform_id] int NOT NULL,
    [service_id] int NOT NULL,
    CONSTRAINT [PK_platform_services] PRIMARY KEY ([id]),
    CONSTRAINT [FK_platform_services_platforms_platform_id] FOREIGN KEY ([platform_id]) REFERENCES [platforms] ([id]) ON DELETE CASCADE,
    CONSTRAINT [FK_platform_services_services_service_id] FOREIGN KEY ([service_id]) REFERENCES [services] ([id]) ON DELETE CASCADE
);
GO

CREATE TABLE [people] (
    [id] int NOT NULL IDENTITY,
    [first_name] nvarchar(max) NOT NULL,
    [middle_name] nvarchar(max) NULL,
    [last_name] nvarchar(max) NOT NULL,
    [name_suffix] nvarchar(5) NULL,
    [email] nvarchar(max) NULL,
    [phone] nvarchar(20) NULL,
    [address_id] int NULL,
    CONSTRAINT [PK_people] PRIMARY KEY ([id]),
    CONSTRAINT [FK_people_addresses_address_id] FOREIGN KEY ([address_id]) REFERENCES [addresses] ([id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [record_labels] (
    [id] int NOT NULL IDENTITY,
    [name] nvarchar(max) NOT NULL,
    [tax_id] nvarchar(max) NULL,
    [email] nvarchar(max) NOT NULL,
    [phone] nvarchar(20) NULL,
    [address_id] int NULL,
    CONSTRAINT [PK_record_labels] PRIMARY KEY ([id]),
    CONSTRAINT [FK_record_labels_addresses_address_id] FOREIGN KEY ([address_id]) REFERENCES [addresses] ([id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [publishers] (
    [id] int NOT NULL IDENTITY,
    [name] nvarchar(max) NOT NULL,
    [tax_id] nvarchar(max) NULL,
    [email] nvarchar(max) NOT NULL,
    [phone] nvarchar(20) NULL,
    [address_id] int NULL,
    [performing_rights_organization_id] int NULL,
    [performing_rights_organization_publisher_number] nvarchar(max) NULL,
    CONSTRAINT [PK_publishers] PRIMARY KEY ([id]),
    CONSTRAINT [FK_publishers_addresses_address_id] FOREIGN KEY ([address_id]) REFERENCES [addresses] ([id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_publishers_performing_rights_organizations_performing_rights_organization_id] FOREIGN KEY ([performing_rights_organization_id]) REFERENCES [performing_rights_organizations] ([id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [artists] (
    [id] int NOT NULL IDENTITY,
    [name] nvarchar(max) NOT NULL,
    [tax_id] nvarchar(max) NULL,
    [has_service_mark] bit NOT NULL,
    [website_url] nvarchar(max) NULL,
    [press_kit_url] nvarchar(max) NULL,
    [record_label_id] int NULL,
    CONSTRAINT [PK_artists] PRIMARY KEY ([id]),
    CONSTRAINT [FK_artists_record_labels_record_label_id] FOREIGN KEY ([record_label_id]) REFERENCES [record_labels] ([id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [users] (
    [id] int NOT NULL IDENTITY,
    [type] int NOT NULL,
    [profile_image_url] nvarchar(max) NULL,
    [authentication_id] nvarchar(max) NOT NULL,
    [authentication_token] nvarchar(max) NOT NULL,
    [last_login] datetime2 NULL,
    [person_id] int NULL,
    [social_security_number] nvarchar(max) NULL,
    [publisher_id] int NULL,
    [record_label_id] int NULL,
    [performing_rights_organization_id] int NULL,
    [performing_rights_organization_member_number] nvarchar(max) NULL,
    [sound_exchange_account_number] nvarchar(max) NULL,
    CONSTRAINT [PK_users] PRIMARY KEY ([id]),
    CONSTRAINT [FK_users_people_person_id] FOREIGN KEY ([person_id]) REFERENCES [people] ([id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_users_performing_rights_organizations_performing_rights_organization_id] FOREIGN KEY ([performing_rights_organization_id]) REFERENCES [performing_rights_organizations] ([id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_users_publishers_publisher_id] FOREIGN KEY ([publisher_id]) REFERENCES [publishers] ([id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_users_record_labels_record_label_id] FOREIGN KEY ([record_label_id]) REFERENCES [record_labels] ([id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [artist_accounts] (
    [id] int NOT NULL IDENTITY,
    [artist_id] int NOT NULL,
    [platform_id] int NOT NULL,
    [username] nvarchar(max) NOT NULL,
    [is_preferred] bit NOT NULL,
    CONSTRAINT [PK_artist_accounts] PRIMARY KEY ([id]),
    CONSTRAINT [FK_artist_accounts_artists_artist_id] FOREIGN KEY ([artist_id]) REFERENCES [artists] ([id]) ON DELETE CASCADE,
    CONSTRAINT [FK_artist_accounts_platforms_platform_id] FOREIGN KEY ([platform_id]) REFERENCES [platforms] ([id]) ON DELETE CASCADE
);
GO

CREATE TABLE [artist_links] (
    [id] int NOT NULL IDENTITY,
    [artist_id] int NOT NULL,
    [platform_id] int NOT NULL,
    [url] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_artist_links] PRIMARY KEY ([id]),
    CONSTRAINT [FK_artist_links_artists_artist_id] FOREIGN KEY ([artist_id]) REFERENCES [artists] ([id]) ON DELETE CASCADE,
    CONSTRAINT [FK_artist_links_platforms_platform_id] FOREIGN KEY ([platform_id]) REFERENCES [platforms] ([id]) ON DELETE CASCADE
);
GO

CREATE TABLE [artist_managers] (
    [id] int NOT NULL IDENTITY,
    [artist_id] int NOT NULL,
    [person_id] int NOT NULL,
    [is_active] bit NOT NULL,
    [started_on] datetime2 NOT NULL,
    [ended_on] datetime2 NULL,
    CONSTRAINT [PK_artist_managers] PRIMARY KEY ([id]),
    CONSTRAINT [FK_artist_managers_artists_artist_id] FOREIGN KEY ([artist_id]) REFERENCES [artists] ([id]) ON DELETE CASCADE,
    CONSTRAINT [FK_artist_managers_people_person_id] FOREIGN KEY ([person_id]) REFERENCES [people] ([id]) ON DELETE CASCADE
);
GO

CREATE TABLE [artist_members] (
    [id] int NOT NULL IDENTITY,
    [artist_id] int NOT NULL,
    [person_id] int NOT NULL,
    [is_active] bit NOT NULL,
    [started_on] datetime2 NOT NULL,
    [ended_on] datetime2 NULL,
    CONSTRAINT [PK_artist_members] PRIMARY KEY ([id]),
    CONSTRAINT [FK_artist_members_artists_artist_id] FOREIGN KEY ([artist_id]) REFERENCES [artists] ([id]) ON DELETE CASCADE,
    CONSTRAINT [FK_artist_members_people_person_id] FOREIGN KEY ([person_id]) REFERENCES [people] ([id]) ON DELETE CASCADE
);
GO

CREATE TABLE [user_accounts] (
    [id] int NOT NULL IDENTITY,
    [user_id] int NOT NULL,
    [platform_id] int NOT NULL,
    [username] nvarchar(max) NOT NULL,
    [is_preferred] bit NOT NULL,
    CONSTRAINT [PK_user_accounts] PRIMARY KEY ([id]),
    CONSTRAINT [FK_user_accounts_platforms_platform_id] FOREIGN KEY ([platform_id]) REFERENCES [platforms] ([id]) ON DELETE CASCADE,
    CONSTRAINT [FK_user_accounts_users_user_id] FOREIGN KEY ([user_id]) REFERENCES [users] ([id]) ON DELETE CASCADE
);
GO

CREATE TABLE [user_invitations] (
    [uuid] uniqueidentifier NOT NULL,
    [invited_by_user_id] int NOT NULL,
    [name] nvarchar(max) NOT NULL,
    [email] nvarchar(max) NOT NULL,
    [type] int NOT NULL,
    [publisher_id] int NULL,
    [record_label_id] int NULL,
    [artist_id] int NULL,
    [sent_on] datetime2 NOT NULL,
    [accepted_on] datetime2 NULL,
    [created_user_id] int NULL,
    CONSTRAINT [PK_user_invitations] PRIMARY KEY ([uuid]),
    CONSTRAINT [FK_user_invitations_artists_artist_id] FOREIGN KEY ([artist_id]) REFERENCES [artists] ([id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_user_invitations_publishers_publisher_id] FOREIGN KEY ([publisher_id]) REFERENCES [publishers] ([id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_user_invitations_record_labels_record_label_id] FOREIGN KEY ([record_label_id]) REFERENCES [record_labels] ([id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_user_invitations_users_created_user_id] FOREIGN KEY ([created_user_id]) REFERENCES [users] ([id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_user_invitations_users_invited_by_user_id] FOREIGN KEY ([invited_by_user_id]) REFERENCES [users] ([id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_addresses_country_id] ON [addresses] ([country_id]);
GO

CREATE INDEX [IX_artist_accounts_artist_id] ON [artist_accounts] ([artist_id]);
GO

CREATE INDEX [IX_artist_accounts_platform_id] ON [artist_accounts] ([platform_id]);
GO

CREATE INDEX [IX_artist_links_artist_id] ON [artist_links] ([artist_id]);
GO

CREATE INDEX [IX_artist_links_platform_id] ON [artist_links] ([platform_id]);
GO

CREATE INDEX [IX_artist_managers_artist_id] ON [artist_managers] ([artist_id]);
GO

CREATE INDEX [IX_artist_managers_person_id] ON [artist_managers] ([person_id]);
GO

CREATE INDEX [IX_artist_members_artist_id] ON [artist_members] ([artist_id]);
GO

CREATE INDEX [IX_artist_members_person_id] ON [artist_members] ([person_id]);
GO

CREATE INDEX [IX_artists_record_label_id] ON [artists] ([record_label_id]);
GO

CREATE INDEX [IX_people_address_id] ON [people] ([address_id]);
GO

CREATE INDEX [IX_performing_rights_organizations_country_id] ON [performing_rights_organizations] ([country_id]);
GO

CREATE INDEX [IX_platform_services_platform_id] ON [platform_services] ([platform_id]);
GO

CREATE INDEX [IX_platform_services_service_id] ON [platform_services] ([service_id]);
GO

CREATE INDEX [IX_publishers_address_id] ON [publishers] ([address_id]);
GO

CREATE INDEX [IX_publishers_performing_rights_organization_id] ON [publishers] ([performing_rights_organization_id]);
GO

CREATE INDEX [IX_record_labels_address_id] ON [record_labels] ([address_id]);
GO

CREATE INDEX [IX_user_accounts_platform_id] ON [user_accounts] ([platform_id]);
GO

CREATE INDEX [IX_user_accounts_user_id] ON [user_accounts] ([user_id]);
GO

CREATE INDEX [IX_user_invitations_artist_id] ON [user_invitations] ([artist_id]);
GO

CREATE INDEX [IX_user_invitations_created_user_id] ON [user_invitations] ([created_user_id]);
GO

CREATE INDEX [IX_user_invitations_invited_by_user_id] ON [user_invitations] ([invited_by_user_id]);
GO

CREATE INDEX [IX_user_invitations_publisher_id] ON [user_invitations] ([publisher_id]);
GO

CREATE INDEX [IX_user_invitations_record_label_id] ON [user_invitations] ([record_label_id]);
GO

CREATE INDEX [IX_users_performing_rights_organization_id] ON [users] ([performing_rights_organization_id]);
GO

CREATE INDEX [IX_users_person_id] ON [users] ([person_id]);
GO

CREATE INDEX [IX_users_publisher_id] ON [users] ([publisher_id]);
GO

CREATE INDEX [IX_users_record_label_id] ON [users] ([record_label_id]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210326191228_v_0_03', N'5.0.3');
GO

COMMIT;
GO


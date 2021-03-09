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

CREATE TABLE [installation] (
    [uuid] uniqueidentifier NOT NULL,
    [version] nvarchar(25) NOT NULL,
    [name] nvarchar(50) NOT NULL,
    [tagline] nvarchar(200) NULL,
    [oauth_id] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_installation] PRIMARY KEY ([uuid])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210308223741_v_0_02', N'5.0.3');
GO

COMMIT;
GO


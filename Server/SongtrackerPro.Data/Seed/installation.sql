BEGIN TRANSACTION;
GO

DECLARE @uuid uniqueidentifier;
DECLARE @version nvarchar(25);
DECLARE @name nvarchar(50);
DECLARE @tagline nvarchar(50);
DECLARE @oauth_id nvarchar(MAX);
DECLARE @oauth_console_url nvarchar(MAX);
DECLARE @hosting_console_url nvarchar(MAX);

SET @uuid = 'c83eac1b-ddb9-4952-951b-97a624e97914';
SET @version = '0.02';
SET @name = 'Songtracker Pro';
SET @tagline = 'Royalties Tracking and Management';
SET @oauth_id = '1084971919395-tkrcdm6pm9fal8d21c4vk552gpqmvq6o.apps.googleusercontent.com';
SET @oauth_console_url = 'https://console.developers.google.com/apis/credentials?authuser=1&project=songtracker-pro';
SET @hosting_console_url = 'https://console.firebase.google.com/u/1/project/songtracker-pro/overview';

INSERT INTO [installation] ([uuid], [version], [name], [tagline], [oauth_id], [oauth_console_url], [hosting_console_url])
VALUES (@uuid, @version, @name, @tagline, @oauth_id, @oauth_console_url, @hosting_console_url);
GO

COMMIT;
GO

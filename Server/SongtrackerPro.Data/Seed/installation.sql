BEGIN TRANSACTION;
GO

DECLARE @uuid uniqueidentifier;
DECLARE @version nvarchar(25);
DECLARE @name nvarchar(50);
DECLARE @tagline nvarchar(50);
DECLARE @oauth_id nvarchar(MAX);

SET @uuid = 'c83eac1b-ddb9-4952-951b-97a624e97914';
SET @version = '0.02';
SET @name = 'Songtracker Pro';
SET @tagline = 'Royalties Tracking and Management';
SET @oauthid = '1084971919395-tkrcdm6pm9fal8d21c4vk552gpqmvq6o.apps.googleusercontent.com';

INSERT INTO installation ([uuid], [version], [name], [tagline], [oauth_id])
VALUES (@uuid, @version, @name, @tagline, @oauth_id);
GO

COMMIT;
GO

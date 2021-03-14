BEGIN TRANSACTION;
GO

DECLARE @usa_country_id int;
SET @usa_country_id = (SELECT [id] FROM [countries] WHERE [name] = 'USA');

INSERT INTO [performing_rights_organizations] ([name], [country_id]) VALUES ('ASCAP', @usa_country_id);
INSERT INTO [performing_rights_organizations] ([name], [country_id]) VALUES ('BMI', @usa_country_id);
GO

COMMIT;
GO
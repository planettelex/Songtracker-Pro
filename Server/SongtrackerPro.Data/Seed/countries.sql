BEGIN TRANSACTION;
GO

INSERT INTO [countries] ([name], [iso_code]) VALUES ('United States', 'USA');
GO

INSERT INTO [countries] ([name], [iso_code]) VALUES ('Canada', 'CAN');
GO

COMMIT;
GO
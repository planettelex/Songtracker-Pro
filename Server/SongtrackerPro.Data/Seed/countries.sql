BEGIN TRANSACTION;
GO

INSERT INTO [countries] ([name], [iso_code]) VALUES ('United States', 'USA');
INSERT INTO [countries] ([name], [iso_code]) VALUES ('Canada', 'CAN');
GO

COMMIT;
GO
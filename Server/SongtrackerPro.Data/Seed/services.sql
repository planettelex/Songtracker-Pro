BEGIN TRANSACTION;
GO

INSERT INTO [services] ([name]) VALUES ('Interactive Streaming');
INSERT INTO [services] ([name]) VALUES ('Non-Interactive Streaming');
INSERT INTO [services] ([name]) VALUES ('Live Streaming');
INSERT INTO [services] ([name]) VALUES ('Video Hosting');
INSERT INTO [services] ([name]) VALUES ('Video Clips');
INSERT INTO [services] ([name]) VALUES ('Social Media');
INSERT INTO [services] ([name]) VALUES ('Digital Sales');
INSERT INTO [services] ([name]) VALUES ('Physical Sales');
INSERT INTO [services] ([name]) VALUES ('Payment');
GO

COMMIT;
GO
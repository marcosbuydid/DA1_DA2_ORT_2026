--run first after database is created without tables to avoid auto incremental ids (not secuential as expected) when any DB crash
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = OFF
GO

--create default roles
INSERT INTO [MediaCatalogDB].[dbo].[Roles] ([Name]) VALUES ('Administrator'), ('User');

--create default user
INSERT INTO [MediaCatalogDB].[dbo].[Users] ([Name], [LastName], [Email], [Password], [RoleId])
VALUES ('Administrator', 'MediaCatalog', 'administrator@mediacatalog.com', 'YfCcXFdr5hMSfeP2PqGnLahaL/Aq7qDX78vZTnxYlB3iC6FQHcQi5AB9ETjAWY66', '1');
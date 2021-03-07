IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'PortalGateDb'))
USE PortalGateDb;

GO

IF OBJECT_ID('UnitStartPageURIes', 'u') IS NULL
DROP TABLE UnitStartPageURIes
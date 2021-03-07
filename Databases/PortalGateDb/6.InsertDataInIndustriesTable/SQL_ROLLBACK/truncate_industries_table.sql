IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'PortalGateDb'))
USE PortalGateDb;

GO

IF OBJECT_ID('Industries', 'u') IS NOT NULL
TRUNCATE TABLE Industries
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'PortalGateDb'))
USE PortalGateDb

GO

IF OBJECT_ID('Units', 'u') IS NOT NULL
TRUNCATE TABLE Units
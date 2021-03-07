IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'PortalGateDb'))
USE PortalGateDb;

GO

IF OBJECT_ID('RailroadList', 'u') IS NOT NULL
DROP TABLE RailroadList;
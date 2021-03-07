IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'PortalGateDb'))
USE PortalGateDb;

GO

IF OBJECT_ID('UnitStartPageURIes', 'u') IS NULL
CREATE TABLE UnitStartPageURIes
(
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	Railroad INT REFERENCES RailroadList (Id),
	Industry INT REFERENCES Industries (Id),
	Unit INT REFERENCES Units (Id),
	Uri NCHAR(100)
)
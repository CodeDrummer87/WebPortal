IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'PortalGateDb'))
USE PortalGateDb;

GO

IF OBJECT_ID('Units', 'u') IS NULL
CREATE TABLE Units
(
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	Railroad INT REFERENCES RailroadList (Id),
	Industry INT REFERENCES Industries (Id),
	ShortTitle NVARCHAR(20),
	FullTitle NVARCHAR(100),
	Address NVARCHAR (100),
	City NVARCHAR(30),
	Code NCHAR(10)
);
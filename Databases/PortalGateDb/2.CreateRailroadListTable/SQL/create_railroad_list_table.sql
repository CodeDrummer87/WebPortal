IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'PortalGateDb'))
USE PortalGateDb;

GO

IF OBJECT_ID('RailroadList', 'u') IS NULL
CREATE TABLE RailroadList
(
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	FullTitle NVARCHAR(50),
	Abbreviation NVARCHAR(20),
	Code NVARCHAR(5)
)
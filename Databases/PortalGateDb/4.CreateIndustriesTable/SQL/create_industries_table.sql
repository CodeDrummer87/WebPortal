IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'PortalGateDb'))
USE PortalGateDb;

GO

IF OBJECT_ID('Industries', 'u') IS NULL
CREATE TABLE Industries
(
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	Abbreviation NVARCHAR(20),
	FullTitle NVARCHAR(50)
);
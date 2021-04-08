IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'TCH2_WestSiberianRailroadDb'))
USE TCH2_WestSiberianRailroadDb;

GO

IF OBJECT_ID('LaborProtectionTelegrams', 'u') IS NULL
CREATE TABLE LaborProtectionTelegrams
(
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	Created DateTime, 
	Subject NVARCHAR(200),
	Content NVARCHAR(MAX),
	IsActual TINYINT DEFAULT 1
)
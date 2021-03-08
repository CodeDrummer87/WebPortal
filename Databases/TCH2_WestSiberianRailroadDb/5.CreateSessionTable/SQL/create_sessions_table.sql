IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'TCH2_WestSiberianRailroadDb'))
USE TCH2_WestSiberianRailroadDb;

GO

IF OBJECT_ID('Sessions', 'u') IS NULL
CREATE TABLE Sessions
(
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	SessionId NVARCHAR(100),
	UserId INT,
	Created DateTime,
	Expired DateTime
)
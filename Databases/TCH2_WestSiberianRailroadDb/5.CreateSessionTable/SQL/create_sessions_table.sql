IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'TCH2_WestSiberianRailroadDb'))
USE TCH2_WestSiberianRailroadDb;

GO

IF OBJECT_ID('Sessions', 'u') IS NULL
CREATE TABLE Sessions
(
	SessionId VARBINARY(8000),
	UserId INT,
	Created DateTime,
	Expired DateTime
)
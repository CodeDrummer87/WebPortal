IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'TCH2_WestSiberianRailroadDb'))
USE TCH2_WestSiberianRailroadDb;

GO

IF OBJECT_ID('EmailConfirmModels', 'u') IS NULL
CREATE TABLE EmailConfirmModels
(
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	UserId INT,
	HashForCheck NVARCHAR(50),

	FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
)

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'TCH2_WestSiberianRailroadDb'))
USE TCH2_WestSiberianRailroadDb;

GO

IF OBJECT_ID('ColumnStaff', 'u') IS NULL
CREATE TABLE ColumnStaff
(
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	ColumnId INT,
	UserId INT,
	IsCrew TINYINT DEFAULT 0,

	FOREIGN KEY (ColumnId) REFERENCES Columns(Id),
	FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
)
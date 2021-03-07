IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'TCH2_WestSiberianRailroadDb'))
USE TCH2_WestSiberianRailroadDb;

GO

IF OBJECT_ID('Roles', 'u') IS NULL
CREATE TABLE Roles
(
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	Name NCHAR(50)
)

GO

INSERT INTO Roles
VALUES ('Admin'), ('Employee'), ('Instructor'), ('Engineer')
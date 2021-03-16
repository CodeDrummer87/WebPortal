IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'TCH2_WestSiberianRailroadDb'))
USE TCH2_WestSiberianRailroadDb

GO


IF OBJECT_ID('Roles', 'u') IS NOT NULL
ALTER TABLE Roles
ADD IsActual TINYINT DEFAULT 1

GO

UPDATE Roles
SET IsActual=1

GO

INSERT INTO Roles
VALUES ('Главный инженер', 0)

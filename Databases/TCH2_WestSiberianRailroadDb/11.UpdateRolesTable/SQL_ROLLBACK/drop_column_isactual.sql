IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'TCH2_WestSiberianRailroadDb'))
USE TCH2_WestSiberianRailroadDb

GO

IF OBJECT_ID('Roles', 'u') IS NOT NULL
ALTER TABLE Roles
DROP COLUMN IsActual;
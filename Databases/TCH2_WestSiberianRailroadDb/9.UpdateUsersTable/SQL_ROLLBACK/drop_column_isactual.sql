IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'TCH2_WestSiberianRailroadDb'))
USE TCH2_WestSiberianRailroadDb

GO

IF OBJECT_ID('Users', 'u') IS NOT NULL
ALTER TABLE Users
DROP COLUMN IsActual;
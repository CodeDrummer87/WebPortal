IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'TCH2_WestSiberianRailroadDb'))
USE TCH2_WestSiberianRailroadDb

GO

IF OBJECT_ID('Users', 'u') IS NOT NULL
ALTER TABLE Users
ADD ConfirmedEmail TINYINT DEFAULT 0 

GO

INSERT INTO Users (ConfirmedEmail)
VALUES (0)
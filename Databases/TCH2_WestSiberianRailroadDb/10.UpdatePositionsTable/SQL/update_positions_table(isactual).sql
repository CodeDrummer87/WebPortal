IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'TCH2_WestSiberianRailroadDb'))
USE TCH2_WestSiberianRailroadDb

GO


IF OBJECT_ID('Positions', 'u') IS NOT NULL
ALTER TABLE Positions
ADD IsActual TINYINT DEFAULT 1

GO

UPDATE Positions
SET IsActual=1

GO

INSERT INTO Positions
VALUES ('Кочегар', '', 0)

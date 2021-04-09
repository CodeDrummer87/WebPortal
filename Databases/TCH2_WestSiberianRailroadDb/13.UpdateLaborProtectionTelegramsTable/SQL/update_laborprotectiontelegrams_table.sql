IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'TCH2_WestSiberianRailroadDb'))
USE TCH2_WestSiberianRailroadDb;

GO

IF OBJECT_ID('LaborProtectionTelegrams', 'u') IS NOT NULL
ALTER TABLE LaborProtectionTelegrams
ALTER COLUMN Created NVARCHAR(25)

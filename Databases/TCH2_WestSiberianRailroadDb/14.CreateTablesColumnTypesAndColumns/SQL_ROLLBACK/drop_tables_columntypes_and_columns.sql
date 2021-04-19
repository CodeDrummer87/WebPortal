IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'TCH2_WestSiberianRailroadDb'))
USE TCH2_WestSiberianRailroadDb;

GO

IF OBJECT_ID('Columns', 'u') IS NOT NULL
DROP TABLE Columns

GO

IF OBJECT_ID('ColumnTypes', 'u') IS NOT NULL
DROP TABLE ColumnTypes

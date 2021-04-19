IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'TCH2_WestSiberianRailroadDb'))
USE TCH2_WestSiberianRailroadDb;

GO

--- ColumnTypes Table ------------------
IF OBJECT_ID('ColumnTypes', 'u') IS NULL
CREATE TABLE ColumnTypes
(
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	Name NVARCHAR(25)
)

GO

IF OBJECT_ID('ColumnTypes', 'u') IS NOT NULL
INSERT INTO ColumnTypes
VALUES
	('Грузовая колонна'),
	('Пассажирская колонна'),
	('Маневровая колонна'),
	('Хозяйственная колонна'),
	('Передаточная колонна'),
	('Экипировочная колонна')
----------------------------------------
--- Columns Table ----------------------

IF OBJECT_ID('Columns', 'u') IS NULL
CREATE TABLE Columns
(
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	Specialization INT,
	Trainer INT,
	IsActual TINYINT DEFAULT 1,

	FOREIGN KEY (Specialization) REFERENCES ColumnTypes(Id) ON DELETE CASCADE,
	FOREIGN KEY (Trainer) REFERENCES Users(Id) ON DELETE CASCADE
)
----------------------------------------

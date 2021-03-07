IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'TCH2_WestSiberianRailroadDb'))
USE TCH2_WestSiberianRailroadDb;

GO

IF OBJECT_ID('Positions', 'u') IS NULL
CREATE TABLE Positions
(
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	FullName NVARCHAR(50),
	Abbreviation NVARCHAR(20)
)

GO

INSERT INTO Positions
VALUES 
	('Системный администратор', ''),
	('Сотрудник отдела кадров', ''),
	('Нарядчик локомотивных бригад', 'ТЧН'),
	('Помощник машиниста', 'ТЧПМ'),
	('Машинист', 'ТЧМ'),
	('Машинист-инструктор', 'ТЧМИ'),
	('Инженер', '');
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'TCH2_WestSiberianRailroadDb'))
USE TCH2_WestSiberianRailroadDb;

GO

IF OBJECT_ID('AppEmailAccounts', 'u') IS NULL
CREATE TABLE AppEmailAccounts
(
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	Email NCHAR(50),
	Password NVARCHAR(50),
	IsActual TINYINT NOT NULL
);

GO

INSERT INTO AppEmailAccounts
VALUES ('tch2.westsibrailroad@mail.ru', ';tktpyfzljhjuf', 1);
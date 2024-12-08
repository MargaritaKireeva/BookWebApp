CREATE DATABASE BooksDb
COLLATE Cyrillic_General_100_CI_AI
GO

USE BooksDb

CREATE TABLE Books (
  Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Books PRIMARY KEY,
  Title nvarchar(100) NOT NULL,
  Author nvarchar(100) NOT NULL,
  Price decimal NOT NULL,
  Quantity int NOT NULL,
);

INSERT INTO [dbo].[Books]
           ([Title]
           ,[Author]
           ,[Price]
           ,[Quantity])
     VALUES
           ('Гарри Поттер и философский камень', 'Джоан Роулинг', 800, 12),
		   ('Преступление и наказание', 'Ф.М. Достоевский', 400, 3)
GO

-- Создание базы данных
CREATE DATABASE OrdersDb
COLLATE Cyrillic_General_100_CI_AI
GO

-- Использование созданной базы данных
USE OrdersDb
GO

-- Создание таблицы Orders
CREATE TABLE Orders (
    Id INT NOT NULL PRIMARY KEY, 
	CartId INT NOT NULL,                     
    TotalAmount DECIMAL(10, 2) NOT NULL,    
    OrderDate DATETIME NOT NULL DEFAULT GETDATE() 
);

-- Создание таблицы OrderItems
CREATE TABLE OrderItems (
    Id INT IDENTITY(1,1) PRIMARY KEY, 
	OrderId INT NOT NULL,                         
    BookId INT NOT NULL,                          
    CartId INT NOT NULL,
	Price DECIMAL(10, 2) NOT NULL, 
    Quantity INT NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(Id)
);
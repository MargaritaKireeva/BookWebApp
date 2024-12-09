-- Создание базы данных
CREATE DATABASE CartDb
COLLATE Cyrillic_General_100_CI_AI
GO

-- Использование созданной базы данных
USE CartDb
GO

-- Создание таблицы Cart
CREATE TABLE Carts (
    Id INT IDENTITY(1,1) PRIMARY KEY, -- Уникальный идентификатор корзины
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE() -- Дата создания корзины
);

-- Создание таблицы CartItem
CREATE TABLE CartItems (
    Id INT IDENTITY(1,1) PRIMARY KEY, 
    CartId INT NOT NULL,
    BookId INT NOT NULL, 
    Quantity INT NOT NULL,
    FOREIGN KEY (CartId) REFERENCES Carts(Id) ON DELETE CASCADE
);
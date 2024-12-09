-- �������� ���� ������
CREATE DATABASE CartDb
COLLATE Cyrillic_General_100_CI_AI
GO

-- ������������� ��������� ���� ������
USE CartDb
GO

-- �������� ������� Cart
CREATE TABLE Carts (
    Id INT IDENTITY(1,1) PRIMARY KEY, -- ���������� ������������� �������
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE() -- ���� �������� �������
);

-- �������� ������� CartItem
CREATE TABLE CartItems (
    Id INT IDENTITY(1,1) PRIMARY KEY, 
    CartId INT NOT NULL,
    BookId INT NOT NULL, 
    Quantity INT NOT NULL,
    FOREIGN KEY (CartId) REFERENCES Carts(Id) ON DELETE CASCADE
);
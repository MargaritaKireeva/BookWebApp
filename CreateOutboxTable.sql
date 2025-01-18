CREATE TABLE Outbox (
  Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Outbox PRIMARY KEY,
  Content nvarchar(MAX) NOT NULL,
  [Status] nvarchar(100) NOT NULL,
  CreatedAt datetime NOT NULL
);

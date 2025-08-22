-- Database creation
CREATE DATABASE UserManagerDb;
GO

USE [UserManagerDb]
GO

-- Tables creation
CREATE TABLE dbo.Company (
    Id INT CONSTRAINT PK_Company PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL,
    CatchPhrase NVARCHAR(200),
    Bs NVARCHAR(200)
);
CREATE TABLE dbo.Address (
    Id INT CONSTRAINT PK_Address PRIMARY KEY IDENTITY,
    Street NVARCHAR(100) NOT NULL,
    Suite NVARCHAR(50) NOT NULL,
    City NVARCHAR(100) NOT NULL,
    Zipcode NVARCHAR(20) NOT NULL,
    Lat NVARCHAR(20),
    Lng NVARCHAR(20)
);
CREATE TABLE dbo.UserRecord (
    Id INT CONSTRAINT PK_UserRecord PRIMARY KEY IDENTITY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Username NVARCHAR(50),
    Phone NVARCHAR(50),
    Website NVARCHAR(100),
    CompanyId INT,
    AddressId INT,
    CreatedAt DATETIME2(3) NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_UserRecord_Company FOREIGN KEY (CompanyId) REFERENCES dbo.Company(Id),
    CONSTRAINT FK_UserRecord_Address FOREIGN KEY (AddressId) REFERENCES dbo.Address(Id)
);

-- Create Index for searching by email
CREATE INDEX IX_UserRecord_Email
ON dbo.UserRecord (Email);
GO

-- Create View that gets users which email ends with '.biz'
CREATE VIEW dbo.vw_BizUserRecord AS
SELECT u.Id, u.FirstName, u.LastName, u.Email, u.Username, u.Phone, u.Website, u.CreatedAt,
    a.Street, a.Suite, a.City, a.Zipcode, a.Lat, a.Lng,
    c.Name as CompanyName, c.CatchPhrase, c.Bs
FROM dbo.UserRecord u
    LEFT JOIN dbo.Address a ON u.AddressId = a.Id
    LEFT JOIN dbo.Company c ON u.CompanyId = c.Id
WHERE u.Email LIKE '%.biz';
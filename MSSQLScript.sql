-- Create Database
CREATE DATABASE LibraryManagement;
GO

USE LibraryManagement;
GO

-- Create Books Table
CREATE TABLE [dbo].[Books]
(
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [CreatedAt] DATETIME2 NOT NULL,
    [UpdatedAt] DATETIME2 NULL,
    [CreatedBy] NVARCHAR(100) NULL,
    [UpdatedBy] NVARCHAR(100) NULL,
    [ISBN] NVARCHAR(13) NOT NULL,
    [Title] NVARCHAR(200) NOT NULL,
    [Author] NVARCHAR(100) NOT NULL,
    [Status] INT NOT NULL DEFAULT(1),  -- 1: Available, 2: Borrowed, 3: UnderMaintenance
    [TotalCopies] INT NOT NULL DEFAULT(1),
    [AvailableCopies] INT NOT NULL DEFAULT(1),
    CONSTRAINT [UQ_Books_ISBN] UNIQUE ([ISBN]),
    CONSTRAINT [CHK_Books_TotalCopies] CHECK (TotalCopies >= 1),
    CONSTRAINT [CHK_Books_AvailableCopies] CHECK (AvailableCopies >= 0),
    CONSTRAINT [CHK_Books_Copies] CHECK (AvailableCopies <= TotalCopies)
);
GO

-- Create Users Table
CREATE TABLE [dbo].[Users]
(
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [CreatedAt] DATETIME2 NOT NULL,
    [UpdatedAt] DATETIME2 NULL,
    [CreatedBy] NVARCHAR(100) NULL,
    [UpdatedBy] NVARCHAR(100) NULL,
    [Name] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(100) NOT NULL,

    CONSTRAINT [UQ_Users_Email] UNIQUE ([Email])
);
GO

-- Create BorrowingRecords Table
CREATE TABLE [dbo].[BorrowingRecords]
(
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [CreatedAt] DATETIME2 NOT NULL,
    [UpdatedAt] DATETIME2 NULL,
    [CreatedBy] NVARCHAR(100) NULL,
    [UpdatedBy] NVARCHAR(100) NULL,
    [BookId] INT NOT NULL,
    [UserId] INT NOT NULL,
    [BorrowDate] DATETIME2 NOT NULL,
    [ReturnDate] DATETIME2 NULL,
    [DueDate] DATETIME2 NOT NULL,

    CONSTRAINT [FK_BorrowingRecords_Books] FOREIGN KEY ([BookId]) 
        REFERENCES [dbo].[Books]([Id]),
    CONSTRAINT [FK_BorrowingRecords_Users] FOREIGN KEY ([UserId]) 
        REFERENCES [dbo].[Users]([Id])
);
GO
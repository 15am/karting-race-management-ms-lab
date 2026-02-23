/*
  Stage 5 placeholder.
  Stage 6 will finalize datatypes, constraints, indexes, and FK behavior.
*/

USE [KartingRaceManagement];
GO

IF OBJECT_ID(N'dbo.Drivers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Drivers
    (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        Name NVARCHAR(120) NOT NULL,
        CreatedAtUtc DATETIME2 NOT NULL
    );
END;
GO

IF OBJECT_ID(N'dbo.Races', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Races
    (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        Name NVARCHAR(160) NOT NULL,
        DateUtc DATETIME2 NOT NULL,
        Status NVARCHAR(32) NOT NULL
    );
END;
GO

IF OBJECT_ID(N'dbo.LapTimes', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.LapTimes
    (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        RaceId UNIQUEIDENTIFIER NOT NULL,
        DriverId UNIQUEIDENTIFIER NOT NULL,
        LapNumber INT NOT NULL,
        TimeMilliseconds INT NOT NULL,
        CreatedAtUtc DATETIME2 NOT NULL
    );
END;
GO

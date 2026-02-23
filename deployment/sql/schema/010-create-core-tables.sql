USE [KartingRaceManagement];
GO

IF OBJECT_ID(N'dbo.Drivers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Drivers
    (
        Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Drivers PRIMARY KEY,
        Name NVARCHAR(80) NOT NULL,
        CreatedAtUtc DATETIME2(7) NOT NULL
    );
END;
GO

IF OBJECT_ID(N'dbo.Races', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Races
    (
        Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Races PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        DateUtc DATETIME2(7) NOT NULL,
        Status NVARCHAR(40) NOT NULL,
        CONSTRAINT CK_Races_Status CHECK (Status IN (N'Scheduled', N'InProgress', N'Completed'))
    );
END;
GO

IF OBJECT_ID(N'dbo.LapTimes', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.LapTimes
    (
        Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_LapTimes PRIMARY KEY,
        RaceId UNIQUEIDENTIFIER NOT NULL,
        DriverId UNIQUEIDENTIFIER NOT NULL,
        LapNumber INT NOT NULL,
        TimeMilliseconds INT NOT NULL,
        CreatedAtUtc DATETIME2(7) NOT NULL,
        CONSTRAINT CK_LapTimes_LapNumber CHECK (LapNumber > 0),
        CONSTRAINT CK_LapTimes_TimeMilliseconds CHECK (TimeMilliseconds > 0),
        CONSTRAINT FK_LapTimes_Races FOREIGN KEY (RaceId) REFERENCES dbo.Races(Id),
        CONSTRAINT FK_LapTimes_Drivers FOREIGN KEY (DriverId) REFERENCES dbo.Drivers(Id)
    );
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_LapTimes_RaceId_DriverId'
      AND object_id = OBJECT_ID(N'dbo.LapTimes')
)
BEGIN
    CREATE INDEX IX_LapTimes_RaceId_DriverId
        ON dbo.LapTimes(RaceId, DriverId);
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'IX_LapTimes_RaceId_TimeMilliseconds_LapNumber'
      AND object_id = OBJECT_ID(N'dbo.LapTimes')
)
BEGIN
    CREATE INDEX IX_LapTimes_RaceId_TimeMilliseconds_LapNumber
        ON dbo.LapTimes(RaceId, TimeMilliseconds, LapNumber);
END;
GO

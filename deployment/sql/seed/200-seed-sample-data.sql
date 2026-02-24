USE [KartingRaceManagement];
GO

DECLARE @RaceId UNIQUEIDENTIFIER = '11111111-1111-1111-1111-111111111111';
DECLARE @DriverOneId UNIQUEIDENTIFIER = '22222222-2222-2222-2222-222222222222';
DECLARE @DriverTwoId UNIQUEIDENTIFIER = '33333333-3333-3333-3333-333333333333';
DECLARE @DriverThreeId UNIQUEIDENTIFIER = '44444444-4444-4444-4444-444444444444';

IF NOT EXISTS (SELECT 1 FROM dbo.Drivers WHERE Id = @DriverOneId)
BEGIN
    INSERT INTO dbo.Drivers (Id, Name, CreatedAtUtc)
    VALUES (@DriverOneId, N'Lewis Hamilton', SYSUTCDATETIME());
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Drivers WHERE Id = '33333333-3333-3333-3333-333333333333')
BEGIN
    INSERT INTO dbo.Drivers (Id, Name, CreatedAtUtc)
    VALUES ('33333333-3333-3333-3333-333333333333', N'Ayrton Senna', SYSUTCDATETIME());
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Drivers WHERE Id = '44444444-4444-4444-4444-444444444444')
BEGIN
    INSERT INTO dbo.Drivers (Id, Name, CreatedAtUtc)
    VALUES ('44444444-4444-4444-4444-444444444444', N'Michael Schumacher', SYSUTCDATETIME());
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Races WHERE Id = '11111111-1111-1111-1111-111111111111')
BEGIN
    INSERT INTO dbo.Races (Id, Name, DateUtc, Status)
    VALUES ('11111111-1111-1111-1111-111111111111', N'Karting Challenge Demo', DATEADD(DAY, -1, SYSUTCDATETIME()), N'Completed');
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.LapTimes WHERE Id = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1')
BEGIN
    INSERT INTO dbo.LapTimes (Id, RaceId, DriverId, LapNumber, TimeMilliseconds, CreatedAtUtc)
    VALUES ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1', '11111111-1111-1111-1111-111111111111', '22222222-2222-2222-2222-222222222222', 1, 60234, SYSUTCDATETIME());
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.LapTimes WHERE Id = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2')
BEGIN
    INSERT INTO dbo.LapTimes (Id, RaceId, DriverId, LapNumber, TimeMilliseconds, CreatedAtUtc)
    VALUES ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2', '11111111-1111-1111-1111-111111111111', '22222222-2222-2222-2222-222222222222', 2, 59876, SYSUTCDATETIME());
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.LapTimes WHERE Id = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3')
BEGIN
    INSERT INTO dbo.LapTimes (Id, RaceId, DriverId, LapNumber, TimeMilliseconds, CreatedAtUtc)
    VALUES ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3', '11111111-1111-1111-1111-111111111111', '33333333-3333-3333-3333-333333333333', 1, 61290, SYSUTCDATETIME());
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.LapTimes WHERE Id = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4')
BEGIN
    INSERT INTO dbo.LapTimes (Id, RaceId, DriverId, LapNumber, TimeMilliseconds, CreatedAtUtc)
    VALUES ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4', '11111111-1111-1111-1111-111111111111', '33333333-3333-3333-3333-333333333333', 2, 60777, SYSUTCDATETIME());
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.LapTimes WHERE Id = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5')
BEGIN
    INSERT INTO dbo.LapTimes (Id, RaceId, DriverId, LapNumber, TimeMilliseconds, CreatedAtUtc)
    VALUES ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5', '11111111-1111-1111-1111-111111111111', '44444444-4444-4444-4444-444444444444', 1, 59010, SYSUTCDATETIME());
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.LapTimes WHERE Id = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa6')
BEGIN
    INSERT INTO dbo.LapTimes (Id, RaceId, DriverId, LapNumber, TimeMilliseconds, CreatedAtUtc)
    VALUES ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa6', '11111111-1111-1111-1111-111111111111', '44444444-4444-4444-4444-444444444444', 2, 58890, SYSUTCDATETIME());
END;
GO

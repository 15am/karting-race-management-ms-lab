USE [KartingRaceManagement];
GO

CREATE OR ALTER PROCEDURE dbo.sp_GetFastestLap
    @RaceId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
        lap.RaceId,
        lap.DriverId,
        driver.Name AS DriverName,
        lap.LapNumber,
        lap.TimeMilliseconds
    FROM dbo.LapTimes AS lap
    INNER JOIN dbo.Drivers AS driver
        ON driver.Id = lap.DriverId
    WHERE lap.RaceId = @RaceId
    ORDER BY
        lap.TimeMilliseconds ASC,
        lap.LapNumber ASC,
        driver.Name ASC,
        lap.DriverId ASC;
END;
GO

USE [KartingRaceManagement];
GO

CREATE OR ALTER PROCEDURE dbo.sp_GetRaceResults
    @RaceId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH Aggregates AS
    (
        SELECT
            lap.DriverId,
            driver.Name AS DriverName,
            SUM(lap.TimeMilliseconds) AS TotalTimeMilliseconds,
            COUNT(1) AS LapCount
        FROM dbo.LapTimes AS lap
        INNER JOIN dbo.Drivers AS driver
            ON driver.Id = lap.DriverId
        WHERE lap.RaceId = @RaceId
        GROUP BY
            lap.DriverId,
            driver.Name
    )
    SELECT
        ROW_NUMBER() OVER (
            ORDER BY
                aggregate_result.TotalTimeMilliseconds ASC,
                aggregate_result.DriverName ASC,
                aggregate_result.DriverId ASC
        ) AS [Position],
        aggregate_result.DriverId,
        aggregate_result.DriverName,
        aggregate_result.TotalTimeMilliseconds,
        aggregate_result.LapCount
    FROM Aggregates AS aggregate_result
    ORDER BY [Position];
END;
GO

/*
  Stage 5 placeholder for Stage 6 implementation.
  Contract target:
  - Input: @RaceId UNIQUEIDENTIFIER
  - Output: position, driver id/name, total time, lap count
*/

USE [KartingRaceManagement];
GO

CREATE OR ALTER PROCEDURE dbo.usp_GetRaceResults
    @RaceId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    /*
      Stage 6 TODO:
      - Aggregate lap times grouped by driver
      - Order by total time ascending
      - Return deterministic position ordering
    */
    SELECT
        CAST(0 AS INT) AS [Position],
        CAST('00000000-0000-0000-0000-000000000000' AS UNIQUEIDENTIFIER) AS DriverId,
        CAST(N'' AS NVARCHAR(120)) AS DriverName,
        CAST(0 AS INT) AS TotalTimeMilliseconds,
        CAST(0 AS INT) AS LapCount
    WHERE 1 = 0;
END;
GO

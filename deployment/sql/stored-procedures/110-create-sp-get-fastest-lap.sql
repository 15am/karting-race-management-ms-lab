/*
  Stage 5 placeholder for Stage 6 implementation.
  Contract target:
  - Input: @RaceId UNIQUEIDENTIFIER
  - Output: race id, driver id/name, lap number, time milliseconds
*/

USE [KartingRaceManagement];
GO

CREATE OR ALTER PROCEDURE dbo.usp_GetFastestLap
    @RaceId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    /*
      Stage 6 TODO:
      - Select minimum lap time for the target race
      - Return deterministic tie-breaking behavior
    */
    SELECT
        @RaceId AS RaceId,
        CAST('00000000-0000-0000-0000-000000000000' AS UNIQUEIDENTIFIER) AS DriverId,
        CAST(N'' AS NVARCHAR(120)) AS DriverName,
        CAST(0 AS INT) AS LapNumber,
        CAST(0 AS INT) AS TimeMilliseconds
    WHERE 1 = 0;
END;
GO

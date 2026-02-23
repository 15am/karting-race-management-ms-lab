export interface RaceResultItem {
  position: number
  driverId: string
  driverName: string
  totalTimeMilliseconds: number
  lapCount: number
}

export interface GetRaceResultsResponse {
  results: RaceResultItem[]
}

export interface FastestLapResponse {
  raceId: string
  driverId: string
  driverName: string
  lapNumber: number
  timeMilliseconds: number
}

export interface ProblemDetails {
  title?: string
  detail?: string
}

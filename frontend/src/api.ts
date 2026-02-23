import type {
  FastestLapResponse,
  GetRaceResultsResponse,
  ProblemDetails,
} from './types'

const baseUrl = (import.meta.env.VITE_API_BASE_URL ?? '/api').trim()

const toEndpoint = (path: string): string => {
  if (baseUrl.length === 0) {
    return path
  }

  const normalizedBase = baseUrl.endsWith('/') ? baseUrl.slice(0, -1) : baseUrl
  return `${normalizedBase}${path}`
}

const createRequestError = async (response: Response): Promise<Error> => {
  let details: ProblemDetails | null = null
  try {
    details = (await response.json()) as ProblemDetails
  } catch {
    return new Error(`Request failed with status ${response.status}.`)
  }

  const message = details.detail ?? details.title ?? `Request failed with status ${response.status}.`
  return new Error(message)
}

export const fetchRaceResults = async (
  raceId: string,
): Promise<GetRaceResultsResponse> => {
  const response = await fetch(toEndpoint(`/races/${raceId}/results`))
  if (!response.ok) {
    throw await createRequestError(response)
  }

  return (await response.json()) as GetRaceResultsResponse
}

export const fetchFastestLap = async (
  raceId: string,
): Promise<FastestLapResponse> => {
  const response = await fetch(toEndpoint(`/races/${raceId}/fastest-lap`))
  if (!response.ok) {
    throw await createRequestError(response)
  }

  return (await response.json()) as FastestLapResponse
}

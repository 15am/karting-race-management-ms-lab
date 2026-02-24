import { useMemo, useState } from 'react'
import type { FormEvent } from 'react'
import { fetchFastestLap, fetchRaceResults } from './api'
import type { FastestLapResponse, RaceResultItem } from './types'
import './App.css'

const seededTestIds = [
  { label: 'Race ID', value: '11111111-1111-1111-1111-111111111111' },
  { label: 'Driver ID 1', value: '22222222-2222-2222-2222-222222222222' },
  { label: 'Driver ID 2', value: '33333333-3333-3333-3333-333333333333' },
  { label: 'Driver ID 3', value: '44444444-4444-4444-4444-444444444444' },
  { label: 'Lap ID 1', value: 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1' },
  { label: 'Lap ID 2', value: 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2' },
]

const formatMilliseconds = (value: number): string => {
  const seconds = Math.floor(value / 1000)
  const milliseconds = value % 1000
  return `${seconds}.${milliseconds.toString().padStart(3, '0')} s`
}

function App() {
  const [raceIdInput, setRaceIdInput] = useState('')
  const [selectedRaceId, setSelectedRaceId] = useState<string>('')
  const [results, setResults] = useState<RaceResultItem[]>([])
  const [fastestLap, setFastestLap] = useState<FastestLapResponse | null>(null)
  const [isLoading, setIsLoading] = useState(false)
  const [errorMessage, setErrorMessage] = useState<string | null>(null)

  const hasData = useMemo(
    () => results.length > 0 || fastestLap !== null,
    [results.length, fastestLap],
  )

  const loadRaceAggregates = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    const raceId = raceIdInput.trim()
    if (raceId.length === 0) {
      setErrorMessage('Provide a race id before searching.')
      return
    }

    setIsLoading(true)
    setErrorMessage(null)

    try {
      const [resultsResponse, fastestLapResponse] = await Promise.all([
        fetchRaceResults(raceId),
        fetchFastestLap(raceId),
      ])

      setSelectedRaceId(raceId)
      setResults(resultsResponse.results)
      setFastestLap(fastestLapResponse)
    } catch (error) {
      const message =
        error instanceof Error ? error.message : 'Unexpected error while loading race data.'
      setErrorMessage(message)
      setResults([])
      setFastestLap(null)
    }

    setIsLoading(false)
  }

  return (
    <main className="app-shell">
      <section className="panel panel--hero">
        <p className="eyebrow">Karting Race Management</p>
        <h1>Race Results Explorer</h1>
        <p className="subtitle">
          Load race standings and fastest-lap details from the backend aggregate endpoints.
        </p>
      </section>

      <section className="panel panel--form">
        <form className="search-form" onSubmit={loadRaceAggregates}>
          <label htmlFor="race-id">Race Id</label>
          <div className="input-row">
            <input
              id="race-id"
              type="text"
              value={raceIdInput}
              onChange={(event) => setRaceIdInput(event.target.value)}
              placeholder="e.g. 11111111-1111-1111-1111-111111111111"
            />
            <button type="submit" disabled={isLoading}>
              {isLoading ? 'Loading...' : 'Load race'}
            </button>
          </div>
        </form>
        {errorMessage ? <p className="status status--error">{errorMessage}</p> : null}
      </section>

      <section className="panel panel--ids">
        <h2>Test IDs</h2>
        <p className="status">Use these seeded values for copy/paste testing.</p>
        <ul className="id-list">
          {seededTestIds.map((item) => (
            <li key={item.value}>
              <strong>{item.label}:</strong> <code>{item.value}</code>
            </li>
          ))}
        </ul>
      </section>

      {selectedRaceId ? (
        <section className="panel panel--meta">
          <strong>Selected race:</strong> {selectedRaceId}
        </section>
      ) : null}

      {hasData ? (
        <section className="results-grid">
          <article className="panel panel--table">
            <h2>Race Results</h2>
            {results.length === 0 ? (
              <p className="status">No lap data found for this race.</p>
            ) : (
              <table>
                <thead>
                  <tr>
                    <th>Pos</th>
                    <th>Driver</th>
                    <th>Total Time</th>
                    <th>Laps</th>
                  </tr>
                </thead>
                <tbody>
                  {results.map((item) => (
                    <tr key={item.driverId}>
                      <td>#{item.position}</td>
                      <td>{item.driverName}</td>
                      <td>{formatMilliseconds(item.totalTimeMilliseconds)}</td>
                      <td>{item.lapCount}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            )}
          </article>

          <article className="panel panel--fastest">
            <h2>Fastest Lap</h2>
            {fastestLap ? (
              <dl>
                <dt>Driver</dt>
                <dd>{fastestLap.driverName}</dd>
                <dt>Lap Number</dt>
                <dd>{fastestLap.lapNumber}</dd>
                <dt>Lap Time</dt>
                <dd>{formatMilliseconds(fastestLap.timeMilliseconds)}</dd>
              </dl>
            ) : (
              <p className="status">No fastest lap available for this race.</p>
            )}
          </article>
        </section>
      ) : (
        <section className="panel panel--empty">
          <p className="status">Enter a race id to view aggregate race data.</p>
        </section>
      )}
    </main>
  )
}

export default App

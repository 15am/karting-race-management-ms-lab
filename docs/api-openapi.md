# API OpenAPI Access

The API now exposes Microsoft OpenAPI output at:

- `GET /openapi/v1.json`

With deployment defaults:

- `http://localhost:8080/openapi/v1.json`

## Interaction option with Microsoft HTTP REPL

Install once:

`dotnet tool install --global Microsoft.dotnet-httprepl`

Connect using OpenAPI metadata:

`httprepl http://localhost:8080 --openapi /openapi/v1.json`

Then execute endpoint calls interactively:

- `get /races/{raceId}/results`
- `get /races/{raceId}/fastest-lap`
- `post /drivers`

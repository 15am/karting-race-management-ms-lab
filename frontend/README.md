# Frontend (Dockerized Node Workflow)

This frontend is intentionally runnable without local `node` or `npm`.

## Prerequisites

- Docker installed and running

## Commands

Run commands from `frontend/`:

- Install dependencies:
  `./scripts/npm-docker.sh install`
- Start dev server:
  `./scripts/npm-docker.sh run dev`
- Build:
  `./scripts/npm-docker.sh run build`
- Lint:
  `./scripts/npm-docker.sh run lint`

Default dev port is `5173`.

Use a custom port:

`FRONTEND_PORT=5174 ./scripts/npm-docker.sh run dev`

## Runtime configuration

Copy `.env.example` to `.env` when you need custom values.

- `VITE_API_BASE_URL`
  - Default: `/api`
  - Use `/api` for local dev proxy mode.
- `VITE_DEV_API_PROXY_TARGET`
  - Default: `http://localhost:8080`
  - Backend target used by Vite dev server proxy.

## CORS note

Local development uses Vite proxy forwarding (`/api` -> backend target), so API CORS changes are not required for the default workflow.

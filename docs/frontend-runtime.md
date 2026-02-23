# Frontend Runtime Notes

Stage 7 frontend runs with Dockerized Node commands from `frontend/`.

## Local flow

1. Start backend API (`api` service) so `http://localhost:8080` is reachable.
2. In `frontend/`, install dependencies:
   `./scripts/npm-docker.sh install`
3. Run dev server:
   `./scripts/npm-docker.sh run dev`

## Environment

- `VITE_API_BASE_URL=/api` (default)
- `VITE_DEV_API_PROXY_TARGET=http://localhost:8080` (default)

`vite.config.ts` proxies `/api` to the backend target and rewrites the `/api` prefix.

This keeps browser calls same-origin in dev mode and avoids mandatory API CORS changes for the default setup.

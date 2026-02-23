# NOTES

## Design decisions

1. Vertical slices and clean boundaries
- Endpoint behavior is modeled by feature handlers and ports.
- Infrastructure adapters implement storage, query, and cache contracts.

2. SQL Server first with provider-ready direction
- SQL Server is the current concrete provider.
- Query behavior is exposed through application ports to allow future provider adapters.

3. Stored procedures for aggregate reads
- Race results and fastest lap are computed through stored procedures.
- Deterministic ordering rules are implemented in SQL for stable output.

4. Cache strategy
- Aggregate read endpoints use distributed cache abstractions.
- `POST /laptimes` invalidates both aggregate cache keys.

5. Frontend runtime without local Node dependency
- Frontend commands use Dockerized Node wrappers.
- Deployment compose includes a frontend service for end-to-end testing.

6. OpenAPI access path
- API exposes Microsoft OpenAPI document output via `/openapi/v1.json`.
- HTTP REPL can consume the document for interactive API calls.

## Security and dependency notes

1. Frontend npm remediation
- `npm audit` high-severity issue was remediated by pinning a safe transitive version using:
  - `frontend/package.json` overrides: `minimatch@^10.2.1`
- Lockfile was refreshed after remediation.

2. Deployment environment
- SQL `SA_PASSWORD` must be replaced from default before runtime startup.
- AppArmor compatibility is explicitly configured in compose services.

## Tradeoffs

1. Frontend compose service uses Vite dev server
- Optimized for quick testability and parity with local frontend workflow.
- Not intended as final production static-serving strategy.

2. OpenAPI exposure is document-first
- Built-in OpenAPI endpoint is enabled for tooling and contract use.
- No additional third-party UI was introduced in this stage.

3. Stage script limitations
- `validate-stage.sh` in this repository currently assumes root-level solution discovery.
- Practical validation uses explicit solution path commands (`api/Karting.slnx`).

## Future work

1. Harden frontend production serving (static build image + reverse proxy strategy).
2. Add authenticated API access and role-aware authorization.
3. Expand DB-backed integration scenarios in CI with SQL service provisioning.
4. Add richer OpenAPI consumer workflow (UI and API examples).

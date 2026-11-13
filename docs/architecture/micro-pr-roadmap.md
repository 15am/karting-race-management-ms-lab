# Hoja de ruta de Micro-PRs: modular monolith a servicios autónomos

**Estado:** propuesta; migración no iniciada.
**Fork/base:** 15am/karting-race-management-ms-lab, development@588c220c3142d3261e1cbeb13e916aae0fcd24fe.
**ADR:** [0001 límites y propiedad de datos](../adr/0001-service-boundaries.md).

## Reglas de ejecución

- Todas las ramas/PRs nacen en el fork y apuntan al development del fork. Actualizar origin/development y exigir HEAD == SHA autorizado antes de crear rama.
- UM870 es predeterminado para cambios sencillos; UM890 para integración, datos y consistencia. Perfiles actuales: UM870 ctx 8192, UM890 ctx 16384, max_tokens 4096, temperatura 0; thinking desactivado en tareas rutinarias.
- Cada prompt define contrato, dependencias y archivos autorizados. Pedir salida limitada a archivo/método/parche; controlador identifica símbolo, verifica hashes/base y aplica en worktree aislado.
- No aplicar reemplazos globales por fragmentos de texto ni hacer doble escritura. Cada PR pasa diff check, pruebas, build y CI.
- Secretos fuera de Git; contenedores, puertos, volúmenes, redes y DBs aislados de upstream.
- No cortar tráfico ni instalar broker antes de aprobar gates de consistencia y operación.

## Micro-PRs propuestos

### PR-01 — CI y baseline reproducible
- **Objetivo/contrato:** PR ejecuta dotnet test de Karting.slnx con .NET 10, npm ci/lint/build, Compose config; test SQL se marca ejecutado o skip real. Agrega .env.example sanitizado y corrige runtime docs/preflight.
- **Dependencias:** ninguna; SHA base exacto. **Archivos:** .github/workflows/ci.yml, deployment/.env.example, validate-runtime.sh, tests/docs.
- **Worker/generación:** UM870 genera un workflow por archivo; controller verifica YAML/placeholders, nunca secretos.
- **Pruebas/aceptación:** CI verde, secret scan, compose config sin password real; SQL no aparece como pass si no ejecutó.
- **Docker:** SQL sólo en job aislado, con project/puerto/volumen propio.
- **Compatibilidad:** sin cambios API/DB. **Rollback:** revertir workflow/template/docs.

### PR-02 — Pruebas reales de SQL Server
- **Objetivo/contrato:** probar FK, constraints, SP, ranking, fastest lap e invalidación de caché en SQL real. InMemory queda para unit tests.
- **Dependencias:** PR-01 o fixture efímera equivalente. **Archivos:** Karting.Api.IntegrationTests, SQL scripts y fixture.
- **Worker/generación:** UM890 genera un escenario por cambio; controller valida IDs y destino.
- **Pruebas/aceptación:** IDs válidos pasan; desconocidos fallan por FK; SP y contrato coinciden; prueba muestra ejecución real.
- **Docker:** SQL efímero, sin volumen persistente/puerto compartido.
- **Compatibilidad:** sólo mejora evidencia. **Rollback:** quitar fixture si CI no puede provisionar, mantener unit tests.

### PR-03 — Redis y errores tras commit
- **Objetivo/contrato:** decidir usar Redis o retirarlo del Compose. Si se conserva, registrar su provider y probarlo; definir resultado HTTP si SQL commit pasa y cache invalidation falla, evitando errores ambiguos/reintentos duplicados.
- **Dependencias:** PR-01/02. **Archivos:** Program/Infrastructure DI, csproj, Compose, handlers/tests.
- **Worker/generación:** UM890 propone cambios DI y test limitados; controller comprueba provider efectivo y efecto del fallo.
- **Pruebas/aceptación:** hit/miss, invalidación, Redis apagado y persistencia real comprobados.
- **Docker:** Redis efímero con healthcheck y project isolation, si se retiene.
- **Compatibilidad:** mismas rutas/DTO/orden. **Rollback:** revertir wiring/config, sin tocar datos.

### PR-04 — Fronteras modulares en proceso
- **Objetivo/contrato:** formalizar Drivers y Competition (Race+Timing) dentro del mismo host y DB, sin distribución.
- **Dependencias:** PR-02/03. **Archivos:** Application features, ports, DI, architecture tests, OpenAPI contracts.
- **Worker/generación:** UM890 para movimientos pequeños, UM870 para tests simples; controller valida renames y dependencias, no regenera árboles.
- **Pruebas/aceptación:** Domain no depende de Infrastructure, ownership de acceso testeado, API/OpenAPI estable.
- **Docker:** ninguno nuevo.
- **Compatibilidad:** comportamiento actual intacto. **Rollback:** revertir movimientos sin DB migration.

### PR-05 — Contrato de Driver/Timing y nombres
- **Objetivo/contrato:** decidir nombre actual vs snapshot, alta reciente, ID desconocido, Registry caído, disponibilidad y staleness.
- **Dependencias:** PR-02/04 y decisión producto/operación. **Archivos:** ADR, API contract tests y docs.
- **Worker/generación:** Luna coordina; UM870 sólo genera casos tras cerrar reglas.
- **Pruebas/aceptación:** status code, retry, identidad y nombre quedan precisos.
- **Docker:** ninguno.
- **Compatibilidad:** todo cambio observable se aprueba antes. **Rollback:** sólo docs/tests hasta decisión.

### PR-06 — Outbox local y evento versionado
- **Objetivo/contrato:** si se decide extraer Registry, persistir DriverRegistered/cambios permitidos en misma transacción; event ID, schema version y ordering key explícitos.
- **Dependencias:** PR-05 y SQL tests. **Archivos:** tabla outbox, Application/Infrastructure, SQL scripts/tests.
- **Worker/generación:** UM890, un evento por intento; controller verifica hashes y schema aditivo/versionado.
- **Pruebas/aceptación:** driver+evento atómicos, rollback de ambos, dedupe y orden.
- **Docker:** sin broker aún; DB de test propia.
- **Compatibilidad:** HTTP sin cambio. **Rollback:** apagar escritor; tabla nueva vacía puede permanecer.

### PR-07 — Transporte de eventos
- **Objetivo/contrato:** elegir sólo tras fijar durabilidad, latencia, volumen, disponibilidad, entorno y operación.
- **Dependencias:** PR-06 y requisitos de deployment. **Archivos:** ADR, publisher adapter, Compose y runbook.
- **Worker/generación:** UM890 en adapter acotado; Sol/Terra por Herdr sólo ante incertidumbre arquitectónica material, no rutina.
- **Pruebas/aceptación:** caída/reinicio, retries/backoff, backlog, duplicados, poison messages y recuperación.
- **Docker:** broker con project name, puertos y volúmenes propios.
- **Compatibilidad:** HTTP intacto. **Rollback:** outbox sobrevive con publisher apagado.

### PR-08 — Proyección local de identidad
- **Objetivo/contrato:** Race/Timing mantiene en su DB el mínimo de identidad necesario para lecturas; consumo idempotente y reconciliable.
- **Dependencias:** PR-05/06/07. **Archivos:** schema de proyección, consumer, query ports y tests.
- **Worker/generación:** UM890; controller separa schema/consumer/query.
- **Pruebas/aceptación:** duplicado, fuera de orden, reconstrucción, lag y nombre probados con SQL/transporte.
- **Docker:** consumer aislado, sin acceso a DB Registry.
- **Compatibilidad:** JSON/orden de results y fastest-lap se conservan.
- **Rollback:** mantener lectura anterior hasta comparación; nuevo path tras feature flag.

### PR-09 — Driver Registry y DB propia
- **Objetivo/contrato:** proceso autónomo, único dueño de Drivers, health/readiness y config aislada.
- **Dependencias:** PR-01/04/05/06/07/08. **Archivos:** API/persistence nuevas, DI/tests, Compose/OpenAPI.
- **Worker/generación:** UM890; skeleton, API y storage en PRs separados y prompts de un handler/archivo.
- **Pruebas/aceptación:** contrato, SQL, readiness y aislamiento; sin conexiones a DB Race/Timing.
- **Docker:** retirar container_name globales, puertos configurables, DB/volumen propios, healthcheck, no exponer SQL.
- **Compatibilidad:** sin tráfico hasta cutover. **Rollback:** apagar perfil; monolith conserva autoridad.

### PR-10 — Backfill y transferencia de autoridad
- **Objetivo/contrato:** preservar IDs, reconciliar counts/hashes y resolver eventos concurrentes; Registry queda como único writer.
- **Dependencias:** PR-09, backup y ensayo. **Archivos:** scripts/tooling y runbook.
- **Worker/generación:** UM890 crea herramienta idempotente; controller corre sólo dry-run/fixtures antes de operación autorizada.
- **Pruebas/aceptación:** cero filas perdidas/duplicadas, reanudación, comparación y restore probados.
- **Docker:** DBs/volúmenes distintos, ningún recurso upstream.
- **Compatibilidad:** facade sigue apuntando a monolith hasta flag.
- **Rollback:** antes de nuevos writes se vuelve; después se requiere replay/freeze/reconciliación, no cambiar DNS solamente.

### PR-11 — Compatibilidad HTTP y cutover
- **Objetivo/contrato:** mantener /api/drivers/DTOs, enrutar a Registry, FK Race local y sin FK cross-DB Driver; aplicar regla de referencia aprobada.
- **Dependencias:** PR-08/10, backup, métricas y rollback. **Archivos:** facade/proxy, Compose, schema, e2e/contract tests.
- **Worker/generación:** UM890, un endpoint por cambio detrás de flag; controller valida schema y destino.
- **Pruebas/aceptación:** parity, Driver recién creado/desconocido, Registry offline, proyección atrasada, duplicados y retorno de resultados.
- **Docker:** servicios/DB separados, readiness, red mínima, secretos fuera de Git.
- **Compatibilidad:** rutas/JSON estables; staleness sólo tras aprobar PR-05.
- **Rollback:** volver a monolith sólo con writes post-cutover reconciliados.

### PR-12 — Observabilidad y retirada legacy
- **Objetivo/contrato:** medir errors, latency, outbox age, projection lag, cache invalidation y discrepancias; retirar legacy tras probar que no recibe tráfico.
- **Dependencias:** PR-11 estable durante periodo acordado. **Archivos:** logs/health/dashboards, scripts/docs.
- **Worker/generación:** UM870 para docs/telemetría; UM890 para integración, separar PRs.
- **Pruebas/aceptación:** alertas, restore, counts y suites SQL/contracts verdes.
- **Docker:** deployment/volúmenes por servicio documentados.
- **Compatibilidad:** API igual. **Rollback:** backup/rehidratación probados antes de borrar fallback.

### PR-13 — Results/Analytics opcional
- **Objetivo/contrato:** sólo si mediciones prueban escala/latencia propia; read model con DB propia sin consultas a Competition.
- **Dependencias:** PR-07/08/12 y umbral aprobado. **Archivos:** servicio read model, consumer, DB, contract/tests.
- **Worker/generación:** UM890; no generar hasta ADR y objetivo cuantitativo.
- **Pruebas/aceptación:** staleness explícita, rebuild, dedupe, ranking determinista y latencia bajo umbral.
- **Docker:** servicio/almacenamiento aislados y reconstruibles.
- **Compatibilidad:** facade conserva rutas; producto acepta frescura.
- **Rollback:** volver a query Competition mientras exista; no retirar temprano.

## Gates

1. Sin corte hasta pruebas SQL reales de FK, SP y migración.
2. Sin transferir Drivers hasta definir nombre, disponibilidad y alta reciente.
3. Sin elegir broker sin requisitos; ningún evento sin outbox/deduplicación.
4. Sin tráfico hasta backup, comparación, flag, métricas y rollback ensayado.
5. Sin separar Analytics sin mediciones.
6. Cada Micro-PR queda en fork, con SHA exacto, diff acotado y CI; esta fase termina antes de migrar código.

# Karting Race Management: preparación del laboratorio de microservicios

**Estado:** preparación, auditoría y planificación; migración no iniciada.
**Fecha:** 2026-11-13.
**Baseline de development:** 588c220c3142d3261e1cbeb13e916aae0fcd24fe.
**Fork:** [15am/karting-race-management-ms-lab](https://github.com/15am/karting-race-management-ms-lab), público y relacionado como fork del upstream.
**Rama de este informe:** planning/architecture-baseline. No se ha creado PR ni alterado development.

## Resumen ejecutivo

El repositorio es un monolito modular pequeño .NET 10. Tiene capas Api, Application, Domain e Infrastructure, con features Drivers, Races y LapTimes; ejecuta un API host, un DbContext y una sola base SQL Server. Los resultados y la vuelta más rápida leen LapTimes y Drivers juntos. Compose crea varios contenedores, pero no servicios autónomos con propiedad de datos.

La recomendación inicial es mantener Race y Timing juntos y conservar Drivers dentro del monolito hasta demostrar una necesidad de ciclo de vida, consumo, despliegue o escala independiente. Driver Registry es candidato futuro, no extracción inmediata. No convertir los proyectos técnicos en microservicios.

## Fork e aislamiento

| Campo | Evidencia |
|---|---|
| Upstream | https://github.com/jrenewhite/karting-race-management |
| Fork real | https://github.com/15am/karting-race-management-ms-lab |
| Propietario | Organización 15am; membresía activa y rol admin verificados por GitHub API |
| Visibilidad/parent | público; fork=true; parent y source son jrenewhite/karting-race-management |
| Default branch | development |
| SHA inicial | 588c220c3142d3261e1cbeb13e916aae0fcd24fe |
| Comparación al crear | identical; ahead_by=0; behind_by=0 |
| origin | git@github.com:15am/karting-race-management-ms-lab.git |
| upstream | git@github.com:jrenewhite/karting-race-management.git |

Antes de cada rama nueva desde development, actualizar origin/development y exigir igualdad exacta entre HEAD y el SHA autorizado. Si difiere, detener cambios y reconciliar el SHA. No hacer push ni PR al upstream.

El árbol rastreado no incluye archivos .env reales. Un barrido halló referencias documentales a archivos locales ignorados y valores de ejemplo, no credenciales conocidas. No se copiaron archivos del checkout homelabs.

## Baseline reproducible

### Backend .NET

El host tiene SDK 8.0.409, incompatible con net10.0. Ejecuté dotnet test Karting.slnx dentro del contenedor efímero mcr.microsoft.com/dotnet/sdk:10.0, montando sólo el clon y sin levantar servicios. Resultado: compilación correcta; 14 pruebas reportadas aprobadas, 0 fallidas: 6 Domain, 4 Application, 4 API integration. Dos de las cuatro pruebas API integration retornan temprano si KARTING_SQL_INTEGRATION no está en true; xUnit las reporta como aprobadas, pero no ejecutan SQL. Por tanto, fueron 12 pruebas con comportamiento y 2 no-op. Las otras dos usan WebApplicationFactory y EF InMemory. No se ejecutaron SQL Server real, sus FK, procedimientos almacenados ni invalidación de caché en SQL Server.

Restore emitió NU1902/NU1903 para Microsoft.OpenApi 2.0.0 y System.Security.Cryptography.Pkcs 9.0.4 transitivos. Advisories pertinentes: [GHSA-v5pm-xwqc-g5wc](https://github.com/microsoft/OpenAPI.NET/security/advisories/GHSA-v5pm-xwqc-g5wc), [GHSA-2fmv-rv2c-59x7](https://github.com/dotnet/runtime/security/advisories/GHSA-2fmv-rv2c-59x7) y [GHSA-846f-f764-76mx](https://github.com/dotnet/runtime/security/advisories/GHSA-846f-f764-76mx). Quedan para triage; no se actualizaron paquetes ni se afirma explotabilidad del uso actual.

### Frontend

En contenedor node:20 ejecuté npm ci, npm run lint y npm run build. Instalación lockfile, ESLint, TypeScript y Vite build pasaron. npm audit detectó 15 vulnerabilidades (2 low, 2 moderate, 11 high, 0 critical); Vite 7.3.1 es dependencia directa, las demás entradas del reporte son transitivas. No se ejecutó npm audit fix.

### Docker/runtime

- docker compose config --quiet pasó usando un valor SQL ficticio limitado al proceso.
- deployment/scripts/validate-runtime.sh falla con Missing env template: deployment/.env.example. No creé .env y no inicié Compose.
- Compose define frontend Vite, API, SQL Server, bootstrap SQL y Redis. Hay container_name fijos, además de puertos predeterminados 1433/6379/8080/4173; así no se pueden ejecutar fork y upstream simultáneamente de forma aislada sin cambios.
- Se desactivan healthchecks de SQL Server y Redis. API depende de service_started en SQL Server/Redis y de bootstrap completado; no espera readiness de Redis.
- docs/deployment-runtime.md afirma que existe .env.example y que hay healthchecks/readiness; contradice el código del SHA evaluado.
- Program.cs registra AddDistributedMemoryCache. No se encontró registro Redis ni paquete AddStackExchangeRedisCache. La cadena Redis llega por Compose, pero la aplicación no usa el contenedor Redis.
- Tags de imagen son móviles: node:20, dotnet/sdk:10.0, mssql/server:2022-latest, redis:7-alpine y mssql-tools. Fijarlos será un trabajo de reproducibilidad de despliegue.

## Estado arquitectónico auditado

| Área | Evidencia actual |
|---|---|
| API | ASP.NET Core .NET 10, controllers, middleware de excepciones y OpenAPI; no hay workflow CI versionado. |
| Application | Handlers y validadores verticales en Drivers/LapTimes/Races; puertos de persistencia y query. |
| Domain | Driver, Race y LapTime, Value Objects y RaceStatus Scheduled/InProgress/Completed. |
| Infrastructure | EF Core, un KartingDbContext, repositorios, stored procedures y adapter de IDistributedCache. |
| HTTP | POST /drivers, POST /races, POST /laptimes, GET /races/{id}/results y GET /races/{id}/fastest-lap. Vite consume /api. |
| SQL | Una base KartingRaceManagement y tablas Drivers/Races/LapTimes; FK LapTimes→Races/Drivers; índices para race/driver y race/time/lap. Scripts numerados, sin ledger de migraciones visible. |
| Queries | SP y fallback LINQ unen LapTimes a Drivers y leen Driver.Name en tiempo de consulta. |
| Cache | Keys por carrera; handler guarda LapTime y luego elimina keys results y fastest-lap, fuera de transacción común. |
| Mensajería | No hay outbox, bus, publishers ni consumers. |
| Frontend | React 19, Vite 7, TypeScript; lint/build, no suite de UI identificada. |
| Tests | Domain/Application y API InMemory; dos pruebas opt-in de SQL retornan como pass sin ejecutar DB. |

### Integridad de las vueltas

CreateLapTimeHandler no invoca IRaceRepository.ExistsAsync ni IDriverRepository.ExistsAsync, aunque los repositorios implementan ExistsAsync. SQL Server hace cumplir ambas referencias mediante FK al guardar. EF InMemory no reproduce esas FK. Al retirar FK_LapTimes_Drivers en una futura separación, hará falta una regla nueva de referencia, explícita y probada; no es simple reubicación de lógica existente.

### Semántica del nombre del piloto

No hay endpoints de edición o borrado de Driver. Los SP sí leen Driver.Name actual al consultar resultados. Falta acordar si una futura edición cambia resultados pasados, si la vuelta guarda snapshot o si se admite propagación eventual. Esta decisión bloquea el corte de ownership de Drivers.

## Propuesta inicial de Luna

1. Mantener un modular monolith durante las etapas de preparación.
2. Competition agrupa ciclo de vida de Race, captura de Timing/LapTime y sus resultados. La FK Race-LapTime y las lecturas agregadas siguen en una propiedad transaccional.
3. Drivers permanece módulo interno hasta que ciclo de vida, consumidores, escala o despliegue independiente justifiquen su extracción. Hoy sólo hay creación de Driver; el beneficio de separarlo es especulativo.
4. Destino futuro condicional: Driver Registry dueño exclusivo de su DB; Race/Timing dueño de Race, LapTime y consultas. Race/Timing mantiene una proyección local de identidad; ningún servicio consulta tablas ajenas ni crea FK entre bases.
5. Conservar rutas y DTOs mediante el API host/facade durante el encaminamiento gradual. Cambios de consistencia eventual son cambios visibles del contrato y requieren acuerdo.
6. Separar Results/Analytics sólo si métricas muestran escala, latencia o ciclo de despliegue independiente.

La [guía .NET de soberanía de datos por microservicio](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/architect-microservice-container-applications/data-sovereignty-per-microservice) fundamenta propiedad autónoma. Para eventos, ver [mensajería asíncrona .NET](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/architect-microservice-container-applications/asynchronous-message-based-communication): publicación confiable, outbox, idempotencia y deduplicación. No elegí broker.

## Consultoría y coordinación

Antes de la aclaración del usuario, se realizó una consulta Sol (gpt-6-sol, medium) mediante delegación interna de Codex. La recomendación fue mantener Race+Timing juntos y condicionar Driver Registry. Se corrigió con código fuente un supuesto del primer mensaje: el handler no llama ExistsAsync; SQL FK garantiza integridad y tests InMemory no lo hacen. Sol confirmó que eliminar la FK exige crear una regla funcional nueva. Valor: identifica el riesgo de integridad y evita describir una validación inexistente.

Esta consulta se hizo fuera de Herdr y cuenta como 1 de las 3 autorizadas; no se repetirá para duplicar la misma respuesta. La guía local Herdr 0.9.5 requiere HERDR_ENV=1 para controlar panes; esta sesión reporta HERDR_ENV ausente. Por ello, no se contactó todavía a Luna ThinkPad ni a Sol/Terra por Herdr. Queda pendiente realizar esa coordinación desde un pane Herdr conforme al esquema del usuario. Inferencia local no usada para generación en esta fase; no hay tokens, energía o memoria de workers medidos.

## Cuotas y eficiencia

Consulta real mediante codex app-server account/rateLimits/read. Snapshot registrado al comenzar el análisis: ventana de 5 h 2% usada y semanal 1% usada; por tanto 98% y 99% disponibles. Plan Plus, ordinaryUsageAllowed=true. La respuesta informaba ventanas de 300 y 10080 minutos; el informe final agregará el segundo snapshot. Los umbrales de reserva internos son 30% y 40% disponibles; no se alcanzaron. No se inventan fechas de reinicio.

Esfuerzo determinista: creación/verificación del fork, revisión de fuente, build/test .NET en SDK 10, npm ci/lint/build/audit y validación Compose. Consultoría costosa: una consulta Sol vía Codex (fuera del flujo Herdr planeado, no se repetirá). Workers locales: no usados en esta fase de diagnóstico/documentación; por ello no hay métricas locales de tokens/energía.

## Decisiones pendientes

- ¿Se permitirá editar, fusionar o eliminar Driver?
- ¿Resultados muestran nombre actual, snapshot por vuelta o nombre al terminar carrera?
- ¿POST /laptimes acepta inmediatamente un Driver recién creado aunque la proyección todavía no lo conozca?
- ¿Qué disponibilidad/latencia se necesita para captura de tiempos y consultas?
- ¿Qué necesidad observable justifica desplegar Driver Registry por separado?
- ¿Dónde se alojarán los servicios y cómo se operarán secrets, TLS, backups, observabilidad y recuperación?
- ¿Qué staleness de proyección/cache se acepta?

## Estado

Fork real, público, relación upstream, remotos y SHA inicial verificados. Baseline backend/frontend terminado; SQL real pendiente. Compose config pasó y preflight falla por env template ausente; no se inició servicio. Upstream no tiene CI. Migración no iniciada. No se modificó upstream, Blueprint, PocketBoard, servicios permanentes ni modelos del homelab.

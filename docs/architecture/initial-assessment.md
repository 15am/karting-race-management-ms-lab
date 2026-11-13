# Karting Race Management: preparación del laboratorio de microservicios

**Estado:** preparación, auditoría y planificación; migración no iniciada.
**Fecha:** 2026-11-13.
**Baseline de development:** 588c220c3142d3261e1cbeb13e916aae0fcd24fe.
**Fork:** [15am/karting-race-management-ms-lab](https://github.com/15am/karting-race-management-ms-lab), público y relacionado como fork del upstream.
**Rama de este informe:** planning/architecture-baseline. No se ha creado PR ni alterado development.

## Resumen ejecutivo

El repositorio es un monolito modular pequeño .NET 10. Tiene capas Api, Application, Domain e Infrastructure, con features Drivers, Races y LapTimes; ejecuta un API host y un DbContext. El proveedor predeterminado es EF Core InMemory; SQL Server se selecciona con `Database:Provider=SqlServer`. En ese modo SQL, una sola base aloja las tablas. Los resultados y la vuelta más rápida leen LapTimes y Drivers juntos. Compose crea varios contenedores, pero no servicios autónomos con propiedad de datos.

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
| SQL | En el modo SQL Server, una base KartingRaceManagement y tablas Drivers/Races/LapTimes; FK LapTimes→Races/Drivers; índices para race/driver y race/time/lap. El proveedor predeterminado del host es InMemory. Scripts numerados, sin ledger de migraciones visible. |
| Queries | SP y fallback LINQ unen LapTimes a Drivers y leen Driver.Name en tiempo de consulta. |
| Cache | Keys por carrera; handler guarda LapTime y luego elimina keys results y fastest-lap, fuera de transacción común. |
| Mensajería | No hay outbox, bus, publishers ni consumers. |
| Frontend | React 19, Vite 7, TypeScript; lint/build, no suite de UI identificada. |
| Tests | Domain/Application y API InMemory; dos pruebas opt-in de SQL retornan como pass sin ejecutar DB. |

### Integridad de las vueltas

CreateLapTimeHandler no invoca IRaceRepository.ExistsAsync ni IDriverRepository.ExistsAsync, aunque los repositorios implementan ExistsAsync. SQL Server hace cumplir ambas referencias mediante FK al guardar; con proveedor InMemory esas FK no existen. En el modo SQL, la violación llega como excepción EF/SQL y el middleware genérico responde 500 `Unexpected error`; no se convierte en el 404 que declara el controller. Antes de retirar FK_LapTimes_Drivers o depender de ella como contrato, definir y probar explícitamente la respuesta HTTP para raceId/driverId desconocidos tanto en InMemory como en SQL Server.

### Semántica del nombre del piloto

No hay endpoints de edición o borrado de Driver. Los SP y el fallback LINQ leen Driver.Name actual al consultar resultados; el agrupamiento y orden por nombre hacen que un cambio pueda alterar presentación y desempates. Falta acordar si una futura edición cambia resultados pasados, si la vuelta guarda snapshot o si se admite propagación eventual, cuál lectura es autoritativa durante backfill/cutover y cómo se reconcilia. Esta decisión bloquea el corte de ownership de Drivers.

### Contrato HTTP

Los controllers exponen rutas canónicas `/drivers`, `/races` y `/laptimes` (además de las rutas de consultas de carreras). El prefijo `/api` lo añade el cliente Vite y su proxy de desarrollo lo elimina; no se encontró evidencia de que `/api/drivers` sea una ruta pública estable en despliegue. Antes de prometer compatibilidad, decidir rutas canónicas del host, responsabilidad del gateway/proxy y pruebas de compatibilidad en desarrollo y despliegue.

La caché tiene dos riesgos distintos para cualquier cambio de persistencia/distribución: la invalidación sucede después de `SaveChangesAsync`, sin transacción compartida con el commit; y lecturas ya servidas desde caché pueden quedar obsoletas. El plan debe especificar y probar ambos modos de fallo; separar servicios por sí solo no cierra esa ventana.

## Propuesta inicial de Luna

1. Mantener un modular monolith durante las etapas de preparación.
2. Competition agrupa ciclo de vida de Race, captura de Timing/LapTime y sus resultados. La FK Race-LapTime y las lecturas agregadas siguen en una propiedad transaccional.
3. Drivers permanece módulo interno hasta que ciclo de vida, consumidores, escala o despliegue independiente justifiquen su extracción. Hoy sólo hay creación de Driver; el beneficio de separarlo es especulativo.
4. Destino futuro condicional: Driver Registry dueño exclusivo de su DB; Race/Timing dueño de Race, LapTime y consultas. Race/Timing mantiene una proyección local de identidad; ningún servicio consulta tablas ajenas ni crea FK entre bases.
5. Conservar rutas y DTOs mediante el API host/facade durante el encaminamiento gradual. Cambios de consistencia eventual son cambios visibles del contrato y requieren acuerdo.
6. Separar Results/Analytics sólo si métricas muestran escala, latencia o ciclo de despliegue independiente.

La extracción de Registry queda bloqueada hasta acordar: edición/fusión/borrado de Driver; fuente del nombre y regla de desempate en resultados; aceptación de drivers recién creados aún no proyectados; respuesta cuando Registry está caído, la proyección atrasada o el ID es desconocido; staleness tolerable sin alterar ranking; y transferencia única de autoridad de escritura con política de reconciliación de escrituras concurrentes.

La [guía .NET de soberanía de datos por microservicio](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/architect-microservice-container-applications/data-sovereignty-per-microservice) fundamenta propiedad autónoma. Para eventos, ver [mensajería asíncrona .NET](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/architect-microservice-container-applications/asynchronous-message-based-communication): publicación confiable, outbox, idempotencia y deduplicación. No elegí broker.

## Consultoría y coordinación

Antes de la aclaración del usuario, se realizó una consulta Sol (gpt-6-sol, medium) mediante delegación interna de Codex. La recomendación fue mantener Race+Timing juntos y condicionar Driver Registry. Se corrigió con código fuente un supuesto del primer mensaje: el handler no llama ExistsAsync; SQL FK garantiza integridad y tests InMemory no lo hacen. Sol confirmó que eliminar la FK exige crear una regla funcional nueva. Valor: identifica el riesgo de integridad y evita describir una validación inexistente.

La revisión de Luna en ThinkPad se realizó desde Herdr 0.9.5, en el agente existente `luna-pm`, como revisión de sólo lectura del branch público y el baseline exacto `588c220c3142d3261e1cbeb13e916aae0fcd24fe`. No editó ni ejecutó código. Señaló tres correcciones factuales (proveedor SQL no predeterminado, FK inválida termina en 500 en modo SQL, y `/api/drivers` no es ruta canónica demostrada) y tres riesgos (semántica de nombre/resultados durante cutover, dos ventanas de caché, y decisiones de producto/consistencia pendientes). Su recomendación fue añadir una matriz HTTP para IDs desconocidos en ambos proveedores, congelar la especificación de resultados antes de PR-05 y explicitar rutas/gateway antes de PR-11. Confirmó que mantener Race+Timing juntos está bien justificado.

Esta revisión en Herdr completa la coordinación pendiente en el flujo habitual; no se pidió a Sol/Terra por Herdr porque la revisión no dejó una incertidumbre arquitectónica que lo requiriera. La consulta Sol previa cuenta como 1 de las 3 autorizadas y no se repitió. La guía requería acceder al agente remoto por perfil Herdr; el perfil `thinkpad` estaba disponible y respondió. No se generó código con workers locales en esta preparación; no hay tokens, energía o memoria de workers medidos.

## Cuotas y eficiencia

Consulta real mediante codex app-server account/rateLimits/read. Snapshot inicial de esta preparación: ventana de 5 h 2% usada y semanal 1% usada (98% y 99% disponibles; no se conservó la hora exacta). Snapshot final, 2026-11-13 09:47 UTC: ventana de 5 h 4% usada y semanal 2% usada (96% y 98% disponibles), Plan Plus y ordinaryUsageAllowed=true. Ventanas reportadas: 300 y 10080 minutos. Próximos reinicios reportados por la interfaz: 2026-11-13 13:29 UTC y 2026-11-20 06:56 UTC. Reservas internas: 30% y 40% disponibles; no se alcanzaron. No se inventaron cuotas ni fechas.

Esfuerzo determinista: creación/verificación del fork, revisión de fuente, build/test .NET en SDK 10, npm ci/lint/build/audit y validación Compose. Consultoría: una revisión Luna vía Herdr ThinkPad y una consulta Sol vía Codex. Workers locales: no usados para generación en esta fase de diagnóstico/documentación; por ello no hay métricas locales de tokens/energía.

## Decisiones pendientes

- ¿Se permitirá editar, fusionar o eliminar Driver?
- ¿Resultados muestran nombre actual, snapshot por vuelta o nombre al terminar carrera?
- ¿Qué contrato HTTP (status/cuerpo) aplica a raceId/driverId desconocidos en los proveedores InMemory y SQL Server?
- ¿POST /laptimes acepta inmediatamente un Driver recién creado aunque la proyección todavía no lo conozca?
- ¿Qué disponibilidad/latencia se necesita para captura de tiempos y consultas?
- ¿Qué necesidad observable justifica desplegar Driver Registry por separado?
- ¿Dónde se alojarán los servicios y cómo se operarán secrets, TLS, backups, observabilidad y recuperación?
- ¿Qué staleness de proyección/cache se acepta, y puede afectar ranking o desempates?
- ¿Durante backfill/cutover qué fuente es autoritativa para nombres/results y cómo se reconcilian escrituras concurrentes?
- ¿Las rutas canónicas públicas son `/drivers` o habrá gateway que garantice `/api/drivers`?

## Estado

Fork real, público, relación upstream, remotos y SHA inicial verificados. Baseline backend/frontend terminado; SQL real pendiente. Compose config pasó y preflight falla por env template ausente; no se inició servicio. Upstream no tiene CI. Migración no iniciada. No se modificó upstream, Blueprint, PocketBoard, servicios permanentes ni modelos del homelab.

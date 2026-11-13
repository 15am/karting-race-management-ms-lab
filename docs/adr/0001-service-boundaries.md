# ADR 0001: límites iniciales y propiedad de datos

- **Estado:** aceptado para la preparación; extracción pendiente de requisitos.
- **Fecha:** 2026-11-13.
- **Baseline:** 588c220c3142d3261e1cbeb13e916aae0fcd24fe.

## Contexto

El repositorio separa capas técnicas y features, pero opera un API host y una DB. LapTimes tiene FK a Races y Drivers; queries y stored procedures unen LapTimes con Drivers. El handler guarda la vuelta y luego invalida dos keys cache. No hay outbox ni transporte de eventos.

CreateLapTimeHandler no invoca ExistsAsync para Race o Driver. Las FK SQL garantizan las referencias existentes; tests InMemory no reproducen esa garantía.

## Decisión

Mantener Drivers, Races y Timing como módulos dentro del mismo deploy mientras se fijan contratos y pruebas. Race y Timing comparten propiedad de datos porque la FK local y las queries de vueltas/resultados dependen de ella.

Driver Registry es un candidato futuro, no un servicio que se extrae ya. Extraerlo sólo ante una necesidad demostrada de ciclo de vida, consumidores, despliegue o escala autónoma. Si se hace, Registry será dueño único de Driver y Race/Timing de Race, LapTime y resultados. No habrá tablas compartidas ni FK entre DBs.

## Consecuencias

- Las capas .NET no se transforman directamente en servicios.
- Se conservan rutas/DTOs durante extracción.
- Eliminar FK_LapTimes_Drivers requiere una regla explícita nueva para IDs; hoy no hay validación de referencia en el handler.
- Una proyección local de Driver necesita eventos confiables, deduplicación, orden/reconciliación y semántica de nombre.
- El transporte queda sin elegir hasta conocer operación y disponibilidad.
- Results no será un servicio aparte sin métricas que prueben necesidad.

## Alternativas consideradas

1. Un servicio por proyecto .NET: rechazado; son capas técnicas, no contextos autónomos.
2. Race y Timing separados ahora: rechazado; comparten integridad y lectura.
3. Extraer Driver Registry ahora: aplazado; sólo hay POST de Driver, sin beneficio operacional medido.
4. Mantener monolito como arquitectura cerrada: no adoptado; Registry seguirá como opción ante nueva evidencia.

Reabrir si hay edición/baja de pilotos, nuevos consumidores de identidad, o escala/despliegue distintos. Antes del corte de Drivers, acordar si resultados muestran nombre actual o snapshot y qué pasa con una vuelta mientras se propaga la identidad.

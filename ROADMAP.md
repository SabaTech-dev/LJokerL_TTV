# Roadmap — LJokerL_TTV Custom Config PRO

> Última revisión: 27/10/2026 (auditoría documental de octubre de 2026).
> Documento vivo: el backlog operativo vigente es [TODO.md](TODO.md); este roadmap describe el estado real del producto y la dirección declarada.

## Estado actual (verificado)

**LJokerL_TTV v13.0.0** es una aplicación de escritorio **C#/.NET 9 con WPF (patrón MVVM)**, publicada en GitHub (`SabaTech-dev/LJokerL_TTV`) con integración continua (GitHub Actions: build Release + suite de pruebas en cada push/PR a `main`/`develop`).

Existe hoy, verificado contra el árbol de fuentes:

- **Aplicación WPF** (`src/LJokerL.App`): `MainWindow.xaml` con 7 pestañas — Dashboard, Auto Config, Perfiles, Tweaks, Monitor, Benchmarks, Settings. Solo **Dashboard** y **Benchmarks** tienen vista implementada; las otras 5 pestañas carecen aún de vista dedicada.
- **ViewModel** (`MainViewModel.cs`): aplicación y reversión de perfiles y tweaks individuales, detección de hardware (CPU/GPU/RAM), monitor de rendimiento con registro de métricas, sistema de logs JSONL con paginación y exportación, y sistema de benchmarks (rápido/completo, comparación y exportación de informes).
- **Núcleo** (`src/LJokerL.Core`): `ConfigLoader` (catálogos JSON), `ExecutionEngine` (acciones directas de registro/sistema), `HardwareDetector` (instantánea de hardware y reglas), `ExecutionLogger` (JSONL + snapshot para rollback) y `BenchmarkRunner`.
- **Catálogos** (`Config/`): 14 reglas de hardware, 10 perfiles, 92 tweaks con reversión definida, 13 servicios y 23 aplicaciones de bloatware.
- **Pruebas**: suite propia (`src/LJokerL.Tests`, runner de consola) con 28 pruebas sobre modelos, detector de hardware, catálogos, perfiles, auto-config y sistema de benchmarks; la ejecuta el pipeline de CI.
- **CI**: `.github/workflows/ci.yml` — `dotnet build -c Release` y `dotnet run --project src/LJokerL.Tests` en `windows-latest` con .NET 9.

**Stack tecnológico actual**: C#/.NET 9 · WPF + MVVM · catálogos JSON · GitHub Actions (CI) · Windows 10/11 con privilegios de administrador (PowerShell invocado en tiempo de ejecución por siete componentes y también para empaquetado; la lógica de la aplicación es C#/.NET; ver DEPENDENCIES.md).

## Backlog vivo

El plan de trabajo vigente es **[TODO.md](TODO.md)**, derivado de la auditoría de documentación y configuración del 27/10/2026 (`Reports/Auditoria_Documentacion_Configuracion_2026-10-27.md`), con tres fases:

- **P0 — Crítico**: coherencia documental, Git/GitHub, CI (en curso; varios ítems ya ejecutados).
- **P1 — Importante**: seguridad, higiene del repositorio y consolidación de infraestructura.
- **P2 — Optimización**: calidad técnica (.editorconfig, migración de tests a xUnit, estrategia de `Resources/`).

Las fases futuras de este roadmap son **Propuestas (no comprometidas)**: no se planifican ni se fechan hasta cerrar el backlog vigente de TODO.md.

## Fases

### Fase 1 — Consolidación (completada)

- [x] Consolidación de múltiples versiones en una única aplicación WPF con catálogos JSON.
- [x] Modelos, loader y motor de ejecución en C# (`LJokerL.Core`).
- [x] Logging JSONL con snapshot de registro para rollback.
- [x] Runner de pruebas propio en `src/LJokerL.Tests` (28 pruebas, integrado en CI).
- [x] Auditoría de código y seguridad; validación de ajustes (SAFE/ADV/EXP) en `Reports/Tweaks2_Validation_Status.md`.

### Fase 2 — Mejoras core (completada en lo esencial)

- [x] Arquitectura modular: WPF + MVVM con modelos separados del código de UI.
- [x] Motor de ejecución con acciones directas y catálogo de reversiones (92 tweaks revertibles).
- [x] Detección de hardware en C# (`HardwareDetector`) conectada a las 14 reglas de `Config/hardware_rules.json`.
- [x] Rollback automático: snapshot en logs + contexto por perfil + reversión por catálogo a nivel de ViewModel.
- [x] CI con build y pruebas en cada push/PR (GitHub Actions).
- [ ] **Pendiente**: suite de pruebas de `ExecutionEngine` (el motor sin cobertura actual, ni directa ni indirecta, pero no tiene grupo de pruebas propio).

### Fase 3 — Interfaz de usuario (parcial)

- [x] Aplicación WPF nativa con navegación por pestañas (7) y tema oscuro propio.
- [x] Vista **Dashboard**: aplicación/reversión de perfiles y tweaks, recomendaciones según hardware, estado del sistema.
- [x] Vista **Benchmarks**: ejecución rápida/completa, comparación antes/después y exportación de informes.
- [ ] **Pendiente**: vistas dedicadas de las 5 pestañas restantes — Auto Config, Perfiles, Tweaks, Monitor y Settings (parte de su funcionalidad ya es accesible desde Dashboard, pero sin vista propia).
- [ ] **Pendiente**: acceso en UI a la reversión por catálogo (el comando existe en el ViewModel, sin botón que lo enlace).
- [ ] **Pendiente**: gráficos y visualizaciones del monitor.
- [ ] **Pendiente**: conmutador de tema en la interfaz (el modo oscuro ya existe vía `ThemeService` con persistencia en `UserSettings`; falta la selección de tema desde la UI).

### Fase 4 — Funcionalidades avanzadas (Propuesto — no comprometido)

- [ ] Sistema de plugins.
- [ ] Base de datos local (SQLite) para histórico de ejecuciones y métricas.
- [ ] Configuración avanzada y perfiles definidos por el usuario.
- [ ] API local para integración con terceros.

### Fase 5 — Expansión (Propuesto — no comprometido)

- [ ] Distribución con actualizador propio y firma de código.
- [ ] Comunidad y contribución externa (guiada por `CONTRIBUTING.md`).
- [ ] Soporte para entornos empresariales.

## Hitos

### Alcanzados

- **Hito 1 — Aplicación nativa v13.0.0** (enero de 2026): reescritura completa en C#/.NET 9 + WPF (MVVM) con motor de ejecución y catálogos JSON.
- **Hito 2 — Calidad y CI** (octubre de 2026): suite de 28 pruebas, pipeline de CI en GitHub y repositorio público `SabaTech-dev/LJokerL_TTV`.

### Siguientes (gobernados por TODO.md)

- **Cierre de P0** (documentación coherente: README, guía de usuario, dependencias, roadmap).
- **P1** (seguridad, higiene del repositorio, consolidación de infraestructura de agentes).
- **P2** (calidad técnica: xUnit, estrategia de `Resources/`, configuración centralizada).

Una vez cerrado TODO.md, los candidatos naturales son completar las vistas pendientes de la Fase 3 y la suite de `ExecutionEngine` (Fase 2).

## Trazabilidad

| Fecha | Hito | Estado |
|---|---|---|
| ago-2025 | Era PowerShell (scripts `.ps1`) | **Sustituida** |
| ene-2026 | Reescritura completa en C#/.NET 9 + WPF (MVVM), versión v13.0.0 | Vigente |
| may-2026 | Catálogos actualizados (`tweaks.json` 2026.01, `profiles.json` 2026.03) | Vigente |
| oct-2026 | Auditoría documental, repositorio público en GitHub, CI y pipeline de tareas (TODO.md) | Vigente |

La era PowerShell (ago-2025) fue sustituida por completo por la aplicación C#/WPF; cualquier mención a scripts `.ps1` como interfaz o lenguaje actual de la aplicación es obsoleta.

---

Este roadmap se actualiza con el progreso real del proyecto. Cambios de alcance o de dirección se registran primero en TODO.md y se reflejan aquí.

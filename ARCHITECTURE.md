# Arquitectura — LJokerL_TTV Custom Config PRO

> Documento de referencia de la estructura real implementada en la versión 13.0.1.
> Cualquier componente citado aquí existe en el árbol de fuentes (`src/`). El trabajo
> pendiente se gestiona exclusivamente en [TODO.md](TODO.md).

## Visión general

Solución .NET 9 (`LJokerL.sln`, `net9.0-windows`) de optimización de Windows para gaming,
construida como aplicación de escritorio WPF con patrón MVVM. Comprende tres proyectos:

| Proyecto | Tipo | Contenido |
| --- | --- | --- |
| `src/LJokerL.App` | `WinExe` WPF | Interfaz de usuario, ViewModel principal y servicios de presentación. |
| `src/LJokerL.Core` | Librería de clases | Modelos de dominio y servicios: detección de hardware, ejecución de optimizaciones, autoconfiguración, benchmarks y métricas. |
| `src/LJokerL.Tests` | Biblioteca de tests xUnit | Suite de tests xUnit; se ejecuta con `dotnet test`. |

## LJokerL.App (capa de presentación)

- **`MainWindow.xaml`**: ventana única con 7 pestañas de navegación (Dashboard, Auto Config,
  Perfiles, Tweaks, Monitor, Benchmarks, Settings), implementadas como `RadioButton`
  conmutados por `DataTrigger`. Vistas con contenido implementado: **Dashboard** y
  **Benchmarks**; las cinco restantes muestran el área de contenido vacía (el XAML no
  contiene elementos de placeholder).
- **`ViewModels/MainViewModel.cs`** (~2500 líneas): orquestador de la aplicación. Carga los
  catálogos JSON al arrancar (`ConfigLoader`), ejecuta la detección de hardware al inicio
  (`HardwareDetector`, con overrides manuales de VRR, GPU, CPU y RAM persistidos en ajustes),
  aplica y revierte perfiles y tweaks (`ExecutionEngine`), gobierna la monitorización en vivo
  (`SystemMetricsService`, intervalo configurable, pausa/reanudación, logging de métricas),
  lanza benchmarks con progreso y comparación antes/después, genera recomendaciones de
  autoconfiguración y ofrece visor de logs de ejecución con filtros, paginación y exportación.
- **`ViewModels/ObservableObject.cs` y `RelayCommand.cs`**: soporte MVVM mínimo propio
  (notificación de cambios y comandos), sin frameworks externos.
- **`Services/ThemeService.cs`**: servicio estático que alterna entre tema claro y oscuro
  sustituyendo los pinceles de `Application.Resources` y notifica mediante el evento
  `ThemeChanged`. La preferencia (`UserSettings.DarkMode`) se persiste en el JSON de ajustes
  y se aplica en el arranque desde `App.OnStartup`.

## LJokerL.Core (dominio y servicios)

### Models (`src/LJokerL.Core/Models`, 7 ficheros)

- `ConfigModels.cs`: catálogos y definiciones (`TweakCatalog`, `TweakDefinition`,
  `ActionDefinition`, `ServiceCatalog`, `ServiceDefinition`, `AppCatalog`,
  `AppPackageDefinition`, `ProfileCatalog`, `ProfileDefinition`, `HardwareRuleCatalog`...).
- `HardwareModels.cs`: instantánea base de hardware (`HardwareSnapshot`, `CpuInfo`,
  `GpuInfo`, `MemoryInfo`, `OsInfo`, `MonitorInfo`).
- `HardwareModelsExtended.cs`: perfil ampliado (`ExtendedHardwareSnapshot`, información de
  placa, módulos de RAM, adaptadores de red, alimentación, entorno software y apps de gaming).
- `MonitoringModels.cs`: métricas en vivo (`SystemMetricsSnapshot` y métricas de CPU, GPU,
  memoria, disco, red y procesos).
- `BenchmarkModels.cs`: resultados de benchmark (`BenchmarkRunResult`, resultados por
  componente, progreso, deltas y comparaciones).
- `AutoConfigModels.cs`: motor de autoconfiguración (`AutoConfigResult`,
  `AutoConfigRecommendation`, `AutoConfigRule`, `RuleCondition`, `RuleAction`...).
- `UserSettings.cs`: preferencias de usuario: overrides de hardware, opciones de
  monitorización (habilitación e intervalo) y modo oscuro. La configuración de benchmark
  (`BenchmarkRunOptions`) no se persiste en ajustes.

### Services (`src/LJokerL.Core/Services`, 18 ficheros)

- **`ConfigLoader`**: carga los catálogos de `Config/*.json` en un `ConfigBundle` único
  (tweaks, servicios, apps, perfiles y reglas de hardware).
- **`ExecutionEngine`**: núcleo de aplicación de cambios. Ejecuta acciones de tipo `registry`
  (con captura previa del valor en un `RegistrySnapshot`) y `command`; habilita/deshabilita
  servicios de Windows; elimina y restaura paquetes Appx invocando PowerShell. Registra cada
  acción en JSONL por contexto y soporta **rollback limitado a acciones de registro**.
- **`DryRunPlanner`**: planificador puro del modo dry-run (M1). Deriva de los catálogos el plan
  de acciones que ejecutaría un perfil (`PlannedAction` con tipo, destino, operación, riesgo y
  reversibilidad, más la lista de ids omitidos por no existir) sin tocar `Process`, el registro
  ni ningún recurso del sistema. `MainViewModel` lo invoca desde el botón «Vista previa
  (dry-run)» y presenta el plan en pantalla; el flujo real de aplicación no cambia. Cubierto por
  `DryRunPlannerTests` contra los catálogos reales de `Config/`.
- **`HardwareDetector`**: instantánea rápida de CPU, GPU, RAM, SO y monitor al arranque,
  con override opcional de VRR.
- **`GpuDetector`** / **`StorageDetector`** / **`SoftwareDetector`** / **`MonitorDetector`**:
  detección detallada de GPU (principal y lista), unidades de almacenamiento, entorno
  software (runtimes y apps de gaming) y monitores conectados.
- **`SystemProfiler`**: perfil extendido combinando WMI y PowerShell (`Win32_Processor`,
  `Win32_PhysicalMemory`, `Get-NetAdapter`...) para poblar `ExtendedHardwareSnapshot`.
- **`HardwareRuleEvaluator`**: evalúa las reglas contextuales de `Config/hardware_rules.json`
  (14 reglas sobre pantalla, RAM, CPU y GPU) contra un `HardwareSnapshot`.
- **`AutoConfigurationEngine`**: recomendador de perfil y tweaks; aplica 15 reglas integradas
  (`low_ram_profile`, `laptop_power`, `enable_hags_nvidia`, ...) sobre el perfil extendido de
  hardware y expone el catálogo completo con `GetAllRules`.
- **`BenchmarkRunner`**: baterías de CPU, memoria (secuencial y de acceso aleatorio), disco
  (solo secuencial) y red (ping configurable), con semillas fijas para reproducibilidad y
  progreso vía `IProgress`.
- **`BenchmarkStorage`** / **`BenchmarkComparer`** / **`BenchmarkReportGenerator`**:
  persistencia de resultados en JSON, comparación antes/después con deltas y generación de
  informes en HTML y Markdown.
- **`SystemMetricsService`**: muestreo de uso de CPU, GPU (contadores de rendimiento de la
  categoría `GPU Engine`), memoria, disco y red; temperatura de CPU vía WMI dinámico por
  reflection (se activa si `System.Management` está disponible).
- **`SystemMetricsLogger`**: volcado de métricas a `Logs/metrics.jsonl`.
- **`SystemRestoreService`**: punto de restauración del sistema fail-safe antes de aplicar un
  perfil (M1). Detecta si Restaurar sistema está habilitado (registro `DisableSR`), consulta el
  último punto vía CIM `root\default` `SystemRestore` (reflection) y crea el punto con
  `Checkpoint-Computer -RestorePointType MODIFY_SETTINGS`. La decisión (proceder, reutilizar un
  punto reciente de menos de 24 h o bloquear la aplicación si falla la creación) vive en la clase
  pura `SystemRestoreGate`, cubierta por la matriz de decisión de `RestoreGateTests`.
- **`ExecutionLogger`** (dentro de `ExecutionEngine.cs`): registro JSONL de cada acción en
  `Logs/execution.jsonl`, con valor previo, resultado y error.
- **`UserSettingsService`**: carga y guarda `UserSettings` como JSON.

## Flujo de datos

```text
Config/*.json (tweaks, services, apps_bloatware, profiles, hardware_rules)
        │
        ▼
  ConfigLoader.LoadAll ──► ConfigBundle ──► MainViewModel
        │                                      │
        │            HardwareDetector / SystemProfiler (arranque)
        │                                      │
        ▼                                      ▼
  UserSettings (%APPDATA%)            ExecutionEngine
                                              │
                        ┌─────────────────────┼─────────────────────┐
                        ▼                     ▼                     ▼
                 Registro (snapshot)     Servicios de Windows    Appx (PowerShell)
                        │                     │                     │
                        └─────────────────────┴─────────────────────┘
                                              ▼
                              Logs/execution.jsonl · Logs/metrics.jsonl
```

- Los catálogos viven en `Config/` y se cargan al arrancar. El perfil activo vive en memoria
  (`MainViewModel`) y el último aplicado se resuelve desde los contextos del log de
  ejecución (`ExecutionLogger`, prefijo `profile:{id}:`); `selected_profile.txt` es un
  artefacto de la era PowerShell (herencia de la v12.5) que ningún código lee ni escribe.
- La reversión se apoya en el JSONL de ejecución: `RollbackContext` restaura únicamente
  acciones de registro (el resto se marca como no soportado).
- Los ajustes de usuario se guardan en `%APPDATA%\LJokerL_TTV\settings.json`
  (overridable con `LJOKERL_SETTINGS_PATH`); `App.OnStartup` aplica el tema leyendo la
  ruta de datos locales (`%LOCALAPPDATA%\LJokerL\settings.json`). Es una divergencia de
  rutas de ajustes conocida entre ambos puntos de acceso.
- Los logs se escriben en `Logs/` (ignorados por Git).
- Los directorios de ejecución (`Logs/`, `Benchmarks/`) se crean bajo demanda
  junto al ejecutable y no se versionan; los directorios vacíos locales
  (`GameProfiles/`, `Updates/`, `Profiles/Templates/`) no forman parte del
  repositorio. `Database/Components/components_db.json` es el único dato
  versionado bajo `Database/`.

## Infraestructura

- **Empaquetado** (`Installer/`): `CreateInstaller.ps1` y `CreatePortable.ps1` generan las
  variantes instalador y portable; `LJokerL_TTV_Installer.iss` define el paquete con
  Inno Setup.
- **CI** (`.github/workflows/ci.yml`): GitHub Actions en `windows-latest` con .NET 9;
  `dotnet build LJokerL.sln -c Release` y `dotnet test LJokerL.sln -c Release --nologo`.
  Se dispara en push y pull request hacia `main` y `develop`.
- **Repositorio**: público en GitHub (`SabaTech-dev/LJokerL_TTV`); la rama `develop` está
  protegida.

## Decisiones técnicas

- **MVVM ligero**: sin frameworks MVVM ni DI externos; `ObservableObject` y `RelayCommand`
  propios en la capa App.
- **Catálogos JSON desacoplados del código**: tweaks, servicios, apps, perfiles y reglas se
  editan en `Config/` sin recompilar; el motor solo conoce los esquemas de `ConfigModels.cs`.
- **PowerShell invocado como proceso externo**: 7 componentes lanzan procesos `powershell`
  (`ExecutionEngine`, `HardwareDetector`, `GpuDetector`, `StorageDetector`,
  `SoftwareDetector`, `MonitorDetector`, `SystemProfiler`). La ruta del ejecutable puede
  sobrescribirse con `LJOKERL_POWERSHELL_PATH`, respetada hoy por `ExecutionEngine` y
  `HardwareDetector`.
- **Sin dependencias NuGet activas**: `LJokerL.App` y `LJokerL.Core` no referencian paquetes;
  las referencias de testing de `LJokerL.Tests` permanecen comentadas a la espera de
  configurar la restauración de NuGet (ver [DEPENDENCIES.md](DEPENDENCIES.md)).
- **Suite de tests xUnit**: ejecutada con `dotnet test`, cubre modelos, detectores, carga de
  configuración, profiler, autoconfiguración y sistema de benchmarks. `ExecutionEngine` no
  tiene cobertura automatizada (muta el registro y servicios reales).

## Trabajo futuro

El trabajo pendiente, priorizado y con criterios de aceptación, se mantiene en
[TODO.md](TODO.md). Este documento no duplica esa lista: aquí solo se describe lo que
existe y funciona en el árbol de fuentes.

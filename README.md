# LJokerL_TTV Custom Config PRO v13.0.1 — WPF Control Center

Aplicación de escritorio para Windows 11, escrita en C#/.NET 9 con WPF y patrón
MVVM, que centraliza la aplicación y la reversión de optimizaciones del sistema
mediante perfiles declarativos y catálogos JSON curados.

## Descripción

**LJokerL_TTV Custom Config PRO** es un centro de control de optimización para
Windows 11. Todas las operaciones (aplicar o revertir) pasan por un motor único
(`ExecutionEngine`) que registra cada cambio en un log JSONL junto con el valor
previo del registro, lo que permite revertir cualquier perfil o tweak individual
de forma fiable.

El estado actual del proyecto es la versión **13.0.1**. Consulta
[CHANGELOG.md](CHANGELOG.md) para el historial de cambios y
[ROADMAP.md](ROADMAP.md) para la evolución planificada.

## Características principales

- **Centro de control WPF (MVVM)**: panel de perfiles, tweaks avanzados con
  filtros, métricas del sistema en tiempo real y benchmark pre/post.
- **Aplicar/revertir perfiles completos** con snapshot de los valores previos
  del registro de Windows.
- **Punto de restauración del sistema antes de aplicar** (configurable, fail-safe: no aplica si falla su creación).
- **Modo dry-run**: vista previa del plan de acciones por perfil (registro, servicios y apps,
  con riesgo y reversibilidad) sin aplicar nada ni requerir administrador.
- **Rollback** por perfil (desde el log JSONL) o por catálogo (tweaks,
  servicios y apps con acción de reversión propia).
- **Detección de hardware** (CPU, GPU, RAM, display) con 14 reglas contextuales,
  override manual y recomendación de perfil.
- **Catálogos JSON curados** con clasificación de riesgo SAFE/ADV/EXP por
  elemento (ver Reports/Tweaks2_Validation_Status.md).
- **Suite de tests propia** (runner de consola) integrada en CI.

## Stack tecnológico

| Componente    | Tecnología                                        |
| ------------- | ------------------------------------------------- |
| Lenguaje      | C# (.NET 9)                                       |
| Interfaz      | WPF con patrón MVVM                               |
| Configuración | Catálogos JSON (`Config/`)                        |
| Empaquetado   | PowerShell 5.1+ (`Installer/`) e Inno Setup 6     |
| Tests         | Runner propio de consola (`src/LJokerL.Tests`)    |
| CI            | GitHub Actions (`.github/workflows/ci.yml`)       |

El proyecto no utiliza bases de datos, contenedores, servicios en la nube ni
bibliotecas de machine learning. Las ideas aún no implementadas se recogen
únicamente en la subsección «Futuro (no implementado)» y en ROADMAP.md.

## Requisitos del sistema

- Windows 11 (los ajustes están validados para Windows 11; Windows 10 no cuenta
  con validación oficial).
- .NET 9 Runtime (Escritorio/WPF).
- Privilegios de administrador (necesarios para aplicar cambios en el sistema).
- PowerShell 5.1+ únicamente para regenerar portable e instalador (`Installer/`).

Hardware recomendado: CPU Intel Core i5 / AMD Ryzen 5 o superior, GPU dedicada
con 4 GB+ de VRAM, 8 GB+ de RAM y SSD NVMe.

## Instalación y empaquetado

### Ejecutable publicado

```bash
dotnet publish src/LJokerL.App -c Release
```

Ejecuta `LJokerL.App.exe` desde
`src/LJokerL.App/bin/Release/net9.0-windows/publish/`.

### Portable

```powershell
powershell -ExecutionPolicy Bypass -File Installer/CreatePortable.ps1
```

Genera `Output/LJokerL_TTV_Portable_v13.0.1.zip` con la aplicación y sus
catálogos.

### Instalador (Inno Setup)

```powershell
powershell -ExecutionPolicy Bypass -File Installer/CreateInstaller.ps1
```

Requiere Inno Setup 6 (`iscc.exe`); el script de instalador es
`Installer/LJokerL_TTV_Installer.iss`.

### Instalación silenciosa (empresas)

El instalador generado por Inno Setup admite de serie los switches estándar,
sin configuración adicional en el `.iss`:

```powershell
LJokerL_TTV_Installer_v13.0.1.exe /VERYSILENT /NORESTART
```

`/SILENT` muestra solo la barra de progreso; `/VERYSILENT` no muestra ninguna
ventana. El portable (`LJokerL_TTV_Portable_v13.0.1.zip`) no necesita
instalador: se descomprime y se ejecuta. Para distribución automatizada vía
winget, véase [Installer/WINGET.md](Installer/WINGET.md).

## Uso

Desde el centro de control WPF:

1. **Selecciona un perfil** base en el panel de perfiles y aplica o revierte
   desde la propia interfaz.
2. **Advanced Tweaks**: aplica o revierte tweaks individuales con filtros de
   búsqueda y clasificación de riesgo.
3. **Monitoring**: consulta las métricas del sistema en tiempo real.
4. **Benchmark**: ejecuta pruebas rápidas o completas y compara resultados
   pre/post optimización.

Cada operación queda registrada en el log JSONL con el valor previo del
registro, lo que permite revertir cambios de forma selectiva.

## Uso por línea de comandos

El ejecutable admite un modo headless (sin ventana) cuando se invoca con
argumentos:

```text
LJokerL.App.exe --list-profiles
LJokerL.App.exe --apply-profile <id> --dry-run
LJokerL.App.exe --revert-profile <id> --dry-run
LJokerL.App.exe --help
```

Códigos de salida: `0` operación correcta, `1` error de argumentos o de carga
de configuración, `2` ejecución real aún no soportada por CLI. En esta fase,
`--apply-profile` y `--revert-profile` solo funcionan con `--dry-run` (muestra
el plan de acciones sin tocar el sistema); la ejecución real por CLI llega en
la siguiente iteración (M2).

> **Nota:** la app es una aplicación de ventanas (WinExe): al ejecutarla desde
> una terminal interactiva la salida de texto puede no mostrarse; usa
> redirección (`LJokerL.App.exe --list-profiles > perfiles.txt`) o el código de
> salida (0/1/2), que siempre funciona. La CLI respeta `LJOKERL_CONFIG_PATH`
> para redirigir la raíz de configuración (ver `.env.example`).

## Perfiles disponibles

Los perfiles se definen en `Config/profiles.json` (versión 2026.03). Los diez
identificadores disponibles son:

| Id                   | Nombre               | Descripción                                                                  |
| -------------------- | -------------------- | ---------------------------------------------------------------------------- |
| `competitive_latency` | Competitive Latency | Baja latencia para gaming de escritorio; evita los tweaks de red y planificador más arriesgados. |
| `smooth_vrr`          | Smooth VRR          | Sistemas G-SYNC/VRR centrado en la estabilidad de frametime y el gaming moderno de Windows 11.   |
| `gaming_pro`          | Gaming Pro          | Máximo rendimiento para gaming competitivo: mínima latencia, máximo FPS.                         |
| `streaming`           | Streaming           | Optimizado para OBS y streaming mientras se juega.                                               |
| `low_resources`       | Low Resources       | Equipos con 8 GB de RAM o menos; libera memoria.                                                 |
| `balanced`            | Balanced            | Buen rendimiento sin romper la funcionalidad del sistema; seguro para uso diario.                |
| `max_performance`     | Maximum Performance | Rendimiento máximo: desactiva casi todo a cambio de potencia bruta.                              |
| `laptop_saver`        | Laptop / Battery Saver | Más duración de batería en portátiles; menos rendimiento, más autonomía.                      |
| `content_creator`     | Content Creator     | Edición de vídeo, streaming y creación de contenido.                                             |
| `vr_ready`            | VR Ready            | Optimizado para Oculus, SteamVR y Windows Mixed Reality.                                         |

Cada perfil declara sus tweaks, servicios y apps asociados; el detalle completo
está en el propio `Config/profiles.json`.

## Estructura del repositorio

```
LJokerL_TTV/
├── src/
│   ├── LJokerL.App/      # Aplicación WPF (vistas, ViewModels, servicios de UI)
│   ├── LJokerL.Core/     # Lógica de negocio: ExecutionEngine, ConfigLoader, HardwareDetector
│   └── LJokerL.Tests/    # Runner de tests propio de consola
├── Config/               # Catálogos JSON (tweaks, profiles, services, apps, hardware_rules)
├── ComprobarPosiblesNuevosTweaks/ # Material fuente de propuestas de tweaks
├── Database/Components/  # components_db.json
├── Installer/            # CreatePortable.ps1, CreateInstaller.ps1, script Inno Setup
├── Reports/              # Informes de validación y auditoría
├── Resources/            # Utilidades y recursos de terceros
├── LJokerL.sln
└── README.md
```

> **Nota:** los directorios de ejecución (`Logs/`, `Benchmarks/` y similares) se
> crean **bajo demanda** junto al ejecutable y **no se versionan**. Los
> directorios vacíos que puedan existir en un checkout local (`GameProfiles/`,
> `Updates/`, `Profiles/Templates/`, `Database/HardwareProfiles/`,
> `Database/Recommendations/`) **no forman parte del repositorio**: Git no
> versiona directorios sin ficheros. El único dato versionado bajo `Database/`
> es `Database/Components/components_db.json`.

## Logs y trazabilidad

La aplicación escribe sus logs en la carpeta `Logs\` de su propio directorio de
instalación (junto a `LJokerL.App.exe`):

- `Logs\execution.jsonl` — registro de operaciones (aplicar/revertir) con
  timestamp, usuario y snapshot de los valores previos del registro.
- `Logs\metrics.jsonl` — registro de métricas del sistema (CPU, GPU, RAM).

El rollback por perfil reconstruye el estado previo desde el snapshot del log;
el rollback por catálogo usa la acción de reversión declarada de cada tweak,
servicio o app.

## Solución de problemas

- **Error de privilegios**: ejecuta la aplicación como administrador.
- **Antivirus/Defender**: añade una excepción para el directorio de la
  aplicación si bloquea la ejecución de cambios en el sistema.
- **Diagnóstico**: revisa `Logs\execution.jsonl` para ver la operación exacta
  que falló y su salida.

## Desarrollo

```bash
# Compilar la solución completa
dotnet build LJokerL.sln -c Release

# Ejecutar la suite de tests (lo mismo que hace CI)
dotnet test LJokerL.sln -c Release --nologo
```

La suite xUnit (`src/LJokerL.Tests`) ejecuta las pruebas de Modelos, Detectores,
Configuración, Profiler, AutoConfig y BenchmarkSystem. El motor de ejecución
(ExecutionEngine) no cuenta actualmente con suite automatizada de pruebas. CI
(`.github/workflows/ci.yml`) ejecuta build en Release y la suite de tests en
cada push a main/develop y en cada pull request hacia esas ramas.

Variables de entorno opcionales: ver `.env.example`.

Para contribuir: crea una rama de funcionalidad, realiza commits claros y
abre una pull request con descripción detallada.

- Guía de contribución: [CONTRIBUTING.md](CONTRIBUTING.md).
- Reporte de vulnerabilidades y política de seguridad: [SECURITY.md](SECURITY.md).

### Política de documentación

La documentación del proyecto se redacta en español de España; los
identificadores, la API y los mensajes de la aplicación siguen el idioma del
código. El CHANGELOG sigue el formato Keep a Changelog. Decisión adoptada: la
documentación permanece en la raíz del repositorio (no se migra a `docs/`)
para no romper los enlaces recién consolidados; `BUILD_INSTRUCTIONS.md`
(inglés): su unificación al español queda como trabajo futuro de traducción
(la tarea P0-6 corrige únicamente sus rutas).

## Futuro (no implementado)

Las siguientes líneas se plantean a medio plazo y **no forman parte de la
versión actual**; el detalle y el estado se siguen en
[ROADMAP.md](ROADMAP.md):

- Recomendación automática de perfil integrada en la UI a partir de la
  detección de hardware (hoy el detector existe y se puede consultar, pero la
  sugerencia no sustituye a la elección manual).
- Ampliación de la suite de tests y cobertura de `ConfigLoader` y filtros.
- Empaquetado y release firmados como flujo continuo.

## Licencia

Este proyecto es propiedad de **LJokerL_TTV** y está diseñado para uso personal
y educativo. Consulta [LICENSE.txt](LICENSE.txt).

## Aviso importante

La aplicación requiere privilegios de administrador y modifica la configuración
de Windows. Haz siempre una copia de seguridad del sistema antes de aplicar
optimizaciones.

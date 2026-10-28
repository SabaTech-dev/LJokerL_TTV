# Changelog

Registro de cambios del proyecto LJokerL_TTV Custom Config PRO.

El formato se basa en [Keep a Changelog](https://keepachangelog.com/es/1.1.0/)
y el proyecto se ajusta a [Versionado Semántico](https://semver.org/lang/es/).

## [Unreleased]

### Anadido
- Punto de restauración del sistema antes de aplicar (configurable, fail-safe: no aplica si falla su creación) (M1).
- ROADMAP: backlog de mejoras competitivas derivado de deep research de 12+ herramientas (M1-M4).
- Chequeo de build de Windows antes de aplicar (Win11 soportado; Win10 19041+ con aviso de no validado) y etiquetas del catálogo "afecta a Defender/Windows Update" (M1).
- CI publica SBOM CycloneDX con attestation de procedencia en cada ejecución (M1).

### Corregido
- Deuda técnica liquidada: instalador compila el .iss in situ (sin copia en TEMP), nuget.config reproducible, docs alineadas (selected_profile, suite xUnit) y URLs del repositorio actualizadas; rutas relativas del .iss corregidas (..\ en lugar de ..\..).

## [13.0.1] - 2026-10-28

### Anadido
- SECURITY.md y CONTRIBUTING.md: políticas de seguridad y contribución (P1-2).
- .env.example con las variables de entorno opcionales documentadas (P1-8).
- Resources/MANIFEST.md: manifiesto SHA-256 de las herramientas externas no versionadas (P2-3).

### Cambios
- TODO.md actualizado con el estado de cierre del plan (28/10/2026).
- Skills de dominio unicas migradas a .github/skills (gaming-optimizer, registry-security, forum-trends) (P1-6, jueces).
- Infraestructura de agentes IA consolidada en .github/; .agent/ y .opencode/ eliminados (P1-6).
- CreatePortable copia la documentacion vigente desde la raiz al empaquetar (P2-4).
- Suite de pruebas migrada a xUnit (dotnet test) e integrada en CI (P2-5).
- LJokerL.Core sin UseWindowsForms y sin Class1.cs (P2-2).
- .editorconfig, global.json y version centralizada en Directory.Build.props (P2-1).
- env example: nota de uso real y valores de VRR (P1-8, jueces).
- TODO.md actualizado con el estado de ejecución del plan (completadas, bloqueadas y desbloqueo).
- Política de documentación fijada: español de España, raíz del repo (P2-6).
- Directorios de runtime (Logs, Benchmarks, vestigios locales) documentados como no versionados (P2-7).
- Eliminadas referencias muertas a Scripts/ y Legacy/ en la configuración de agentes (P1-9).
- ARCHITECTURE reescrito con la estructura real implementada (P1-4).
- DEPENDENCIES actualizado a v13.0 con inventario real de herramientas externas y política de no versionado de binarios (P0-5).
- README reescrito: stack real (sin infraestructura ficticia), sin scripts inexistentes, sin secciones duplicadas, perfiles alineados con profiles.json (P0-3).
- Corregidas imprecisiones de cobertura de pruebas, disparo de CI, estructura del repositorio y requisitos (P0-3, ronda de jueces).
- Guía de usuario reescrita para la aplicación WPF v13.0 (P0-4).
- USER_GUIDE: sección de reversión exacta (solo registro; catálogo sin UI), requisitos e instalador corregidos (P0-4, ronda de jueces).
- Informes obsoletos de la era PowerShell movidos a Reports/archive/ (P1-5).
- ROADMAP reconciliado con el estado real del proyecto (P0-7).
- SECURITY: precisión sobre red (ping del benchmark) y canal único de reporte (P1-2, jueces).
- BUILD_INSTRUCTIONS: rutas correctas de los scripts de empaquetado (P0-6).

### Corregido
- El perfil por defecto seleccionado ("NEXT GEN GAMING", id inexistente) pasa a ser "gaming_pro"; prueba de regresión en la suite (D-12).

## 13.0-mantenimiento - 2026-02 a 2026-06

### Anadido
- Scaffolding de agentes de IA del repositorio (`.opencode/` feb 2026; `.github/` con copilot-instructions, agentes y reglas por área en may 2026).
- Catálogo `Config/profiles.json` v2026.03 con 10 perfiles reales; `Config/tweaks.json` v2026.01; `Resources/WindowsReset.zip` (may 2026).

### Cambios
- Ajustes finales de documentación y `.gitignore` (16-jun-2026).

### Notas
- Entradas reconstruidas desde marcas de fecha de archivos (sin historial Git previo al 27-10-2026).

## [13.0.0] - 2026-01-31

Release inicial "Next Gen" con arquitectura unificada WPF + JSON configs.
Build en Release con 0 errores y 0 advertencias.

### Anadido
- Monitor de uso de GPU universal via Windows Performance Counters
  (compatible NVIDIA, AMD e Intel sin SDKs propietarios).
- Lectura de temperatura de CPU mediante WMI dinamico por reflection
  (se activa si el paquete System.Management esta disponible).
- Sistema de deteccion de hardware con 14 reglas contextuales:
  - Display: 2 reglas (VRR / no VRR).
  - Memoria: 3 reglas (8 GB, 16 GB, 32 GB+).
  - CPU: 4 reglas (Intel, AMD, nucleos bajos, nucleos altos).
  - GPU: 5 reglas (NVIDIA x2, AMD x2, Intel).
- Deteccion automatica de hardware con soporte de override manual.
- Sistema de temas con modo oscuro y persistencia en UserSettings.
- ThemeService con notificacion de cambios (ThemeChanged).
- 22 pruebas unitarias con estructura xUnit y runner personalizado;
  cobertura de Config, Execution, Hardware, Metrics, Benchmark, Rules y Settings.
- Perfiles de optimizacion Competitive Latency y Smooth VRR con
  catalogos de tweaks, servicios y bloatware reversibles.
- Logging JSONL persistente con snapshot de valores previos (ExecutionLogger).
- Rollback completo por perfil (RollbackContext + revert de catalogo).
- Empaquetado portable (ZIP de 93 MB) listo para distribuir.
- Script de instalador Inno Setup profesional y automatizacion PowerShell.

### Cambios
- Unificacion de multiples versiones previas en arquitectura WPF + JSON unica.
- UI WPF (MainWindow.xaml) con panel de perfiles, estadisticas y
  Advanced Tweaks con filtros y rollback.

### Notas tecnicas
- Temperatura de CPU: muestra "N/A" si System.Management no esta presente.
  Para habilitarla, anadir el paquete NuGet `System.Management`.
- Modo oscuro: la aplicacion completa requiere reinicio para aplicarse por
  completo; futuras versiones requeriran rework del XAML para conmutacion en vivo.
- Pruebas: la estructura xUnit esta lista; algunas pruebas presentan
  problemas de compatibilidad no criticos. La aplicacion principal es
  plenamente funcional.

[13.0.1]: https://github.com/SabaTech-dev/LJokerL_TTV/compare/v13.0.0...v13.0.1
[13.0.0]: https://github.com/SabaTech-dev/LJokerL_TTV/releases/tag/v13.0.0
[Unreleased]: https://github.com/SabaTech-dev/LJokerL_TTV/compare/main...develop

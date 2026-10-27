# Informe de auditoría: alineación de documentación y configuración

| Campo | Valor |
|---|---|
| Proyecto | LJokerL_TTV — Custom Config PRO v13.0 (WPF Control Center) |
| Fecha de emisión | 27 de octubre de 2026 |
| Tipo | Auditoría de alineación documental y de configuración |
| Clasificación | Uso interno |
| Versión del informe | 1.0 |
| Alcance | Repositorio completo en `C:\LJokerL_TTV` (553 MB) |
| Entregables derivados | Este informe y `TODO.md` en la raíz del repositorio |

## 1. Resumen ejecutivo

**Veredicto: desalineación sustancial (puntuación global 3,6/10).** El producto real —una aplicación de escritorio C#/.NET 9 con WPF, una librería de negocio de 8 modelos y 15 servicios, una suite de pruebas y catálogos de datos actualizados hasta mayo de 2026— es internamente coherente. Sin embargo, la capa documental arrastra contenido de dos eras anteriores (v12.x basada en scripts de consola PowerShell y un «roadmap aspiracional» de tipo web/cloud que nunca se implementó), y la configuración presenta artefactos huérfanos y ausencias relevantes.

Hechos clave:

1. Se han documentado **veinte desalineaciones con evidencia** (`archivo:línea`), detalladas en la sección 4.3. Las más graves: `USER_GUIDE.md` y `DEPENDENCIES.md` congelados en v12.5/v12.0, `ROADMAP.md` que declara como stack actual «PowerShell 5.1+ / interfaz Web», y `README.md` con un stack ficticio (Docker, Kubernetes, ML.NET, PostgreSQL) y referencias a scripts que no existen.
2. **No existe repositorio Git** pese a que `README.md` (líneas 249 y 399) y `CHANGELOG.md` (línea 49) presuponen Git, GitHub, Pull Requests y CI/CD con GitHub Actions.
3. **Defecto funcional latente**: `Config/selected_profile.txt` contiene «NEXT GEN GAMING», un identificador que no existe entre los diez perfiles reales de `Config/profiles.json` (v2026.03).
4. **Gestión de secretos deficiente**: existe un `.env` con claves de API de herramientas ajenas al proyecto, sin `.env.example` y sin un `.gitignore` efectivo (no hay Git).
5. **El 97 % del peso del repositorio (537 de 553 MB) es artefacto, no código ni documentación**: `Output/` (272 MB, build portátil ya empaquetada), `Resources/` (222 MB, ejecutables de terceros versionados) y `.unlighthouse/` (43 MB, informes de sitios web ajenos).
6. Existe una **triple infraestructura de agentes de IA** (`.agent/`, `.opencode/`, `.github/`) que solapa el mismo propósito, con referencias a rutas muertas (`Scripts/…`, `Legacy/`).
7. El documento más fiable y actual es `.github/copilot-instructions.md`; los documentos orientados a usuario de la raíz son los más desfasados.

Conclusión de dirección: el problema no es el producto, sino que **la documentación va dos versiones por detrás del código**. El plan de acción propuesto (`TODO.md`, 23 acciones: 7 de prioridad P0, 9 de P1 y 7 de P2) corrige primero trazabilidad (Git), después exactitud documental y, por último, higiene y optimización.

## 2. Alcance y metodología

- Análisis estático completo del directorio `C:\LJokerL_TTV` (no destructivo: no se ejecutaron build ni pruebas, ni se modificó archivo alguno antes de los entregables).
- Lectura íntegra de los documentos Markdown relevantes (más de 25), inventario de archivos de proyecto y configuración, y verificación cruzada de cada afirmación documental contra los artefactos reales (código fuente, `.csproj`, catálogos JSON, scripts de empaquetado).
- Cada hallazgo se cita como `archivo:línea`. Las comprobaciones puntuales se realizaron mediante búsqueda de patrones sobre los archivos citados.
- Clasificación por severidad (Crítica / Alta / Media / Baja) y matriz de riesgos.
- Entregables: este informe y `TODO.md` con el plan de acción detallado (qué, por qué, ventajas, inconvenientes y criterio de aceptación de cada acción).

## 3. Contexto del activo

### 3.1 Propósito declarado

Herramienta de optimización universal para Windows 11: aplicación de escritorio con privilegios de administrador que aplica ajustes (tweaks) de registro, servicios y bloatware UWP mediante perfiles predefinidos, con registro de ejecución JSONL, snapshot y rollback. Licencia MIT (`LICENSE.txt`).

### 3.2 Stack real verificado

| Componente | Detalle |
|---|---|
| Lenguaje/plataforma | C# / .NET 9 (`net9.0-windows`) |
| UI | WPF con patrón MVVM (`UseWPF`; `MainWindow.xaml`, ViewModels, ThemeService) |
| Solución | `LJokerL.sln`, 3 proyectos: `LJokerL.App` (UI), `LJokerL.Core` (8 modelos, 15 servicios), `LJokerL.Tests` (runner propio de consola, ~28 pruebas) |
| Empaquetado | PowerShell (`Installer/CreatePortable.ps1`, `Installer/CreateInstaller.ps1`) + Inno Setup (`Installer/LJokerL_TTV_Installer.iss`) |
| Datos | Catálogos JSON en `Config/`: `tweaks.json` (v2026.01), `profiles.json` (v2026.03, 10 perfiles), `services.json`, `apps_bloatware.json`, `hardware_rules.json` (15 reglas) |
| Dependencias NuGet | Ninguna activa (cero `PackageReference` sin comentar) |

No existe Node, Python, Go, Rust, Docker ni infraestructura cloud en el proyecto.

### 3.3 Cronología del proyecto (por marcas de archivo; no hay historial Git)

| Periodo | Actividad |
|---|---|
| Ago-2024 | `Resources/SCEHUB-1.2.0/` (herramienta de terceros incluida) |
| Ago-2025 | Era PowerShell: informes legacy de `Reports/` y logs históricos |
| Ene-2026 (23-31) | Reescritura a C# WPF; docs raíz, `src/`, `Config/`; release v13.0.0 (CHANGELOG, 31-ene); portable v13.0 generado |
| Feb-2026 | Scaffolding de `.opencode/` |
| May-2026 | Scaffolding de `.github/` (copilot, agentes); catálogos actualizados (`profiles.json` 2026.03) |
| Jun-2026 (16) | Últimos ajustes conocidos (`CHANGELOG.md`, `.gitignore`) |
| Oct-2026 (hoy) | Sin cambios funcionales documentados desde junio |

### 3.4 Distribución del peso del repositorio

| Carpeta | Tamaño | Proporción | Naturaleza |
|---|---|---|---|
| `Output/` | 272 MB | 49 % | Build portátil ya empaquetada (incluye copias de docs que envejecen) |
| `Resources/` | 222 MB | 40 % | Ejecutables de terceros versionados (NVPI-R, NVCleanstall, MemTest64, MPOGPUFIX, MSI_util, SCEHUB…) |
| `.unlighthouse/` | 43 MB | 8 % | Informes Lighthouse de sitios web ajenos al proyecto |
| `src/` | 9,4 MB | 2 % | Código fuente |
| Resto | ~6 MB | 1 % | Documentación y configuración |

## 4. Resultados de la auditoría

### 4.1 Valoración por documento

Leyenda: [VIGENTE] fiel al estado actual · [PARCIAL] mezcla contenido vigente y obsoleto · [DESFASE] describe una versión o era anterior · [OBSOLETO] corresponde a una era ya eliminada · [AJENO] no pertenece al proyecto.

| Documento | Estado | Observación esencial |
|---|---|---|
| `.github/copilot-instructions.md` | [VIGENTE] | El más fiable; rutas de build correctas |
| `Reports/Tweaks2_Validation_Status.md` | [VIGENTE] | Base actual de validación de catálogos |
| `Reports/Tweak_Verification_Report_2026.md` | [VIGENTE] | Criterios SAFE/ADVANCED/EXPERIMENTAL |
| `README.md` | [PARCIAL] | v13.0, pero con stack ficticio, secciones duplicadas y scripts inexistentes |
| `BUILD_INSTRUCTIONS.md` | [PARCIAL] | Vigente en requisitos; rutas de scripts incorrectas |
| `ARCHITECTURE.md` | [PARCIAL] | Visión MVVM correcta; marca como «Pendiente» lo ya implementado |
| `ROADMAP.md` | [PARCIAL] | Contradicciones internas de fases y stack |
| `CHANGELOG.md` | [PARCIAL] | Única entrada (13.0.0, 31-ene) pese a cambios hasta junio |
| `USER_GUIDE.md` | [DESFASE] | Describe la v12.5 de consola (menú por tecla, auto-updater, perfiles inexistentes) |
| `DEPENDENCIES.md` | [DESFASE] | Tres versiones internas (12.0/12.5/13.0); directorio `Legacy/` inexistente |
| `Reports/Tweaks2_Functions_Mapping.md` | [OBSOLETO] | Mapea funciones de un script eliminado |
| `Reports/Critical_Dependencies_Analysis.md` | [OBSOLETO] | Análisis de la era PowerShell |
| `Reports/Duplication_Analysis_Report.md` | [OBSOLETO] | Duplicaciones de scripts ya eliminados |
| `Reports/Refactoring_Detailed_Plan.md` | [OBSOLETO] | Plan de arquitectura .psm1 abandonada |
| `Reports/Security_Audit_Initial.md` | [OBSOLETO] | Auditoría de scripts eliminados |
| `Output/LJokerL_TTV_Portable_v13.0/*.md` | [DESFASE] | Copias congeladas de los docs raíz dentro del portable |
| `.unlighthouse/` | [AJENO] | Informes de webs ajenas |

### 4.2 Configuración: hallazgos

**Coherente con el propósito:**

- `LJokerL.sln` y los tres `.csproj` (versión 13.0.0 unánime, publish win-x64 single-file ReadyToRun).
- Empaquetado (`Installer/`): coherente con .NET 9 y v13.0.
- Catálogos de `Config/`: consumidos por `ConfigLoader` y alineados con el código.

**Anomalías:**

- `src/LJokerL.Core/LJokerL.Core.csproj` activa `UseWindowsForms` pese a que la UI es WPF.
- `src/LJokerL.Tests/` referencia la aplicación ejecutable y mantiene los paquetes xUnit comentados (runner propio de consola).
- `Config/selected_profile.txt` = «NEXT GEN GAMING»: identificador inexistente en `profiles.json`.
- `.gitignore` correcto en contenido pero inaplicable (no hay Git); además no cubre `.env` y contiene entradas redundantes (`output/` minúscula y `Output/LJokerL_TTV_Portable_*/`).
- `.env` con claves de herramientas ajenas al stack (EXA, CONTEXT7, Playwright…), sin relación con el proyecto .NET.
- `.vscode/settings.json` vacío (2 bytes); `.unlighthouse/` y `.mimosa/` ajenos al proyecto.
- Triple scaffolding de agentes de IA (`.agent/`, `.opencode/`, `.github/`) con solapamiento y referencias muertas.

**Ausencias relevantes:** `.editorconfig`, `global.json`, `.env.example`, CI (`.github/workflows/`), `CONTRIBUTING.md`, `SECURITY.md`.

### 4.3 Registro de desalineaciones (evidencia citada)

| ID | Evidencia | Desalineación | Severidad |
|---|---|---|---|
| D-01 | No existe `.git/`; README.md:249, 399; CHANGELOG.md:49 | Docs presuponen Git/GitHub/PR/CI que no existen | Crítica |
| D-02 | README.md:222, 461 | Referencia a `LJokerL_TTV_Auto_Updater.ps1` inexistente | Alta |
| D-03 | README.md:199, 453 | Referencia a `LJokerL_TTV_Hardware_Database.ps1` inexistente | Alta |
| D-04 | BUILD_INSTRUCTIONS.md:24, 30 | Rutas `.\CreatePortable.ps1` sin el prefijo `Installer/` | Media |
| D-05 | DEPENDENCIES.md:15-16 | «Scripts en `Legacy/`»: directorio inexistente | Media |
| D-06 | USER_GUIDE.md:1, 81 vs csproj 13.0.0 | Guía de usuario congelada en v12.5 | Crítica |
| D-07 | DEPENDENCIES.md:5 y 203 | Tres versiones (12.0/12.5) en un mismo documento frente a 13.0 real | Alta |
| D-08 | ROADMAP.md:153-157 | «Stack actual: PowerShell 5.1+ / interfaz Web» frente a C#/.NET 9 WPF | Crítica |
| D-09 | ARCHITECTURE.md:44, 79, 102 | Hardware Detection «Pendiente» aunque `HardwareDetector.cs` existe y CHANGELOG lo da por hecho | Alta |
| D-10 | ROADMAP.md:58-59 vs 74 | Fase 2 (WPF+MVVM) completada pero Fase 3 vuelve a marcar «Pendiente: interfaz WPF» | Alta |
| D-11 | README.md:389-394 | Stack ficticio: ASP.NET, SQLite, PostgreSQL, ML.NET, TensorFlow, Docker, Kubernetes, Azure, AWS | Alta |
| D-12 | USER_GUIDE.md:29-36; Config/selected_profile.txt | Perfiles documentados y perfil seleccionado inexistentes en `profiles.json` (10 ids distintos) | Crítica |
| D-13 | README.md:198 vs 452 | Dos rutas de logs contradictorias dentro del mismo documento | Media |
| D-14 | .github/agents/orquestador/memory.md, changelog.md | Referencias a `Scripts/Apply-Win11-SmoothVRR-RTX4080.ps1` y `Scripts/state/` inexistentes | Media |
| D-15 | Reports/Tweaks2_Functions_Mapping.md y 3 informes más | Miles de referencias a scripts PowerShell eliminados | Media |
| D-16 | .agent/workflows/deploy_release.md (paso 6) | Referencia a `Legacy` inexistente | Baja |
| D-17 | USER_GUIDE.md:42, 65 | Describe la UI de consola (tecla [A]) de la era anterior, no la app WPF | Alta |
| D-18 | copilot-instructions.md:19; ExecutionEngine.cs:547; HardwareDetector.cs:186 | `LJOKERL_POWERSHELL_PATH` se usa en código pero no existe documentación de dónde definirla ni `.env.example` | Media |
| D-19 | README.md:114-116 vs BUILD_INSTRUCTIONS.md:87 | README exige «PowerShell 5.1+ (solo Legacy)»; BUILD declara los scripts legacy eliminados | Media |
| D-20 | README.md:273-326 vs 416-489 | Secciones internas duplicadas (Riesgos, Métricas, Trazabilidad, Comunidad, Próximos pasos) | Media |

### 4.4 Matriz de riesgos

| ID | Riesgo | Probabilidad | Impacto | Severidad |
|---|---|---|---|---|
| R1 | Trazabilidad nula: sin Git no hay historial, rollback ni capacidad de auditar cambios | Cierta | Alto | Crítica |
| R2 | Exposición de claves: `.env` sin protección efectiva ni ejemplo documentado | Media | Alto | Alta |
| R3 | Experiencia de usuario engañosa: la guía oficial describe un producto distinto | Cierta | Alto | Alta |
| R4 | Perfil por defecto roto (`selected_profile.txt` con id inexistente) | Media | Medio | Media |
| R5 | Sobrecoste de almacenamiento y distribución: 537 MB de artefactos ajenos al código | Cierta | Medio | Media |
| R6 | Deriva documental del portable: copias de docs en `Output/` se desincronizan de la raíz | Cierta | Medio | Media |
| R7 | Regresiones sin detectar: sin CI, las ~28 pruebas existentes no se ejecutan jamás de forma automática | Media | Alto | Alta |
| R8 | Confusión de herramientas de IA y colaboradores: tres sistemas de agentes y referencias muertas | Media | Medio | Media |

## 5. Evaluación cuantitativa

| Dimensión | Nota | Justificación |
|---|---|---|
| Coherencia propósito ↔ producto | 6/10 | Producto sólido y coherente internamente; README contamina la percepción |
| Exactitud y vigencia documental | 3/10 | 20 desalineaciones; 2 de 7 docs raíz con desfase de versión |
| Alineación de configuración | 5/10 | csproj/Installer/Config coherentes; anomalías y ausencias relevantes |
| Seguridad y gestión de secretos | 4/10 | `.env` ajeno sin cobertura; sin SECURITY.md; proyecto con privilegios de administrador |
| Automatización (CI/CD, pruebas) | 2/10 | Sin CI; suite de pruebas existe pero nunca se ejecuta automáticamente |
| Trazabilidad y versionado | 2/10 | Sin Git; CHANGELOG con una única entrada |
| Higiene del repositorio | 3/10 | 97 % del peso es artefacto; triple scaffolding de IA; directorios vacíos |
| **Media global** | **3,6/10** | **Desalineación sustancial: la documentación va a la zaga del código** |

## 6. Conclusiones y recomendación estratégica

1. El desalineamiento es de **documentación a la zaga del código**, no de producto defectuoso: el stack real es verificable, coherente y está a la altura de la versión 13.0 que declara la solución.
2. Secuencia recomendada (desarrollada en `TODO.md`): **(1)** control de versiones Git con `.gitignore` endurecido, **(2)** corrección documental P0 (README, USER_GUIDE, DEPENDENCIES, ROADMAP, BUILD, `selected_profile.txt`), **(3)** automatización y seguridad (CI, SECURITY/CONTRIBUTING, `.env.example`, CHANGELOG), **(4)** limpieza y optimización (informes obsoletos, triple scaffolding IA, artefactos huérfanos, estrategia de `Resources/`).
3. Ninguna eliminación debe ejecutarse antes de tener el primer commit de respaldo (dependencia explícita de P0-1 sobre las acciones de tipo ELIMINAR).

## 7. Beneficios esperados del plan

- **Confianza documental**: una única fuente de verdad por tema; elimina las tres versiones de producto que hoy conviven en la documentación.
- **Trazabilidad**: historial completo de cambios y base para releases reproducibles.
- **Detección temprana**: CI que ejecuta build y pruebas en cada cambio.
- **Reducción de superficie**: menos 43-537 MB de artefactos ajenos según la política que se adopte para `Resources/` y `Output/`; menor confusión para herramientas de IA y futuros colaboradores.
- **Seguridad**: secretos fuera del alcance del versionado y política de reporte de vulnerabilidades para una herramienta que modifica el registro y servicios de Windows.

## 8. Limitaciones de la auditoría

- Auditoría estática: no se ejecutaron `dotnet build` ni la suite de pruebas (criterio no destructivo).
- Al no existir Git, la cronología se basa en marcas de fecha de archivos; puede haber actividad no registrada.
- Los binarios de terceros de `Resources/` se inventariaron pero no se verificaron (hashes, firmas, licencias).
- La puntuación de la sección 5 es una valoración cualitativa ponderada del auditor, no una métrica objetiva medible.

## Anexo A. Inventario documental completo

| Ruta | Idioma | Estado |
|---|---|---|
| README.md | ES | PARCIAL |
| BUILD_INSTRUCTIONS.md | EN | PARCIAL |
| ARCHITECTURE.md | ES | PARCIAL |
| USER_GUIDE.md | ES | DESFASE |
| ROADMAP.md | ES | PARCIAL |
| DEPENDENCIES.md | ES | DESFASE |
| CHANGELOG.md | ES | PARCIAL |
| AGENTS.md (raíz) | EN | VIGENTE (puntero) |
| LICENSE.txt | EN | VIGENTE |
| .github/copilot-instructions.md | EN | VIGENTE |
| .github/instructions/*.md (5) | EN | VIGENTE |
| .github/agents/*.md (4) + orquestador/ | ES/EN | PARCIAL (referencias muertas) |
| .agent/rules/PROJECT_RULES.md, skills/ (5), workflows/ (2) | ES | PARCIAL (referencia `Legacy/`) |
| .opencode/ (AGENTS.md, 8 agentes, 8 comandos, 3 skills) | ES/EN | VIGENTE pero redundante |
| Reports/Tweaks2_Validation_Status.md | ES | VIGENTE |
| Reports/Tweak_Verification_Report_2026.md | ES | VIGENTE |
| Reports/Tweaks2_Functions_Inventory.md | ES | HISTÓRICO |
| Reports/Tweaks2_Functions_Mapping.md | ES | OBSOLETO |
| Reports/Critical_Dependencies_Analysis.md | ES | OBSOLETO |
| Reports/Duplication_Analysis_Report.md | ES | OBSOLETO |
| Reports/Refactoring_Detailed_Plan.md | ES | OBSOLETO |
| Reports/Security_Audit_Initial.md | ES | OBSOLETO |
| Output/LJokerL_TTV_Portable_v13.0/*.md | ES/EN | DESFASE (copias congeladas) |
| ComprobarPosiblesNuevosTweaks/tweaks1.md, tweaks2.md | ES | Material fuente |
| Resources/SCEHUB-1.2.0/README.md | EN | Terceros |

## Anexo B. Inventario de configuración

| Ruta | Qué configura | Coherencia |
|---|---|---|
| LJokerL.sln | Solución de 3 proyectos | Coherente |
| src/LJokerL.App/LJokerL.App.csproj | WPF, v13.0.0, publish single-file R2R | Coherente |
| src/LJokerL.Core/LJokerL.Core.csproj | Librería de negocio, v13.0.0 | Anomalía: `UseWindowsForms` innecesario |
| src/LJokerL.Tests/LJokerL.Tests.csproj | Runner propio, v13.0.0 | Anomalía: xUnit comentado; referencia a la app WinExe |
| Config/tweaks.json, profiles.json, services.json, apps_bloatware.json, hardware_rules.json | Catálogos de datos del producto | Coherentes con ConfigLoader; `selected_profile.txt` inválido |
| Installer/CreatePortable.ps1, CreateInstaller.ps1, LJokerL_TTV_Installer.iss | Empaquetado portable e instalador | Coherentes |
| .gitignore | Exclusiones de versionado | Inaplicable (sin Git); no cubre `.env` |
| .env | Claves de herramientas ajenas | Huérfano |
| .vscode/settings.json | Editor | Vacío |
| .github/ (sin workflows/) | Copilot workspace | Vigente; sin CI |
| .agent/, .opencode/ | Scaffolding de agentes IA | Redundante entre sí |
| .unlighthouse/, .mimosa/ | Análisis de terceros | Ajenos al proyecto |

No existen: `.editorconfig`, `global.json`, `nuget.config`, `Directory.Build.props`, `.env.example`, `.github/workflows/`, Dockerfile.

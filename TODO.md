# TODO — Plan de mejora y optimización de LJokerL_TTV

> Derivado del informe de auditoría `Reports/Auditoria_Documentacion_Configuracion_2026-10-27.md` (27/10/2026).
> Convenciones — **Tipo**: IMPLEMENTAR (crear), MODIFICAR (editar existente), ELIMINAR (borrar o archivar). **Prioridad**: P0 crítico, P1 importante, P2 optimización. **Esfuerzo**: S (<1 h), M (1-4 h), L (>4 h).
> Regla de dependencia: ninguna acción de tipo ELIMINAR se ejecuta antes de P0-1 (primer commit de respaldo en Git).

## Estado de ejecución — 27/10/2026

**Completadas y fusionadas en `develop`** (pipeline completo: DoR → worktree → TDD cuando aplica → RDD → jueces ciegos → CI → PR): P0-1 y P1-1 (bootstrap del repositorio público con CI y protección de rama), P0-3 (PR #2), P0-4 (PR #3), P0-5 (PR #4), P0-7 (PR #5), P1-4 (PR #6), P1-3 (PR #7), P1-9 (PR #8), P2-3 (PR #9), P2-7 (PR #10), P2-6 (PR #11).

**Bloqueadas por el revisor nativo de RDD** (el CLI `claude` que invoca gentle-ai no tiene sesión iniciada; desbloqueo: ejecutar `claude` y completar `/login`, luego reanudar cada transacción con `gentle-ai review status --lineage=<id> --next-transition` desde su worktree):

- **P0-2** — PR #1 abierto, CI verde, jueces sin hallazgos severos; transacción `review-c87fe45d98454b41` (worktree `p0-2-selected-profile`); tras completar la revisión, actualizar rama y fusionar.
- **P0-6** — cambios listos sin commit (worktree `p0-6-buildinstr`); transacción `review-e0b24f9a9efe0a2f`.
- **P1-2** — SECURITY.md y CONTRIBUTING.md listos sin commit (worktree `p1-2-security`); transacción `review-c1b4a7794c668878`.
- **P1-5** — 5 informes movidos a `Reports/archive/` sin commit (worktree `p1-5-archive-reports`); transacción `review-b79f44c01b117823`.
- **P1-8** — PR #12 abierto; corrección de jueces (añadir `LJOKERL_VRR_OVERRIDE` y `LJOKERL_LOG_EXPORT_PATH`) lista sin commit (worktree `p1-8-envexample`); transacción `review-e1c60ad3f3505a4f`.
- **P1-6, P1-7, P2-1, P2-2, P2-4, P2-5** — no iniciadas: sus cambios (consolidación de agentes, borrados, configuración .NET, código) disparan lentes de revisión y requieren el mismo desbloqueo.

## Tabla resumen

| ID | Acción | Tipo | Prioridad | Esfuerzo | Riesgo |
|---|---|---|---|---|---|
| P0-1 | Inicializar Git con .gitignore endurecido | IMPLEMENTAR | P0 | M | Bajo |
| P0-2 | Corregir Config/selected_profile.txt | MODIFICAR | P0 | S | Bajo |
| P0-3 | Reescribir README.md | MODIFICAR | P0 | M | Bajo |
| P0-4 | Actualizar USER_GUIDE.md a v13.0 | MODIFICAR | P0 | M | Bajo |
| P0-5 | Actualizar DEPENDENCIES.md a v13.0 | MODIFICAR | P0 | S | Bajo |
| P0-6 | Corregir rutas en BUILD_INSTRUCTIONS.md | MODIFICAR | P0 | S | Bajo |
| P0-7 | Reconciliar ROADMAP.md | MODIFICAR | P0 | S | Bajo |
| P1-1 | CI con GitHub Actions (build + tests) | IMPLEMENTAR | P1 | M | Medio |
| P1-2 | SECURITY.md y CONTRIBUTING.md | IMPLEMENTAR | P1 | S | Bajo |
| P1-3 | Completar CHANGELOG.md (feb-jun 2026) | MODIFICAR | P1 | S | Bajo |
| P1-4 | Actualizar ARCHITECTURE.md | MODIFICAR | P1 | M | Bajo |
| P1-5 | Archivar informes obsoletos de Reports/ | ELIMINAR | P1 | S | Bajo |
| P1-6 | Consolidar la infraestructura de agentes IA | ELIMINAR + MODIFICAR | P1 | M | Medio |
| P1-7 | Eliminar artefactos huérfanos | ELIMINAR | P1 | S | Medio |
| P1-8 | Crear .env.example y documentar LJOKERL_POWERSHELL_PATH | IMPLEMENTAR | P1 | S | Bajo |
| P1-9 | Corregir referencias muertas del scaffolding de agentes | MODIFICAR | P1 | S | Bajo |
| P2-1 | .editorconfig, global.json y Directory.Build.props | IMPLEMENTAR | P2 | S | Bajo |
| P2-2 | Limpiar LJokerL.Core (WinForms, Class1.cs) | MODIFICAR + ELIMINAR | P2 | S | Bajo |
| P2-3 | Estrategia para Resources/ (222 MB de binarios) | MODIFICAR | P2 | M | Medio |
| P2-4 | Docs del portable generadas por el script | MODIFICAR | P2 | S | Medio |
| P2-5 | Migrar el runner de tests a xUnit | MODIFICAR | P2 | L | Medio |
| P2-6 | Política de idioma y estructura de documentación | MODIFICAR | P2 | S | Bajo |
| P2-7 | Directorios vacíos: documentar o eliminar | MODIFICAR + ELIMINAR | P2 | S | Bajo |

---

## Fase P0 — Crítico (coherencia básica y confiabilidad)

### P0-1 — Inicializar repositorio Git con .gitignore endurecido
- **Qué**: `git init`, ajustar `.gitignore` (añadir `.env`, `.unlighthouse/`, `.mimosa/`, consolidar `Output/`; mantener exclusiones de binarios) y crear el primer commit solo con código, documentación y catálogos.
- **Por qué**: tres documentos (README.md:249, 399; CHANGELOG.md:49) presuponen Git/GitHub/PR/CI; sin control de versiones no hay trazabilidad, rollback ni base para CI (riesgo R1, desalineación D-01).
- **Ventajas**: historial completo; capacidad de deshacer; habilita CI y releases; desbloquea todas las acciones ELIMINAR seguras; cumple lo que la documentación ya promete.
- **Inconvenientes**: exige disciplina de commits; obliga a decidir qué versionar (excluir Output/ y probablemente los binarios de Resources/); ligera curva inicial.
- **Criterio de aceptación**: `git status` limpio tras el primer commit; `.env` y artefactos quedan fuera del índice.

### P0-2 — Corregir Config/selected_profile.txt
- **Qué**: sustituir «NEXT GEN GAMING» por un identificador existente en `Config/profiles.json` (por ejemplo `competitive_latency` o `gaming_pro`, según intención del perfil).
- **Por qué**: el perfil por defecto apunta a un id inexistente entre los diez reales; cualquier carga del perfil seleccionado fallará silenciosamente o degradará (riesgo R4, desalineación D-12).
- **Ventajas**: elimina un defecto funcional latente con un cambio de una línea; coste mínimo.
- **Inconvenientes**: obliga a una microdecisión de producto (qué perfil es el nuevo defecto); verificar en la app que carga correctamente.
- **Criterio de aceptación**: el id cargado existe en `profiles.json`; la app inicia sin error de perfil.

### P0-3 — Reescribir README.md
- **Qué**: eliminar el stack ficticio (Docker, Kubernetes, ASP.NET, ML.NET, PostgreSQL, cloud: README.md:389-394); eliminar referencias a `LJokerL_TTV_Auto_Updater.ps1` y `LJokerL_TTV_Hardware_Database.ps1` (D-02, D-03); fusionar las secciones duplicadas (Riesgos, Métricas, Trazabilidad, Comunidad, Próximos pasos: D-20); unificar la ruta de logs (D-13); alinear la lista de perfiles con `profiles.json` (D-12); describir requisitos reales sin «PowerShell Legacy» (D-19).
- **Por qué**: el README es la portada del proyecto y hoy mezcla tres eras de producto; un lector no puede distinguir qué existe de verdad.
- **Ventajas**: fuente de verdad única; mejora la incorporación de cualquier lector o herramienta; elimina promesas incumplidas (CI/CD, scripts).
- **Inconvenientes**: esfuerzo de redacción; se pierde el «horizonte aspiracional» del documento (mitigación: moverlo a ROADMAP.md como futuro declarado, no como presente).
- **Criterio de aceptación**: cero referencias a rutas inexistentes verificables por búsqueda; una sola sección por tema; perfiles = ids de `profiles.json`.

### P0-4 — Actualizar USER_GUIDE.md a v13.0
- **Qué**: reescribir la guía para la aplicación WPF real (ventanas, flujo de selección de perfil, ejecución, logs, rollback); eliminar menú por tecla [A], auto-updater y perfiles de la v12.5 (D-06, D-17, D-12).
- **Por qué**: la guía oficial describe un producto distinto del que se distribuye; riesgo directo para el usuario final (R3).
- **Ventajas**: guía utilizable; reduce soporte y confusión; coherencia con el portable que ya empaqueta esta guía.
- **Inconvenientes**: requiere recorrer la UI real (capturas opcionales); más esfuerzo que los otros docs por su naturaleza.
- **Criterio de aceptación**: cada acción descrita es ejecutable tal cual en v13.0; sin menciones a v12.x ni a consola.

### P0-5 — Actualizar DEPENDENCIES.md a v13.0
- **Qué**: unificar versión (hoy conviven 12.0, 12.5 y 13.0: D-07); eliminar la referencia a `Legacy/` (D-05); inventariar las herramientas reales de `Resources/` (faltan MemTest64, MPOGPUFIX, MSI_util, InSpectre, CrystalMark…; sobran «Autoruns» y «Windows Update Blocker» no presentes); describir requisitos reales (.NET 9, privilegios de administrador, PowerShell para empaquetado).
- **Por qué**: documento de soporte para diagnóstico y despliegue; hoy induce a error.
- **Ventajas**: inventario verificable; base para P2-3 (estrategia de Resources/).
- **Inconvenientes**: exige revisar licencias/versiones de cada binario de terceros.
- **Criterio de aceptación**: cada herramienta listada existe en `Resources/` y cada requisito es real.

### P0-6 — Corregir rutas en BUILD_INSTRUCTIONS.md
- **Qué**: `.\CreatePortable.ps1` → `Installer/CreatePortable.ps1` y `.\CreateInstaller.ps1` → `Installer/CreateInstaller.ps1` (D-04); alinear con las rutas ya correctas de `.github/copilot-instructions.md:26-27`.
- **Por qué**: un ingeniero nuevo que siga el documento literalmente no encontrará los scripts.
- **Ventajas**: corrección de dos líneas; el documento pasa a ser fiable al completo.
- **Inconvenientes**: ninguno relevante.
- **Criterio de aceptación**: los comandos del documento existen tal cual ejecutándose desde la raíz.

### P0-7 — Reconciliar ROADMAP.md
- **Qué**: resolver la contradicción de fases (Fase 2 «WPF+MVVM» completada vs Fase 3 «Pendiente: interfaz WPF»: D-10); corregir el stack declarado («PowerShell 5.1+ / interfaz Web» → C#/.NET 9 WPF: D-08); refechar el estado real (may-jun 2026).
- **Por qué**: el roadmap es el contrato de dirección; hoy se contradice consigo mismo y con el producto.
- **Ventajas**: visión única del estado; facilita decidir los siguientes hitos reales.
- **Inconvenientes**: exige decidir qué ítems del roadmap aspiracional se conservan, se aparcan o se descartan.
- **Criterio de aceptación**: ninguna fase marcada «Pendiente» está ya implementada; el stack coincide con los `.csproj`.

---

## Fase P1 — Importante (riesgo, seguridad e higiene)

### P1-1 — CI con GitHub Actions
- **Qué**: crear `.github/workflows/ci.yml` que ejecute `dotnet build -c Release` y la suite de tests del runner propio en cada push/PR; requiere repositorio remoto (tras P0-1).
- **Por qué**: README.md:399 promete CI/CD que no existe (D-01); ~28 pruebas nunca se ejecutan automáticamente (riesgo R7).
- **Ventajas**: detección temprana de regresiones; base para automatizar portable/instalador y releases; credibilidad del proyecto.
- **Inconvenientes**: el runner de tests es un ejecutable de consola propio (hay que consumir su exit code en el workflow); requiere cuenta/repo remoto en GitHub; primeros ajustes de runner Windows.
- **Criterio de aceptación**: un push que rompe el build hace fallar el workflow; uno correcto pasa build+tests.

### P1-2 — SECURITY.md y CONTRIBUTING.md
- **Qué**: `SECURITY.md` con política de reporte de vulnerabilidades (proyecto que modifica registro y servicios con privilegios de administrador) y `CONTRIBUTING.md` con el proceso de ramas/commits que README.md:246-251 ya describe.
- **Por qué**: README promete un proceso de contribución inexistente; no hay canal de reporte de seguridad para una herramienta de nivel de sistema (señal estándar esperable en repos públicos).
- **Ventajas**: cumplimiento de lo prometido; reduce fricción de colaboración; posición formal ante vulnerabilidades.
- **Inconvenientes**: mantenimiento mínimo; deben mantenerse al día si cambia el proceso.
- **Criterio de aceptación**: los enlaces del README a contribución/security apuntan a archivos existentes.

### P1-3 — Completar CHANGELOG.md
- **Qué**: añadir entradas (formato Keep a Changelog) para los cambios de feb-jun 2026: scaffolding `.opencode/`/`.github/`, catálogos `tweaks.json` 2026.01 y `profiles.json` 2026.03, ajustes finales de junio.
- **Por qué**: única entrada es 13.0.0 (31-ene) pese a actividad hasta el 16-jun; el changelog es la única trazabilidad mientras no hay convención de commits.
- **Ventajas**: continuidad histórica verificable; cumplimiento del propio formato adoptado.
- **Inconvenientes**: reconstrucción parcial desde marcas de archivo (sin Git); fechas aproximadas en algunos ítems.
- **Criterio de aceptación**: cada cambio posterior a enero tiene entrada datada.

### P1-4 — Actualizar ARCHITECTURE.md
- **Qué**: marcar Hardware Detection como implementado (`HardwareDetector.cs` existe: D-09); documentar la estructura real (Models/Services de Core, ViewModels de App, TestRunner, catálogos de `Config/`, flujo de empaquetado).
- **Por qué**: documento clave para mantenimiento; hoy marca «Pendiente» lo que ya funciona.
- **Ventajas**: mapa fiel para el mantenimiento y para herramientas de IA; base para decisiones futuras.
- **Inconvenientes**: esfuerzo moderado de documentación técnica.
- **Criterio de aceptación**: cada componente descrito existe en el árbol de fuentes y viceversa (a nivel de primer orden).

### P1-5 — Archivar informes obsoletos de Reports/
- **Qué**: mover a `Reports/archive/` (o eliminar, con Git ya activo) los cinco informes de la era PowerShell: `Critical_Dependencies_Analysis.md`, `Duplication_Analysis_Report.md`, `Refactoring_Detailed_Plan.md`, `Security_Audit_Initial.md`, `Tweaks2_Functions_Mapping.md` (D-15, D-16); mantener `Tweaks2_Validation_Status.md` y `Tweak_Verification_Report_2026.md` como vigentes.
- **Por qué**: referencian scripts y arquitectura eliminados; contaminan búsquedas y confunden a lectores y agentes de IA.
- **Ventajas**: señal/ruido; la carpeta Reports vuelve a reflejar solo estado vigente; con Git no se pierde historia si se archiva.
- **Inconvenientes**: pérdida de consulta rápida del histórico (mitigada por Git o por `archive/`).
- **Criterio de aceptación**: ningún informe visible en `Reports/` referencia archivos inexistentes.

### P1-6 — Consolidar la infraestructura de agentes IA
- **Qué**: elegir un único sistema canónico — recomendado `.github/` (copilot-instructions + agents), por ser el más actual — y migrar lo útil de `.agent/` y `.opencode/` antes de eliminarlos (D-14, triple scaffolding).
- **Por qué**: tres sistemas solapados para el mismo propósito multiplican mantenimiento y contradicciones; `.opencode/AGENTS.md` duplica ARCHITECTURE.md; hay referencias muertas (`Scripts/…`, `Legacy/`).
- **Ventajas**: un punto de verdad para IA/colaboradores; menos tokens de contexto; menos deriva documental futura.
- **Inconvenientes**: pérdida potencial de prompts/skills específicos (mitigación: revisar y migrar antes de borrar); esfuerzo de consolidación moderado.
- **Criterio de aceptación**: un único árbol de configuración de agentes; cero referencias a rutas inexistentes.

### P1-7 — Eliminar artefactos huérfanos
- **Qué**: tras P0-1, eliminar `.unlighthouse/` (43 MB, webs ajenas), `.env` (claves de otras herramientas; verificar antes que ninguna toolchain activa las use), `.vscode/settings.json` vacío y los logs históricos de componentes eliminados en `Logs/` (WebMonitor_*, Updater_*, GameProfiles_*, HardwareDB_*).
- **Por qué**: son ajenos al proyecto o vestigiales; añaden 43 MB de peso y ruido; `.env` es superficie de exposición de claves (R2).
- **Ventajas**: repositorio más ligero y legible; menos superficie de filtración.
- **Inconvenientes**: irreversible sin el commit de respaldo (de ahí la dependencia de P0-1); `.env` podría servir a otra instalación de toolchain (verificar antes).
- **Criterio de aceptación**: el árbol no contiene `.unlighthouse/` ni `.env`; `Logs/` solo contiene logs vigentes.

### P1-8 — Crear .env.example y documentar LJOKERL_POWERSHELL_PATH
- **Qué**: `.env.example` con `LJOKERL_POWERSHELL_PATH` (usada en `ExecutionEngine.cs:547` y `HardwareDetector.cs:186`) y cualquier otra variable soportada; documentarla en README/DEVELOPMENT.
- **Por qué**: la variable se usa en código pero no existe documentación de dónde definirla (D-18); el `.env` actual es de otra toolchain.
- **Ventajas**: configuración reproducible; cero magia para nuevos entornos.
- **Inconvenientes**: mantenimiento si se añaden variables.
- **Criterio de aceptación**: clonar el proyecto y definir solo las variables del ejemplo permite ejecutar la app con PowerShell externo si aplica.

### P1-9 — Corregir referencias muertas del scaffolding de agentes
- **Qué**: en `.github/agents/orquestador/{memory,changelog,todo}.md` eliminar/rebasar referencias a `Scripts/Apply-Win11-SmoothVRR-RTX4080.ps1` y `Scripts/state/` (D-14); en `.agent/workflows/deploy_release.md` eliminar `Legacy` (D-16). (Si se ejecuta P1-6, queda absorbido.)
- **Por qué**: los agentes IA seguirán intentando operar sobre rutas inexistentes.
- **Ventajas**: instrucciones de agente fiables; menos alucinaciones de herramientas.
- **Inconvenientes**: ninguno relevante.
- **Criterio de aceptación**: búsqueda de `Scripts/` y `Legacy` sin resultados en la configuración de agentes.

---

## Fase P2 — Optimización (calidad técnica)

### P2-1 — Configuración de calidad .NET
- **Qué**: añadir `.editorconfig` (estilo C#), `global.json` (fijar SDK .NET 9) y `Directory.Build.props` centralizando `Version=13.0.0` (hoy duplicada en tres `.csproj`).
- **Por qué**: sin fijación de SDK, builds de reproducibilidad incierta entre máquinas; la versión triplicada deriva con facilidad.
- **Ventajas**: formato consistente entre editores; builds reproducibles; versión en un solo punto.
- **Inconvenientes**: mínimo; conviene una pasada de formato inicial.
- **Criterio de aceptación**: `dotnet --version` respeta `global.json`; cambiar la versión solo toca un archivo.

### P2-2 — Limpiar LJokerL.Core
- **Qué**: quitar `UseWindowsForms` de `LJokerL.Core.csproj` (la UI es WPF) y eliminar `Class1.cs` (plantilla vacía) verificando que nada lo referencia.
- **Por qué**: dependencia innecesaria en la librería de negocio; archivo plantilla documentado incluso por `.opencode/AGENTS.md`.
- **Ventajas**: grafo de dependencias limpio; menos superficie; compilación más ligera.
- **Inconvenientes**: riesgo bajo: verificar que ningún tipo WinForms se usa en Core antes de quitarlo.
- **Criterio de aceptación**: `dotnet build` correcto sin `UseWindowsForms` en Core; `Class1.cs` fuera del árbol.

### P2-3 — Estrategia para Resources/ (222 MB)
- **Qué**: decidir política: (a) mantener binarios versionados documentando versión+licencia+hash de cada uno, o (b) descarga bajo demanda con manifiesto de hashes. Documentar el resultado en DEPENDENCIES.md.
- **Por qué**: el 40 % del peso del repo son ejecutables de terceros con licencias propias y riesgo de cadena de suministro; hoy solo 4 de ~10 están documentados.
- **Ventajas de (a)**: instalador offline completo, sin dependencia de redes externas. **Inconvenientes de (a)**: peso y responsabilidad de actualización/licencias.
- **Ventajas de (b)**: repositorio ligero; verificación criptográfica. **Inconvenientes de (b)**: la instalación requiere conexión; los hashes deben mantenerse.
- **Criterio de aceptación**: cada herramienta de `Resources/` tiene documentado nombre, versión, origen, licencia y hash.

### P2-4 — Docs del portable generadas por script
- **Qué**: que `Installer/CreatePortable.ps1` copie README/USER_GUIDE/BUILD desde la raíz al empaquetar, y dejar de mantener manualmente las copias de `Output/LJokerL_TTV_Portable_v13.0/` (excluidas de Git tras P0-1).
- **Por qué**: hoy existen copias congeladas que envejecen y divergen (R6).
- **Ventajas**: cero deriva documental del distribuible; un solo original.
- **Inconvenientes**: requiere probar el script tras modificarlo; el portable se regenera al publicar.
- **Criterio de aceptación**: regenerar el portable produce docs idénticas a la raíz.

### P2-5 — Migrar el runner de tests a xUnit
- **Qué**: sustituir el runner propio (570 líneas, ~28 pruebas) por xUnit (paquetes ya ensayados y comentados en el `.csproj`), desacoplando `LJokerL.Tests` de `LJokerL.App`.
- **Por qué**: un runner propio no se integra con CI estándar ni con herramientas de cobertura; referencia anómala a la app WinExe.
- **Ventajas**: integración nativa con `dotnet test` y GitHub Actions; reportes y cobertura estándar.
- **Inconvenientes**: esfuerzo de migración de aserciones; el runner actual funciona (decisión de coste/beneficio, no urgencia).
- **Criterio de aceptación**: `dotnet test` ejecuta el total de pruebas migradas en CI.

### P2-6 — Política de idioma y estructura de documentación
- **Qué**: fijar idioma único para docs (hoy mezcla ES/EN: BUILD en inglés, resto en español) y valorar mover documentación de mantenimiento a `docs/` dejando en raíz README/CHANGELOG/LICENSE/TODO.
- **Por qué**: la mezcla de idiomas y la raíz saturada (9 documentos) dificultan la navegación; AGENTS.md ya define un orden de lectura largo.
- **Ventajas**: experiencia de lectura consistente; raíz despejada.
- **Inconvenientes**: rompe enlaces entrantes existentes (agentes, instrucciones) que habría que actualizar; requiere redirigir referencias.
- **Criterio de aceptación**: política documentada y aplicada; enlaces verificados.

### P2-7 — Directorios vacíos: documentar o eliminar
- **Qué**: para `GameProfiles/`, `Profiles/Templates/`, `Updates/` y `Database/{Components,HardwareProfiles,Recommendations}`: si son salidas de runtime, documentar su propósito en ARCHITECTURE.md y añadirlos a `.gitignore`; si son vestigios, eliminarlos.
- **Por qué**: un directorio vacío sin propósito documentado es ruido y ambigüedad.
- **Ventajas**: árbol sin zonas muertas.
- **Inconvenientes**: si el runtime los espera, hay que verificar tras eliminar (crear-bajo-demanda).
- **Criterio de aceptación**: cada directorio restante tiene propósito documentado o desaparece.

---

## Nota de cierre
Este plan es la traducción operativa del informe de auditoría. Los ítems P0 restablecen la confianza básica (verdad documental y trazabilidad); los P1 reducen riesgo y ruido; los P2 optimizan mantenimiento y distribución. Se recomienda ejecutar por orden de fase y cerrar cada ítem contra su criterio de aceptación.

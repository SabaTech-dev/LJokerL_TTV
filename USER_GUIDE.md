# Guía de usuario — LJokerL_TTV Custom Config PRO v13.0.1

Bienvenido a **LJokerL Control Center**, la aplicación de escritorio (WPF) de la suite LJokerL_TTV
Custom Config PRO. Esta guía describe la versión **13.0.1** tal y como funciona hoy: detección de
hardware, perfiles de optimización, monitorización en vivo y benchmarks, con registro completo de
cada cambio realizado.

---

## Requisitos

- Windows 10 compilación 19041 o superior (versión 2004), o Windows 11, 64 bits.
  En Windows 11 (build 22000 o superior) la aplicación está totalmente soportada. En
  Windows 10 (build 19041–21999) puede aplicar perfiles, pero al hacerlo verá el aviso
  «AVISO: build de Windows 10 sin validación oficial; el resultado no está garantizado»:
  la combinación no ha sido validada oficialmente, úsela bajo su responsabilidad. En builds
  anteriores a 19041 la aplicación de perfiles está bloqueada con el mensaje
  «Build de Windows no soportada».
- [.NET 9 Desktop Runtime (x64)](https://dotnet.microsoft.com/download/dotnet/9.0).
- Permisos de **administrador** para aplicar o revertir cambios del sistema. Sin ellos puede abrir
  la aplicación, ver la detección de hardware, monitorizar y ejecutar benchmarks, pero los botones
  de aplicación mostrarán el aviso «Requiere permisos de administrador».
- La carpeta `Config` (con `profiles.json`, `tweaks.json`, `services.json`, `apps_bloatware.json` y
  `hardware_rules.json`) junto al ejecutable: sin ella la aplicación muestra «Config directory not
  found» y no carga perfiles.
- Resolución mínima de ventana 900 × 600 (la ventana abre a 1280 × 800).

## Instalación

**Opción A — Instalador (recomendado).** Ejecute `LJokerL_TTV_Installer_v13.0.1.exe` (generado con
Inno Setup 6). El asistente instala la aplicación en `C:\Program Files\LJokerL_TTV`, crea accesos
directos en el menú Inicio y, si lo desea, en el escritorio, y ofrece abrir la aplicación al
terminar.

**Opción B — Versión portable.** Descomprima el ZIP portable (`LJokerL_TTV_Portable_v13.0.1.zip`) en
cualquier carpeta y ejecute `LJokerL.App.exe`. La aplicación no escribe en el registro de Windows
salvo cuando usted aplica o revierte ajustes; para moverla, copie la carpeta completa (incluye
`Config`).

Para generar cualquiera de los dos paquetes desde el código fuente, consulte
`BUILD_INSTRUCTIONS.md` (scripts `Installer/CreateInstaller.ps1` y `Installer/CreatePortable.ps1`).

## Primer inicio

1. Haga clic derecho sobre el ejecutable (o el acceso directo) y elija **Ejecutar como
   administrador**. La aplicación no eleva privilegios por sí sola; si arranca sin administrador,
   solo podrá consultar información.
2. En el arranque, la aplicación **detecta su hardware automáticamente** (CPU, GPU con VRAM, RAM,
   sistema operativo y compatibilidad VRR del monitor) y evalúa las reglas de
   `Config/hardware_rules.json` para mostrar recomendaciones en el Dashboard.
3. La cabecera superior muestra el uso de CPU, GPU y RAM en vivo.

## Interfaz y flujo de uso

La ventana **LJokerL Control Center** organiza las funciones en siete pestañas. En la versión
actual **Dashboard** y **Benchmarks** tienen contenido completo; las pestañas **Auto Config**,
**Perfiles**, **Tweaks**, **Monitor** y **Settings** aparecen en la navegación pero aún están
pendientes de implementación (se muestran vacías).

### 1. Dashboard

- **Hardware detectado**: resumen de CPU (núcleos/hilos), GPU y VRAM, RAM, almacenamiento y estado
  del monitor/VRR.
- **Quick Profiles**: tarjetas con los perfiles disponibles y botones **Apply Selected** y
  **Revert** (véase «Aplicar un perfil» más abajo).
- **Recommendations**: sugerencias generadas por las reglas de hardware (por ejemplo, perfil
  recomendado según su RAM o su monitor).
- **Status**: estado de carga de la configuración, última acción realizada y estado de la
  detección de hardware.
- **Monitoring**: uso de CPU, GPU, memoria y disco con barras de progreso, botón
  **Pausar/Reanudar** y casilla **Log metrics** para guardar las métricas en disco (por defecto se
  muestrea cada 2 segundos).

Flujo recomendado paso a paso:

1. Abra la aplicación como administrador y revise **Hardware detectado**.
2. Lea las **Recommendations**: indican qué encaja mejor con su equipo.
3. Aplique el perfil elegido (véase la sección siguiente).
4. Observe **Monitoring** unos minutos y, si quiere histórico, active **Log metrics** antes de
   aplicar el perfil.
5. Ejecute un benchmark antes y otro después (pestaña **Benchmarks**) para medir el efecto real.

### 2. Aplicar un perfil

Un perfil agrupa ajustes de registro (tweaks), servicios de Windows y aplicaciones preinstaladas
(bloatware). Al aplicarlo, la aplicación comprueba que tiene permisos de administrador, guarda una
instantánea de cada valor de registro antes de modificarlo, aplica los tweaks, desactiva los
servicios y elimina las aplicaciones del perfil una a una, registrando cada acción en el log de
ejecución, y muestra el resultado en **Status** (acciones realizadas o errores, con la ruta del
log).

**Punto de restauración (activado por defecto).** Antes de aplicar un perfil, la aplicación crea
un punto de restauración de Windows del tipo «modificar configuración», de forma que puedas
volver atrás desde el Restaurar sistema de Windows si algo no va bien. Ten en cuenta:

- Windows solo permite crear un punto de restauración cada 24 horas. Si ya existe uno reciente,
  la aplicación lo reutiliza y no crea otro (aparece una nota en **Status**).
- Si la creación del punto falla (por ejemplo, porque Restaurar sistema está deshabilitado en tu
  equipo), el perfil **no se aplica**: es una protección a favor de tu sistema.
- Puedes desactivar este comportamiento con la casilla «Crear punto de restauración antes de
  aplicar» del Dashboard.

**Vista previa (dry-run).** El botón **Vista previa (dry-run)**, junto a **Apply Selected**,
muestra el plan completo de acciones que se ejecutarían con el perfil seleccionado — cada valor
de registro a modificar, cada servicio a desactivar y cada aplicación a eliminar, con su riesgo
y si es reversible — **sin aplicar nada**. Es lectura pura: no necesita permisos de
administrador, no toca el registro, no detiene servicios ni desinstala nada, y no escribe en el
log de ejecución. Úsalo antes de aplicar un perfil para revisar exactamente qué va a cambiar.

En la versión actual, las tarjetas de perfil del Dashboard aún no admiten selección con clic; los
botones **Apply Selected** y **Revert** avisan con «No profile selected» hasta que esa
interacción esté disponible.

### 3. Benchmarks

- **Benchmark Rápido**: CPU y memoria (sin disco ni red); ideal para comparar en pocos minutos.
- **Benchmark Completo**: añade pruebas de disco (escritura de un archivo de 100 MB), de latencia
  de memoria y de red (ping a tres servidores públicos).
- El progreso se muestra con una barra y el resultado se guarda como un «run» en la carpeta
  `Benchmarks/Results`.
- **Comparar y exportar**: seleccione un run anterior (Antes) y uno posterior (Después), pulse
  **Comparar** para ver el resumen de diferencias y **Reporte Profesional** para generar un
  informe HTML en `Benchmarks/Reports`, que se abre automáticamente en su navegador.

### 4. Watcher de juegos (opt-in)

El watcher de juegos aplica automáticamente un perfil cuando detecta que se está ejecutando un
juego. Está **desactivado por defecto**; usted decide activarlo:

1. Edite `Config/game_watcher.json` junto al ejecutable y mapee el proceso del juego al perfil
   deseado, con `"enabled": true`. Ejemplo:

   ```json
   {
     "version": "2026.10",
     "mappings": [
       { "processName": "cyberpunk2077", "profileId": "gaming_pro", "enabled": true }
     ]
   }
   ```

   - `processName`: nombre del proceso **sin `.exe`** (no distingue mayúsculas).
   - `profileId`: `id` de un perfil de `Config/profiles.json`.
   - `enabled`: la entrada solo se considera con `true`. El catálogo de fábrica trae dos
     entradas de ejemplo desactivadas.
   - Si varios procesos vigilados coinciden a la vez, se aplica el **primero del catálogo** y se
     anota el conflicto en el estado.

2. Marque la casilla **«Watcher de juegos»** en el Dashboard. La app sondea los procesos cada
   5 segundos mientras esté abierta; el resultado de cada decisión aparece en **Status**.

Cada aplicación automática pasa por los mismos gates que un clic manual: permisos de
administrador, build de Windows soportada y punto de restauración previo. Sin administrador no
aplica nada y lo indica una sola vez. El watcher no revierte perfiles al cerrar el juego.

## Perfiles disponibles

Estos son los diez perfiles incluidos en `Config/profiles.json`:

| Perfil | Público al que va dirigido |
| :--- | :--- |
| `competitive_latency` | Juego competitivo en escritorio con latencia baja, evitando los ajustes de red y planificador más agresivos. |
| `smooth_vrr` | Monitores G-SYNC/VRR: estabilidad de frametimes y comportamiento moderno en Windows 11. |
| `gaming_pro` | Máximo rendimiento para juego competitivo: latencia mínima y FPS altos. |
| `streaming` | Transmitir con OBS mientras se juega. |
| `low_resources` | Equipos con 8 GB de RAM o menos: libera memoria. |
| `balanced` | Buen rendimiento sin romper funcionalidades del sistema; seguro para uso diario. |
| `max_performance` | Modo rendimiento extremo: desactiva casi todo a cambio de potencia bruta. |
| `laptop_saver` | Portátiles: mayor duración de batería con menor rendimiento. |
| `content_creator` | Edición de vídeo, streaming y creación de contenido. |
| `vr_ready` | Realidad virtual (Oculus, SteamVR, Windows Mixed Reality). |

## Registro y logs

Toda la actividad queda registrada en texto plano (JSON Lines) dentro de la carpeta de la
aplicación:

- `Logs/execution.jsonl` — cada acción aplicada o revertida (perfil, tweak, servicio o
  aplicación), con resultado, código de salida y marca de tiempo.
- `Logs/metrics.jsonl` — métricas de sistema si activa **Log metrics**.
- `Benchmarks/Results/` — los runs de benchmark en JSON.
- `Benchmarks/Reports/` — informes de comparación en HTML.
- `%APPDATA%\LJokerL_TTV\settings.json` — preferencias de usuario (estado e intervalo de
  monitorización y anulaciones manuales de hardware).

Si la carpeta `Logs` no puede crearse al iniciar, la aplicación continúa con el log alternativo
`%TEMP%\ljokerl_execution.jsonl` y lo indica en **Status**. Las rutas pueden redirigirse con
variables de entorno opcionales (`LJOKERL_LOG_PATH`, `LJOKERL_METRICS_LOG_PATH`,
`LJOKERL_SETTINGS_PATH`, `LJOKERL_CONFIG_PATH`, `LJOKERL_BENCHMARK_RESULTS_PATH`).

## Revertir cambios

La reversión tiene capacidades distintas según la vía:

- **Botón Revert (Dashboard)**: restaura **únicamente los valores de registro** que la aplicación
  del perfil modificó, guardados como instantánea la última vez que aplicó ese perfil. Los
  servicios desactivados y las aplicaciones desinstaladas **no se restauran** por esta vía: el
  motor de ejecución los omite como no soportados en la reversión. Requiere haber aplicado el
  perfil desde la propia aplicación (se apoya en su log de ejecución) y permisos de administrador.
- **Reversión por catálogo (motor, sin UI)**: los tweaks definen su acción de reversión en
  `Config/tweaks.json`; la reversión de servicios y aplicaciones la calcula el motor
  (re-habilitar servicio / re-registrar la aplicación). En la versión 13.0.1 esta reversión
  **no está expuesta en la interfaz**: no existe ningún botón que la ejecute. Es una capacidad
  del motor aún no accesible desde la aplicación.

En ningún caso pueden revertirse cambios aplicados fuera de la aplicación (con scripts o
manualmente): la aplicación no dispone de esa instantánea.

## Modo línea de comandos (CLI)

El ejecutable también funciona sin ventana cuando se invoca con argumentos.
Comandos disponibles en esta fase:

- `--list-profiles`: muestra los identificadores y nombres de los perfiles.
- `--apply-profile <id> --dry-run` / `--revert-profile <id> --dry-run`: muestra
  el plan de acciones del perfil (tweaks, servicios, aplicaciones) sin ejecutar
  nada.
- `--help`: muestra la ayuda con todos los códigos de salida.

Por seguridad, la ejecución real de apply/revert por CLI no está disponible:
sin `--dry-run` estos comandos no tocan el sistema y devuelven el código de
salida `2`. La ejecución real por CLI llega en la siguiente iteración (M2).

> **Nota:** la app es una aplicación de ventanas (WinExe): al ejecutarla desde
> una terminal interactiva la salida de texto puede no mostrarse; usa
> redirección (`LJokerL.App.exe --list-profiles > perfiles.txt`) o el código de
> salida (0/1/2), que siempre funciona.

## Solución de problemas

| Síntoma | Causa y solución |
| :--- | :--- |
| «Requiere permisos de administrador» | Reinicie la aplicación con **Ejecutar como administrador**. |
| «Config directory not found» / «Config load failed» | Ejecute `LJokerL.App.exe` desde su carpeta de instalación, junto a la carpeta `Config`. |
| «No profile selected» | Las tarjetas de perfil aún no admiten selección con clic (pendiente de implementación). |
| «Init failed; using fallback log path» | La carpeta `Logs` no es escribible; el log se escribe en `%TEMP%`. Revise permisos de la carpeta de instalación. |
| «Hardware detection failed» | Compruebe que WMI/CIM funciona en su sistema; los valores pueden corregirse manualmente en `settings.json` (anulaciones de CPU, GPU, RAM, SO y VRR). |
| La GPU aparece a 0 % en Monitoring | El contador «GPU Engine» de Windows puede tardar en activarse; espere unos segundos o reinicie la aplicación. |
| El botón **Re-escanear** no hace nada | Está pendiente de conectar a la detección de hardware; reinicie la aplicación para volver a detectar. |

## Más información

- [README.md](README.md) — visión general, arquitectura y catálogo de ajustes.
- [CHANGELOG.md](CHANGELOG.md) — historial de cambios por versión.
- [BUILD_INSTRUCTIONS.md](BUILD_INSTRUCTIONS.md) — cómo compilar y empaquetar.

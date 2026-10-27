# Dependencias — LJokerL_TTV Custom Config PRO

- **Versión del documento**: 13.0.0 (alineada con `src/LJokerL.App/LJokerL.App.csproj` y `CHANGELOG.md`).
- **Plataforma**: aplicación WPF (.NET 9) para Windows.

## 1. Dependencias en tiempo de ejecución

| Dependencia | Requisito | Notas |
| --- | --- | --- |
| Windows 11 | Objetivo principal | Windows 10 2004+ puede funcionar, pero no está validado por CI ni pruebas. |
| .NET Desktop Runtime | 9.0 x64 (`net9.0-windows`) | Incluye WPF. |
| Privilegios de administrador | Obligatorios | El motor ejecuta cambios de registro, servicios y procesos. |
| PowerShell 5.1+ | Condicional | Solo para las operaciones del motor que lo invocan (apartado 1.1). |
| Servicio WMI (Winmgmt) | Recomendado | Detección de hardware y métricas; si falta, el sistema degrada con valores por defecto. |
| Contadores de rendimiento («GPU Engine») | Recomendados | Monitor de uso de GPU; sin ellos las métricas se muestran parciales. |

### 1.1 PowerShell en el motor

`ExecutionEngine` y `HardwareDetector` (LJokerL.Core) invocan PowerShell en
operaciones concretas de consulta y aplicación. La ruta del ejecutable se
resuelve en este orden:

1. Variable de entorno `LJOKERL_POWERSHELL_PATH` (sobrecarga manual; permite
   apuntar a PowerShell 7 si está instalado).
2. En su defecto, `powershell.exe` del sistema (Windows PowerShell 5.1).

El resto de la aplicación no depende de PowerShell.

## 2. Dependencias de desarrollo y compilación

| Herramienta | Versión | Uso |
| --- | --- | --- |
| .NET SDK | 9.0 | Compilación, pruebas y publicación (`dotnet`). |
| Inno Setup | 6.x | Generación del instalador mediante `iscc` (ver `BUILD_INSTRUCTIONS.md`). |
| PowerShell | 5.1+ | Scripts de empaquetado y automatización. |
| git | Versión actual | Control de versiones y CI. |

Paquetes NuGet: solo el proyecto de pruebas declara referencias (`xunit`
2.9.2, `xunit.runner.visualstudio` 2.8.2, `FluentAssertions` 6.12.2,
`Moq` 4.20.72, `coverlet.collector` 6.0.2, `Microsoft.NET.Test.Sdk` 17.12).
Los proyectos `LJokerL.App` y `LJokerL.Core` no dependen de paquetes NuGet:

- `System.Management` se carga por reflection y es opcional; sin él, la
  temperatura de CPU muestra «N/A».
- El uso de GPU se lee de la categoría «GPU Engine» de los contadores de
  rendimiento de Windows; no requiere SDK del fabricante.

## 3. Herramientas externas en `Resources/`

**Política vigente**: los binarios de terceros NO se versionan en el
repositorio (`Resources/*.exe`, `*.dll`, `*.sys`, `*.zip`, `*.msi`, `*.pdb`
y `Resources/SCEHUB-1.2.0/` están excluidos por `.gitignore`). Este apartado
documenta el inventario local auditado; el manifiesto con hashes SHA-256
queda pendiente como tarea P2-3.

**Licencias**: todas las herramientas de esta sección son de terceros. NO
están cubiertas por la licencia MIT del proyecto y su redistribución NO
está cubierta: deben obtenerse de sus fuentes originales.

| Herramienta | Versión conocida | Propósito | Licencia |
| --- | --- | --- | --- |
| `NVCleanstall_1.19.0.exe` | 1.19.0 | Instalación limpia y personalizada de drivers NVIDIA. | Terceros |
| `MemTest64.exe` | Sin versión en el nombre | Prueba de estabilidad de memoria RAM. | Terceros |
| `MPOGPUFIX.exe` | Sin versión en el nombre | Corrección de problemas de Multi-Plane Overlay (MPO) en GPU. | Terceros |
| `GoInterruptPolicy.exe` | Sin versión en el nombre | Ajuste de afinidad y modo de interrupciones por dispositivo. | Terceros |
| `CrystalMark3D25_1_0_0.exe` | 25.1.0.0 | Benchmark 3D. | Terceros |
| `Counter Control.exe` | Sin versión en el nombre | Gestión de contadores de rendimiento de GPU. | Terceros |
| `InSpectre.exe` | Sin versión en el nombre | Consulta y gestión de mitigaciones Spectre/Meltdown. | Terceros |
| `MSI_util_v3.exe` | v3 | Activación de interrupciones señaladas (MSI) por dispositivo. | Terceros |
| `winscript-portable.exe` | Sin versión en el nombre | Scripts de optimización y limpieza de Windows (portable). | Terceros |
| `WindowsReset.zip` | Sin versión en el nombre | Utilidades de restablecimiento de Windows. | Terceros |
| `ActualizarNvidiaDLSS/` | Sin versión en el nombre | DLLs de NVIDIA DLSS (dlss/dlssd/dlssg, ~142 MB) para actualizar DLSS en el sistema. | Terceros |
| `SCEHUB-1.2.0/` | 1.2.0 | Conjunto de utilidades (~18 MB) que incluye `UEFITool.exe`. | Terceros |
| `Regedits & Profiles/` | Sin versión en el nombre | Ajustes de registro (`.reg`) y perfiles NVIDIA (`.nip`). | Terceros |

Las filas sin versión no la declaran en el nombre de archivo; verifíquese en
el origen de cada herramienta. El inventario corresponde al listado auditado
del directorio `Resources/`: los binarios no están en el árbol de git, por lo
que la verificación se realiza contra ese listado y el directorio local
(`git ls-files Resources/` frente a `Resources/`).

## 4. Catálogos de datos internos (`Config/`)

Dependencias de datos de primera parte, versionadas en el repositorio y
consumidas por el motor:

- `tweaks.json` — catálogo de tweaks.
- `services.json` — catálogo de servicios.
- `apps_bloatware.json` — catálogo de aplicaciones bloatware.
- `profiles.json` — perfiles de optimización.
- `hardware_rules.json` — reglas contextuales de detección de hardware.
- `selected_profile.txt` — estado local del perfil seleccionado (no es contrato).

## 5. APIs y servicios de Windows utilizados

- Windows Management Instrumentation (WMI), cargado por reflection.
- Contadores de rendimiento de Windows (categoría «GPU Engine»).
- Registro de Windows, control de servicios y gestión de procesos.

---

**Nota**: la diferencia entre `git ls-files Resources/` y el contenido del
directorio local es esperada: los binarios de terceros se excluyen por
`.gitignore` según la política del apartado 3.

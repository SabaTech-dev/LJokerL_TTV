# E2E — Pruebas end-to-end en Windows Sandbox

Este directorio contiene el arnes de pruebas E2E de LJokerL_TTV que se ejecuta
**dentro de una Windows Sandbox desechable**. La maquina anfitriona nunca recibe
mutaciones del sistema: en el host solo se crean los ficheros de este directorio
(`run.wsb`, transcripts) y los artefactos de resultado (`E2E\RESULTS.md`).

## Ejecucion

```powershell
# Desde la raiz del repo (host):
powershell -ExecutionPolicy Bypass -File E2E\WindowsSandbox\new-sandbox.ps1
```

El generador:

1. Comprueba (solo lectura) que la caracteristica `Containers-DisposableClientVM`
   (Windows Sandbox) esta habilitada. Si no lo esta, falla con mensaje claro.
2. Genera `E2E\WindowsSandbox\run.wsb` mapeando la raiz del repo con `ReadOnly=false`.
   La carpeta mapeada se monta en la sandbox en `C:\mnt\<N>` (orden alfabetico de
   `HostFolder`; con una sola carpeta, `C:\mnt\1`). El `LogonCommand` descubre el
   punto de montaje en tiempo de ejecucion, asi que no depende de `N`.
3. Arranca la sandbox con `WindowsSandbox.exe run.wsb`.

Dentro de la sandbox, el logon ejecuta `E2E\WindowsSandbox\e2e-inside.ps1`, que
auto-detecta la unidad mapeada (marcador `E2E\WindowsSandbox\e2e-inside.ps1` en
A:..Z:), escribe resultados incrementalmente en `E2E\RESULTS.md` y, al terminar,
escribe `===E2E-DONE===` y apaga la sandbox.

Desde el host se puede sondear el progreso:

```powershell
Get-Content E2E\RESULTS.md -Tail 10
```

## Escenarios validados

| Escenario | Que valida | Muta el host? |
|-----------|------------|---------------|
| S0 ENTORNO | Build de Windows, RAM, System Restore deshabilitado (esperado en sandbox), elevacion admin | No |
| S1 SDK | Instala .NET 9 SDK (winget; fallback: instalador aka.ms) y verifica `dotnet --list-sdks` | Solo sandbox |
| S2 BUILD | `dotnet build LJokerL.sln -c Release` con 0 errores | Artefactos bin/obj del repo mapeado |
| S3 TESTS | `dotnet test LJokerL.sln -c Release` (recuento esperado: 78) | Artefactos bin/obj del repo mapeado |
| S4 CLI REAL | `--list-profiles` (0 + gaming_pro), `--help` (0), `--apply-profile gaming_pro --dry-run` (0 + menciona acciones), `--apply-profile gaming_pro` sin dry-run (exit 2, barrera de seguridad) | No |
| S5 GUI SMOKE | El exe sin argumentos sobrevive 20 s y se cierra limpiamente | No |
| S6 MOTOR REAL | Harness en `C:\e2e-harness` con ProjectReference a `LJokerL.Core`: aplica el perfil `balanced` con `ExecutionEngine`, verifica `Logs\execution.jsonl` (entradas applied con `PreviousValue`), comprueba UNA clave de registro contra el valor aplicado, revierte por catalogo (`RevertTweak`/`RevertService`/`RestoreApp`) y comprueba que la clave volvio al `PreviousValue` | SOLO sandbox (registro) |
| S7 RESTORE FAIL-SAFE | Con System Restore deshabilitado: `SystemRestoreService.IsEnabled()` (esperado false), `SystemRestoreGate.Decide(setting on, enabled false)` (esperado `SkipFailSafe`/`SystemRestoreDisabled`); con `CreateRestorePointBeforeApply=true` el apply NO llega a ejecutarse | SOLO sandbox |
| S8 WATCHER | Escribe settings (EnableGameWatcher=true, CreateRestorePointBeforeApply=false), habilita mapping `notepad -> balanced` en el Config de salida del App, lanza la GUI, lanza notepad y verifica que `Logs\execution.jsonl` del App recibe entradas applied del perfil balanced | SOLO sandbox (config de usuario + registro) |
| S9 INSTALADOR | Instala Inno Setup 6 (winget; fallback: is.exe), ejecuta `Installer\CreateInstaller.ps1` y verifica `Output\LJokerL_TTV_Installer_v13.0.1.exe` | SOLO sandbox (mas bin/obj y Output del repo mapeado) |

## Aviso importante

**S6, S7 y S8 mutan el interior de la sandbox** (claves de registro via el motor
real, ficheros de configuracion de usuario `%APPDATA%\LJokerL_TTV\settings.json`).
Esa es su finalidad: probar el motor con mutaciones reales en un entorno
desechable. La sandbox se apaga sola al terminar (`Stop-Computer -Force`) y todo
su estado se destruye. Ningun escenario muta el sistema del host.

## Artefactos generados

- `E2E\RESULTS.md` — resultados incrementales S0-S9 con veredicto PASSED/FAILED.
- `E2E\WindowsSandbox\transcript-inside.txt` — transcript completo de la sesion
  PowerShell interior.
- `E2E\WindowsSandbox\run.wsb` — configuracion de la sandbox (regenerable).

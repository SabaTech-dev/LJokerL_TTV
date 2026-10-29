# Resultados E2E seguros en host — 2026-10-29

Ejecución: 28/10/2026 (tarde), sobre el host, sin Windows Sandbox. Binario tras rebuild Release.

| Escenario | Resultado | Evidencia |
| --- | --- | --- |
| E-CLI-1 | PASSED | exit 0; perfil `gaming_pro` presente |
| E-CLI-2 | PASSED | usage impreso correctamente |
| E-CLI-3 | PASSED | 61 acciones: registry 32, command 5, service 10, app 14; 61 reversibles |
| E-CLI-4 | PASSED | barrera exit 2; `HKCU\System\GameConfigStore\GameDVR_Enabled=0` antes y después; logs sin cambios |
| E-GUI-1 | PASSED | proceso vivo 20 s, sin ficheros nuevos en Logs al arrancar, cierre limpio |
| E-GUI-2 | PASSED | sin procesos colgados |
| E-TESTS | PASSED | 78/78 |

## Limitaciones

- Windows Sandbox: el cliente sale silenciosamente sin crear la VM en este host (característica instalada, hipervisor operativo); pendiente de reintento tras reinicio del host.
- El binario Release obsoleto causó una falsa alarma inicial; tras rebuild todo en verde.

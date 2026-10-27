# 🛡️ Informe Inicial de Auditoría de Seguridad y Código

## Resumen Ejecutivo

- Alcance: Escaneo estático de scripts PowerShell del proyecto para patrones de alto riesgo y prácticas sensibles.
- Estado: Completo el barrido inicial (3 lotes de patrones). Se detectan usos esperados de privilegios elevados y políticas de ejecución. No se detectaron cargas/descargas remotas sospechosas ni IEX.
- Prioridades: Mitigar uso de `ExecutionPolicy Bypass` en ejecuciones y accesos con `-Verb RunAs` con controles de integridad y consentimiento. Documentar justificación de `-WindowStyle Hidden` en operaciones legítimas.

## Hallazgos Técnicos

### 1) Network/Descargas Remotas
- Resultado: Sin coincidencias de alto riesgo para `Invoke-Expression`/`IEX`.
- Descargas legítimas:
  - `LJokerL_TTV_Auto_Updater.ps1`: Uso de `Invoke-RestMethod` con timeout y headers en `L167` y `L184` (solicitud a API/version.json). Integridad validada con `Get-FileHash` (SHA256) en `L364` y `L586`.
- Riesgo: Bajo (controlado, con hash y sin ejecución directa de contenido remoto).
- Recomendación:
  - Mantener validación SHA256 obligatoria y verificar TLS 1.2+ a nivel de sistema.

### 2) Execution Policy / Elevación de Privilegios
- `ExecutionPolicy Bypass` detectado:
  - `LJokerL_TTV_Custom_Config_PRO_ADAPTIVE.ps1` L7472
  - `Backups/..._Backup.ps1` L7457
  - `LJokerL_TTV_Installer.ps1` L374, L386, L665
- Elevación con `Start-Process -Verb RunAs`:
  - Múltiples usos en main y backup (instalación de herramientas, reinvocación administrativa, ejecutables externos).
- Riesgo: Medio — esperado por la naturaleza del proyecto, pero requiere salvaguardas.
- Recomendaciones:
  - Añadir confirmación explícita al usuario antes de reinvocación con Bypass.
  - Limitar `Bypass` a ámbito de proceso y solo cuando sea imprescindible.
  - Registrar acciones elevadas (origen, hora, comando) en `Logs/`.

### 3) Silenciamiento de Errores
- Uso extensivo de `-ErrorAction SilentlyContinue` en operaciones de servicio, registro y archivos.
- Riesgo: Medio — puede ocultar fallos críticos.
- Recomendaciones:
  - Cambiar a `-ErrorAction Stop` en operaciones críticas y encapsular con try/catch con logging y códigos de salida.
  - Para limpiezas no críticas, mantener SilentlyContinue pero con contador y resumen de elementos afectados.

### 4) Tareas Programadas y Cambios de Sistema
- `schtasks` para (des)habilitar desfragmentación y crear tarea `LJokerL_CleanTemp`.
- Operaciones de registro vía `regedit /s` y herramientas con `-WindowStyle Hidden` (aplicación de perfiles NVIDIA y .reg silencioso).
- Riesgo: Medio — válido, pero requiere auditoría y registro.
- Recomendaciones:
  - Registrar creación/modificación de tareas con parámetros completos.
  - Para `.reg`, validar origen y hash antes de aplicar; registrar ruta y resultado.

### 5) Firewall y TLS
- No se detectaron reglas de firewall agregadas o deshabilitadas.
- No se fuerza explícitamente TLS desde scripts; depender de configuración del sistema.
- Riesgo: Bajo.
- Recomendación: Verificar que el host usa TLS 1.2+ por política del sistema. Documentar requisito.

## Matriz de Riesgo (Resumen)
- Alto: 0
- Medio: 3 (Bypass+Elevación, Silenciamiento de errores extensivo, tareas programadas/reg ocultos)
- Bajo: 2 (Descargas con hash, dependencias TLS del sistema)

## Acciones Prioritarias (Next Steps)
1) Agregar logging reforzado y confirmaciones para cualquier reinvocación con `-Verb RunAs` o `ExecutionPolicy Bypass`.
2) Endurecer manejo de errores: `-ErrorAction Stop` + try/catch con `Write-Log` y códigos de salida en operaciones críticas.
3) Validar SHA256 de ficheros `.reg` y recursos antes de `regedit /s`; documentar hash en `Resources/manifest.json`.
4) Registrar todas las operaciones de `schtasks` (creación/cambio) y permitir rollback.
5) Añadir sección de seguridad en `README.md` con políticas: mínimos privilegios, integridad de recursos, y trazabilidad de cambios.

## Evidencia de Búsquedas (extractos)
- Execution Policy Bypass:
  - `LJokerL_TTV_Custom_Config_PRO_ADAPTIVE.ps1:7472`
  - `Backups\LJokerL_TTV_Custom_Config_PRO_ADAPTIVE_Backup.ps1:7457`
  - `LJokerL_TTV_Installer.ps1:374, 386, 665`
- Elevación RunAs:
  - `LJokerL_TTV_Custom_Config_PRO_ADAPTIVE.ps1:451, 704, 4850, 7473`
- Descargas y verificación:
  - `LJokerL_TTV_Auto_Updater.ps1:167, 184, 364, 586`
- Ventana oculta legítima:
  - `LJokerL_TTV_Custom_Config_PRO_ADAPTIVE.ps1:497, 621`
- Tareas programadas:
  - `LJokerL_TTV_Custom_Config_PRO_ADAPTIVE.ps1:8329, 8364, 9810, 9814, 10002`
  - `PC_TOP.ps1:287`
  - `LJokerL_TTV_Hardware_Database.ps1:1317`

## Cierre
Este informe cubre el barrido inicial. Próximo paso: aplicar endurecimiento en puntos críticos y preparar reporte detallado por módulo con líneas afectadas y propuesta de edits.

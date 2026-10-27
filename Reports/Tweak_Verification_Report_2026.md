# Tweak Verification Report 2026 (Win11 25H2)

Version: 0.1 (inicial)
Fecha: 2026-01-26
Alcance: `ComprobarPosiblesNuevosTweaks/tweaks1.md` y `ComprobarPosiblesNuevosTweaks/tweaks2.md`

## Fuentes consultadas (oficiales + comunidad)

Oficial/semioficial:
- NVIDIA MPO workaround (registro para desactivar MPO): https://nvidia.custhelp.com/app/answers/detail/a_id/5157/
- TechPowerUp NVCleanstall (driver slim/limpio NVIDIA): https://www.techpowerup.com/download/techpowerup-nvcleanstall/

Comunidad (foros especializados):
- Blur Busters G-SYNC 101 (VRR/V-Sync/FPS cap): https://blurbusters.com/gsync/gsync101-input-lag-tests-and-settings/
- Blur Busters forum (VRR + límites FPS): https://forums.blurbusters.com/viewtopic.php?t=9477
- Guru3D forum (tweaks AMD/driver): https://forums.guru3d.com/threads/radeonmod-fortnite-tweak.434553/
- ElevenForum (ajustes UI/Widgets Windows 11): https://www.elevenforum.com/t/is-there-a-way-to-reload-reinstall-the-widgets.654/
- Reddit /r/Windows11 (opiniones/experiencias generales): https://old.reddit.com/r/Windows11/
- Overclock.net (comunidad de OC/tweaks; acceso parcial por JS): https://www.overclock.net/search/?q=HPET

Repositorios relevantes (gaming configs / tweaks):
- ChrisTitusTech WinUtil: https://github.com/ChrisTitusTech/winutil
- AtlasOS Playbook: https://github.com/Atlas-OS/Atlas
- Hone (tool gaming): https://github.com/auraside/Hone
- Win10 Initial Setup Script (referencia histórica): https://github.com/Disassembler0/Win10-Initial-Setup-Script

Notas:
- Algunas fuentes oficiales de Microsoft no son accesibles desde el entorno (404). Para esos casos, se marca “Sin doc oficial” y se toma consenso comunitario + experiencia de campo.
- Foros se usan como contraste, no como fuente única.

## Criterios de clasificación

- SAFE: documentado o consenso fuerte; reversible; bajo riesgo.
- ADVANCED: impacto real pero depende de hardware/uso; requiere validación o reboot; aplicado por defecto según tu criterio.
- EXPERIMENTAL: no documentado, evidencia mixta o riesgo de degradación/estabilidad; solo manual.
- NO RECOMENDADO: seguridad comprometida, obsoleto, alto riesgo o sin beneficio verificable.

## Perfiles base (a implementar en la nueva UI)

Competitive Latency:
- VRR opcional; V-Sync OFF; sin cap o cap alto.
- Evitar colas de render; prioridad a input lag.

Smooth VRR:
- VRR ON + V-Sync ON (driver); FPS cap ≈ Hz-2/3.
- Basado en recomendaciones Blur Busters (VRR + V-SYNC + límite FPS dentro del rango).

## Revisión detallada

### A) `tweaks1.md`

Resumen: contiene claves internas de DirectX/DXGKrnl y ajustes kernel no documentados. Alta probabilidad de placebo o regresiones en Win11 25H2. Recomendación: NO aplicar por defecto; solo Experimental con advertencia clara.

#### A.1 DirectX (HKLM\SOFTWARE\Microsoft\DirectX)

Sin documentación oficial publicada; foros reportan resultados inconsistentes. Clasificación: EXPERIMENTAL.

- D3D12_ENABLE_UNSAFE_COMMAND_BUFFER_REUSE = 1
- D3D12_ENABLE_RUNTIME_DRIVER_OPTIMIZATIONS = 1
- D3D12_RESOURCE_ALIGNMENT = 1
- D3D11_MULTITHREADED = 1
- D3D12_MULTITHREADED = 1
- D3D11_DEFERRED_CONTEXTS = 1
- D3D12_DEFERRED_CONTEXTS = 1
- D3D11_ALLOW_TILING = 1
- D3D11_ENABLE_DYNAMIC_CODEGEN = 1
- D3D12_ALLOW_TILING = 1
- D3D12_CPU_PAGE_TABLE_ENABLED = 1
- D3D12_HEAP_SERIALIZATION_ENABLED = 1
- D3D12_MAP_HEAP_ALLOCATIONS = 1
- D3D12_RESIDENCY_MANAGEMENT_ENABLED = 1

#### A.2 DXGKrnl (HKLM\SYSTEM\CurrentControlSet\Services\DXGKrnl)

Claves internas sin doc oficial; riesgo alto de inestabilidad gráfica. Clasificación: NO RECOMENDADO / EXPERIMENTAL.

- CreateGdiPrimaryOnSlaveGPU = 1
- DriverSupportsCddDwmInterop = 1
- DxgkCddSyncDxAccess = 1
- DxgkCddSyncGPUAccess = 1
- DxgkCddWaitForVerticalBlankEvent = 1
- DxgkCreateSwapChain = 1
- DxgkFreeGpuVirtualAddress = 1
- DxgkOpenSwapChain = 1
- DxgkShareSwapChainObject = 1
- DxgkWaitForVerticalBlankEvent = 1
- DxgkWaitForVerticalBlankEvent2 = 1
- SwapChainBackBuffer = 1
- TdrResetFromTimeoutAsync = 1

#### A.3 Kernel / Scheduler (Session Manager\kernel)

Valores críticos sin soporte público; pueden degradar estabilidad y tiempos de interrupción. Clasificación: NO RECOMENDADO.

- DistributeTimers
- DpcWatchdogProfileOffset
- DpcTimeout
- IdealDpcRate
- MaximumDpcQueueDepth
- MinimumDpcRate
- DpcWatchdogPeriod
- UnlimitDpcQueue
- MaxDynamicTickDuration
- MaxDynamicTickDurationSize
- MinDynamicTickDuration
- SerializeTimerExpiration
- MaximumSharedReadyQueueSize
- BufferSize
- LargeSystemCache
- IoQueueWorkItem
- IoQueueWorkItemToNode
- IoQueueWorkItemEx
- IoQueueThreadIrp
- ExTryQueueWorkItem
- ExQueueWorkItem
- IoEnqueueIrp
- DisableLowQosTimerResolution
- XMMIZeroingEnable
- UseNormalStack
- UseNewEaBuffering
- StackSubSystemStackSize
- PriorityControl (kernel)
- DisableOverlappedExecution
- TimeIncrement
- QuantumLength

#### A.4 PriorityControl (HKLM\SYSTEM\CurrentControlSet\Control\PriorityControl)

Valores no documentados; riesgo de scheduling errático. Clasificación: NO RECOMENDADO.

- ForegroundBoost
- ThreadBoostType
- ThreadSchedulingModel
- AdjustDpcThreshold
- DeepIoCoalescingEnabled
- IdealDpcRate
- SchedulerAssistThreadFlagOverride

#### A.5 Seguridad/mitigaciones

Desactiva protecciones críticas. Clasificación: NO RECOMENDADO.

- DisableExceptionChainValidation = 1
- KernelSEHOPEnabled = 0
- DontVerifyRandomDrivers = 1
- NoDataExecutionPrevention = 1
- DisableHHDEP = 1
- HVCIMATRequired = 0

#### A.6 Memory Management

Sin evidencia de beneficio en Win11 25H2. Clasificación: NO RECOMENDADO.

- MoveImages
- SessionPoolSize
- SessionViewSize

#### A.7 Power / Latency (Control\Power y GraphicsDrivers\Power)

Valores de latencia forzados a 1; sin doc oficial. Clasificación: EXPERIMENTAL.

- ExitLatency / Latency / LatencyTolerance* / RtlCapabilityCheckLatency
- DefaultD3TransitionLatency* / DefaultLatencyTolerance* / DefaultMemoryRefreshLatencyTolerance*
- MaxIAverageGraphicsLatencyInOneBucket / MonitorLatencyTolerance / TransitionLatency

#### A.8 Networking/Server

- LanmanServer\Parameters SizeReqBuf: sin doc de beneficio en gaming. NO RECOMENDADO.

#### A.9 AMD GPU Registry (Class GUID 4d36e968...)

LTR* / KMD_RpmComputeLatency / Dal* / memClockSwitchLatency / PP_* / BGM_*:
Sin documentación pública; riesgo de degradación o conflictos con driver. EXPERIMENTAL.

#### A.10 MMCSS

- HKLM\SYSTEM\CurrentControlSet\Services\MMCSS Start=4 (deshabilitar): NO RECOMENDADO.
  MMCSS es clave para audio/video y scheduling multimedia.

#### A.11 Otros

- CoalescingTimerInterval (Session Manager\Power): NO RECOMENDADO.
- Reliability\TimeStampInterval: NO RECOMENDADO.
- Ole\LegacyImpersonationLevel: NO RECOMENDADO.
- DeviceGuard\HVCI Enabled=1: contradictorio con otras desactivaciones; requiere política clara.

Conclusión `tweaks1.md`:
- No se incluirá en perfiles automáticos.
- Solo se expondrá en “Experimental” con advertencia fuerte.

### B) `tweaks2.md` (parte batch “Gaming Optimizer PRO”)

#### B.1 Limpieza profunda

- Limpieza de TEMP/cache/logs/shaders: SAFE (manual/opt-in)
- Borrado de Shadow Copies: ADVANCED (pérdida de puntos de restauración)
- Desactivar programador de defrag: NO RECOMENDADO (Windows gestiona SSD/HDD)

#### B.2 Servicios

Desactivar servicios masivo puede romper Windows Update, Xbox, impresión, búsquedas, etc. Clasificación general: ADVANCED (con lista editable y reversión granular). No aplicar a ciegas.

#### B.3 Visual / UI

Desactivar animaciones/transparencia: SAFE (impacto mínimo, más que nada UX).

#### B.4 Startup

Desactivar startups comunes: SAFE (por usuario).

#### B.5 Energía

Plan Ultimate/High Performance: ADVANCED (según hardware; no recomendado en portátiles).
Desactivar hibernación: ADVANCED (ahorra espacio, afecta suspensión).

#### B.6 Game Mode / Game DVR / HAGS

Game Mode + Game DVR OFF: ADVANCED (depende de juego y drivers).
HAGS: ADVANCED (depende de GPU/driver y juego). Sin doc oficial accesible, se mantiene condicional.

#### B.7 Red

Desactivar autotuning / forzar offloads: EXPERIMENTAL.
Forzar TcpAckFrequency/TCPNoDelay global: ADVANCED/EXPERIMENTAL (depende del stack y router).

#### B.8 Background apps

Desactivar apps en segundo plano: SAFE (UX y privacidad). No afecta FPS directamente.

#### B.9 GPU (general)

- HAGS ON: ADVANCED.
- Disable GPU preemption (EnablePreemption=0): NO RECOMENDADO.
- DirectXUserGlobalSettings VRR flags: EXPERIMENTAL (sin doc).

#### B.10 Input (mouse/keyboard)

- Mouse acceleration OFF: SAFE.
- Queue sizes: ADVANCED (sensible por hardware/driver).

#### B.11 Debloat / Features

- Remover apps OEM/bloat: ADVANCED (depende del usuario).
- Desactivar features (XPS/WorkFolders/Media Player): ADVANCED (opt-in).

#### B.12 GPU específica NVIDIA/AMD

Registry tweaks (PowerMizer/ULPS/etc): EXPERIMENTAL.
Recomendación: usar herramientas oficiales (panel NVIDIA/AMD) o NVCleanstall (TechPowerUp).

### C) `tweaks2.md` (JSON de ajustes UI/privacidad/sistema)

La sección JSON contiene múltiples toggles UI/privacidad/sistema. Impacto gaming bajo o nulo. Clasificación general:

- UI & Personalization: SAFE (preferencias del usuario).
- Privacy & Telemetry: ADVANCED (depende de edición y políticas; requiere reversión clara).
- System (hibernación, power plan, delivery optimization, etc.): ADVANCED.

Incluye grupos adicionales:
- Gaming: XboxGameBar, XboxGameTips, GPUScheduling.
- Scheduled tasks: CleanupTask, SoftwareDistributionTask, TempTask.
- Microsoft Defender & Security: NetworkProtection, PUAppsDetection, DefenderSandbox, PowerShellModulesLogging, PowerShellScriptsLogging, AppsSmartScreen, SaveZoneInformation, WindowsSandbox, DNSoverHTTPS, LocalSecurityAuthority.
- Context menu: MSIExtractContext, CABInstallContext, EditWithClipchampContext, EditWithPhotosContext, EditWithPaintContext, PrintCMDContext, CompressedFolderNewContext, MultipleInvokeContext, UseStoreOpenWith, OpenWindowsTerminalContext, OpenWindowsTerminalAdminContext.
- Update Policies: ScanRegistryPolicies.
- WSL/UWP: Install-WSL, Uninstall-UWPApps.

Clasificación recomendada:
- UI/Context menu: SAFE.
- Gaming (GameBar/Tips) y GPUScheduling: ADVANCED.
- Defender/SmartScreen/LSA/Logging: SAFE cuando habilitan seguridad; NO RECOMENDADO al deshabilitar.
- DNSoverHTTPS: ADVANCED (depende de DNS/ISP/latencia).
- Scheduled tasks: ADVANCED (solo tareas específicas con reversión clara).

Inventario de funciones (JSON): `Reports/Tweaks2_Functions_Inventory.md`.
Estado: se requiere mapear cada “Function” a su acción real antes de asignarlo a perfiles automáticos.

### D) `tweaks2.md` (scripts adicionales posteriores al JSON)

Esta parte incluye un “pack” adicional de optimizaciones con comandos directos. Contiene duplicados y contradicciones (HAGS ON/OFF, HPET ON/OFF, MPO ON/OFF, mitigaciones ON/OFF). Clasificación general:

- Restauración y servicios masivos: ADVANCED (lista editable + rollback).
- GameDVR/GameBar: ADVANCED.
- Network throttling / TCP tweaks / netsh autotuning: EXPERIMENTAL.
- HAGS/MPO/TdrLevel: ADVANCED para HAGS y MPO (condicional), NO RECOMENDADO para TdrLevel=0.
- MSI mode y afinidad IRQ GPU: EXPERIMENTAL (depende de driver/placa).
- ResourcePolicyStore (CPU/flags/importance): EXPERIMENTAL (sin doc oficial).
- Timer resolution / HPET / useplatformtick: EXPERIMENTAL (evidencia mixta en foros).
- Power tweaks “latency”: EXPERIMENTAL (no documentado).
- Memory Management tweaks (DisablePagingExecutive, LargeSystemCache, IoPageLockLimit, PoolUsageMaximum, FeatureSettingsOverride): NO RECOMENDADO por riesgo y seguridad.
- NVIDIA NVTweak registry (CoolBits, LowLatencyMode, EnableVRR, ForceP0State, ResizableBAR, etc.): EXPERIMENTAL (no oficial; preferir panel NVIDIA / NVCleanstall).
- Prefetcher/DisableMemoryCompression: ADVANCED/EXPERIMENTAL (solo con métricas).
- SysFX audio/IPv6 off: ADVANCED (depende de hardware/uso).

Conclusión sección D:
- No aplicar en automático.
- Solo exponer en “Experimental” o “Advanced” muy controlado por hardware y con rollback.

## Recomendaciones inmediatas para integrar en la app

- MPO toggle solo si hay síntomas (flicker/stutter) y GPU NVIDIA (basado en NVIDIA KB).
- VRR + V-SYNC + FPS cap en perfil Smooth VRR (BlurBusters).
- Low Latency/Reflex/Anti-Lag como recomendaciones por juego, no “forzado global”.
- NVCleanstall como opción guiada (driver slim) en sección avanzada para NVIDIA.
- HAGS: auto solo si GPU/driver y juego lo justifican; caso contrario recomendado manual.
- Servicios innecesarios: aplicar solo lista curada por perfil y con rollback (ver `config/services.json`).
- Bloatware Windows: desinstalacion por lista controlada y reversible (ver `config/apps_bloatware.json`).

## Siguientes pasos (paso a paso)

1) Completar inventario completo del JSON de `tweaks2.md`.
2) Marcar cada “Function” con acción real y categoría SAFE/ADV/EXP.
3) Generar catálogo `tweaks.json` para la nueva UI.
4) Crear reglas hardware-aware para los dos perfiles base.

# Estado de validacion de propuestas (tweaks2)

Este documento clasifica los elementos marcados como "Propuesto" en
`Reports/Tweaks2_Functions_Mapping.md` en:
- VALIDADO: clave/accion conocida y estable
- EXPERIMENTAL: requiere parametros, tiene impacto de seguridad, o hay duda
- PENDIENTE: sin referencia clara

## VALIDADO (catalogo OK)
### Privacy & Telemetry
- FeedbackFrequency (ADV)
- SigninInfo (ADV)
- LanguageListAccess (SAFE)
- WindowsWelcomeExperience (SAFE)
- WindowsTips (SAFE)
- SettingsSuggestedContent (SAFE)
- AppsSilentInstalling (SAFE)
- WhatsNewInWindows (SAFE)
- TailoredExperiences (SAFE)

### UI & Explorer
- CheckBoxes (SAFE)
- MergeConflicts (SAFE)
- OpenFileExplorerTo (SAFE)
- OneDriveFileExplorerAd (SAFE)
- FileTransferDialog (SAFE)
- RecycleBinDeleteConfirmation (SAFE)
- QuickAccessRecentFiles (SAFE)
- QuickAccessFrequentFolders (SAFE)
- TaskbarAlignment (SAFE)
- TaskbarWidgets (SAFE)
- TaskbarSearch (SAFE)
- SearchHighlights (SAFE)
- TaskViewButton (SAFE)
- SecondsInSystemClock (SAFE)
- TaskbarEndTask (SAFE)
- ControlPanelView (SAFE)
- FirstLogonAnimation (SAFE)
- JPEGWallpapersQuality (SAFE)
- ShortcutsSuffix (SAFE)
- PrtScnSnippingTool (SAFE)
- NavigationPaneExpand (SAFE)
- RecentlyAddedStartApps (SAFE)
- MostUsedStartApps (SAFE)
- StartRecommendedSection (ADV)
- StartRecommendationsTips (SAFE)
- StartAccountNotifications (SAFE)

### OneDrive
- OneDrive (DisableFileSyncNGSC) (ADV)

### System
- StorageSense (SAFE)
- Hibernation (ADV)
- Win32LongPathsSupport (SAFE)
- BSoDStopError (SAFE)
- DeliveryOptimization (ADV)
- WindowsManageDefaultPrinter (SAFE)
- UpdateMicrosoftProducts (ADV)
- RestartDeviceAfterUpdate (ADV)
- Autoplay (SAFE)
- ThumbnailCacheRemoval (SAFE)
- SaveRestartableApps (SAFE)
- RestorePreviousFolders (SAFE)
- NetworkDiscovery (ADV)

### File Associations / Terminal
- DefaultTerminalApp (ADV)

### Update Policies
- ScanRegistryPolicies (SAFE)

## EXPERIMENTAL (manual / no default)
### UI & Explorer
- SnapAssist (clave no confirmada)
- ClockInNotificationCenter (clave no confirmada)
- StartLayout (requiere XML y politica)

### System
- AdminApprovalMode (impacta cuenta admin integrada)
- WindowsFeatures (requiere nombres exactos)
- WindowsCapabilities (requiere nombres exactos)
- RestartNotification (clave no confirmada)
- ActiveHours (requiere valores)
- WindowsLatestUpdate (clave no confirmada)
- NetworkAdaptersSavePower (requiere adaptador)
- Set-UserShellFolderLocation (requiere ruta)
- WinPrtScrFolder (requiere ruta)
- ReservedStorage (impacto sistema, puede no aplicar)
- StickyShift (flags no confirmados)

### File Associations / Terminal
- Set-Association / Export-Associations / Import-Associations (requieren ruta)

### Runtimes
- Install-VCRedist (depende de winget)
- Install-DotNetRuntimes (depende de winget)

### System Misc
- PreventEdgeShortcutCreation (clave no confirmada)
- RegistryBackup (requiere ruta)

### WSL / UWP
- Install-WSL (requiere compatibilidad + reinicio)

### Defender & Security
- NetworkProtection (cambio de seguridad)
- PUAppsDetection (cambio de seguridad)
- DefenderSandbox (cambio de seguridad)
- DismissSmartScreenFilter (cambio de seguridad)
- PowerShellModulesLogging (cambio de seguridad)
- PowerShellScriptsLogging (cambio de seguridad)
- AppsSmartScreen (cambio de seguridad)
- SaveZoneInformation (clave no confirmada)
- WindowsSandbox (cambio de seguridad)
- DNSoverHTTPS (clave no confirmada)
- LocalSecurityAuthority (cambio de seguridad)

### Context Menu
- MSIExtractContext
- CABInstallContext
- EditWithClipchampContext
- EditWithPhotosContext
- EditWithPaintContext
- PrintCMDContext
- CompressedFolderNewContext
- MultipleInvokeContext
- UseStoreOpenWith
- OpenWindowsTerminalContext
- OpenWindowsTerminalAdminContext

## PENDIENTE (sin accion definida)
- AppsLanguageSwitch
- Cursors
- FolderGroupBy
- InputMethod
- LatestInstalled.NET
- RecommendedTroubleshooting
- F1HelpPage
- CapsLock
- RKNBypass
- DismissMSAccount
- EventViewerCustomView

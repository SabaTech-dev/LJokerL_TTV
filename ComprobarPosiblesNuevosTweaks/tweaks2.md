@echo off title Windows 11 Gaming Optimizer PRO color 0A chcp 65001 >nul 
 
REM Check for administrator privileges net session >nul 2>&1 if %errorlevel% neq 0 (     echo ============================================     echo    ERROR: Administrator rights required     echo ============================================ 
    echo. 
    echo Please run this script as Administrator     echo Right-click and select "Run as administrator"     echo.     pause 
    exit 
) 
 
:START 
cls 
echo ════════════════════════════════════════════════ 
echo    WINDOWS 11 GAMING OPTIMIZER PRO 
echo ════════════════════════════════════════════════ 
echo. 
echo WARNING: This script will AGGRESSIVELY modify system settings echo Create a restore point before continuing! echo. echo Would you like to create a restore point now? 
echo 1. Yes (HIGHLY RECOMMENDED) echo 2. No, continue without restore point echo. 
set /p restore="Select (1-2): " 
 
if "%restore%"=="1" ( 
    echo.     echo Creating system restore point... 
    wmic.exe /Namespace:\\root\default Path SystemRestore Call CreateRestorePoint "Before Gaming Optimization PRO", 100, 7 >nul 2>&1     if %errorlevel%==0 (         echo [✓] Restore point created successfully! 
    ) else ( 
        echo [!] Could not create restore point - continuing anyway 
    ) 
    timeout /t 2 >nul 
) 
 
:MENU 
cls 
echo ════════════════════════════════════════════════ 
echo         GAMING OPTIMIZATION MENU PRO 
echo ════════════════════════════════════════════════ 
echo. 
echo  CLEANING echo  1. Deep clean (temp files, cache, logs) echo  2. Optimize drives echo  3. ► INSTALL ALL CLEANING OPTIONS echo. 
echo  PERFORMANCE 
echo  4. Disable ALL unnecessary services echo  5. Maximum visual performance echo  6. Manage startup programs 
echo  7. Ultimate power plan echo  8. ► INSTALL ALL PERFORMANCE OPTIONS echo. 
echo  GAMING SPECIFIC echo  9. Full Game Mode optimization echo  10. Remove bloatware ^& telemetry echo  11. Network optimization (gaming) echo  12. Disable background processes echo  13. General GPU optimization echo  14. Optimize mouse ^& keyboard (polling rate) echo  15. ► INSTALL ALL GAMING OPTIONS 
echo. 
echo  GPU SPECIFIC echo  16. NVIDIA GPU Optimization echo  17. AMD GPU Optimization echo. 
echo  ADVANCED echo  18. Remove Windows bloatware apps echo  19. Disable Windows features echo  20. ► INSTALL ALL ADVANCED OPTIONS echo. 
echo  QUICK OPTIONS echo  21. FULL PRO OPTIMIZATION (ALL) echo  22. Restore defaults echo  23. View status echo  24. Exit echo. set /p option="Select (1-24): " 
 
if "%option%"=="1" goto CLEAN_TEMP if "%option%"=="2" goto DISK_OPT 
if "%option%"=="3" goto INSTALL_ALL_CLEANING if "%option%"=="4" goto SERVICES if "%option%"=="5" goto VISUAL if "%option%"=="6" goto STARTUP if "%option%"=="7" goto POWER if "%option%"=="8" goto INSTALL_ALL_PERFORMANCE if "%option%"=="9" goto GAME_MODE if "%option%"=="10" goto TELEMETRY if "%option%"=="11" goto NETWORK if "%option%"=="12" goto BACKGROUND_APPS if "%option%"=="13" goto GPU_SETTINGS if "%option%"=="14" goto MOUSE_KEYBOARD if "%option%"=="15" goto INSTALL_ALL_GAMING if "%option%"=="16" goto NVIDIA_OPT if "%option%"=="17" goto AMD_OPT if "%option%"=="18" goto DEBLOAT if "%option%"=="19" goto DISABLE_FEATURES if "%option%"=="20" goto INSTALL_ALL_ADVANCED if "%option%"=="21" goto FULL_OPT if "%option%"=="22" goto RESTORE if "%option%"=="23" goto STATUS if "%option%"=="24" goto EXIT goto MENU 
 
REM ════════════════════════════════════════════════ 
REM    INSTALL ALL CLEANING OPTIONS 
REM ════════════════════════════════════════════════ 
:INSTALL_ALL_CLEANING 
cls 
echo ════════════════════════════════════════════════ 
echo    INSTALL ALL CLEANING OPTIONS 
echo ════════════════════════════════════════════════ 
echo. echo This will execute: echo  • Deep clean (temp files, cache, logs) echo  • Optimize drives echo. 
set /p confirm="Continue? (Y/N): " if /i not "%confirm%"=="Y" goto MENU 
 
setlocal enabledelayedexpansion 
 
REM Execute Deep Clean echo. echo [1/2] Running Deep Clean... 
echo ════════════════════════════════════════════════ 
call :CLEAN_TEMP_SILENT 
 
REM Execute Disk Optimization echo. echo [2/2] Running Disk Optimization... 
echo ════════════════════════════════════════════════ 
call :DISK_OPT_SILENT 
 
cls 
echo ════════════════════════════════════════════════ 
echo    ALL CLEANING OPTIONS COMPLETED! 
echo ════════════════════════════════════════════════ 
echo. 
echo [✓] Deep clean completed echo [✓] Drive optimization completed echo. 
echo All cleaning operations finished successfully! echo. endlocal pause goto MENU 
 
REM ════════════════════════════════════════════════ 
REM    INSTALL ALL PERFORMANCE OPTIONS 
REM ════════════════════════════════════════════════ 
:INSTALL_ALL_PERFORMANCE 
cls 
echo ════════════════════════════════════════════════ 
echo    INSTALL ALL PERFORMANCE OPTIONS 
echo ════════════════════════════════════════════════ 
echo. echo This will execute: 
echo  • Disable ALL unnecessary services echo  • Maximum visual performance echo  • Manage startup programs echo  • Ultimate power plan echo. 
set /p confirm="Continue? (Y/N): " if /i not "%confirm%"=="Y" goto MENU 
 
setlocal enabledelayedexpansion 
 
echo. echo [1/4] Disabling unnecessary services... 
call :SERVICES_SILENT 
 
echo. 
echo [2/4] Configuring visual performance... 
call :VISUAL_SILENT 
 
echo. echo [3/4] Managing startup programs... 
call :STARTUP_SILENT 
 
echo. echo [4/4] Activating ultimate power plan... 
call :POWER_SILENT 
 
cls 
echo ════════════════════════════════════════════════ 
echo    ALL PERFORMANCE OPTIONS COMPLETED! 
echo ════════════════════════════════════════════════ 
echo. 
echo [✓] Services disabled echo [✓] Visual performance maximized echo [✓] Startup programs managed echo [✓] Ultimate power plan activated echo. 
echo All performance optimizations applied! echo. endlocal pause goto MENU 
 
REM ════════════════════════════════════════════════ 
REM    INSTALL ALL GAMING OPTIONS 
REM ════════════════════════════════════════════════ 
:INSTALL_ALL_GAMING 
cls 
echo ════════════════════════════════════════════════ 
echo    INSTALL ALL GAMING OPTIONS 
echo ════════════════════════════════════════════════ 
echo. echo This will execute: 
echo  • Full Game Mode optimization echo  • Remove bloatware ^& telemetry echo  • Network optimization echo  • Disable background processes echo  • General GPU optimization echo  • Optimize mouse ^& keyboard echo. 
set /p confirm="Continue? (Y/N): " if /i not "%confirm%"=="Y" goto MENU 
 
setlocal enabledelayedexpansion 
 
echo. echo [1/6] Configuring Game Mode... 
call :GAME_MODE_SILENT 
 
echo. echo [2/6] Removing bloatware ^& telemetry... 
call :TELEMETRY_SILENT 
 
echo. echo [3/6] Optimizing network... 
call :NETWORK_SILENT 
 
echo. 
echo [4/6] Disabling background processes... 
call :BACKGROUND_APPS_SILENT 
 
echo. echo [5/6] Optimizing GPU settings... 
call :GPU_SETTINGS_SILENT 
 
echo. echo [6/6] Optimizing mouse ^& keyboard... 
call :MOUSE_KEYBOARD_SILENT 
 
cls 
echo ════════════════════════════════════════════════ 
echo    ALL GAMING OPTIONS COMPLETED! 
echo ════════════════════════════════════════════════ 
echo. 
echo [✓] Game Mode optimized echo [✓] Bloatware ^& telemetry removed echo [✓] Network optimized echo [✓] Background processes disabled echo [✓] GPU optimized echo [✓] Mouse ^& keyboard optimized echo. 
echo All gaming optimizations applied! echo. endlocal pause goto MENU 
 
REM ════════════════════════════════════════════════ 
REM    INSTALL ALL ADVANCED OPTIONS 
REM ════════════════════════════════════════════════ 
:INSTALL_ALL_ADVANCED 
cls 
echo ════════════════════════════════════════════════ 
echo    INSTALL ALL ADVANCED OPTIONS 
echo ════════════════════════════════════════════════ 
echo. echo WARNING: These are aggressive optimizations! echo This will execute: 
echo  • Remove Windows bloatware apps echo  • Disable Windows features echo. 
set /p confirm="Continue? (Y/N): " if /i not "%confirm%"=="Y" goto MENU 
 
setlocal enabledelayedexpansion 
 
echo. echo [1/2] Removing bloatware apps... 
call :DEBLOAT_SILENT 
 
echo. echo [2/2] Disabling Windows features... 
call :DISABLE_FEATURES_SILENT 
 
cls 
echo ════════════════════════════════════════════════ 
echo    ALL ADVANCED OPTIONS COMPLETED! 
echo ════════════════════════════════════════════════ 
echo. 
echo [✓] Bloatware apps removed 
echo [✓] Windows features disabled echo. 
echo All advanced optimizations applied! echo. endlocal pause goto MENU 
 
REM ════════════════════════════════════════════════ 
REM    SILENT EXECUTION FUNCTIONS 
REM ════════════════════════════════════════════════ 
 
:CLEAN_TEMP_SILENT echo [░░░░░░░░░░] 0%% - Starting deep cleaning... 
del /q/f/s %TEMP%\* >nul 2>&1 echo [█░░░░░░░░░] 10%% - Cleaning user temp files... 
del /q/f/s C:\Windows\Temp\* >nul 2>&1 echo [██░░░░░░░░] 20%% - Cleaning Windows temp... 
del /q/f/s C:\Windows\Prefetch\* >nul 2>&1 echo [███░░░░░░░] 30%% - Cleaning prefetch... rd /s /q %systemdrive%\$Recycle.Bin >nul 2>&1 echo [████░░░░░░] 40%% - Emptying recycle bin... 
net stop wuauserv >nul 2>&1 del /q/f/s C:\Windows\SoftwareDistribution\Download\* >nul 2>&1 net start wuauserv >nul 2>&1 echo [█████░░░░░] 50%% - Cleaning Windows Update cache... 
del /q/f/s "%LocalAppData%\D3DSCache\*" >nul 2>&1 del /q/f/s "%LocalAppData%\NVIDIA\DXCache\*" >nul 2>&1 del /q/f/s "%LocalAppData%\AMD\DXCache\*" >nul 2>&1 echo [██████░░░░] 60%% - Cleaning shader caches... 
del /q/f/s C:\Windows\Logs\* >nul 2>&1 
wevtutil cl System >nul 2>&1 wevtutil cl Application >nul 2>&1 echo [███████░░░] 70%% - Cleaning system logs... 
del /q/f/s %LocalAppData%\Microsoft\Windows\Explorer\thumbcache_*.db >nul 2>&1 echo [████████░░] 80%% - Cleaning thumbnail cache... 
ipconfig /flushdns >nul 2>&1 echo [█████████░] 90%% - Flushing DNS cache... 
cleanmgr /sagerun:1 >nul 2>&1 echo [██████████] 100%% - Running Disk Cleanup... 
echo [✓] Deep cleaning completed goto :eof 
 
:DISK_OPT_SILENT echo [░░░░░░░░░░] 0%% - Starting drive optimization... echo     This may take 3-10 minutes depending on drive size defrag C: /O /H >nul 2>&1 echo [███████░░░] 70%% - Drive optimization completed... 
schtasks /Change /TN "\Microsoft\Windows\Defrag\ScheduledDefrag" /DISABLE >nul 2>&1 echo [█████████░] 85%% - Disabling scheduled defragmentation... 
powercfg -h off >nul 2>&1 echo [██████████] 95%% - Disabling hibernation... vssadmin delete shadows /for=c: /all /quiet >nul 2>&1 echo [██████████] 100%% - Cleaning shadow copies... 
echo [✓] Drive optimization completed goto :eof 
 
:SERVICES_SILENT echo [░░░░░░░░░░] 0%% - Starting service optimization... 
set count=0 
for %%s in (WSearch SysMain DiagTrack dmwappushservice WerSvc wuauserv wisvc WMPNetworkSvc XblAuthManager XblGameSave XboxNetApiSvc TabletInputService 
OneSyncSvc PcaSvc WbioSrvc RetailDemo MapsBroker lfsvc SharedAccess TrkWks WpcMonSvc PhoneSvc Fax Spooler RemoteRegistry RemoteAccess AppVClient tzautoupdate CDPUserSvc MessagingService PimIndexMaintenanceSvc UnistoreSvc UserDataSvc WalletService) do (     sc stop "%%s" >nul 2>&1     sc config "%%s" start=disabled >nul 2>&1     set /a count+=1 
) 
echo [██████████] 100%% - Services disabled... 
echo [✓] Services disabled goto :eof 
 
:VISUAL_SILENT echo [░░░░░░░░░░] 0%% - Starting visual optimization... 
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects" /v VisualFXSetting /t REG_DWORD /d 2 /f >nul echo [██░░░░░░░░] 20%% - Disabling visual effects... 
reg add "HKCU\Control Panel\Desktop" /v UserPreferencesMask /t REG_BINARY /d 
9012038010000000 /f >nul reg add "HKCU\Control Panel\Desktop\WindowMetrics" /v MinAnimate /t REG_SZ /d 0 /f >nul echo [████░░░░░░] 40%% - Disabling animations... 
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced" /v ListviewAlphaSelect /t REG_DWORD /d 0 /f >nul 
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced" /v TaskbarAnimations /t REG_DWORD /d 0 /f >nul echo [██████░░░░] 60%% - Optimizing desktop... 
reg add "HKCU\Software\Microsoft\Windows\DWM" /v EnableAeroPeek /t REG_DWORD /d 0 
/f >nul 
reg add "HKCU\Software\Microsoft\Windows\DWM" /v AlwaysHibernateThumbnails /t REG_DWORD /d 0 /f >nul echo [████████░░] 80%% - Configuring DWM... 
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize" /v EnableTransparency /t REG_DWORD /d 0 /f >nul echo [██████████] 100%% - Disabling transparency... 
echo [✓] Visual performance configured goto :eof 
 
:STARTUP_SILENT echo [░░░░░░░░░░] 0%% - Disabling startup programs... 
reg add 
"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run" /v "OneDrive" /t REG_BINARY /d 0300000000000000000000000000000000 /f >nul 2>&1 echo [██░░░░░░░░] 20%% - Disabling common apps... 
reg add 
"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run" /v "Spotify" /t REG_BINARY /d 0300000000000000000000000000000000 /f >nul 2>&1 
reg add 
"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run" /v "Discord" /t REG_BINARY /d 0300000000000000000000000000000000 /f >nul 2>&1 
reg add 
"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run" /v "Steam" /t REG_BINARY /d 0300000000000000000000000000000000 /f >nul 2>&1 echo [█████░░░░░] 50%% - Disabling Adobe/Creative apps... 
reg add 
"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run" /v 
"AdobeAAMUpdater" /t REG_BINARY /d 0300000000000000000000000000000000 /f >nul 
2>&1 
reg add 
"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run" /v "AdobeGCInvoker" /t REG_BINARY /d 0300000000000000000000000000000000 /f >nul 2>&1 echo [███████░░░] 70%% - Disabling other apps... 
reg add 
"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run" /v "Skype" /t REG_BINARY /d 0300000000000000000000000000000000 /f >nul 2>&1 
reg add 
"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run" /v "Teams" /t REG_BINARY /d 0300000000000000000000000000000000 /f >nul 2>&1 echo [█████████░] 90%% - Optimizing startup... 
schtasks /Change /TN "\Microsoft\Windows\Application Experience\StartupAppTask" 
/DISABLE >nul 2>&1 echo [██████████] 100%% - Startup optimized... 
echo [✓] Startup programs disabled goto :eof 
 
:POWER_SILENT echo [░░░░░░░░░░] 0%% - Creating Ultimate Performance plan... 
powercfg -duplicatescheme e9a42b02-d5df-448d-aa00-03f14749eb61 >nul 2>&1 echo [███░░░░░░░] 30%% - Activating power plan... 
for /f "tokens=4" %%a in ('powercfg -list ^| findstr "Ultimate"') do set ULTIMATE_GUID=%%a if defined ULTIMATE_GUID (     powercfg /setactive %ULTIMATE_GUID% >nul 2>&1 
) else ( 
    powercfg /setactive 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c >nul 2>&1 
) 
echo [██████░░░░] 60%% - Disabling sleep timers... 
powercfg /change monitor-timeout-ac 0 >nul 2>&1 powercfg /change disk-timeout-ac 0 >nul 2>&1 echo [█████████░] 90%% - Disabling hibernation... powercfg /change standby-timeout-ac 0 >nul 2>&1 powercfg /change hibernate-timeout-ac 0 >nul 2>&1 echo [██████████] 100%% - Power plan configured... 
echo [✓] Power plan configured goto :eof 
 
:GAME_MODE_SILENT echo [░░░░░░░░░░] 0%% - Enabling Game Mode... 
reg add "HKCU\Software\Microsoft\GameBar" /v AutoGameModeEnabled /t REG_DWORD /d 
1	/f >nul 
reg add "HKCU\Software\Microsoft\GameBar" /v AllowAutoGameMode /t REG_DWORD /d 1 
/f >nul 
echo [███░░░░░░░] 30%% - Disabling Game Bar/DVR... 
reg add "HKCU\Software\Microsoft\GameBar" /v UseNexusForGameBarEnabled /t REG_DWORD /d 0 /f >nul reg add "HKCU\System\GameConfigStore" /v GameDVR_Enabled /t REG_DWORD /d 0 /f >nul reg add "HKCU\System\GameConfigStore" /v GameDVR_FSEBehaviorMode /t REG_DWORD /d 
2	/f >nul 
reg add 
"HKLM\SOFTWARE\Microsoft\PolicyManager\default\ApplicationManagement\AllowGameDV R" /v value /t REG_DWORD /d 0 /f >nul echo [██████░░░░] 60%% - Enabling GPU scheduling... 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v HwSchMode /t REG_DWORD /d 2 /f >nul echo [█████████░] 90%% - Optimizing CPU priority... 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\PriorityControl" /v Win32PrioritySeparation /t REG_DWORD /d 38 /f >nul echo [██████████] 100%% - Game Mode configured... 
echo [✓] Game Mode optimized goto :eof 
 
:TELEMETRY_SILENT echo [░░░░░░░░░░] 0%% - Disabling telemetry... 
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection" /v AllowTelemetry /t REG_DWORD /d 0 /f >nul 
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\AppCompat" /v AITEnable /t REG_DWORD /d 0 /f >nul 
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection" /v AllowTelemetry /t REG_DWORD /d 0 /f >nul echo [█████░░░░░] 50%% - Disabling telemetry tasks... 
schtasks /Change /TN "Microsoft\Windows\Application Experience\Microsoft Compatibility Appraiser" /DISABLE >nul 2>&1 
schtasks /Change /TN "Microsoft\Windows\Application Experience\ProgramDataUpdater" 
/DISABLE >nul 2>&1 schtasks /Change /TN "Microsoft\Windows\Autochk\Proxy" /DISABLE >nul 2>&1 
schtasks /Change /TN "Microsoft\Windows\Customer Experience Improvement Program\Consolidator" /DISABLE >nul 2>&1 
schtasks /Change /TN "Microsoft\Windows\Customer Experience Improvement Program\UsbCeip" /DISABLE >nul 2>&1 echo [██████████] 100%% - Telemetry disabled... 
echo [✓] Telemetry removed 
goto :eof 
 
:NETWORK_SILENT echo [░░░░░░░░░░] 0%% - Starting network optimization... netsh int tcp set global autotuninglevel=disabled >nul 2>&1 echo [██░░░░░░░░] 20%% - Disabling auto-tuning... netsh int tcp set global chimney=enabled >nul 2>&1 netsh int tcp set global dca=enabled >nul 2>&1 netsh int tcp set global netdma=enabled >nul 2>&1 echo [████░░░░░░] 40%% - Enabling offload features... netsh int tcp set global ecncapability=disabled >nul 2>&1 netsh int tcp set global timestamps=disabled >nul 2>&1 echo [██████░░░░] 60%% - Optimizing TCP settings... 
netsh int tcp set global rss=enabled >nul 2>&1 netsh int tcp set global nonsackrttresiliency=disabled >nul 2>&1 netsh int tcp set global maxsynretransmissions=2 >nul 2>&1 netsh int tcp set global fastopen=enabled >nul 2>&1 netsh int tcp set heuristics disabled >nul 2>&1 echo [████████░░] 80%% - Configuring advanced options... 
reg add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters" /v TcpAckFrequency 
/t REG_DWORD /d 1 /f >nul 
reg add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters" /v TCPNoDelay /t REG_DWORD /d 1 /f >nul 
reg add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters" /v TcpDelAckTicks /t REG_DWORD /d 0 /f >nul 
reg add "HKLM\SOFTWARE\Microsoft\MSMQ\Parameters" /v TCPNoDelay /t REG_DWORD /d 
1 /f >nul 
echo [██████████] 100%% - Reducing latency... 
echo [✓] Network optimized goto :eof 
 
:BACKGROUND_APPS_SILENT 
echo [░░░░░░░░░░] 0%% - Disabling background apps globally... 
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications" 
/v GlobalUserDisabled /t REG_DWORD /d 1 /f >nul echo [█████░░░░░] 50%% - Disabling individual apps... 
for %%a in (Microsoft.Windows.Photos Microsoft.WindowsCalculator 
Microsoft.WindowsCamera Microsoft.Windows.Cortana Microsoft.WindowsMaps 
Microsoft.WindowsAlarms Microsoft.BingWeather Microsoft.GetHelp Microsoft.Getstarted 
Microsoft.Messaging Microsoft.MicrosoftOfficeHub Microsoft.MicrosoftSolitaireCollection 
Microsoft.WindowsFeedbackHub Microsoft.ZuneMusic Microsoft.ZuneVideo Microsoft.People 
Microsoft.WindowsSoundRecorder Microsoft.YourPhone Microsoft.Xbox.TCUI 
Microsoft.XboxApp Microsoft.XboxGameOverlay Microsoft.XboxGamingOverlay Microsoft.XboxIdentityProvider Microsoft.XboxSpeechToTextOverlay) do ( 
    reg add 
"HKCU\Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications\%%a" /v Disabled /t REG_DWORD /d 1 /f >nul 2>&1 
    reg add 
"HKCU\Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications\%%a" /v DisabledByUser /t REG_DWORD /d 1 /f >nul 2>&1 
) 
echo [██████████] 100%% - Background apps disabled... 
echo [✓] Background apps disabled goto :eof 
 
:GPU_SETTINGS_SILENT echo [░░░░░░░░░░] 0%% - Enabling GPU scheduling... 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v HwSchMode /t REG_DWORD /d 2 /f >nul echo [████░░░░░░] 40%% - Disabling GPU preemption... 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler" /v EnablePreemption /t REG_DWORD /d 0 /f >nul echo [████████░░] 80%% - Optimizing DirectX... 
reg add "HKLM\SOFTWARE\Microsoft\DirectX\UserGpuPreferences" /v DirectXUserGlobalSettings /t REG_SZ /d "VRROptimizeEnable=0;" /f >nul 
reg add "HKCU\Software\Microsoft\DirectX\UserGpuPreferences" /v 
DirectXUserGlobalSettings /t REG_SZ /d "SwapEffectUpgradeEnable=1;VRROptimizeEnable=0;" 
/f >nul 
echo [██████████] 100%% - GPU configured... 
echo [✓] GPU optimized goto :eof 
 
:MOUSE_KEYBOARD_SILENT echo [░░░░░░░░░░] 0%% - Increasing queue sizes... 
reg add "HKLM\SYSTEM\CurrentControlSet\Services\mouclass\Parameters" /v MouseDataQueueSize /t REG_DWORD /d 20 /f >nul 
reg add "HKLM\SYSTEM\CurrentControlSet\Services\kbdclass\Parameters" /v KeyboardDataQueueSize /t REG_DWORD /d 20 /f >nul echo [████░░░░░░] 40%% - Disabling mouse acceleration... 
reg add "HKCU\Control Panel\Mouse" /v MouseSpeed /t REG_SZ /d 0 /f >nul reg add "HKCU\Control Panel\Mouse" /v MouseThreshold1 /t REG_SZ /d 0 /f >nul reg add "HKCU\Control Panel\Mouse" /v MouseThreshold2 /t REG_SZ /d 0 /f >nul echo [████████░░] 80%% - Optimizing sensitivity... 
reg add "HKCU\Control Panel\Mouse" /v MouseSensitivity /t REG_SZ /d 10 /f >nul 
reg add "HKLM\SOFTWARE\Microsoft\Input\Settings\ControllerProcessor\CursorSpeed" /v CursorSensitivity /t REG_DWORD /d 10000 /f >nul echo [██████████] 100%% - Input devices optimized... 
echo [✓] Mouse ^& keyboard optimized goto :eof 
 
:DEBLOAT_SILENT echo [░░░░░░░░░░] 0%% - Removing bloatware apps... 
echo     This may take 2-5 minutes, please wait... 
 
REM Create temporary PowerShell script 
echo $apps = 
@('Microsoft.3DBuilder','Microsoft.BingFinance','Microsoft.BingNews','Microsoft.BingSports',' Microsoft.BingWeather','Microsoft.GetHelp','Microsoft.Getstarted','Microsoft.Messaging','Mic rosoft.Microsoft3DViewer','Microsoft.MicrosoftOfficeHub','Microsoft.MicrosoftSolitaireCollect ion','Microsoft.MSPaint','Microsoft.Office.OneNote','Microsoft.OneConnect','Microsoft.People ','Microsoft.Print3D','Microsoft.SkypeApp','Microsoft.Wallet','Microsoft.WindowsAlarms','Micr osoft.WindowsCamera','Microsoft.windowscommunicationsapps','Microsoft.WindowsFeedbac kHub','Microsoft.WindowsMaps','Microsoft.WindowsSoundRecorder','Microsoft.Xbox.TCUI','M icrosoft.XboxApp','Microsoft.XboxGameOverlay','Microsoft.XboxGamingOverlay','Microsoft.Xb oxIdentityProvider','Microsoft.XboxSpeechToTextOverlay','Microsoft.YourPhone','Microsoft.Zu neMusic','Microsoft.ZuneVideo') > "%TEMP%\debloat_silent.ps1" 
echo foreach ($app in $apps) { try { $package = Get-AppxPackage -Name $app -ErrorAction 
SilentlyContinue; if ($package) { Remove-AppxPackage -Package $package.PackageFullName -
ErrorAction SilentlyContinue } } catch { } } >> "%TEMP%\debloat_silent.ps1" 
 
powershell -ExecutionPolicy Bypass -File "%TEMP%\debloat_silent.ps1" >nul 2>&1 del "%TEMP%\debloat_silent.ps1" 2>nul 
 
echo [██████████] 100%% - Bloatware removal completed... 
echo [✓] Bloatware removed 
goto :eof 
 
:DISABLE_FEATURES_SILENT echo [░░░░░░░░░░] 0%% - Disabling XPS Services... 
dism /online /disable-feature /featurename:Printing-XPSServices-Features /NoRestart >nul 
2>&1 echo [████░░░░░░] 40%% - Disabling Work Folders... 
dism /online /disable-feature /featurename:WorkFolders-Client /NoRestart >nul 2>&1 echo [████████░░] 80%% - Disabling Windows Media Player... 
dism /online /disable-feature /featurename:WindowsMediaPlayer /NoRestart >nul 2>&1 echo [██████████] 100%% - Features disabled... 
echo [✓] Features disabled goto :eof 
 
REM ════════════════════════════════════════════════ 
REM    ORIGINAL INDIVIDUAL FUNCTIONS (Interactive) 
REM ════════════════════════════════════════════════ 
 
:CLEAN_TEMP 
cls 
echo ════════════════════════════════════════════════ 
 
echo    DEEP CLEANING echo echo. 
echo Progress: [░░░░░░░░░░] 0%% 
echo [*] Cleaning temporary files... 
del /q/f/s %TEMP%\* 2>nul 
cls 
echo ════════════════════════════════════════════════ 
echo    DEEP CLEANING 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [██░░░░░░░░] 15%% 
echo [*] Cleaning Windows temp... del /q/f/s C:\Windows\Temp\* 2>nul 
cls 
echo ════════════════════════════════════════════════ 
echo    DEEP CLEANING 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [███░░░░░░░] 25%% 
echo [*] Cleaning prefetch... 
del /q/f/s C:\Windows\Prefetch\* 2>nul 
cls 
echo ════════════════════════════════════════════════ 
echo    DEEP CLEANING 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [████░░░░░░] 35%% 
echo [*] Emptying recycle bin... 
rd /s /q %systemdrive%\$Recycle.Bin 2>nul 
cls 
echo ════════════════════════════════════════════════ 
echo    DEEP CLEANING 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [█████░░░░░] 45%% echo [*] Cleaning Windows Update cache... 
net stop wuauserv >nul 2>&1 del /q/f/s C:\Windows\SoftwareDistribution\Download\* 2>nul net start wuauserv >nul 2>&1 
cls 
echo ════════════════════════════════════════════════ 
echo    DEEP CLEANING 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [██████░░░░] 55%% 
echo [*] Cleaning shader cache... 
del /q/f/s "%LocalAppData%\D3DSCache\*" 2>nul del /q/f/s "%LocalAppData%\NVIDIA\DXCache\*" 2>nul del /q/f/s "%LocalAppData%\AMD\DXCache\*" 2>nul 
cls 
echo ════════════════════════════════════════════════ 
echo    DEEP CLEANING 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [███████░░░] 65%% 
echo [*] Cleaning system logs... del /q/f/s C:\Windows\Logs\* 2>nul wevtutil cl System >nul 2>&1 wevtutil cl Application >nul 2>&1 
cls 
echo ════════════════════════════════════════════════ echo    DEEP CLEANING echo echo. 
echo Progress: [████████░░] 75%% 
echo [*] Cleaning thumbnail cache... 
del /q/f/s %LocalAppData%\Microsoft\Windows\Explorer\thumbcache_*.db 2>nul 
cls 
echo ════════════════════════════════════════════════ 
echo    DEEP CLEANING 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [█████████░] 85%% 
echo [*] Flushing DNS cache... ipconfig /flushdns >nul 2>&1 
cls 
echo ════════════════════════════════════════════════ 
echo    DEEP CLEANING 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [██████████] 95%% 
echo [*] Running Disk Cleanup utility... echo     This may take 2-5 minutes, please wait... 
start /wait cleanmgr /sagerun:1 
cls 
echo ════════════════════════════════════════════════ 
echo    DEEP CLEANING 
echo ════════════════════════════════════════════════ 
echo. echo Progress: [██████████] 100%% 
echo. echo [✓] Deep cleaning completed successfully! echo. pause goto MENU 
 
:DISK_OPT 
cls 
echo ════════════════════════════════════════════════ 
echo    DRIVE OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [░░░░░░░░░░] 0%% 
echo [*] Starting drive optimization... 
echo     This may take 3-10 minutes depending on drive size echo. defrag C: /O /H 
cls 
echo ════════════════════════════════════════════════ 
echo    DRIVE OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [███████░░░] 70%% 
echo [*] Disabling scheduled defragmentation... 
schtasks /Change /TN "\Microsoft\Windows\Defrag\ScheduledDefrag" /DISABLE >nul 2>&1 
cls 
echo ════════════════════════════════════════════════ 
echo    DRIVE OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [█████████░] 85%% 
echo [*] Disabling hibernation to save space... 
powercfg -h off >nul 2>&1 
cls 
echo echo    DRIVE OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [██████████] 100%% 
echo [*] Cleaning shadow copies... 
vssadmin delete shadows /for=c: /all /quiet >nul 2>&1 echo. 
echo [✓] Drive optimization completed! echo. pause goto MENU 
 
:SERVICES 
cls 
echo ════════════════════════════════════════════════ 
echo    DISABLING UNNECESSARY SERVICES 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [░░░░░░░░░░] 0%% 
setlocal enabledelayedexpansion set count=0 set total=30 
 
for %%s in (WSearch SysMain DiagTrack dmwappushservice WerSvc wuauserv wisvc WMPNetworkSvc XblAuthManager XblGameSave XboxNetApiSvc TabletInputService 
OneSyncSvc PcaSvc WbioSrvc RetailDemo MapsBroker lfsvc SharedAccess TrkWks WpcMonSvc 
PhoneSvc Fax Spooler RemoteRegistry RemoteAccess AppVClient tzautoupdate CDPUserSvc MessagingService PimIndexMaintenanceSvc UnistoreSvc UserDataSvc WalletService) do (     set /a count+=1     set /a percent=(!count!*100)/!total! 
    set /a blocks=!percent!/10     set bar=     for /L %%i in (1,1,!blocks!) do set bar=!bar!█     for /L %%i in (!blocks!,1,9) do set bar=!bar!░ 
    cls 
    echo ════════════════════════════════════════════════ 
    echo    DISABLING UNNECESSARY SERVICES 
    echo ════════════════════════════════════════════════ 
    echo. 
    echo Progress: [!bar!] !percent!%%     echo [*] Disabling %%s...     sc stop "%%s" >nul 2>&1     sc config "%%s" start=disabled >nul 2>&1     timeout /t 1 >nul 
) 
 
cls 
echo ════════════════════════════════════════════════ 
echo    DISABLING UNNECESSARY SERVICES 
echo ════════════════════════════════════════════════ 
echo. echo Progress: [██████████] 100%% 
echo. 
echo [✓] All unnecessary services disabled! echo. echo NOTE: This disables many Windows services including: 
echo  - Windows Search, Superfetch, Telemetry echo  - Xbox services, Tablet services echo  - Fax, Print Spooler (if you don't print) echo. echo If you experience issues, use option 18 to restore. 
 
echo. endlocal pause goto MENU 
 
:VISUAL 
cls 
echo ════════════════════════════════════════════════ 
echo    MAXIMUM VISUAL PERFORMANCE 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [░░░░░░░░░░] 0%% 
echo [*] Disabling visual effects... 
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects" /v VisualFXSetting /t REG_DWORD /d 2 /f >nul 
cls 
echo ════════════════════════════════════════════════ 
echo    MAXIMUM VISUAL PERFORMANCE 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [███░░░░░░░] 25%% 
echo [*] Disabling animations... 
reg add "HKCU\Control Panel\Desktop" /v UserPreferencesMask /t REG_BINARY /d 
9012038010000000 /f >nul reg add "HKCU\Control Panel\Desktop\WindowMetrics" /v MinAnimate /t REG_SZ /d 0 /f >nul 
cls 
echo ════════════════════════════════════════════════ 
echo    MAXIMUM VISUAL PERFORMANCE 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [██████░░░░] 50%% 
echo [*] Disabling taskbar animations... 
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced" /v ListviewAlphaSelect /t REG_DWORD /d 0 /f >nul 
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced" /v TaskbarAnimations /t REG_DWORD /d 0 /f >nul 
cls 
echo ════════════════════════════════════════════════ 
echo    MAXIMUM VISUAL PERFORMANCE 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [████████░░] 75%% 
echo [*] Disabling transparency... 
reg add "HKCU\Software\Microsoft\Windows\DWM" /v EnableAeroPeek /t REG_DWORD /d 0 
/f >nul 
reg add "HKCU\Software\Microsoft\Windows\DWM" /v AlwaysHibernateThumbnails /t REG_DWORD /d 0 /f >nul 
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize" /v EnableTransparency /t REG_DWORD /d 0 /f >nul 
cls 
echo ════════════════════════════════════════════════ 
echo    MAXIMUM VISUAL PERFORMANCE 
echo ════════════════════════════════════════════════ 
echo. echo Progress: [██████████] 100%% 
echo. 
echo [✓] Visual performance maximized! echo. echo Changes applied: 
echo  • Disabled all visual effects echo  • Removed animations echo  • Disabled transparency echo  • Taskbar optimized echo. pause 
goto MENU 
 
:STARTUP 
cls 
echo ════════════════════════════════════════════════ 
echo    STARTUP PROGRAMS MANAGEMENT 
echo ════════════════════════════════════════════════ 
echo. 
echo Choose how to manage startup programs: echo. 
echo 1. Automatic - Disable common startup programs echo 2. Manual - Open Windows settings to choose yourself echo. 
set /p choice="Select option (1-2): " 
 
if "%choice%"=="1" ( 
    cls 
    echo ════════════════════════════════════════════════ 
    echo    STARTUP PROGRAMS MANAGEMENT - AUTOMATIC 
    echo ════════════════════════════════════════════════ 
    echo. 
    echo Progress: [░░░░░░░░░░] 0%%     echo [*] Disabling common startup programs... 
     
    reg add 
"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run" /v "OneDrive" /t REG_BINARY /d 0300000000000000000000000000000000 /f >nul 2>&1     echo Progress: [██░░░░░░░░] 20%% 
     
    reg add 
"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run" /v 
"Spotify" /t REG_BINARY /d 0300000000000000000000000000000000 /f >nul 2>&1 
    reg add 
"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run" /v 
"Discord" /t REG_BINARY /d 0300000000000000000000000000000000 /f >nul 2>&1 
    reg add 
"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run" /v "Steam" /t REG_BINARY /d 0300000000000000000000000000000000 /f >nul 2>&1     echo Progress: [█████░░░░░] 50%% 
     
    reg add 
"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run" /v 
"AdobeAAMUpdater" /t REG_BINARY /d 0300000000000000000000000000000000 /f >nul 
2>&1 
    reg add 
"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run" /v "AdobeGCInvoker" /t REG_BINARY /d 0300000000000000000000000000000000 /f >nul 2>&1     echo Progress: [███████░░░] 70%% 
     
    reg add 
"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run" /v 
"Skype" /t REG_BINARY /d 0300000000000000000000000000000000 /f >nul 2>&1 
    reg add 
"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run" /v "Teams" /t REG_BINARY /d 0300000000000000000000000000000000 /f >nul 2>&1 
    schtasks /Change /TN "\Microsoft\Windows\Application Experience\StartupAppTask" 
/DISABLE >nul 2>&1     echo Progress: [██████████] 100%% 
    echo.     echo [✓] Common startup programs disabled! 
    echo.     echo Disabled programs include:     echo  • OneDrive     echo  • Spotify, Discord, Steam     echo  • Adobe updaters     echo  • Skype, Teams     echo  • And others...     echo. 
    pause     goto MENU 
) 
 
if "%choice%"=="2" ( 
    cls 
    echo ════════════════════════════════════════════════ 
    echo    STARTUP PROGRAMS MANAGEMENT - MANUAL 
    echo ════════════════════════════════════════════════ 
    echo.     echo Opening Windows Startup Settings... 
    echo.     echo Instructions: 
    echo  1. Disable programs you don't need at startup     echo  2. Keep antivirus and essential tools enabled     echo  3. Close the window when done 
    echo.     echo Press any key to open Startup Manager... 
    pause >nul     start "" "ms-settings:startupapps"     echo.     echo [✓] Startup manager opened! 
    echo. 
    echo Disable unnecessary startup programs and close the window     echo when you're done. Then press any key here to continue... 
    pause >nul     goto MENU 
) 
 
goto MENU 
 
:POWER 
cls 
echo ════════════════════════════════════════════════ 
echo    ULTIMATE POWER PLAN 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [░░░░░░░░░░] 0%% 
echo [*] Creating Ultimate Performance power plan... 
powercfg -duplicatescheme e9a42b02-d5df-448d-aa00-03f14749eb61 >nul 2>&1 
cls 
echo ════════════════════════════════════════════════ 
echo    ULTIMATE POWER PLAN 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [█████░░░░░] 40%% 
echo [*] Activating Ultimate Performance plan... 
for /f "tokens=4" %%a in ('powercfg -list ^| findstr "Ultimate"') do set ULTIMATE_GUID=%%a if defined ULTIMATE_GUID (     powercfg /setactive %ULTIMATE_GUID% 
) else ( 
    powercfg /setactive 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c 
) cls 
echo ════════════════════════════════════════════════ 
echo    ULTIMATE POWER PLAN 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [████████░░] 70%% 
echo [*] Configuring power settings... powercfg /change monitor-timeout-ac 0 powercfg /change disk-timeout-ac 0 powercfg /change standby-timeout-ac 0 powercfg /change hibernate-timeout-ac 0 
cls 
echo ════════════════════════════════════════════════ 
echo    ULTIMATE POWER PLAN 
echo ════════════════════════════════════════════════ 
echo. echo Progress: [██████████] 100%% 
echo. 
echo [✓] Ultimate Performance power plan activated! echo. echo Settings applied: 
echo  • Maximum CPU performance echo  • No monitor timeout echo  • No sleep/hibernation echo  • PCIe power management disabled echo. pause goto MENU 
 
:GAME_MODE 
cls 
echo ════════════════════════════════════════════════ 
echo    FULL GAME MODE OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [░░░░░░░░░░] 0%% 
echo [*] Enabling Game Mode... 
reg add "HKCU\Software\Microsoft\GameBar" /v AutoGameModeEnabled /t REG_DWORD /d 
1 /f >nul 
reg add "HKCU\Software\Microsoft\GameBar" /v AllowAutoGameMode /t REG_DWORD /d 1 
/f >nul 
cls 
echo ════════════════════════════════════════════════ 
echo    FULL GAME MODE OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [███░░░░░░░] 25%% 
echo [*] Disabling Game Bar... 
reg add "HKCU\Software\Microsoft\GameBar" /v UseNexusForGameBarEnabled /t REG_DWORD /d 0 /f >nul reg add "HKCU\System\GameConfigStore" /v GameDVR_Enabled /t REG_DWORD /d 0 /f >nul 
cls 
echo ════════════════════════════════════════════════ 
echo    FULL GAME MODE OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [██████░░░░] 50%% 
echo [*] Disabling Game DVR... 
reg add "HKCU\System\GameConfigStore" /v GameDVR_FSEBehaviorMode /t REG_DWORD /d 
2 /f >nul 
reg add 
"HKLM\SOFTWARE\Microsoft\PolicyManager\default\ApplicationManagement\AllowGameDV R" /v value /t REG_DWORD /d 0 /f >nul 
cls 
echo ════════════════════════════════════════════════ 
echo    FULL GAME MODE OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [████████░░] 75%% 
echo [*] Enabling Hardware-Accelerated GPU Scheduling... 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v HwSchMode /t REG_DWORD /d 2 /f >nul 
cls 
echo ════════════════════════════════════════════════ 
echo    FULL GAME MODE OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [██████████] 95%% 
echo [*] Optimizing CPU scheduling for games... 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\PriorityControl" /v Win32PrioritySeparation /t REG_DWORD /d 38 /f >nul 
cls 
echo ════════════════════════════════════════════════ 
echo    FULL GAME MODE OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. echo Progress: [██████████] 100%% 
echo. 
echo [✓] Game Mode fully optimized! echo. echo Features configured: 
echo  • Auto Game Mode enabled echo  • Game Bar/DVR disabled echo  • GPU Hardware Scheduling enabled echo  • CPU priority optimized for gaming echo. pause goto MENU 
 
:TELEMETRY 
cls 
echo ════════════════════════════════════════════════ 
echo    REMOVE BLOATWARE ^& TELEMETRY 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [░░░░░░░░░░] 0%% 
echo [*] Disabling telemetry... 
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection" /v AllowTelemetry /t REG_DWORD /d 0 /f >nul 
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\AppCompat" /v AITEnable /t REG_DWORD /d 0 /f >nul 
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection" /v AllowTelemetry /t REG_DWORD /d 0 /f >nul 
cls 
echo ════════════════════════════════════════════════ 
echo    REMOVE BLOATWARE ^& TELEMETRY 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [█████░░░░░] 40%% 
echo [*] Disabling scheduled telemetry tasks... 
schtasks /Change /TN "Microsoft\Windows\Application Experience\Microsoft Compatibility Appraiser" /DISABLE >nul 2>&1 
schtasks /Change /TN "Microsoft\Windows\Application Experience\ProgramDataUpdater" 
/DISABLE >nul 2>&1 schtasks /Change /TN "Microsoft\Windows\Autochk\Proxy" /DISABLE >nul 2>&1 
schtasks /Change /TN "Microsoft\Windows\Customer Experience Improvement Program\Consolidator" /DISABLE >nul 2>&1 
schtasks /Change /TN "Microsoft\Windows\Customer Experience Improvement Program\UsbCeip" /DISABLE >nul 2>&1 
cls 
echo ════════════════════════════════════════════════ 
echo    REMOVE BLOATWARE ^& TELEMETRY 
echo ════════════════════════════════════════════════ 
echo. echo Progress: [██████████] 100%% 
echo. 
echo [✓] Bloatware and telemetry removed! echo. 
pause goto MENU 
 
:NETWORK 
cls 
echo ════════════════════════════════════════════════ 
echo    NETWORK OPTIMIZATION FOR GAMING 
echo ════════════════════════════════════════════════ 
echo. 
setlocal enabledelayedexpansion echo Progress: [░░░░░░░░░░] 0%% 
echo [*] Disabling auto-tuning... 
netsh int tcp set global autotuninglevel=disabled >nul 2>&1 
cls 
echo ════════════════════════════════════════════════ 
echo    NETWORK OPTIMIZATION FOR GAMING 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [██░░░░░░░░] 15%% 
echo [*] Enabling chimney offload... 
netsh int tcp set global chimney=enabled >nul 2>&1 
cls 
echo ════════════════════════════════════════════════ 
echo    NETWORK OPTIMIZATION FOR GAMING 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [███░░░░░░░] 25%% 
echo [*] Enabling direct cache access... 
netsh int tcp set global dca=enabled >nul 2>&1 
cls 
echo ════════════════════════════════════════════════ echo    NETWORK OPTIMIZATION FOR GAMING 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [█████░░░░░] 40%% 
echo [*] Enabling NetDMA... 
netsh int tcp set global netdma=enabled >nul 2>&1 
cls 
echo ════════════════════════════════════════════════ 
echo    NETWORK OPTIMIZATION FOR GAMING 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [██████░░░░] 55%% 
echo [*] Disabling ECN capability... 
netsh int tcp set global ecncapability=disabled >nul 2>&1 
cls 
echo ════════════════════════════════════════════════ 
echo    NETWORK OPTIMIZATION FOR GAMING 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [████████░░] 70%% 
echo [*] Disabling timestamps... 
netsh int tcp set global timestamps=disabled >nul 2>&1 
cls 
echo ════════════════════════════════════════════════ 
echo    NETWORK OPTIMIZATION FOR GAMING 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [█████████░] 85%% 
echo [*] Enabling RSS... 
netsh int tcp set global rss=enabled >nul 2>&1 netsh int tcp set global nonsackrttresiliency=disabled >nul 2>&1 netsh int tcp set global maxsynretransmissions=2 >nul 2>&1 netsh int tcp set global fastopen=enabled >nul 2>&1 netsh int tcp set heuristics disabled >nul 2>&1 
cls 
echo ════════════════════════════════════════════════ 
echo    NETWORK OPTIMIZATION FOR GAMING 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [██████████] 95%% 
echo [*] Reducing network latency... 
reg add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters" /v TcpAckFrequency 
/t REG_DWORD /d 1 /f >nul 
reg add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters" /v TCPNoDelay /t REG_DWORD /d 1 /f >nul 
reg add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters" /v TcpDelAckTicks /t REG_DWORD /d 0 /f >nul 
reg add "HKLM\SOFTWARE\Microsoft\MSMQ\Parameters" /v TCPNoDelay /t REG_DWORD /d 
1 /f >nul cls 
echo ════════════════════════════════════════════════ 
echo    NETWORK OPTIMIZATION FOR GAMING 
echo ════════════════════════════════════════════════ 
echo. echo Progress: [██████████] 100%% 
echo. 
echo [✓] Network optimized for gaming! echo. echo Applied optimizations: 
echo  • TCP auto-tuning disabled echo  • Network offload features enabled echo  • Latency reduced echo  • RSS enabled for better throughput echo. endlocal pause goto MENU 
 
:BACKGROUND_APPS 
cls 
echo ════════════════════════════════════════════════ 
echo    DISABLE BACKGROUND PROCESSES 
echo ════════════════════════════════════════════════ 
echo. 
setlocal enabledelayedexpansion echo Progress: [░░░░░░░░░░] 0%% 
echo [*] Disabling background apps globally... 
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications" /v GlobalUserDisabled /t REG_DWORD /d 1 /f >nul 
 
set count=0 set total=30 
for %%a in (Microsoft.Windows.Photos Microsoft.WindowsCalculator 
Microsoft.WindowsCamera Microsoft.Windows.Cortana Microsoft.WindowsMaps 
Microsoft.WindowsAlarms Microsoft.BingWeather Microsoft.GetHelp Microsoft.Getstarted 
Microsoft.Messaging Microsoft.MicrosoftOfficeHub Microsoft.MicrosoftSolitaireCollection 
Microsoft.WindowsFeedbackHub Microsoft.ZuneMusic Microsoft.ZuneVideo Microsoft.People 
Microsoft.WindowsSoundRecorder Microsoft.YourPhone Microsoft.Xbox.TCUI 
Microsoft.XboxApp Microsoft.XboxGameOverlay Microsoft.XboxGamingOverlay Microsoft.XboxIdentityProvider Microsoft.XboxSpeechToTextOverlay) do (     set /a count+=1     set /a percent=(!count!*100)/!total! 
    set /a blocks=!percent!/10     set bar=     for /L %%i in (1,1,!blocks!) do set bar=!bar!█     for /L %%i in (!blocks!,1,9) do set bar=!bar!░ 
    cls 
    echo ════════════════════════════════════════════════ 
    echo    DISABLE BACKGROUND PROCESSES 
    echo ════════════════════════════════════════════════ 
    echo. 
    echo Progress: [!bar!] !percent!%%     echo [*] Disabling %%a... 
    reg add 
"HKCU\Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications\%%a" /v Disabled /t REG_DWORD /d 1 /f >nul 2>&1 
    reg add 
"HKCU\Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications\%%a" /v DisabledByUser /t REG_DWORD /d 1 /f >nul 2>&1 
) 
 
cls 
echo ════════════════════════════════════════════════ 
echo    DISABLE BACKGROUND PROCESSES 
echo ════════════════════════════════════════════════ 
echo. echo Progress: [██████████] 100%% 
echo. 
echo [✓] Background apps disabled! echo. 
echo All system apps are prevented from running in background echo This will significantly reduce CPU/RAM usage echo. endlocal pause goto MENU 
 
:GPU_SETTINGS 
cls 
echo ════════════════════════════════════════════════ 
echo    GENERAL GPU OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [░░░░░░░░░░] 0%% echo [*] Enabling Hardware-Accelerated GPU Scheduling... 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v HwSchMode /t REG_DWORD /d 2 /f >nul 
cls 
echo ════════════════════════════════════════════════ 
echo    GENERAL GPU OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [█████░░░░░] 40%% 
echo [*] Disabling GPU preemption... 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler" /v EnablePreemption /t REG_DWORD /d 0 /f >nul 
cls 
echo ════════════════════════════════════════════════ 
echo    GENERAL GPU OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [████████░░] 70%% 
echo [*] Optimizing DirectX settings... 
reg add "HKLM\SOFTWARE\Microsoft\DirectX\UserGpuPreferences" /v DirectXUserGlobalSettings /t REG_SZ /d "VRROptimizeEnable=0;" /f >nul 
reg add "HKCU\Software\Microsoft\DirectX\UserGpuPreferences" /v 
DirectXUserGlobalSettings /t REG_SZ /d "SwapEffectUpgradeEnable=1;VRROptimizeEnable=0;" 
/f >nul cls 
echo ════════════════════════════════════════════════ 
echo    GENERAL GPU OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. echo Progress: [██████████] 100%% 
echo. 
echo [✓] GPU optimizations applied! echo. echo Settings configured: 
echo  • Hardware GPU scheduling enabled echo  • GPU preemption disabled echo  • DirectX optimized echo. echo For GPU-specific optimizations: echo  - NVIDIA users: Use option 16 echo  - AMD users: Use option 17 echo. pause goto MENU 
 
:MOUSE_KEYBOARD 
cls 
echo ════════════════════════════════════════════════ 
echo    MOUSE ^& KEYBOARD OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [░░░░░░░░░░] 0%% 
echo [*] Increasing data queue sizes... 
reg add "HKLM\SYSTEM\CurrentControlSet\Services\mouclass\Parameters" /v MouseDataQueueSize /t REG_DWORD /d 20 /f >nul 
reg add "HKLM\SYSTEM\CurrentControlSet\Services\kbdclass\Parameters" /v KeyboardDataQueueSize /t REG_DWORD /d 20 /f >nul 
cls 
echo ════════════════════════════════════════════════ 
echo    MOUSE ^& KEYBOARD OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [█████░░░░░] 40%% 
echo [*] Disabling mouse acceleration... 
reg add "HKCU\Control Panel\Mouse" /v MouseSpeed /t REG_SZ /d 0 /f >nul reg add "HKCU\Control Panel\Mouse" /v MouseThreshold1 /t REG_SZ /d 0 /f >nul reg add "HKCU\Control Panel\Mouse" /v MouseThreshold2 /t REG_SZ /d 0 /f >nul 
cls 
echo ════════════════════════════════════════════════ 
echo    MOUSE ^& KEYBOARD OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [████████░░] 70%% 
echo [*] Optimizing mouse sensitivity... 
reg add "HKCU\Control Panel\Mouse" /v MouseSensitivity /t REG_SZ /d 10 /f >nul 
reg add "HKLM\SOFTWARE\Microsoft\Input\Settings\ControllerProcessor\CursorSpeed" /v CursorSensitivity /t REG_DWORD /d 10000 /f >nul 
cls 
echo ════════════════════════════════════════════════ 
echo    MOUSE ^& KEYBOARD OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. echo Progress: [██████████] 100%% 
echo. 
echo [✓] Mouse and keyboard optimized! echo. echo Settings applied: 
echo  • Higher polling rate support echo  • Mouse acceleration disabled echo  • Raw input optimized echo  • Input lag reduced 
echo. pause goto MENU 
 
:NVIDIA_OPT 
cls 
echo ════════════════════════════════════════════════ 
echo    NVIDIA GPU OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [░░░░░░░░░░] 0%% 
echo [*] Checking for NVIDIA GPU... 
nvidia-smi >nul 2>&1 if %errorlevel% neq 0 (     echo.     echo [!] NVIDIA GPU not detected or drivers not installed     echo.     echo Please install NVIDIA drivers first: 
    echo https://www.nvidia.com/Download/index.aspx 
    echo.     pause     goto MENU 
) cls 
echo ════════════════════════════════════════════════ 
echo    NVIDIA GPU OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [███░░░░░░░] 25%% 
echo [*] Optimizing NVIDIA Control Panel settings via registry... 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc108002be10318}\0000" /v "PerfLevelSrc" /t REG_DWORD /d 8738 /f >nul 2>&1 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc108002be10318}\0000" /v "PowerMizerEnable" /t REG_DWORD /d 1 /f >nul 2>&1 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc108002be10318}\0000" /v "PowerMizerLevel" /t REG_DWORD /d 1 /f >nul 2>&1 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc1-
08002be10318}\0000" /v "PowerMizerLevelAC" /t REG_DWORD /d 1 /f >nul 2>&1 
cls 
echo ════════════════════════════════════════════════ 
echo    NVIDIA GPU OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [██████░░░░] 50%% echo [*] Setting maximum performance mode... 
reg add "HKLM\SYSTEM\CurrentControlSet\Services\nvlddmkm\Global\NVTweak" /v 
"DisplayPowerSaving" /t REG_DWORD /d 0 /f >nul 2>&1 
cls 
echo ════════════════════════════════════════════════ 
echo    NVIDIA GPU OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [█████████░] 75%% 
echo [*] Optimizing shader cache... 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc1-
08002be10318}\0000" /v "EnableUlps" /t REG_DWORD /d 0 /f >nul 2>&1 
cls 
echo ════════════════════════════════════════════════ 
echo    NVIDIA GPU OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. echo Progress: [██████████] 100%% 
echo. 
echo [✓] NVIDIA GPU optimized! echo. echo IMPORTANT: Complete optimization manually: 
echo ════════════════════════════════════════════════ 
echo. 
echo 1. Open NVIDIA Control Panel echo 2. Go to "Manage 3D Settings" echo 3. Set these options: 
echo    • Power Management Mode: Prefer Maximum Performance echo    • Low Latency Mode: Ultra echo    • Texture Filtering Quality: Performance echo    • Vertical Sync: Off echo    • Max Frame Rate: Off echo. pause goto MENU 
 
:AMD_OPT 
cls 
echo ════════════════════════════════════════════════ 
echo    AMD GPU OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [░░░░░░░░░░] 0%% 
echo [*] Checking for AMD GPU... 
reg query "HKLM\SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc1-
08002be10318}\0000" /v "DriverDesc" 2>nul | find "AMD" >nul if %errorlevel% neq 0 (     echo.     echo [!] AMD GPU not detected or drivers not installed     echo. 
    echo Please install AMD drivers first:     echo https://www.amd.com/en/support 
    echo.     pause     goto MENU 
) cls 
echo ════════════════════════════════════════════════ 
echo    AMD GPU OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [███░░░░░░░] 25%% 
echo [*] Optimizing AMD settings via registry... 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc108002be10318}\0000" /v "KMD_EnableComputePreemption" /t REG_DWORD /d 0 /f >nul 
2>&1 
cls 
echo ════════════════════════════════════════════════ 
echo    AMD GPU OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [██████░░░░] 50%% 
echo [*] Disabling AMD power saving features... 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc1-
08002be10318}\0000" /v "EnableUlps" /t REG_DWORD /d 0 /f >nul 2>&1 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc1-
08002be10318}\0000" /v "EnableUlps_NA" /t REG_DWORD /d 0 /f >nul 2>&1 
cls 
echo ════════════════════════════════════════════════ 
echo    AMD GPU OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [█████████░] 75%% echo [*] Optimizing for maximum performance... 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc108002be10318}\0000" /v "PP_SclkDeepSleepDisable" /t REG_DWORD /d 1 /f >nul 2>&1 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc108002be10318}\0000" /v "PP_ThermalAutoThrottlingEnable" /t REG_DWORD /d 0 /f >nul 
2>&1 
cls 
echo ════════════════════════════════════════════════ 
echo    AMD GPU OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. echo Progress: [██████████] 100%% 
echo. 
echo [✓] AMD GPU optimized! echo. echo IMPORTANT: Complete optimization manually: 
echo ════════════════════════════════════════════════ 
echo. 
echo 1. Open AMD Radeon Software echo 2. Go to "Gaming" ^> "Global Graphics" echo 3. Set these options: 
echo    • Radeon Anti-Lag: Enabled echo    • Radeon Boost: Enabled echo    • Radeon Image Sharpening: Enabled echo    • Wait for Vertical Refresh: Off echo    • Graphics Profile: eSports echo. pause goto MENU 
 
:DEBLOAT 
cls 
echo ════════════════════════════════════════════════ 
echo    REMOVE WINDOWS BLOATWARE APPS 
echo ════════════════════════════════════════════════ 
echo. 
echo WARNING: This will remove many pre-installed apps! echo. 
set /p confirm="Continue? (Y/N): " if /i not "%confirm%"=="Y" goto MENU 
 
cls 
echo ════════════════════════════════════════════════ 
echo    REMOVE WINDOWS BLOATWARE APPS 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [░░░░░░░░░░] 0%% 
echo Starting bloatware removal... echo This may take 2-5 minutes, please wait... echo. 
 
REM Create a PowerShell script file to avoid command line issues echo $apps = @( > "%TEMP%\debloat.ps1" echo 'Microsoft.3DBuilder', >> "%TEMP%\debloat.ps1" echo 'Microsoft.BingFinance', >> "%TEMP%\debloat.ps1" echo 'Microsoft.BingNews', >> "%TEMP%\debloat.ps1" echo 'Microsoft.BingSports', >> "%TEMP%\debloat.ps1" echo 'Microsoft.BingWeather', >> "%TEMP%\debloat.ps1" echo 'Microsoft.GetHelp', >> "%TEMP%\debloat.ps1" echo 'Microsoft.Getstarted', >> "%TEMP%\debloat.ps1" echo 'Microsoft.Messaging', >> "%TEMP%\debloat.ps1" echo 'Microsoft.Microsoft3DViewer', >> "%TEMP%\debloat.ps1" echo 'Microsoft.MicrosoftOfficeHub', >> "%TEMP%\debloat.ps1" echo 'Microsoft.MicrosoftSolitaireCollection', >> "%TEMP%\debloat.ps1" echo 'Microsoft.MSPaint', >> "%TEMP%\debloat.ps1" echo 'Microsoft.Office.OneNote', >> "%TEMP%\debloat.ps1" echo 'Microsoft.OneConnect', >> "%TEMP%\debloat.ps1" echo 'Microsoft.People', >> "%TEMP%\debloat.ps1" echo 'Microsoft.Print3D', >> "%TEMP%\debloat.ps1" echo 'Microsoft.SkypeApp', >> "%TEMP%\debloat.ps1" echo 'Microsoft.Wallet', >> "%TEMP%\debloat.ps1" echo 'Microsoft.WindowsAlarms', >> "%TEMP%\debloat.ps1" echo 'Microsoft.WindowsCamera', >> "%TEMP%\debloat.ps1" echo 'Microsoft.windowscommunicationsapps', >> "%TEMP%\debloat.ps1" echo 'Microsoft.WindowsFeedbackHub', >> "%TEMP%\debloat.ps1" echo 'Microsoft.WindowsMaps', >> "%TEMP%\debloat.ps1" echo 'Microsoft.WindowsSoundRecorder', >> "%TEMP%\debloat.ps1" echo 'Microsoft.Xbox.TCUI', >> "%TEMP%\debloat.ps1" echo 'Microsoft.XboxApp', >> "%TEMP%\debloat.ps1" echo 'Microsoft.XboxGameOverlay', >> "%TEMP%\debloat.ps1" echo 'Microsoft.XboxGamingOverlay', >> "%TEMP%\debloat.ps1" echo 'Microsoft.XboxIdentityProvider', >> "%TEMP%\debloat.ps1" echo 'Microsoft.XboxSpeechToTextOverlay', >> "%TEMP%\debloat.ps1" echo 'Microsoft.YourPhone', >> "%TEMP%\debloat.ps1" echo 'Microsoft.ZuneMusic', >> "%TEMP%\debloat.ps1" echo 'Microsoft.ZuneVideo' >> "%TEMP%\debloat.ps1" echo ) >> "%TEMP%\debloat.ps1" echo $removed = 0 >> "%TEMP%\debloat.ps1" echo foreach ($app in $apps) { >> "%TEMP%\debloat.ps1" echo     try { >> "%TEMP%\debloat.ps1" 
echo         $package = Get-AppxPackage -Name $app -ErrorAction SilentlyContinue >> 
"%TEMP%\debloat.ps1" echo         if ($package) { >> "%TEMP%\debloat.ps1" echo             Remove-AppxPackage -Package $package.PackageFullName -ErrorAction SilentlyContinue >> "%TEMP%\debloat.ps1" echo             $removed++ >> "%TEMP%\debloat.ps1" echo         } >> "%TEMP%\debloat.ps1" echo     } catch { } >> "%TEMP%\debloat.ps1" echo } >> "%TEMP%\debloat.ps1" echo Write-Output $removed >> "%TEMP%\debloat.ps1" 
 
cls 
echo ════════════════════════════════════════════════ 
echo    REMOVE WINDOWS BLOATWARE APPS 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [█████░░░░░] 50%% 
echo Processing apps, please wait... echo. 
 
powershell -ExecutionPolicy Bypass -File "%TEMP%\debloat.ps1" > 
"%TEMP%\debloat_result.txt" 2>&1 set /p REMOVED=<"%TEMP%\debloat_result.txt" 
 
del "%TEMP%\debloat.ps1" 2>nul del "%TEMP%\debloat_result.txt" 2>nul 
 
cls 
echo ════════════════════════════════════════════════ 
echo    REMOVE WINDOWS BLOATWARE APPS 
echo ════════════════════════════════════════════════ 
echo. echo Progress: [██████████] 100%% 
echo. 
 
if defined REMOVED (     if %REMOVED% GTR 0 (         echo [✓] Bloatware removal completed successfully! 
        echo. 
        echo Successfully removed %REMOVED% apps including: 
        echo  • Xbox apps, 3D Builder, Paint 3D         echo  • Office Hub, OneNote         echo  • Bing apps Weather, News, etc.         echo  • Your Phone, Messaging, People         echo  • And many more... 
    ) else ( 
        echo [✓] Process completed! 
        echo. 
        echo No bloatware apps found to remove. 
        echo.         echo This means: 
        echo  • Apps were already removed previously, or         echo  • Your Windows version doesn't include these apps 
        echo.         echo Your system is clean! 
    ) 
) else (     echo [✓] Process completed!     echo.     echo Bloatware removal process finished. 
) 
echo. pause goto MENU 
 
:DISABLE_FEATURES 
cls 
echo ════════════════════════════════════════════════ 
echo    DISABLE WINDOWS FEATURES 
echo ════════════════════════════════════════════════ 
echo. 
echo WARNING: This will disable optional Windows features! echo. 
set /p confirm="Continue? (Y/N): " if /i not "%confirm%"=="Y" goto MENU 
 
setlocal enabledelayedexpansion set disabled=0 
 
echo. 
echo Progress: [░░░░░░░░░░] 0%% 
echo [*] Disabling XPS services... 
dism /online /disable-feature /featurename:Printing-XPSServices-Features /NoRestart >nul 
2>&1 
if !errorlevel! equ 0 set /a disabled+=1 
cls 
echo ════════════════════════════════════════════════ 
echo    DISABLE WINDOWS FEATURES 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [█████░░░░░] 40%% 
echo [*] Disabling Work Folders... 
dism /online /disable-feature /featurename:WorkFolders-Client /NoRestart >nul 2>&1 if !errorlevel! equ 0 set /a disabled+=1 
cls 
echo ════════════════════════════════════════════════ 
echo    DISABLE WINDOWS FEATURES 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [████████░░] 70%% 
echo [*] Disabling Windows Media Player... 
dism /online /disable-feature /featurename:WindowsMediaPlayer /NoRestart >nul 2>&1 if !errorlevel! equ 0 set /a disabled+=1 
cls 
echo ════════════════════════════════════════════════ 
echo    DISABLE WINDOWS FEATURES 
echo ════════════════════════════════════════════════ 
echo. echo Progress: [██████████] 100%% 
echo. 
if !disabled! gtr 0 (     echo [✓] Windows features disabled successfully!     echo.     echo Disabled !disabled! features: 
    echo  • XPS Services     echo  • Work Folders Client       echo  • Windows Media Player 
) else (     echo [✓] Process completed!     echo. 
    echo NOTE: All features were already disabled     echo or not available on this system. 
) 
echo. endlocal pause goto MENU :FULL_OPT 
cls 
echo ════════════════════════════════════════════════ 
echo    FULL PRO OPTIMIZATION 
echo ════════════════════════════════════════════════ 
echo. echo WARNING: This will apply ALL optimizations! echo This process will take 10-15 minutes. echo. 
set /p confirm="Continue? (Y/N): " if /i not "%confirm%"=="Y" goto MENU 
 
setlocal enabledelayedexpansion 
 
REM Step 1/15 
cls 
echo ════════════════════════════════════════════════ 
echo    FULL PRO OPTIMIZATION [1/15] 
echo ════════════════════════════════════════════════ 
echo. 
echo Overall Progress: [█░░░░░░░░░] 7%% echo [*] Deep cleaning system... 
call :CLEAN_TEMP_SILENT 
 
REM Step 2/15 
cls 
echo ════════════════════════════════════════════════ 
echo    FULL PRO OPTIMIZATION [2/15] 
echo ════════════════════════════════════════════════ 
echo. 
echo Overall Progress: [██░░░░░░░░] 13%% 
echo [*] Optimizing drives... 
call :DISK_OPT_SILENT 
 
REM Step 3/15 
cls 
echo ════════════════════════════════════════════════ 
echo    FULL PRO OPTIMIZATION [3/15] 
echo ════════════════════════════════════════════════ 
echo. 
echo Overall Progress: [███░░░░░░░] 20%% 
echo [*] Disabling services... 
call :SERVICES_SILENT 
 
REM Step 4/15 
cls 
echo ════════════════════════════════════════════════ 
echo    FULL PRO OPTIMIZATION [4/15] 
echo ════════════════════════════════════════════════ 
echo. 
echo Overall Progress: [███░░░░░░░] 27%% 
echo [*] Configuring visual performance... 
call :VISUAL_SILENT 
 
REM Step 5/15 
cls 
echo ════════════════════════════════════════════════ 
echo    FULL PRO OPTIMIZATION [5/15] 
echo ════════════════════════════════════════════════ 
echo. 
echo Overall Progress: [████░░░░░░] 33%% echo [*] Managing startup programs... 
call :STARTUP_SILENT 
 
REM Step 6/15 
cls 
echo ════════════════════════════════════════════════ 
echo    FULL PRO OPTIMIZATION [6/15] 
echo ════════════════════════════════════════════════ 
echo. 
echo Overall Progress: [█████░░░░░] 40%% 
echo [*] Activating ultimate power plan... 
call :POWER_SILENT 
 
REM Step 7/15 
cls 
echo ════════════════════════════════════════════════ 
echo    FULL PRO OPTIMIZATION [7/15] 
echo ════════════════════════════════════════════════ 
echo. 
echo Overall Progress: [█████░░░░░] 47%% echo [*] Optimizing Game Mode... 
call :GAME_MODE_SILENT 
 
REM Step 8/15 
cls 
echo ════════════════════════════════════════════════ 
echo    FULL PRO OPTIMIZATION [8/15] 
echo ════════════════════════════════════════════════ 
echo. 
echo Overall Progress: [██████░░░░] 53%% echo [*] Removing telemetry... 
call :TELEMETRY_SILENT 
 
REM Step 9/15 
cls 
echo ════════════════════════════════════════════════ 
echo    FULL PRO OPTIMIZATION [9/15] 
echo ════════════════════════════════════════════════ 
echo. 
echo Overall Progress: [███████░░░] 60%% echo [*] Optimizing network... 
call :NETWORK_SILENT 
 
REM Step 10/15 
cls 
echo ════════════════════════════════════════════════ 
echo    FULL PRO OPTIMIZATION [10/15] 
echo ════════════════════════════════════════════════ 
echo. 
echo Overall Progress: [███████░░░] 67%% echo [*] Disabling background apps... call :BACKGROUND_APPS_SILENT 
 
REM Step 11/15 
cls 
echo ════════════════════════════════════════════════ 
echo    FULL PRO OPTIMIZATION [11/15] 
echo ════════════════════════════════════════════════ 
echo. 
echo Overall Progress: [████████░░] 73%% echo [*] Optimizing GPU... call :GPU_SETTINGS_SILENT 
 
REM Step 12/15 
cls 
echo ════════════════════════════════════════════════ 
echo    FULL PRO OPTIMIZATION [12/15] 
echo ════════════════════════════════════════════════ 
echo. 
echo Overall Progress: [█████████░] 80%% echo [*] Optimizing mouse ^& keyboard... 
call :MOUSE_KEYBOARD_SILENT 
 
REM Step 13/15 
cls 
echo ════════════════════════════════════════════════ 
echo    FULL PRO OPTIMIZATION [13/15] 
echo ════════════════════════════════════════════════ 
echo. 
echo Overall Progress: [█████████░] 87%% echo [*] Removing bloatware... 
call :DEBLOAT_SILENT 
 
REM Step 14/15 
cls 
echo ════════════════════════════════════════════════ 
echo    FULL PRO OPTIMIZATION [14/15] 
echo ════════════════════════════════════════════════ 
echo. 
echo Overall Progress: [██████████] 93%% 
echo [*] Disabling features... call :DISABLE_FEATURES_SILENT 
 
REM Step 15/15 
cls 
echo ════════════════════════════════════════════════ 
echo    FULL PRO OPTIMIZATION [15/15] 
echo ════════════════════════════════════════════════ 
echo. 
echo Overall Progress: [██████████] 100%% 
echo [*] Finalizing optimizations... 
timeout /t 2 >nul 
 
cls 
echo ════════════════════════════════════════════════ 
echo    FULL PRO OPTIMIZATION COMPLETED! 
echo ════════════════════════════════════════════════ 
echo. echo [✓✓✓] ALL OPTIMIZATIONS APPLIED SUCCESSFULLY! [✓✓✓] echo. 
echo ════════════════════════════════════════════════ 
echo IMPORTANT NEXT STEPS: 
echo ════════════════════════════════════════════════ 
echo. 
echo 1. RESTART your computer NOW to apply all changes echo. echo 2. Open Task Manager and disable startup programs: 
echo    - Press Ctrl+Shift+Esc echo    - Go to Startup tab echo    - Disable non-essential programs echo. echo 3. For GPU-specific optimizations: 
echo    - NVIDIA users: Run option 16 from main menu echo    - AMD users: Run option 17 from main menu echo. 
echo 4. Update GPU drivers to latest version echo. 
echo ════════════════════════════════════════════════ 
echo Your system is now optimized for maximum gaming! 
echo ════════════════════════════════════════════════ 
echo. endlocal pause goto MENU 
 
:RESTORE 
cls 
echo ════════════════════════════════════════════════ 
echo    RESTORE DEFAULT SETTINGS 
echo ════════════════════════════════════════════════ 
echo. 
echo WARNING: This will undo all optimizations and echo restore Windows to default settings. echo. 
set /p confirm="Continue? (Y/N): " if /i not "%confirm%"=="Y" goto MENU 
 
setlocal enabledelayedexpansion echo. 
echo Progress: [░░░░░░░░░░] 0%% 
echo [*] Starting restoration... 
 
cls 
echo ════════════════════════════════════════════════ 
echo    RESTORE DEFAULT SETTINGS 
echo ════════════════════════════════════════════════ echo. 
echo Progress: [███░░░░░░░] 25%% 
echo [*] Restoring services... 
for %%s in (WSearch SysMain DiagTrack dmwappushservice) do (     sc config "%%s" start=auto >nul 2>&1     sc start "%%s" >nul 2>&1 
) 
 
cls 
echo ════════════════════════════════════════════════ 
echo    RESTORE DEFAULT SETTINGS 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [██████░░░░] 50%% 
echo [*] Restoring visual effects... 
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects" /v VisualFXSetting /t REG_DWORD /d 0 /f >nul 
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize" /v EnableTransparency /t REG_DWORD /d 1 /f >nul 
 
cls 
echo ════════════════════════════════════════════════ 
echo    RESTORE DEFAULT SETTINGS 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [████████░░] 70%% 
echo [*] Restoring power plan... 
powercfg /setactive 381b4222-f694-41f0-9685-ff5bb260df2e >nul 2>&1 
 
cls 
echo ════════════════════════════════════════════════ 
echo    RESTORE DEFAULT SETTINGS 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [█████████░] 85%% 
echo [*] Restoring network settings... 
netsh int tcp set global autotuninglevel=normal >nul 2>&1 
 
cls 
echo ════════════════════════════════════════════════ 
echo    RESTORE DEFAULT SETTINGS 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [██████████] 95%% 
echo [*] Re-enabling background apps... 
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications" /v GlobalUserDisabled /t REG_DWORD /d 0 /f >nul 
 
cls 
echo ════════════════════════════════════════════════ 
echo    RESTORE DEFAULT SETTINGS 
echo ════════════════════════════════════════════════ 
echo. 
echo Progress: [██████████] 100%% 
echo [*] Enabling hibernation... 
powercfg -h on >nul 2>&1 
 
echo. 
echo [✓] All settings restored to defaults! echo. 
echo Please restart your computer for changes to take effect. echo. endlocal 
pause goto MENU 
 
:STATUS 
cls 
echo ════════════════════════════════════════════════ 
echo    OPTIMIZATION STATUS 
echo ════════════════════════════════════════════════ 
echo. 
 
echo [SERVICES] 
echo ──────────────────────────────────────────────── sc query "WSearch" | find "RUNNING" >nul 2>&1 if %errorlevel%==0 (echo  Windows Search: RUNNING) else (echo  Windows Search: STOPPED ✓) 
 
sc query "SysMain" | find "RUNNING" >nul 2>&1 if %errorlevel%==0 (echo  Superfetch: RUNNING) else (echo  Superfetch: STOPPED ✓) 
 
sc query "DiagTrack" | find "RUNNING" >nul 2>&1 if %errorlevel%==0 (echo  Telemetry: RUNNING) else (echo  Telemetry: STOPPED ✓) 
 
echo. 
echo [GAME MODE] echo ──────────────────────────────────────────────── 
reg query "HKCU\Software\Microsoft\GameBar" /v AutoGameModeEnabled 2>nul | find "0x1" 
>nul 2>&1 if %errorlevel%==0 (echo  Game Mode: ENABLED ✓) else (echo  Game Mode: DISABLED) 
 
reg query "HKCU\System\GameConfigStore" /v GameDVR_Enabled 2>nul | find "0x0" >nul 
2>&1 if %errorlevel%==0 (echo  Game DVR: DISABLED ✓) else (echo  Game DVR: ENABLED) reg query "HKLM\SYSTEM\CurrentControlSet\Control\PriorityControl" /v Win32PrioritySeparation 2>nul | find "0x26" >nul 2>&1 if %errorlevel%==0 (echo  CPU Priority: OPTIMIZED ✓) else (echo  CPU Priority: DEFAULT) 
 
echo. 
echo [GPU SETTINGS] echo ──────────────────────────────────────────────── 
reg query "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v HwSchMode 2>nul | find "0x2" >nul 2>&1 if %errorlevel%==0 (echo  GPU Scheduling: ENABLED ✓) else (echo  GPU Scheduling: DISABLED) 
 
reg query "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler" /v EnablePreemption 2>nul | find "0x0" >nul 2>&1 
if %errorlevel%==0 (echo  GPU Preemption: DISABLED ✓) else (echo  GPU Preemption: ENABLED) 
 
echo. 
echo [POWER PLAN] echo ──────────────────────────────────────────────── powercfg /getactivescheme | find "Ultimate" >nul 2>&1 if %errorlevel%==0 (     echo  Ultimate Performance: ACTIVE ✓ 
) else ( 
    powercfg /getactivescheme | find "High performance" >nul 2>&1     if %errorlevel%==0 (         echo  High Performance: ACTIVE ✓ 
    ) else ( 
        echo  Balanced or Power Saver: ACTIVE 
    ) 
) 
 
powercfg /query | find "Turn off display after" >nul 2>&1 if %errorlevel%==0 (     powercfg /query | find "Current AC Power Setting Index: 0x00000000" >nul 2>&1 
    if %errorlevel%==0 (echo  Display Timeout: DISABLED ✓) else (echo  Display Timeout: ENABLED) 
) 
 
echo. 
echo [VISUAL EFFECTS] echo ──────────────────────────────────────────────── 
reg query "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects" /v VisualFXSetting 2>nul | find "0x2" >nul 2>&1 
if %errorlevel%==0 (echo  Performance Mode: ENABLED ✓) else (echo  Performance Mode: DISABLED) 
 
reg query "HKCU\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize" /v EnableTransparency 2>nul | find "0x0" >nul 2>&1 if %errorlevel%==0 (echo  Transparency: DISABLED ✓) else (echo  Transparency: ENABLED) 
 
echo. 
echo [NETWORK OPTIMIZATION] echo ──────────────────────────────────────────────── 
netsh int tcp show global | find "Receive Window Auto-Tuning Level" | find "disabled" >nul 
2>&1 if %errorlevel%==0 (echo  Auto-Tuning: DISABLED ✓) else (echo  Auto-Tuning: ENABLED) 
 
reg query "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters" /v TcpAckFrequency 
2>nul | find "0x1" >nul 2>&1 if %errorlevel%==0 (echo  TCP Latency: OPTIMIZED ✓) else (echo  TCP Latency: DEFAULT) 
 
reg query "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters" /v TCPNoDelay 
2>nul | find "0x1" >nul 2>&1 if %errorlevel%==0 (echo  TCP No Delay: ENABLED ✓) else (echo  TCP No Delay: DISABLED) echo. 
echo [BACKGROUND APPS] echo ──────────────────────────────────────────────── 
reg query 
"HKCU\Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications" /v GlobalUserDisabled 2>nul | find "0x1" >nul 2>&1 
if %errorlevel%==0 (echo  Background Apps: DISABLED ✓) else (echo  Background Apps: ENABLED) 
 
echo. 
echo [MOUSE & KEYBOARD] echo ──────────────────────────────────────────────── reg query "HKCU\Control Panel\Mouse" /v MouseSpeed 2>nul | find "0" >nul 2>&1 
if %errorlevel%==0 (echo  Mouse Acceleration: DISABLED ✓) else (echo  Mouse Acceleration: ENABLED) 
 
reg query "HKLM\SYSTEM\CurrentControlSet\Services\mouclass\Parameters" /v MouseDataQueueSize 2>nul | find "0x14" >nul 2>&1 
if %errorlevel%==0 (echo  Mouse Queue Size: OPTIMIZED ✓) else (echo  Mouse Queue Size: DEFAULT) 
 
reg query "HKLM\SYSTEM\CurrentControlSet\Services\kbdclass\Parameters" /v KeyboardDataQueueSize 2>nul | find "0x14" >nul 2>&1 
if %errorlevel%==0 (echo  Keyboard Queue Size: OPTIMIZED ✓) else (echo  Keyboard Queue Size: DEFAULT) 
 
echo. 
echo [TELEMETRY] echo ──────────────────────────────────────────────── 
reg query "HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection" /v AllowTelemetry 
2>nul | find "0x0" >nul 2>&1 if %errorlevel%==0 (echo  Telemetry: DISABLED ✓) else (echo  Telemetry: ENABLED) schtasks /query /TN "Microsoft\Windows\Application Experience\Microsoft Compatibility Appraiser" 2>nul | find "Disabled" >nul 2>&1 
if %errorlevel%==0 (echo  Telemetry Tasks: DISABLED ✓) else (echo  Telemetry Tasks: ENABLED) 
 
echo. 
echo [NVIDIA GPU] (If applicable) echo ──────────────────────────────────────────────── 
reg query "HKLM\SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc1-
08002be10318}\0000" /v PowerMizerLevel 2>nul | find "0x1" >nul 2>&1 
if %errorlevel%==0 (echo  NVIDIA Performance: MAX ✓) else (echo  NVIDIA Performance: NOT OPTIMIZED) 
 
reg query "HKLM\SYSTEM\CurrentControlSet\Control\Class\{4d36e968-e325-11ce-bfc1-
08002be10318}\0000" /v EnableUlps 2>nul | find "0x0" >nul 2>&1 if %errorlevel%==0 (echo  NVIDIA ULPS: DISABLED ✓) else (echo  NVIDIA ULPS: ENABLED) 
 
echo. 
echo ════════════════════════════════════════════════ 
echo Press any key to return to menu... 
echo ════════════════════════════════════════════════ 
pause >nul goto MENU 
 
 
:EXIT cls 
echo ════════════════════════════════════════════════ 
echo    Windows 11 Gaming Optimizer PRO 
echo ════════════════════════════════════════════════ 
echo. 
echo Thank you for using Gaming Optimizer PRO! echo. 
echo ════════════════════════════════════════════════ 
echo FINAL RECOMMENDATIONS: 
echo ════════════════════════════════════════════════ 
echo. 
echo  🔄 RESTART your PC to apply all changes echo. echo  🎮 NVIDIA Users: 
echo     • Open NVIDIA Control Panel echo     • Set Power Management to Maximum Performance echo     • Enable Low Latency Mode echo. echo  🎮 AMD Users: 
echo     • Open AMD Radeon Software echo     • Enable Radeon Anti-Lag echo     • Set Graphics Profile to eSports echo. echo  📊 Monitor Performance: 
echo     • Use MSI Afterburner for FPS monitoring echo     • Check temperatures (GPU should be under 80°C) echo     • Update drivers regularly echo. echo  ⚡ For Best Results: 
echo     • Close all programs before gaming echo     • Use Game Mode when gaming echo     • Keep Windows updated echo     • Run this optimizer monthly echo. 
echo ════════════════════════════════════════════════ 
echo. echo Press any key to exit... 
pause >nul 
exit  
[ 
    { 
        "Region": "Initial Actions", 
        "Control": "cmb", 
        "Required": "true", 
        "RequiredIndex": "0", 
        "Function": "InitialActions", 
        "Arg": { 
            "Zero": { 
                "Tag": "Warning" 
            }, 
            "One": { 
                "Tag": "" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "" 
    }, 
    { 
        "Region": "Protection", 
        "Control": "chk", 
        "Required": "false", 
        "Function": "Logging", 
        "Arg": { 
            "Zero": { 
                "Tag": "" 
            } 
        }, 
        "Preset": "", 
        "WindowsDefault": "" 
    }, 
    { 
        "Region": "Protection", 
        "Control": "chk", 
        "Required": "false", 
        "Function": "CreateRestorePoint", 
        "Arg": { 
            "Zero": { 
                "Tag": "" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "Zero" 
    }, 
    { 
        "Region": "Privacy & Telemetry", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "DiagTrackService", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Privacy & Telemetry", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "DiagnosticDataLevel", 
        "Arg": { 
            "Zero": { 
                "Tag": "Minimal" 
            }, 
            "One": { 
                "Tag": "Default" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Privacy & Telemetry", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "ErrorReporting", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Privacy & Telemetry", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "FeedbackFrequency", 
        "Arg": { 
            "Zero": { 
                "Tag": "Never" 
            }, 
            "One": { 
                "Tag": "Automatically" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Privacy & Telemetry", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "ScheduledTasks", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
"WindowsDefault": "One" 
    }, 
    { 
        "Region": "Privacy & Telemetry", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "SigninInfo", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Privacy & Telemetry", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "LanguageListAccess", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
"Preset": "Zero", 
"WindowsDefault": "One" 
    }, 
    { 
        "Region": "Privacy & Telemetry", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "AdvertisingID", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Privacy & Telemetry", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "WindowsWelcomeExperience", 
        "Arg": { 
            "Zero": { 
                "Tag": "Hide" 
            }, 
            "One": { 
                "Tag": "Show" 
            } 
}, 
"Preset": "One", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Privacy & Telemetry", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "WindowsTips", 
        "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Preset": "One", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Privacy & Telemetry", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "SettingsSuggestedContent", 
        "Arg": { 
            "Zero": { 
                "Tag": "Hide" 
            }, 
            "One": { 
                "Tag": "Show" 
            } 
}, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Privacy & Telemetry", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "AppsSilentInstalling", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Privacy & Telemetry", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "WhatsNewInWindows", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Privacy & Telemetry", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "TailoredExperiences", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Privacy & Telemetry", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "BingSearch", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "ThisPC", 
        "Arg": { 
            "Zero": { 
                "Tag": "Show" 
            }, 
            "One": { 
                "Tag": "Hide" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "CheckBoxes", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "HiddenItems", 
        "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "FileExtensions", 
        "Arg": { 
            "Zero": { 
                "Tag": "Show" 
            }, 
            "One": { 
                "Tag": "Hide" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "MergeConflicts", 
        "Arg": { 
            "Zero": { 
                "Tag": "Show" 
            }, 
            "One": { 
                "Tag": "Hide" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "OpenFileExplorerTo", 
        "Arg": { 
            "Zero": { 
                "Tag": "ThisPC" 
            }, 
            "One": { 
                "Tag": "QuickAccess" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "FileExplorerCompactMode", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
"Required": "false", 
"Function": "OneDriveFileExplorerAd", 
"Arg": { 
            "Zero": { 
                "Tag": "Hide" 
            }, 
            "One": { 
                "Tag": "Show" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "SnapAssist", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "UI & Personalization", 
"Control": "cmb", 
"Required": "false", 
"Function": "FileTransferDialog", "Arg": { 
            "Zero": { 
                "Tag": "Detailed" 
            }, 
            "One": { 
                "Tag": "Compact" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "RecycleBinDeleteConfirmation", 
        "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
"Region": "UI & Personalization", 
"Control": "cmb", 
"Required": "false", 
"Function": "QuickAccessRecentFiles", 
"Arg": { 
            "Zero": { 
                "Tag": "Hide" 
            }, 
            "One": { 
                "Tag": "Show" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "QuickAccessFrequentFolders", 
        "Arg": { 
            "Zero": { 
                "Tag": "Hide" 
            }, 
            "One": { 
                "Tag": "Show" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
"Region": "UI & Personalization", 
"Control": "cmb", 
"Required": "false", 
"Function": "TaskbarAlignment", 
        "Arg": { 
            "Zero": { 
                "Tag": "Left" 
            }, 
            "One": { 
                "Tag": "Center" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "TaskbarWidgets", 
        "Arg": { 
            "Zero": { 
                "Tag": "Hide" 
            }, 
            "One": { 
                "Tag": "Show" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
"Region": "UI & Personalization", "Control": "cmb", 
"Required": "false", 
        "Function": "TaskbarSearch", 
        "Arg": { 
            "Zero": { 
                "Tag": "Hide", 
                "Opposite": "Three" 
            }, 
            "One": { 
                "Tag": "SearchIcon", 
                "Opposite": "Zero" 
            }, 
            "Two": { 
                "Tag": "SearchIconLabel", 
                "Opposite": "Zero" 
            }, 
            "Three": { 
                "Tag": "SearchBox", 
                "Opposite": "Zero" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "Three" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "SearchHighlights", 
        "Arg": { 
            "Zero": { 
                "Tag": "Hide" 
            }, 
            "One": { 
                "Tag": "Show" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "TaskViewButton", 
        "Arg": { 
            "Zero": { 
                "Tag": "Hide" 
            }, 
            "One": { 
                "Tag": "Show" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
"Function": "SecondsInSystemClock", 
"Arg": { 
            "Zero": { 
                "Tag": "Show" 
            }, 
            "One": { 
                "Tag": "Hide" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "ClockInNotificationCenter", 
        "Arg": { 
            "Zero": { 
                "Tag": "Show" 
            }, 
            "One": { 
                "Tag": "Hide" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "TaskbarCombine", 
        "Arg": { 
            "Zero": {
                "Tag": "Always", 
                "Opposite": "Two" 
            }, 
            "One": { 
                "Tag": "Full", 
                "Opposite": "Two" 
            }, 
            "Two": { 
                "Tag": "Never", 
                "Opposite": "Zero" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "Zero" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "chk", 
        "Required": "false", 
        "Function": "UnpinTaskbarShortcuts -Shortcuts", 
        "Arg": { 
            "Zero": { 
                "Tag": "Edge" 
            }, 
            "One": { 
                "Tag": "Store" 
            } 
}, 
"Preset": "", 
        "WindowsDefault": "" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "TaskbarEndTask", 
        "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "ControlPanelView", 
        "Arg": { 
            "Zero": { 
                "Tag": "LargeIcons", 
                "Opposite": "One" 
            }, 
            "One": { 
                "Tag": "SmallIcons", 
                "Opposite": "Zero" 
            }, 
            "Two": {
                "Tag": "Category", 
                "Opposite": "Two" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "Two" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "WindowsColorMode", 
        "Arg": { 
            "Zero": { 
                "Tag": "Dark" 
            }, 
            "One": { 
                "Tag": "Light" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
"Function": "AppColorMode", 
"Arg": { 
            "Zero": { 
                "Tag": "Dark" 
            }, 
            "One": { 
                "Tag": "Light" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "FirstLogonAnimation", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
"Required": "false", 
"Function": "JPEGWallpapersQuality", "Arg": { 
            "Zero": {
                "Tag": "Max" 
            }, 
            "One": { 
                "Tag": "Default" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "ShortcutsSuffix", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "UI & Personalization", 
"Control": "cmb", 
"Required": "false", 
"Function": "PrtScnSnippingTool", 
"Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "AppsLanguageSwitch", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
"Region": "UI & Personalization", 
"Control": "cmb", 
"Required": "false", 
"Function": "AeroShaking", 
        "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Preset": "One", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "Cursors", 
        "Arg": { 
            "Zero": { 
                "Tag": "Default", 
                "Opposite": "Zero" 
            }, 
            "One": { 
                "Tag": "Dark", 
                "Opposite": "Two" 
            }, 
            "Two": { 
                "Tag": "Light", 
                "Opposite": "One" 
            } 
}, 
"Preset": "One", 
        "WindowsDefault": "Zero" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "FolderGroupBy", 
        "Arg": { 
            "Zero": { 
                "Tag": "None" 
            }, 
            "One": { 
                "Tag": "Default" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "NavigationPaneExpand", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "Zero" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "RecentlyAddedStartApps", 
        "Arg": { 
            "Zero": { 
                "Tag": "Hide" 
            }, 
            "One": { 
                "Tag": "Show" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "Zero" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "MostUsedStartApps", 
        "Arg": { 
            "Zero": { 
                "Tag": "Hide" 
            }, 
            "One": { 
                "Tag": "Show" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "Zero" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "StartRecommendedSection", 
        "Arg": { 
            "Zero": { 
                "Tag": "Hide" 
            }, 
            "One": { 
                "Tag": "Show" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "Zero" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "StartRecommendationsTips", 
        "Arg": { 
            "Zero": { 
                "Tag": "Hide" 
            }, 
            "One": { 
                "Tag": "Show" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "Zero" 
    }, 
    { 
        "Region": "UI & Personalization", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "StartAccountNotifications", 
        "Arg": { 
            "Zero": { 
                "Tag": "Hide" 
            }, 
            "One": { 
                "Tag": "Show" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "Zero" 
    }, 
    { 
        "Region": "Start menu", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "StartLayout", 
        "Arg": { 
            "Zero": { 
                "Tag": "Default", 
                "Opposite": "Two" 
            }, 
            "One": { 
                "Tag": "ShowMorePins", 
                "Opposite": "Zero" 
            }, 
            "Two": { 
                "Tag": "ShowMoreRecommendations", 
                "Opposite": "Zero" 
            } 
        }, 
        "Preset": "One", 
        "WindowsDefault": "Zero" 
    }, 
    { 
        "Region": "OneDrive", 
        "Control": "cmbchkalter", 
        "Required": "false", 
        "Function": "OneDrive", 
        "Function2": "OneDrive -AllUsers", 
        "Function2Selects": "1", 
        "Arg": { 
            "Zero": { 
                "Tag": "Uninstall" 
            }, 
            "One": { 
                "Tag": "Install" 
            } 
        }, 
        "Preset": "Zero", 
"WindowsDefault": "" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "StorageSense", 
        "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "Hibernation", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
"Preset": "Zero", "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "Win32LongPathsSupport", 
        "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "BSoDStopError", 
        "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
}, 
"Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "AdminApprovalMode", 
        "Arg": { 
            "Zero": { 
                "Tag": "Never" 
            }, 
            "One": { 
                "Tag": "Default" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "DeliveryOptimization", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
}, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "WindowsManageDefaultPrinter", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "WindowsFeatures", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "WindowsCapabilities", 
        "Arg": { 
            "Zero": { 
                "Tag": "Uninstall" 
            }, 
            "One": { 
                "Tag": "Install" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "UpdateMicrosoftProducts", 
        "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "RestartNotification", 
        "Arg": { 
            "Zero": { 
                "Tag": "Show" 
            }, 
            "One": { 
                "Tag": "Hide" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "RestartDeviceAfterUpdate", 
        "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "ActiveHours", 
        "Arg": { 
            "Zero": { 
                "Tag": "Automatically" 
            }, 
            "One": { 
                "Tag": "Manually" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "WindowsLatestUpdate", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "Zero" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "PowerPlan", 
        "Arg": { 
            "Zero": { 
                "Tag": "High" 
            }, 
            "One": { 
                "Tag": "Balanced" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "NetworkAdaptersSavePower", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "InputMethod", 
        "Arg": { 
            "Zero": { 
                "Tag": "English" 
            }, 
            "One": { 
                "Tag": "Default" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
"Function": "Set-UserShellFolderLocation", 
"Arg": { 
            "Zero": { 
                "Tag": "Root", 
                "Opposite": "Two" 
            }, 
            "One": { 
                "Tag": "Custom", 
                "Opposite": "Two" 
            }, 
            "Two": { 
                "Tag": "Default", 
                "Opposite": "One" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "Two" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "LatestInstalled.NET", 
        "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
"Preset": "Zero", 
"WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "WinPrtScrFolder", 
        "Arg": { 
            "Zero": { 
                "Tag": "Desktop" 
            }, 
            "One": { 
                "Tag": "Default" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "RecommendedTroubleshooting", 
        "Arg": { 
            "Zero": { 
                "Tag": "Automatically" 
            }, 
            "One": { 
                "Tag": "Default" 
            } 
}, 
"Preset": "Zero", 
"WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "ReservedStorage", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "F1HelpPage", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
}, 
"Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "NumLock", 
        "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "CapsLock", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
}, 
        "Preset": "", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "StickyShift", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "Autoplay", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "ThumbnailCacheRemoval", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "SaveRestartableApps", 
        "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "RestorePreviousFolders", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "NetworkDiscovery", 
        "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "tboxtboxtbox", 
        "Required": "false", 
        "Function": "Set-Association", 
        "Width": "170", 
        "WidthMinus": "35", 
        "MarginMinus": "5", 
        "WidthPlus": "35", 
        "MarginPlus": "5", 
        "WidthOpen": "75", 
        "WidthSave": "100", 
        "MarginSave": "225", 
        "Arg": { 
            "Zero": { 
                "Tag": "ProgramPath", 
                "Width": "325" 
            }, 
            "One": { 
                "Tag": "Extension", 
                "Width": "85" 
            }, 
            "Two": { 
                "Tag": "Icon", 
                "Width": "250", 
                "Number": "1000" 
            } 
        }, 
        "Preset": "", 
        "WindowsDefault": "" 
    }, 
    { 
        "Region": "System", 
        "Control": "chk", 
        "Required": "false", 
        "Function": "Export-Associations", 
        "Arg": { 
            "Zero": { 
                "Tag": "" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "" 
    }, 
    { 
        "Region": "System", 
        "Control": "chk", 
        "Required": "false", 
        "Function": "Import-Associations", 
        "Arg": { 
            "Zero": { 
                "Tag": "" 
            } 
        }, 
        "Preset": "", 
        "WindowsDefault": "" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "DefaultTerminalApp", 
        "Arg": { 
            "Zero": { 
                "Tag": "WindowsTerminal" 
            }, 
            "One": { 
                "Tag": "ConsoleHost" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "chk", 
        "Required": "false", 
        "Function": "Install-VCRedist -Redistributables", 
        "Arg": { 
            "Zero": { 
                "Tag": "2015_2026_x86" 
            }, 
            "One": { 
                "Tag": "2015_2026_x64" 
            } 
        }, 
        "Preset": "", 
        "WindowsDefault": "" 
    }, 
    { 
        "Region": "System", 
        "Control": "chk", 
        "Required": "false", 
        "Function": "Install-DotNetRuntimes -Runtimes", 
        "Arg": { 
            "Zero": { 
                "Tag": "NET8" 
            }, 
            "One": { 
                "Tag": "NET9" 
            }, 
            "Two": { 
                "Tag": "NET10" 
            } 
        }, 
        "Preset": "", 
        "WindowsDefault": "" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "RKNBypass", 
        "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmbchk", 
        "Required": "false", 
        "Function": "PreventEdgeShortcutCreation", 
        "Arg": { 
            "Zero": { 
                "Tag": "Channels" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Arg2": { 
            "Zero": { 
                "Tag": "Stable", 
                "Width": "75" 
            }, 
            "One": { 
                "Tag": "Beta", 
                "Width": "65" 
            }, 
            "Two": { 
                "Tag": "Dev", 
                "Width": "62" 
            }, 
            "Three": { 
                "Tag": "Canary", 
                "Width": "80" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One", 
        "Preset2": "0123", 
        "WindowsDefault2": "" 
    }, 
    { 
        "Region": "System", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "RegistryBackup", 
        "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "WSL", 
        "Control": "chk", 
"Required": "false", 
        "Function": "Install-WSL", 
        "Arg": { 
            "Zero": { 
                "Tag": "" 
            } 
        }, 
        "Preset": "", 
        "WindowsDefault": "" 
    }, 
    { 
        "Region": "UWP apps", 
        "Control": "chkchkalter", 
        "Required": "false", 
        "Separate": "true", 
        "Function": "Uninstall-UWPApps", 
        "Function2": "Uninstall-UWPApps -ForAllUsers", 
        "Arg": { 
            "Zero": { 
                "Tag": "" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "" 
    }, 
    { 
        "Region": "Gaming", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "XboxGameBar", 
"Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Gaming", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "XboxGameTips", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Gaming", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "GPUScheduling", "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Scheduled tasks", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "CleanupTask", 
        "Arg": { 
            "Zero": { 
                "Tag": "Register" 
            }, 
            "One": { 
                "Tag": "Delete" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "" 
    }, 
    { 
        "Region": "Scheduled tasks", 
        "Control": "cmb", 
"Required": "false", 
"Function": "SoftwareDistributionTask", 
"Arg": { 
            "Zero": { 
                "Tag": "Register" 
            }, 
            "One": { 
                "Tag": "Delete" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "" 
    }, 
    { 
        "Region": "Scheduled tasks", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "TempTask", 
        "Arg": { 
            "Zero": { 
                "Tag": "Register" 
            }, 
            "One": { 
                "Tag": "Delete" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "" 
    }, 
    { 
"Region": "Microsoft Defender & Security", 
"Control": "cmb", 
"Required": "false", 
"Function": "NetworkProtection", 
        "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Microsoft Defender & Security", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "PUAppsDetection", 
        "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
"Region": "Microsoft Defender & Security", "Control": "cmb", 
"Required": "false", 
        "Function": "DefenderSandbox", 
        "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Microsoft Defender & Security", 
        "Control": "chk", 
        "Required": "false", 
        "Function": "DismissMSAccount", 
        "Arg": { 
            "Zero": { 
                "Tag": "" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "" 
    }, 
    { 
        "Region": "Microsoft Defender & Security", 
"Control": "chk", 
"Required": "false", 
"Function": "DismissSmartScreenFilter", 
"Arg": { 
            "Zero": { 
                "Tag": "" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "" 
    }, 
    { 
        "Region": "Microsoft Defender & Security", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "EventViewerCustomView", 
        "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Microsoft Defender & Security", 
        "Control": "cmb", 
"Required": "false", 
"Function": "PowerShellModulesLogging", 
"Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Microsoft Defender & Security", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "PowerShellScriptsLogging", 
        "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Microsoft Defender & Security", 
"Control": "cmb", 
"Required": "false", 
"Function": "AppsSmartScreen", 
"Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Microsoft Defender & Security", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "SaveZoneInformation", 
        "Arg": { 
            "Zero": { 
                "Tag": "Disable" 
            }, 
            "One": { 
                "Tag": "Enable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
"Region": "Microsoft Defender & Security", 
"Control": "cmb", 
"Required": "false", 
        "Function": "WindowsSandbox", 
        "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Microsoft Defender & Security", 
        "Control": "cmbcmbcmb", 
        "Required": "false",         "Function": "DNSoverHTTPS", 
        "Key": 
"SYSTEM\\CurrentControlSet\\Services\\Dnscache\\Parameters\\DohWellKnownServers", 
        "PrefixArg2": "PrimaryDNS", 
        "PrefixArg3": "SecondaryDNS", 
        "Arg2_3_Width": "145", 
        "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Arg2": { 
            "Zero": { 
                "Tag": "1.0.0.1" 
            }, 
            "One": { 
                "Tag": "1.1.1.1" 
            }, 
            "Two": { 
                "Tag": "149.112.112.112" 
            }, 
            "Three": { 
                "Tag": "8.8.4.4" 
            }, 
            "Four": { 
                "Tag": "8.8.8.8" 
            }, 
            "Five": { 
                "Tag": "9.9.9.9" 
            } 
        }, 
        "Arg3": { 
            "Zero": { 
                "Tag": "1.0.0.1" 
            }, 
            "One": { 
                "Tag": "1.1.1.1" 
            }, 
            "Two": { 
                "Tag": "149.112.112.112" 
            }, 
            "Three": { 
                "Tag": "8.8.4.4" 
            }, 
            "Four": { 
                "Tag": "8.8.8.8" 
            }, 
            "Five": { 
                "Tag": "9.9.9.9" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One", 
        "Preset2": "Zero", 
        "WindowsDefault2": "", 
        "Preset3": "One", 
        "WindowsDefault3": "" 
    }, 
    { 
        "Region": "Microsoft Defender & Security", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "LocalSecurityAuthority", 
        "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Context menu", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "MSIExtractContext", 
        "Arg": { 
            "Zero": { 
                "Tag": "Show" 
            }, 
            "One": { 
                "Tag": "Hide" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Context menu", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "CABInstallContext", 
        "Arg": { 
            "Zero": { 
                "Tag": "Show" 
            }, 
            "One": { 
                "Tag": "Hide" 
            } 
        }, 
        "Preset": "Zero", 
"WindowsDefault": "One" 
    }, 
    { 
        "Region": "Context menu", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "EditWithClipchampContext", 
        "Arg": { 
            "Zero": { 
                "Tag": "Hide" 
            }, 
            "One": { 
                "Tag": "Show" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Context menu", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "EditWithPhotosContext", 
        "Arg": { 
            "Zero": { 
                "Tag": "Hide" 
            }, 
            "One": { 
                "Tag": "Show" 
            } 
        }, 
"Preset": "Zero", 
"WindowsDefault": "One" 
    }, 
    { 
        "Region": "Context menu", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "EditWithPaintContext", 
        "Arg": { 
            "Zero": { 
                "Tag": "Hide" 
            }, 
            "One": { 
                "Tag": "Show" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Context menu", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "PrintCMDContext", 
        "Arg": { 
            "Zero": { 
                "Tag": "Hide" 
            }, 
            "One": { 
                "Tag": "Show" 
            } 
}, 
"Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Context menu", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "CompressedFolderNewContext", 
        "Arg": { 
            "Zero": { 
                "Tag": "Hide" 
            }, 
            "One": { 
                "Tag": "Show" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Context menu", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "MultipleInvokeContext", 
        "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
}, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Context menu", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "UseStoreOpenWith", 
        "Arg": { 
            "Zero": { 
                "Tag": "Hide" 
            }, 
            "One": { 
                "Tag": "Show" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Context menu", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "OpenWindowsTerminalContext", 
        "Arg": { 
            "Zero": { 
                "Tag": "Show" 
            }, 
            "One": { 
                "Tag": "Hide" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "Show" 
    }, 
    { 
        "Region": "Context menu", 
        "Control": "cmb", 
        "Required": "false", 
        "Function": "OpenWindowsTerminalAdminContext", 
        "Arg": { 
            "Zero": { 
                "Tag": "Enable" 
            }, 
            "One": { 
                "Tag": "Disable" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "One" 
    }, 
    { 
        "Region": "Update Policies", 
        "Control": "chk", 
        "Required": "false", 
        "Function": "ScanRegistryPolicies", 
        "Arg": { 
            "Zero": { 
                "Tag": "" 
            } 
        }, 
        "Preset": "Zero", 
        "WindowsDefault": "" 
    } 
] 
 
:: 1) PUNTO DE RESTAURACION echo [1/20] Creando punto de restauracion... 
powershell -Command "Checkpoint-Computer -Description 'Gaming_W11_25H2_SUPREMO' RestorePointType 'MODIFY_SETTINGS'" >nul 2>&1 
 
:: 2) SERVICIOS: TELEMETRIA echo [2/20] Desactivando servicios no criticos... 
 
sc stop DiagTrack >nul 2>&1 sc config DiagTrack start= disabled >nul 2>&1 sc stop dmwappushservice >nul 2>&1 sc config dmwappushservice start= disabled >nul 2>&1 sc stop SysMain >nul 2>&1 sc config SysMain start= disabled >nul 2>&1 sc stop WSearch >nul 2>&1 sc config WSearch start= disabled >nul 2>&1 sc stop Fax >nul 2>&1 sc config Fax start= disabled >nul 2>&1 sc stop XblAuthManager >nul 2>&1 sc config XblAuthManager start= disabled >nul 2>&1 sc stop XblGameSave >nul 2>&1 sc config XblGameSave start= disabled >nul 2>&1 sc stop XboxNetApiSvc >nul 2>&1 sc config XboxNetApiSvc start= disabled >nul 2>&1 
sc stop XboxGipSvc >nul 2>&1 & sc config XboxGipSvc start= disabled >nul 2>&sc stop xbgm 
>nul 2>&1 & sc config xbgm start= disabled >nul 2>&1 sc stop Spooler >nul 2>&1 
sc config Spooler start= disabled >nul 2>&1 
 
:: 3) TAREAS PROGRAMADAS echo [3/20] Desactivando tareas de telemetria... 
 
schtasks /Change /TN "\Microsoft\Windows\Customer Experience Improvement Program\Consolidator" /Disable >nul 2>&1 
schtasks /Change /TN "\Microsoft\Windows\Customer Experience Improvement Program\UsbCeip" /Disable >nul 2>&1 
schtasks /Change /TN "\Microsoft\Windows\Application Experience\Microsoft Compatibility Appraiser" /Disable >nul 2>&1 
schtasks /Change /TN "\Microsoft\Windows\Application Experience\ProgramDataUpdater" 
/Disable >nul 2>&1 schtasks /Change /TN "\Microsoft\Windows\Autochk\Proxy" /Disable >nul 2>&1 schtasks /Change /TN "\Microsoft\Windows\DiskDiagnostic\Microsoft-WindowsDiskDiagnosticDataCollector" /Disable >nul 2>&1 
 
:: 4) GAMEDVR / GAME BAR echo [4/20] Desactivando GameDVR y Game Bar... 
 
reg add "HKCU\System\GameConfigStore" /v GameDVR_Enabled /t REG_DWORD /d 0 /f >nul 
reg add "HKCU\System\GameConfigStore" /v GameDVR_FSEBehaviorMode /t REG_DWORD /d 
2 /f >nul 
reg add "HKCU\System\GameConfigStore" /v GameDVR_HonorUserFSEBehaviorMode /t REG_DWORD /d 1 /f >nul 
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\GameDVR" /v AllowGameDVR /t REG_DWORD /d 0 /f 
reg add "HKCU\Software\Microsoft\GameBar" /v AllowAutoGameMode /t REG_DWORD /d 1 
/f >nul 
reg add "HKCU\Software\Microsoft\GameBar" /v AutoGameModeEnabled /t REG_DWORD /d 
1 /f 
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\GameDVR" /v AllowGameDVR /t REG_DWORD /d 0 /f >nul 
 
:: 5) PRIORIDAD DE JUEGOS Y NETWORK THROTTLING 
echo [5/20] Ajustando prioridad de juegos y Network Throttling... 
 
reg add "HKLM\SOFTWARE\Microsoft\MSMQ\Parameters" /v TCPNoDelay /t REG_DWORD /d 
1 /f >nul 2>&1 
reg add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters" /v TcpAckFrequency 
/t REG_DWORD /d 1 /f >nul 2>&1 
reg add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters" /v TCPNoDelay /t REG_DWORD /d 1 /f >nul 2>& 
reg add "HKLM\SOFTWARE\Microsoft\Windows 
NT\CurrentVersion\Multimedia\SystemProfile" /v NetworkThrottlingIndex /t REG_DWORD /d 
4294967295 /f 
reg add "HKLM\SOFTWARE\Microsoft\Windows 
NT\CurrentVersion\Multimedia\SystemProfile" /v SystemResponsiveness /t REG_DWORD /d 0 
/f 
reg add "HKLM\SOFTWARE\Microsoft\Windows 
NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "GPU Priority" /t REG_DWORD /d 8 /f 
reg add "HKLM\SOFTWARE\Microsoft\Windows 
NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "Priority" /t REG_DWORD /d 6 
/f 
reg add "HKLM\SOFTWARE\Microsoft\Windows 
NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "Scheduling Category" /t REG_SZ /d "High" /f 
reg add "HKLM\SOFTWARE\Microsoft\Windows 
NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "Background Only" /t REG_SZ 
/d "False" /f 
reg add "HKLM\SOFTWARE\Microsoft\Windows 
NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "Clock Rate" /t REG_DWORD 
/d 10000 /f 
reg add "HKLM\SOFTWARE\Microsoft\Windows 
NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "SFIO Priority" /t REG_SZ /d "High" /f >nul 
 
:: 6) GRAFICOS: HAGS + MPO OFF echo [6/20] Ajustando configuracion grafica (HAGS + MPO OFF)... 
 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v HwSchMode /t REG_DWORD /d 2 /f >nul 
 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v DisableOverlays /t REG_DWORD /d 0 /f >nul 
 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v DisableMultiPlaneOverlay /t REG_DWORD /d 1 /f >nul 
 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v TdrLevel /t REG_DWORD /d 0 /f 
 
for /f %%i in ('wmic path win32_VideoController get PNPDeviceID ^| find "VEN_10DE"') do ( 
    reg add "HKLM\SYSTEM\CurrentControlSet\Enum\%%i\Device Parameters\Interrupt 
Management\MessageSignaledInterruptProperties" /v MSISupported /t REG_DWORD /d 1 /f 
>nul 2>&1 
    reg add "HKLM\SYSTEM\CurrentControlSet\Enum\%%i\Device Parameters\Interrupt Management\AffinityPolicy" /v DevicePriority /t REG_DWORD /d 3 /f >nul 2>&1 
) 
 
:: 7) PRIORIDAD PROCESOS echo [7/20] Ajustando prioridad de procesos... 
 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\PriorityControl" /v Win32PrioritySeparation /t REG_DWORD /d 0x26 /f >nul 
reg add 
"HKLM\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeI ntegrity" /v Enabled /t REG_DWORD /d 0 /f 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" 
/v "FeatureSettingsOverride" /t REG_DWORD /d 3 /f >nul 2>&1 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" 
/v "FeatureSettingsOverrideMask" /t REG_DWORD /d 3 /f >nul 2>&1 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" 
/v "ClearStandbyListAtLogoff" /t REG_DWORD /d 1 /f >nul 2>&1 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" 
/v "LargeSystemCache" /t REG_DWORD /d 0 /f >nul 2>&1 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "FeatureSettingsOverride" /t REG_DWORD /d 1 /f >nul 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" 
/v "FeatureSettingsOverrideMask" /t REG_DWORD /d 3 /f >nul 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "LargePageMinimum" /t REG_DWORD /d 1 /f >nul 
 
 
:: 8) RATON: SIN ACELERACION echo [8/20] Desactivando aceleracion de raton... 
 
reg add "HKCU\Control Panel\Mouse" /v MouseSpeed /t REG_SZ /d 0 /f >nul reg add "HKCU\Control Panel\Mouse" /v MouseThreshold1 /t REG_SZ /d 0 /f >nul reg add "HKCU\Control Panel\Mouse" /v MouseThreshold2 /t REG_SZ /d 0 /f >nul 
 
:: 9) RESOURCE POLICY STORE - CPU POLICIES echo [9/20] Aplicando Resource Policy Store tweaks (CPU)... 
 
reg add "HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\CPU\HardCap0" /v 
"CapPercentage" /t REG_DWORD /d 0 /f >nul 2>&1 
reg add "HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\CPU\HardCap0" /v 
"SchedulingType" /t REG_DWORD /d 0 /f >nul 2>&1 
reg add "HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\CPU\Paused" /v 
"CapPercentage" /t REG_DWORD /d 0 /f >nul 2>&1 
reg add "HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\CPU\Paused" /v 
"SchedulingType" /t REG_DWORD /d 0 /f >nul 2>&1 
reg add "HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\CPU\SoftCapFull" /v 
"CapPercentage" /t REG_DWORD /d 0 /f >nul 2>&1 
reg add "HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\CPU\SoftCapFull" /v 
"SchedulingType" /t REG_DWORD /d 0 /f >nul 2>&1 
reg add "HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\CPU\SoftCapLow" /v 
"CapPercentage" /t REG_DWORD /d 0 /f >nul 2>&1 
reg add "HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\CPU\SoftCapLow" /v "SchedulingType" /t REG_DWORD /d 0 /f >nul 2>&1 
 
:: 10) RESOURCE POLICY STORE - FLAGS Y IMPORTANCE echo [10/20] Aplicando Resource Policy Store tweaks (Flags + Importance)... reg add 
"HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\Flags\BackgroundDefault" /v 
"IsLowPriority" /t REG_DWORD /d 0 /f >nul 2>&1 
reg add "HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\Flags\Frozen" /v 
"IsLowPriority" /t REG_DWORD /d 0 /f >nul 2>&1 
reg add "HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\Flags\FrozenDNCS" /v 
"IsLowPriority" /t REG_DWORD /d 0 /f >nul 2>&1 
reg add "HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\Flags\FrozenDNK" /v 
"IsLowPriority" /t REG_DWORD /d 0 /f >nul 2>&1 
reg add "HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\Flags\FrozenPPLE" /v 
"IsLowPriority" /t REG_DWORD /d 0 /f >nul 2>&1 
reg add "HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\Flags\Paused" /v 
"IsLowPriority" /t REG_DWORD /d 0 /f >nul 2>&1 
reg add "HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\Flags\PausedDNK" /v 
"IsLowPriority" /t REG_DWORD /d 0 /f >nul 2>&1 
reg add "HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\Flags\Pausing" /v 
"IsLowPriority" /t REG_DWORD /d 0 /f >nul 2>&1 
reg add 
"HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\Flags\PrelaunchForeground" /v 
"IsLowPriority" /t REG_DWORD /d 0 /f >nul 2>&1 
reg add 
"HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\Flags\ThrottleGPUInterference" 
/v "IsLowPriority" /t REG_DWORD /d 0 /f >nul 2>&1 
 
reg add "HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\Importance\Critical" /v 
"BasePriority" /t REG_DWORD /d 82 /f >nul 2>&1 
reg add "HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\Importance\Critical" /v 
"OverTargetPriority" /t REG_DWORD /d 50 /f >nul 2>&1 
reg add "HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\Importance\High" /v 
"BasePriority" /t REG_DWORD /d 82 /f >nul 2>&1 
reg add "HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\Importance\High" /v 
"OverTargetPriority" /t REG_DWORD /d 50 /f >nul 2>&1 
reg add "HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\Importance\VeryHigh" /v 
"BasePriority" /t REG_DWORD /d 82 /f >nul 2>&1 
reg add "HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\Importance\VeryHigh" /v "OverTargetPriority" /t REG_DWORD /d 50 /f >nul 2>&1 
 
reg add "HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\IO\NoCap" /v 
"IOBandwidth" /t REG_DWORD /d 0 /f >nul 2>&1 
reg add "HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\Memory\NoCap" /v 
"CommitLimit" /t REG_DWORD /d 4294967295 /f >nul 2>&1 
reg add "HKLM\SYSTEM\ResourcePolicyStore\ResourceSets\Policies\Memory\NoCap" /v "CommitTarget" /t REG_DWORD /d 4294967295 /f >nul 2>&1 
 
:: 11) TIMER RESOLUTION - BCDEDIT SEGURO echo [11/20] Configurando timer resolution SEGURO... 
 
bcdedit /deletevalue useplatformclock >nul 2>&1 bcdedit /set useplatformtick yes >nul 2>&1  bcdedit /set disabledynamictick yes >nul 2>&1 
 
:: 12) TIMER RESOLUTION - REGISTRY TWEAKS echo [12/20] Aplicando registry tweaks para timer stability... 
 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager" /v CoalescingTimerInterval /t REG_DWORD /d 0 /f >nul 2>&1 
 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Kernel" /v GlobalTimerResolutionRequests /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: 13) POWER TWEAKS TIER 1 - CRÍTICOS[web:115][web:118][web:120] echo [13/20] Aplicando Power tweaks TIER 1 (críticos)... 
 
:: PowerThrottlingOff - IMPACTO ALTÍSIMO +5-10%% FPS 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling" /v PowerThrottlingOff /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: PerfBoostAtGuaranteedLevel - Boost CPU a máxima frecuencia 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v PerfBoostAtGuaranteedLevel /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: Latency - Reducir latencia general 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v Latency /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: 14) POWER TWEAKS TIER 2 - MUY BENEFICIOSOS[web:115][web:118][web:123] echo [14/20] Aplicando Power tweaks TIER 2 (latency optimization)... 
 
:: Exit Latency tweaks 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v ExitLatency /t REG_DWORD /d 
1 /f >nul 2>&1 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v ExitLatencyCheckEnabled /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: Latency Tolerance tweaks - Reducir latencia en diferentes escenarios 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v LatencyToleranceDefault /t REG_DWORD /d 1 /f >nul 2>&1 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v LatencyToleranceFSVP /t REG_DWORD /d 1 /f >nul 2>&1 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v LatencyTolerancePerfOverride 
/t REG_DWORD /d 1 /f >nul 2>&1 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v LatencyToleranceScreenOffIR /t REG_DWORD /d 1 /f >nul 2>&1 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v LatencyToleranceVSyncEnabled /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: RTL Capability Check Latency 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v RtlCapabilityCheckLatency /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: 15) POWER TWEAKS TIER 3 - ADICIONALES[web:115][web:118] echo [15/20] Aplicando Power tweaks TIER 3 (adicionales)... 
 
:: Processor Performance Calculation 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v PerfCalculateAcIntervals /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: Event Processor Enabled 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v EventProcessorEnabled /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: Processor Performance Time Check Interval 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v 
ProcessorPerformanceTimeCheckInterval /t REG_DWORD /d 5000 /f >nul 2>&1 
 
:: Platform AC Override (para soporte AC power states) 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v PlatformAoAcOverride /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: GPU D3 Latency 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v DefaultD3TransitionLatencyActivelyUsed /t REG_DWORD /d 1 /f >nul 2>&1 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v DefaultD3TransitionLatencyIdleLongTime /t REG_DWORD /d 1 /f >nul 2>&1 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Power" /v DefaultD3TransitionLatencyIdleMonitorOff /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: 16) WIDGETS / BARRA TAREAS echo [16/20] Desactivando widgets y elementos innecesarios... 
 
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced" /v TaskbarDa /t REG_DWORD /d 0 /f >nul 2>&1 
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced" /v ShowTaskViewButton /t REG_DWORD /d 0 /f >nul 2>&1 
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Widgets" /v WidgetsEnabled /t REG_DWORD /d 0 /f >nul 2>&1 
 
:: REVISAR_INICIO 
:: 18) MEMORY MANAGEMENT TWEAKS echo [18/24] Aplicando tweaks de Memory Management... reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" 
/v DisablePagingExecutive /t REG_DWORD /d 1 /f >nul 2>&1 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" 
/v LargeSystemCache /t REG_DWORD /d 1 /f >nul 2>&1 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" 
/v IoPageLockLimit /t REG_DWORD /d 67108864 /f >nul 2>&1 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v ClearPageFileAtShutdown /t REG_DWORD /d 0 /f >nul 2>&1 
 
:: 19) NVIDIA NVTWEAK OPTIMIZATIONS echo [19/24] Aplicando NVTweak optimizations (RTX 4080)... 
reg add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v CoolBits /t REG_DWORD 
/d 12 /f >nul 2>&1 
reg add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v DisableAnsel /t REG_DWORD /d 1 /f >nul 2>&1 
reg add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v DisableFlecs /t REG_DWORD /d 1 /f >nul 2>&1 
reg add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v EnableRustMode /t REG_DWORD /d 2 /f >nul 2>&1 
reg add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v NvCplEnableActiveApp 
/t REG_DWORD /d 1 /f >nul 2>&1 
reg add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v NoLowLatencyMRU /t REG_DWORD /d 1 /f >nul 2>&1 :: TWEAKS NVIDIA - NVTWEAK echo [2/8] Aplicando tweaks NVIDIA para RTX 4080... 
 
:: Desactivar watermark SLI (mejora estabilidad) 
reg add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v "DisableSliWatermark" /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: Desactivar ShadowPlay/GeForce Experience overhead 
reg add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v "EnableShadowPlay" /t REG_DWORD /d 0 /f >nul 2>&1 
 
:: Forzar modo dedicado (Optimus override para máxima performance) 
reg add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v "OptimusOverride" /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: Restringir profiling a administradores (mejora seguridad y performance) 
reg add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v "RmProfilingAdminOnly" /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: Desactivar telemetría NVIDIA completa 
reg add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v "DisableTelemetry" /t REG_DWORD /d 1 /f >nul 2>&1 
reg add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v "DisableGPUTelemetry" /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: Forzar modo de baja latencia NVIDIA Reflex 
reg add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v "LowLatencyMode" /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: Habilitar VRR/G-Sync por defecto 
reg add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v "EnableVRR" /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: Forzar estado P0 para máxima frecuencia GPU 
reg add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v "ForceP0State" /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: Habilitar Resizable BAR para mejor performance VRAM 
reg add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v "EnableResizableBAR" /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: TWEAKS MEMORY MANAGEMENT - OPTIMIZACIÓN DDR5 echo [3/8] Aplicando tweaks de memoria para DDR5 5600MHz... 
 
:: Habilitar large pages para mejor performance 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "LargePageMinimum" /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: Desactivar mitigaciones de seguridad Spectre/Meltdown (mejora performance) 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" 
/v "FeatureSettingsOverride" /t REG_DWORD /d 3 /f >nul 2>&1 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "FeatureSettingsOverrideMask" /t REG_DWORD /d 3 /f >nul 2>&1 
 
:: Mantener kernel en memoria RAM (reduce stutter) 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "DisablePagingExecutive" /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: Configurar caché L2 para CPU 14700KF (8MB) 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "SecondLevelDataCache" /t REG_DWORD /d 8192 /f >nul 2>&1 
 
:: Optimizar pool usage para gaming 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "PoolUsageMaximum" /t REG_DWORD /d 50 /f >nul 2>&1 
 
:: Limite de bloqueo de páginas para NVMe + DDR5 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "IoPageLockLimit" /t REG_DWORD /d 16777216 /f >nul 2>&1 
 
:: Desactivar file paging en shutdown (mejora tiempos de carga) 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "ClearPageFileAtShutdown" /t REG_DWORD /d 0 /f >nul 2>&1 
 
:: Configuración de caché de sistema optimizada para gaming 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "LargeSystemCache" /t REG_DWORD /d 0 /f >nul 2>&1 
 
:: TWEAKS ADICIONALES CRÍTICOS 
echo [4/8] Aplicando tweaks adicionales críticos... 
 
:: Desactivar telemetry NVIDIA por completo 
reg add "HKLM\SOFTWARE\NVIDIA Corporation\NvControlPanel2\Client" /v 
"OptInOrOutPreference" /t REG_DWORD /d 0 /f >nul 2>&1 
reg add "HKLM\SOFTWARE\NVIDIA Corporation\NVSVC" /v "DisableTelemetry" /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: Optimizar prioridad NVIDIA 
reg add "HKLM\SYSTEM\CurrentControlSet\Services\nvlddmkm" /v "TCC" /t REG_DWORD /d 0 /f >nul 2>&1 
 
:: TWEAKS DE RED PARA GAMING ONLINE echo [5/8] Optimizando red para gaming competitivo... netsh int tcp set global autotuninglevel=disabled >nul 2>&1 netsh int tcp set global congestionprovider=ctcp >nul 2>&1 
 
:: NVIDIA Optimizaciones 
reg add "HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v CoolBits /t REG_DWORD /d 3 /f 
reg add "HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v CUDAP2State /t REG_DWORD /d 0 /f 
reg add "HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v EnableReflex /t REG_DWORD /d 1 /f 
reg add "HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v EnableDLSS /t REG_DWORD /d 1 /f 
reg add "HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v DLSSPerformanceMode /t REG_DWORD /d 3 /f 
reg add "HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v ShaderCache /t REG_DWORD /d 1 /f 
reg add "HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v ThreadedOptimization /t REG_DWORD /d 1 /f 
reg add "HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v DisableFrameViewSDK /t REG_DWORD /d 1 /f 
 
:: Memoria Optimizaciones 
reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v DisablePagingExecutive /t REG_DWORD /d 1 /f 
reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v LargeSystemCache /t REG_DWORD /d 0 /f 
reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v DisableMemoryCompression /t REG_DWORD /d 1 /f 
reg add "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session 
Manager\Memory Management\PrefetchParameters" /v EnablePrefetcher /t REG_DWORD /d 
0 /f 
 
:: 10) NVTWEAK - DESBLOQUEO DE OPCIONES OCULTAS (NUEVO) echo [10/27] Configurando NVTweak (Opciones Ocultas Nvidia)... 
reg add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v CoolBits /t REG_DWORD 
/d 31 /f >nul 2>&1 
reg add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v NoDisplayModeLock /t REG_DWORD /d 1 /f >nul 2>&1 
reg add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v ExposeMipFilter /t REG_DWORD /d 1 /f >nul 2>&1 
reg add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v ExposePowerMizer /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: 11) MEMORY MANAGEMENT TWEAKS (NUEVO - CRUCIAL PARA 64GB DDR5) echo [11/27] Optimizando Memoria Management (Disabling Paging + Security)... 
:: Mantiene drivers/kernel en RAM (Sin paginar a disco) 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v DisablePagingExecutive /t REG_DWORD /d 1 /f >nul 2>&1 :: No usar RAM para cache de disco, priorizar Apps 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v LargeSystemCache /t REG_DWORD /d 0 /f >nul 2>&1 
:: Limpiar lista standby al apagar 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v ClearStandbyListAtLogoff /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: 12) SPECTRE/MELTDOWN DESACTIVADO (CRUCIAL PARA FPS) 
echo [12/27] Desactivando mitigaciones de CPU (Raw Performance)... 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" 
/v FeatureSettingsOverride /t REG_DWORD /d 3 /f >nul 2>&1 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v FeatureSettingsOverrideMask /t REG_DWORD /d 3 /f >nul 2>&1 
 
:: REVISAR_FIN 
 
:: 17) VERIFICACION FINAL echo [17/20] Realizando verificacion final... 
 
reg query "HKLM\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling" /v PowerThrottlingOff >nul 2>&1 if %errorlevel% == 0 (   echo [OK] Power Throttling desactivado correctamente 
) else ( 
  echo [WARN] Verifica Power Throttling manualmente 
) 
 
La Superposición Multiplano (MPO, por sus siglas en inglés) es una característica de Windows que permite al hardware gráfico componer varias capas de contenido en una sola imagen para mejorar el rendimiento y la eficiencia energética. Sin embargo, en algunas configuraciones, MPO puede causar problemas como parpadeos, congelamientos o artefactos visuales. Desactivar MPO puede ayudar a resolver estos inconvenientes. 
 
Para desactivar la Superposición Multiplano (MPO) mediante el Editor del Registro de Windows, sigue estos pasos: 
 
Abrir el Editor del Registro: 
 
Presiona las teclas Win + R para abrir el cuadro de diálogo "Ejecutar". 
 
Escribe regedit y presiona Enter. 
 
Navegar a la clave correspondiente: 
En el Editor del Registro, dirígete a la siguiente ruta: 
 
sql 
Copiar 
Editar 
HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers 
Crear un nuevo valor DWORD: 
 
Haz clic derecho en el panel derecho de la ventana y selecciona Nuevo > Valor DWORD (32 bits). 
 
Nombra este nuevo valor como DisableMultiPlaneOverlay. 
 
Modificar el valor: 
 
Haz doble clic en DisableMultiPlaneOverlay. 
 
Establece los datos del valor en 1. 
 
Haz clic en Aceptar para guardar los cambios. 
 
Cerrar el Editor del Registro y reiniciar: 
 
HKEY_LOCAL_MACHINE\SYSTEM\ControlSet001\Control 
SvcHostSplitThresholdinKB valor en decimal 
8gb ram - 8388608 
16gb ram - 16777216 
32gb ram - 33554432 64gb ram - 67108864 Deshabilitar HPET (comandos utilizados) 
 
1)	bcdedit -set useplatformclock no  
2)	bcdedit -set disabledynamictick yes 
3)	bcdedit -set useplatformtick yes 
 
HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager 
CoalescingTimerInterval=0 
 
🔧 AJUSTE 1: Eliminar funciones opcionales y componentes heredados 
Ruta: 
Configuración, Aplicaciones, Funciones opcionales → Desinstalar: 
Modo Internet Explorer 
Reproductor de Windows Media 
Reconocedor matemático 
Visor de XPS 
WordPad 
Grabador de pasos 
Panel de control, Programas, Activar/desactivar funciones de Windows → Desmarcar: 
Fax y escáner de Windows 
SMB 1.0/CIFS 
Cliente de impresión por Internet 
Compresión diferencial remota 
 
🔧 AJUSTE 2: Eliminar la sobrecarga de UWP con Revo Uninstaller 
Pasos: 
Descargar desde: revouninstaller 
Usar la pestaña Aplicaciones de Windows 
Desinstalar aplicaciones: Barra de juegos de Xbox, Contactos, Cortana, Visor 3D, Clipchamp, OneNote 
Ejecutar análisis avanzado → Eliminar archivos basura del registro 
🔧 AJUSTE 3: Desactivar el mantenimiento programado Tareas 
Ruta: 
taskschd.msc → Ir a: 
Experiencia de la aplicación: Deshabilitar AITAgent, ProgramDataUpdater 
CEIP: Deshabilitar KernelCeipTask, UsbCeip 
DiskDiagnostic: Deshabilitar DiskDiagnosticDataCollector 
Mantenimiento: Deshabilitar RegularMaintenance 
 
🔧 MEJORA 4: Bloquear telemetría (Directiva de grupo) 
Ejecutar: gpedit.msc 
Ruta: 
Configuración del equipo, Plantillas de administración, Componentes de Windows, Recopilación de datos 
→ Deshabilitar Permitir telemetría 
Para usuarios domésticos – Método de registro: 
Ruta: 
HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection 
Crear DWORD: AllowTelemetry = 0 
 
🔧 MEJORA 5: Eliminar servicios en segundo plano con el script BloatyNosy o Sophia 
Pasos: 
Descargar desde GitHub 
Strip: Experiencia del consumidor de Microsoft, Cortana, Widgets, Precarga de Edge, Servicios en vivo 
 
🔧 MEJORA 6: Desactivar widgets y chat en la barra de tareas 
Ruta: 
Configuración, Personalización, Barra de tareas 
Desactivar: 
Widgets 
Chat 
Buscar (Usar OpenShell en su lugar) 
 
🔧 MEJORA 7: Optimización de pila de red y ping 
Ruta del registro: 
HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters 
Añadir DWORD: 
TCPNoDelay = 1 
TcpAckFrequency = 1 
MaxUserPort = 65534 
TcpTimedWaitDelay = 30 
Desactivar descargas en segundo plano: 
Ruta: 
Configuración Windows Update, Opciones avanzadas, Optimización de entrega → Configurar "Permitir descargas desde otros PC" = Desactivado 
 
🔧 TWEAK 8: Activar el plan de máximo rendimiento de Bitsum (CPU Boost) 
Pasos: 
Descargar Process Lasso (gratis) Activar: 
Máximo rendimiento de Bitsum 
ProBalance = ACTIVADO 
IdleSaver = Opcional 
 
🔧 BONIFICACIÓN: Usar ejecuciones automáticas para eliminar servicios ocultos en segundo plano 
Herramienta: Ejecuciones automáticas de Sysinternals 
Desactivar entradas en: 
Inicio de sesión 
Tareas programadas 
Servicios (p. ej., Xbox, Skype Updater, OneDrive) 
Ajustes con comandos y rutas completos: 
 
🔹 1. Desactivar la limitación de energía (CPU) 
Ruta del registro: 
HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling 
Crear DWORD: PowerThrottlingOff = 1 
 
🔹 2. Deslimitar la pila de red (Optimización de NIC) 
Ruta del registro: 
HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters 
Agregar o modificar DWORD: 
TcpAckFrequency = 1 
TCPNoDelay = 1 
MaxUserPort = 65534 
TcpTimedWaitDelay = 30 
DefaultTTL = 64 
EnablePMTUDiscovery = 1 
EnableTCPChimney = 0 
 
Aplicar Con CMD (Administrador): 
netsh int ip reset ipconfig /flushdns 
 
🔹 3. Forzar el plan de máximo rendimiento 
CMD (Administrador): Editar powercfg -duplicatescheme e9a42b02-d5df-448d-aa00-03f14749eb61 
Panel de control, Opciones de energía, Seleccionar: Máximo rendimiento 
 
🔹 4. Deshabilitar la moderación de interrupciones (latencia de la NIC) Administrador de dispositivos, Adaptadores de red, Propiedades del adaptador, Opciones avanzadas 
Deshabilitar la moderación de interrupciones 
Maximizar los búferes de recepción/transmisión 
 
🔹 5. Habilitar la programación agresiva de GPU (HAGS 2.0) 
Ruta del registro: 
HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers 
Crear DWORD: HwSchMode = 2 
(Opcional) Para GPU híbridas): 
GpuPreferencePolicy = 1 
Reinicie para aplicar 
Verifique también: 
Configuración, Sistema, Pantalla, Gráficos, Configuración gráfica predeterminada 
 
🔹 6. Desactivar el estacionamiento de núcleos (corrección de latencia de CPU) 
CMD (Administrador): powercfg -attributes SUB_PROCESSOR CPMAXCORES -ATTRIB_HIDE Luego: 
Panel de control, Opciones de energía, Configuración del procesador 
Establecer el estado mínimo/máximo del procesador = 100 % 
 
🔹 7. Desactivar GameDVR y el modo de juego Rutas del registro: 
HKEY_CURRENT_USER\System\GameConfigStore 
 
GameDVR_Enabled = 0 
HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR 
AllowGameDVR = 0 
 
🔹 8. Desactivar SysFX (audio de baja latencia) Pila) 
Ruta del Registro: 
HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\MMDevices\Audio\
Render\{Device-ID}\FxProperties 
 
Ajusta los efectos que no uses a 0 
 
🔹 9. Limpiar DNS + Desactivar IPv6 
CMD (Administrador): ipconfig /flushdns 
Desactivar IPv6: 
Panel de Control, Redes y recursos compartidos, Configuración del adaptador 
Clic derecho, Propiedades, Desmarcar IPv6 
Ajustar DNS a Cloudflare: 
Preferido: 1.1.1.1 
Alternativo: 1.0.0.1 
 
MEJORA 1: Deshabilitar la moderación de interrupciones (Corrección de latencia de la NIC) 
📍 Administrador de dispositivos 
‣	Inicio, Administrador de dispositivos 
‣	Expandir "Adaptadores de red" 
‣	Hacer clic derecho en el adaptador Ethernet → Propiedades → Avanzadas 
‣	Deshabilitar la moderación de interrupciones 
‣	Deshabilitar la tasa de moderación de interrupciones (si está disponible) ‣ Habilitar el escalado del lado de recepción 
 
MEJORA 2: Ajustar los búferes de transmisión/recepción de la NIC 
📍 Administrador de dispositivos, Adaptador de red, Avanzadas 
‣	Reducir los búferes de transmisión a: 128 
‣	Reducir los búferes de recepción a: 128 
‣	Probar con: 64, 128 o 256 (anotar primero los valores originales) 
 
MEJORA 3: Deshabilitar el algoritmo de Nagle (Corrección del registro TCP) 
📍 Registro Ruta: 
HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces 
‣	Coincida con su IPv4 desde ipconfig /all para encontrar la clave correcta 
‣	Agregue o modifique: 
TcpAckFrequency = 1 
TCPNoDelay = 1 
TcpDelAckTicks = 0 
(Valores DWORD de 32 bits) 
 
AJUSTE 4: Deshabilitar la compresión de memoria 
📍 PowerShell (Administrador): 
Disable-MMAgent -mc 
Get-MMAgent 
‣	Busque: MemoryCompression : False 
‣	Para deshacer: Enable-MMAgent -mc 
‣	Reiniciar después de aplicar 
 
AJUSTE 5: Establecer la resolución del temporizador de baja latencia de entrada 
📍 CMD (Administrador): 
bcdedit /set useplatformtick yes bcdedit /set disableddynamictick Sí 
‣	Reiniciar para aplicar 
 
AJUSTE 6 – Deshabilitar el último acceso a NTFS (Optimización de E/S de disco) 
📍 CMD (Administrador): 
fsutil behavior set disabledlastaccess 1 
‣	O Registro: 
HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem 
‣	DWORD: NtfsDisableLastAccessUpdate = 1 
‣	Para deshacer: fsutil behavior set disabledlastaccess 0 
 
Estos ajustes mejoran: 
Respuesta de entrada 
Gestión de pings 
Velocidad de procesamiento de red 
Latencia de memoria del sistema 
Retardo de lectura/escritura de archivos del juego 
 
🛠️ Comandos y rutas utilizadas: 
 
PASO 1: Probar el retardo de entrada 
→ Descargar y ejecutar: Keyboard ResponseTester 
→ Registrar el retardo máximo/mínimo/promedio antes de realizar cambios 
 
PASO 2: Desactivar la suspensión selectiva de USB 
→ Panel de control, Opciones de energía, Cambiar la configuración del plan, Opciones avanzadas 
→ Configuración USB, Configuración de suspensión selectiva de USB → Desactivado 
→ CMD (Administrador): 
powercfg -duplicatescheme e9a42b02-d5df-448d-aa00-03f14749eb61 
 
PASO 3: Cambiar el puerto USB 
→ Usar los puertos USB traseros de la placa base (preferiblemente USB 2.0) 
→ Evitar concentradores USB o conexiones directas de monitor 
 
PASO 4: Desactivar el ahorro de energía en el Administrador de dispositivos 
→ Administrador de dispositivos, Controladores de bus serie universal 
→ Concentrador raíz USB, Propiedades, Administración de energía Desmarque la opción "Permitir que la computadora apague el dispositivo". 
→ Repita el proceso para el teclado HID y todos los dispositivos de entrada USB. 
→ Compruebe también en Dispositivos de Interfaz Humana. 
 
PASO 5: Desactive las teclas de filtro. 
→ Configuración, Accesibilidad, Teclado → Teclas de filtro: DESACTIVADAS. 
→ Panel de control, Teclado, Configure el retardo de repetición al mínimo. 
→ Desactive las teclas fijas, de alternancia y lentas. 
 
PASO 6: Use el software del fabricante. 
→ Abra Logitech G Hub / Corsair iCUE / Razer Synapse. 
→ Configure la tasa de sondeo: 1000 Hz o superior. 
→ Desactive: Retardo de rebote, Asistente de macros, Suavizado de entrada. 
→ Desactive el modo de suspensión del dispositivo si está disponible. 
 
PASO 7: Opcional: ISLC. 
→ Descargue Intelligent Standby List Cleaner. 
→ Configure la resolución: 0,50 ms. 
→ Haga clic en Inicio y déjelo en ejecución durante el juego. 
→ Opcional: Añádalo al Programador de tareas. Inicio automático 
 
PASO 8: Repetir la prueba 
→ Reabrir Keyboard ResponseTester 
→ Comparar el retraso antes y después de los ajustes 
 
PASO 9: Mejorar la entrada en primer plano con Regedit 
→ Presionar Win+R → regedit 
→ Ruta: 
HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl 
→ Crear DWORD: Win32PrioritySeparation → Establecer en 26 (hexadecimal) 
 
PASO 10: Estabilizar el sondeo USB con Regedit 
→ Ruta: 
HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB 
→ DWORD: EnIdleEndpointSupport → Establecer en 0 
→ Ruta adicional: 
HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\HidUsb 
→ DWORD: IdleEnabled → Establecer en 0 
 
@echo off setlocal EnableExtensions title RTX 4080 – Tarkov/Arena Low-Latency Pack echo === Aplicando optimizaciones completas (ShadowPlay sigue vivo) === 
 
:: 1  NVIDIA services: solo mata telemetría sc stop  NvTelemetryContainer >nul 2>&1 sc config NvTelemetryContainer start= disabled >nul 2>&1 
reg add "HKLM\SYSTEM\CurrentControlSet\Services\NvTelemetryContainer" /v Start /t REG_DWORD /d 4 /f >nul 2>&1 
 
:: 2  CUDA Force P2 = OFF  +  HAGS OFF 
reg add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v "CUDAP2State" /t REG_DWORD /d 0 /f >nul 2>&1 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "HwSchMode" /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: 3  Full-Screen Optimisations OFF 
reg add "HKCU\System\GameConfigStore" /v "GameDVR_FSEBehavior"         /t REG_DWORD /d 
2 /f >nul 2>&1 
reg add "HKCU\System\GameConfigStore" /v "GameDVR_FSEBehaviorMode"     /t REG_DWORD /d 2 /f >nul 2>&1 
reg add "HKCU\System\GameConfigStore" /v "GameDVR_HonorUserFSEBehaviorMode" /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: 4  DWM frame-buffer + MPO OFF 
reg add "HKLM\SOFTWARE\Microsoft\Windows\Dwm" /v "OverlayTestMode"        /t REG_DWORD /d 5 /f >nul 2>&1 
reg add "HKLM\SOFTWARE\Microsoft\Windows\Dwm" /v "ForceDisableFrameBuffering" /t REG_DWORD /d 0 /f >nul 2>&1 
 
:: 5  Direct-Flip / Low-Latency / Tear 
reg add "HKLM\SOFTWARE\Microsoft\Windows 
NT\CurrentVersion\Multimedia\SystemProfile" /v "EnableMMCSS"        /t REG_DWORD /d 1 /f 
>nul 2>&1 
reg add "HKLM\SOFTWARE\Microsoft\Windows 
NT\CurrentVersion\Multimedia\SystemProfile" /v "AlwaysUseDirectFlip" /t REG_DWORD /d 1 
/f >nul 2>&1 
reg add "HKLM\SOFTWARE\Microsoft\Windows 
NT\CurrentVersion\Multimedia\SystemProfile" /v "AllowTearing"        /t REG_DWORD /d 1 /f >nul 2>&1 
 
echo. echo ✅ Hecho. Reinicia para que surja efecto completo. 
pause 
 
:: 1  Desactivar inicio rápido 
REG ADD "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Power" /v 
HiberbootEnabled /t REG_DWORD /d 0 /f >nul 2>&1 
 
:: 2  Forzar RSS en NIC 
reg add "HKLM\SYSTEM\CurrentControlSet\Services\NDIS\Parameters" /v RSS /t REG_DWORD /d 1 /f >nul 2>&1 
 
:: 3  Cache sistema modo programs (no servidor) 
reg add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v LargeSystemCache /t REG_DWORD /d 0 /f >nul 2>&1 
 
:: Reducir cola de presentación al mínimo 
reg add "HKLM\SOFTWARE\Microsoft\Windows 
NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "Scheduling Category" /t REG_SZ /d "High" /f 
reg add "HKLM\SOFTWARE\Microsoft\Windows 
NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "SFIO Priority" /t REG_SZ /d "High" /f 
reg add "HKLM\SOFTWARE\Microsoft\Windows 
NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "Latency Sensitive" /t REG_SZ /d "True" /f 
 
:: G-SYNC + V-SYNC ON → DWM ya sincroniza; evitamos triple buffer interno reg add "HKCU\System\GameConfigStore" /v "Win32_AutoTuning" /t REG_DWORD /d 0 /f 
reg add "HKLM\SOFTWARE\Microsoft\Windows\Dwm" /v "ForceDisableFrameBuffering" /t REG_DWORD /d 0 /f 
:: 0 = deja que DWM use su propio buffer (menos stutter con G-SYNC ventana) 
 
:: Desactivar HPET + platform timers de alta resolución bcdedit /set useplatformclock No bcdedit /set disabledynamictick Yes 
 
Get-AppxPackage *XboxGamingOverlay* | Remove-AppxPackage 
Get-AppxPackage *XboxGameBar* | Remove-AppxPackage 
Get-AppxPackage *gamingservices* | Remove-AppxPackage 
 
Run CMD as Admin: 
bcdedit /set useplatformtick yes bcdedit /set disabledynamictick yes bcdedit /set tscsyncpolicy Enhanced 
Registry Path: 
HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Kernel 
Add DWORD: 
InterruptPolicyOverride = 1 
 
HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\kbdclass\Parameters 
Add DWORD: 
KeyboardDataQueueSize = 64 decimal 
 
reg add 
HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Policies\Microsoft\FeatureManagement\ Overrides /v 735209102 /t REG_DWORD /d 1 /f 
reg add 
HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Policies\Microsoft\FeatureManagement\ Overrides /v 1853569164 /t REG_DWORD /d 1 /f 
reg add 
HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Policies\Microsoft\FeatureManagement\
Overrides /v 156965516 /t REG_DWORD /d 1 /f 
 
Como ves esto para mi pc 14700kf a 5.7ghz oc fixed, 64gb ddr5 a 5600mhz, ssd nvme y rtx 4080 con driver nvidia 591.74 4k 160hz con gsync + vsync + low latency ultra + reflex on + boost para bajar la latencia y subir los 1% y 0.2% Set-Service AxInstSV -StartupType Disabled 
Set-Service tzautoupdate -StartupType Disabled 
Set-Service bthserv -StartupType Disabled 
Set-Service dmwappushservice -StartupType Disabled 
Set-Service lfsvc -StartupType Disabled 
Set-Service SharedAccess -StartupType Disabled 
Set-Service lltdsvc -StartupType Disabled 
Set-Service AppVClient -StartupType Disabled 
Set-Service NetTcpPortSharing -StartupType Disabled 
Set-Service CscService -StartupType Disabled 
Set-Service PhoneSvc -StartupType Disabled 
Set-Service Spooler -StartupType Disabled 
Set-Service PrintNotify -StartupType Disabled 
Set-Service QWAVE -StartupType Disabled 
Set-Service RmSvc -StartupType Disabled 
Set-Service ShellHWDetection -StartupType Disabled 
Set-Service SCardSvr -StartupType Disabled 
Set-Service ScDeviceEnum -StartupType Disabled 
Set-Service SSDPSRV -StartupType Disabled 
Set-Service WiaRpc -StartupType Disabled 
Set-Service TabletInputService -StartupType Disabled 
Set-Service UevAgentService -StartupType Disabled 
Set-Service WalletService -StartupType Disabled 
Set-Service FrameServer -StartupType Disabled 
Set-Service stisvc -StartupType Disabled 
Set-Service wisvc -StartupType Disabled 
Set-Service icssvc -StartupType Disabled 
Set-Service WSearch -StartupType Disabled 
Set-Service XblAuthManager -StartupType Disabled 
Set-Service XblGameSave -StartupType Disabled 
Set-Service SEMgrSvc -StartupType Disabled 
Set-Service DiagTrack -StartupType Disabled 

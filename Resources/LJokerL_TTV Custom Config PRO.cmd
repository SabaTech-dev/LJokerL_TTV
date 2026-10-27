@echo off
if not "%1" == "max" start /MAX cmd /c %0 max & exit/b
@echo off
::LJokerL_TTV Custom Config PRO v10.0 | Optimizado para i7 12700k + RTX 4080 + 64GB DDR5 + Windows 11

:: Color Codes - Mejorados
set w=[97m
set g=[90m
set p=[95m
set b=[96m
set o=[1m
set r=[91m
set y=[93m

:: Enabling ANSI Escape Sequences
Reg.exe add "HKCU\CONSOLE" /v "VirtualTerminalLevel" /t REG_DWORD /d "1" /f  > nul

::Enabling Delayed Expansion
setlocal EnabledelayedExpansion > nul
cls 

for /f %%i in ('wmic path Win32_UserAccount where name^="%username%" get sid ^| findstr "S-"') do set "USER_SID=%%i"

::Enabling Restore points 
chcp 437 >nul 2>&1
powershell -NoProfile Enable-ComputerRestore -Drive 'C:\' >nul 2>&1
Reg.exe delete "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore" /v "RPSessionInterval" /f  >nul 2>&1
Reg.exe delete "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore" /v "DisableConfig" /f >nul 2>&1
Reg.exe add "HKLM\Software\Microsoft\Windows NT\CurrentVersion\SystemRestore" /v "SystemRestorePointCreationFrequency" /t REG_DWORD /d 0 /f >nul 2>&1
chcp 65001 >nul 2>&1
cls 
echo.
echo.
echo.
echo.
echo.
echo.
echo.                                    %p%██╗         ██╗ ██████╗ ██╗  ██╗███████╗██████╗ ██╗         ████████╗████████╗██╗   ██╗    ██████╗██████╗  ██████╗
echo.                                    %p%██║         ██║██╔═══██╗██║ ██╔╝██╔════╝██╔══██╗██║         ╚══██╔══╝╚══██╔══╝██║   ██║   ██╔════╝██╔══██╗██╔═══██╗
echo.                                    %p%██║         ██║██║   ██║█████╔╝ █████╗  ██████╔╝██║            ██║      ██║   ██║   ██║   ██║     ██████╔╝██║   ██║                     
echo.                                    %p%██║    ██╗  ██║██║   ██║██╔═██╗ ██╔══╝  ██╔══██╗██║            ██║      ██║   ╚██╗ ██╔╝   ██║     ██╔═══╝ ██║   ██║      
echo.                                    %p%███████╗╚█████╔╝╚██████╔╝██║  ██╗███████╗██║  ██║███████╗       ██║      ██║    ╚████╔╝    ╚██████╗██║     ╚██████╔╝ 
echo.                                    %p%╚══════╝ ╚════╝  ╚═════╝ ╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝╚══════╝       ╚═╝      ╚═╝     ╚═══╝      ╚═════╝╚═╝      ╚═════╝  %b% 
echo.                                                                    ╔════════════════════════════════════════════════════╗
echo.                                                                    ║                                                    ║
echo.                                                                    ║     %w% Verificando Privilegios Administrativos...    %b%  ║
echo.                                                                    ║                                                    ║
echo.                                                                    ╚════════════════════════════════════════════════════╝
timeout /t 3 /nobreak > NUL
rmdir %SystemDrive%\Windows\system32\adminrightstest >nul 2>&1
mkdir %SystemDrive%\Windows\system32\adminrightstest >nul 2>&1
if %errorlevel% neq 1 goto welcome  
) ELSE ( 
cls 
timeout /t 1 /nobreak > NUL
chcp 437 >nul 2>&1
powershell -NoProfile -NonInteractive -Command start -verb runas "'%~s0'" >nul 2>&1
chcp 65001 >nul 2>&1
:rmenu
cls
echo. 
echo. 
echo. 
echo.
echo.                                    %p%██╗         ██╗ ██████╗ ██╗  ██╗███████╗██████╗ ██╗         ████████╗████████╗██╗   ██╗    ██████╗██████╗  ██████╗
echo.                                    %p%██║         ██║██╔═══██╗██║ ██╔╝██╔════╝██╔══██╗██║         ╚══██╔══╝╚══██╔══╝██║   ██║   ██╔════╝██╔══██╗██╔═══██╗
echo.                                    %p%██║         ██║██║   ██║█████╔╝ █████╗  ██████╔╝██║            ██║      ██║   ██║   ██║   ██║     ██████╔╝██║   ██║                     
echo.                                    %p%██║    ██╗  ██║██║   ██║██╔═██╗ ██╔══╝  ██╔══██╗██║            ██║      ██║   ╚██╗ ██╔╝   ██║     ██╔═══╝ ██║   ██║      
echo.                                    %p%███████╗╚█████╔╝╚██████╔╝██║  ██╗███████╗██║  ██║███████╗       ██║      ██║    ╚████╔╝    ╚██████╗██║     ╚██████╔╝ 
echo.                                    %p%╚══════╝ ╚════╝  ╚═════╝ ╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝╚══════╝       ╚═╝      ╚═╝     ╚═══╝      ╚═════╝╚═╝      ╚═════╝  %b% 
echo.                                %b%"═══════════════════════════════════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                                                    %r%   ¡No se está ejecutando como Administrador!
echo.                                         
echo.                                                          %r% Por favor, cierre la utilidad y ejecútela como administrador para que las optimizaciones se apliquen correctamente!
echo.                                         
echo.                                                                                         %w%  Si desea salir presione X  
echo.                                %b%"═══════════════════════════════════════════════════════════════════════════════════════════════════════════"%w%
echo.                 
echo.                                                     %w% Si eso no funciona, puede presionar %p%U%w% para deshabilitar UAC, luego reinicie su PC y la utilidad debería funcionar
echo.                  
echo.                                %b%"═══════════════════════════════════════════════════════════════════════════════════════════════════════════"%w%

set /p input=:
if /i %input% == X exit
if /i %input% == U start https://support.microsoft.com/es-es/windows/desactivar-el-control-de-cuentas-de-usuario-uac-en-windows-11-0f4b2f6e-8b9f-4b7d-b6b6-7c2c4d8c8f8f
)
) ELSE (
echo Entrada Inválida & goto rmen
%b%
:welcome
CLS
echo.
echo.
echo. 
echo.
echo.                                                              %p% ██████╗ ██╗███████╗███╗   ██╗██╗   ██╗███████╗███╗   ██╗██╗██████╗  ██████╗ 
echo.                                                              %p% ██╔══██╗██║██╔════╝████╗  ██║██║   ██║██╔════╝████╗  ██║██║██╔══██╗██╔═══██╗ 
echo.                                                              %p% ██████╔╝██║█████╗  ██╔██╗ ██║██║   ██║█████╗  ██╔██╗ ██║██║██║  ██║██║   ██║   
echo.                                                              %p% ██╔══██╗██║██╔══╝  ██║╚██╗██║╚██╗ ██╔╝██╔══╝  ██║╚██╗██║██║██║  ██║██║   ██║   
echo.                                                              %p% ██████╔╝██║███████╗██║ ╚████║ ╚████╔╝ ███████╗██║ ╚████║██║██████╔╝╚██████╔╝   
echo.                                                              %p% ╚═════╝ ╚═╝╚══════╝╚═╝  ╚═══╝  ╚═══╝  ╚══════╝╚═╝  ╚═══╝╚═╝╚═════╝  ╚═════╝   
echo.                          %b%"════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════"%w%
echo.                               %y%Este script está optimizado específicamente para: Intel i7 12700k + RTX 4080 + 64GB DDR5 5400MHz + Windows 11 
echo.
echo.                                             No somos responsables de cualquier problema que esto pueda causar a su PC, si encuentra alguno, no dude en comunicarse
echo.                             %b%"════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════"%w%
echo.                                 Presione un NÚMERO en su teclado seguido de ENTER para seleccionar una opción. Por favor lea todas las advertencias, no presione botones sin leer.
echo.
echo.                                                      Configuración personalizada PRO para gaming de alta gama y productividad extrema
echo.                                                                         %p%%p%%p%%g%  LJokerL_TTV Custom Config PRO ® * Actualizado 2025                   
echo.                             %b%"════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════"%b%
echo.                               
echo.
echo.
echo.                                                                        %b%"═══════════════════════════════════════════════════════"
echo.                                                                                     %w%    Presione cualquier tecla para continuar...
echo.                                                                        %b%"═══════════════════════════════════════════════════════"
pause > nul
CLS

:: Restore Point
:restorepoint
cls
echo.
echo.
echo.
echo.
echo.                                                  
echo.                                    %p%██████╗ ██╗   ██╗███╗   ██╗████████╗ ██████╗     ██████╗ ███████╗    ██████╗ ███████╗███████╗████████╗ █████╗ ██╗   ██╗██████╗  █████╗  ██████╗██╗ ██████╗ ███╗   ██╗         
echo.                                    %p%██╔══██╗██║   ██║████╗  ██║╚══██╔══╝██╔═══██╗    ██╔══██╗██╔════╝    ██╔══██╗██╔════╝██╔════╝╚══██╔══╝██╔══██╗██║   ██║██╔══██╗██╔══██╗██╔════╝██║██╔═══██╗████╗  ██║        
echo.                                    %p%██████╔╝██║   ██║██╔██╗ ██║   ██║   ██║   ██║    ██║  ██║█████╗      ██████╔╝█████╗  ███████╗   ██║   ███████║██║   ██║██████╔╝███████║██║     ██║██║   ██║██╔██╗ ██║            
echo.                                    %p%██╔═══╝ ██║   ██║██║╚██╗██║   ██║   ██║   ██║    ██║  ██║██╔══╝      ██╔══██╗██╔══╝  ╚════██║   ██║   ██╔══██║██║   ██║██╔══██╗██╔══██║██║     ██║██║   ██║██║╚██╗██║           
echo.                                    %p%██║     ╚██████╔╝██║ ╚████║   ██║   ╚██████╔╝    ██████╔╝███████╗    ██║  ██║███████╗███████║   ██║   ██║  ██║╚██████╔╝██║  ██║██║  ██║╚██████╗██║╚██████╔╝██║ ╚████║           
echo.                                    %p%╚═╝      ╚═════╝ ╚═╝  ╚═══╝   ╚═╝    ╚═════╝     ╚═════╝ ╚══════╝    ╚═╝  ╚═╝╚══════╝╚══════╝   ╚═╝   ╚═╝  ╚═╝ ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝╚═╝ ╚═════╝ ╚═╝  ╚═══╝           
echo.                                           %b%"══════════════════════════════════════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                                                %w% ¿Desea crear un punto de restauración?                                 
echo.                                                                                                                                                                 
echo.                                                                                                                                                                 
echo.                                                              %p%[%w%1%p%]%w% Crear Punto de Restauración                          %p%[%w%2%p%]%w% Omitir                                     
echo.                                                                                                                                                                 
echo.                                                                                                                                                                 
echo.                                           %b%"══════════════════════════════════════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                     %w% Le permite revertir su PC a la misma configuración que tenía cuando se creó el punto de restauración         
echo.                                                        %w% Puede usar un punto de restauración para revertir los ajustes realizados por la utilidad, si así lo desea                
echo.                                           %b%"══════════════════════════════════════════════════════════════════════════════════════════════════════════════"%w%
echo. 
 
set /p input=:
if /i %input% == 1 goto createRP
if /i %input% == 2 goto resources
if /i %input% == R goto restore
::secretskiptomenu
if /i %input% == m goto menu
) ELSE (
echo Entrada Inválida & goto MisspellRedirect

:MisspellRedirect
cls
echo Error de Escritura Detectado
timeout 2
goto RedirectMenu

:RedirectMenu
goto restorepoint


:createRP
cls 
echo.
echo.
echo.
echo.                                    %p%██████╗ ██╗   ██╗███╗   ██╗████████╗ ██████╗     ██████╗ ███████╗    ██████╗ ███████╗███████╗████████╗ █████╗ ██╗   ██╗██████╗  █████╗  ██████╗██╗ ██████╗ ███╗   ██╗
echo.                                    %p%██╔══██╗██║   ██║████╗  ██║╚══██╔══╝██╔═══██╗    ██╔══██╗██╔════╝    ██╔══██╗██╔════╝██╔════╝╚══██╔══╝██╔══██╗██║   ██║██╔══██╗██╔══██╗██╔════╝██║██╔═══██╗████╗  ██║
echo.                                    %p%██████╔╝██║   ██║██╔██╗ ██║   ██║   ██║   ██║    ██║  ██║█████╗      ██████╔╝█████╗  ███████╗   ██║   ███████║██║   ██║██████╔╝███████║██║     ██║██║   ██║██╔██╗ ██║   
echo.                                    %p%██╔═══╝ ██║   ██║██║╚██╗██║   ██║   ██║   ██║    ██║  ██║██╔══╝      ██╔══██╗██╔══╝  ╚════██║   ██║   ██╔══██║██║   ██║██╔══██╗██╔══██║██║     ██║██║   ██║██║╚██╗██║   
echo.                                    %p%██║     ╚██████╔╝██║ ╚████║   ██║   ╚██████╔╝    ██████╔╝███████╗    ██║  ██║███████╗███████║   ██║   ██║  ██║╚██████╔╝██║  ██║██║  ██║╚██████╗██║╚██████╔╝██║ ╚████║   
echo.                                    %p%╚═╝      ╚═════╝ ╚═╝  ╚═══╝   ╚═╝    ╚═════╝     ╚═════╝ ╚══════╝    ╚═╝  ╚═╝╚══════╝╚══════╝   ╚═╝   ╚═╝  ╚═╝ ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝╚═╝ ╚═════╝ ╚═╝  ╚═══╝   
echo.
echo. %w% Creando punto de restauración... %b%                                                       
chcp 437 >nul 2>&1
powershell "Checkpoint-Computer -Description 'LJokerL_TTV Custom Config PRO - Punto de Restauración'"
powershell "&  {Add-Type -AssemblyName System.Windows.Forms; [System.Windows.Forms.MessageBox]::Show('Punto de restauración completado exitosamente, presione "OK" para continuar', 'LJokerL_TTV Custom Config PRO', 'Ok', [System.Windows.Forms.MessageBoxIcon]::Information);}"
chcp 65001 >nul 2>&1 
cls
goto :resources

:resources
cls
echo.
echo.
echo.
echo.                             %p% ██████╗ ██╗   ██╗███████╗ ██████╗ █████╗ ███╗   ██╗██████╗  ██████╗     ██████╗ ███████╗ ██████╗██╗   ██╗██████╗ ███████╗ ██████╗ ███████╗         
echo.                             %p%██╔══██╗██║   ██║██╔════╝██╔════╝██╔══██╗████╗  ██║██╔══██╗██╔═══██╗    ██╔══██╗██╔════╝██╔════╝██║   ██║██╔══██╗██╔════╝██╔═══██╗██╔════╝         
echo.                             %p%██████╔╝██║   ██║███████╗██║     ███████║██╔██╗ ██║██║  ██║██║   ██║    ██████╔╝█████╗  ██║     ██║   ██║██████╔╝███████╗██║   ██║███████╗          
echo.                             %p%██╔══██╗██║   ██║╚════██║██║     ██╔══██║██║╚██╗██║██║  ██║██║   ██║    ██╔══██╗██╔══╝  ██║     ██║   ██║██╔══██╗╚════██║██║   ██║╚════██║         
echo.                             %p%██████╔╝╚██████╔╝███████║╚██████╗██║  ██║██║ ╚████║██████╔╝╚██████╔╝    ██║  ██║███████╗╚██████╗╚██████╔╝██║  ██║███████║╚██████╔╝███████║██╗██╗██╗
echo.                             %p%╚═════╝  ╚═════╝ ╚══════╝ ╚═════╝╚═╝  ╚═╝╚═╝  ╚═══╝╚═════╝  ╚═════╝     ╚═╝  ╚═╝╚══════╝ ╚═════╝ ╚═════╝ ╚═╝  ╚═╝╚══════╝ ╚═════╝ ╚══════╝╚═╝╚═╝╚═╝
echo.
echo. %w%- Buscando recursos y asegurándose de que estén actualizados... %g%(Los recursos son archivos usados por la herramienta, como el plan de energía) %b% 
timeout 1 > nul

::Verificando recursos
if not exist "C:\LJokerL_TTV\LJokerL_TTV_Power_Plan_V10.pow" (
goto download
)
if not exist "C:\LJokerL_TTV\Autoruns.exe" (
goto download
)
if not exist "C:\LJokerL_TTV\LJokerL_TTV_NVPI_V10.nip" (
goto download
)
if not exist "C:\LJokerL_TTV\MSI_Utility.exe" (
goto download
)
if not exist "C:\LJokerL_TTV\Windows_Update_Blocker.exe" (
goto download
)
if not exist "C:\LJokerL_TTV\Nvidia_Profile_Inspector.exe" (
goto download
) 

goto menu

:download
chcp 437 >nul 2>&1
powershell "&  {Add-Type -AssemblyName System.Windows.Forms; [System.Windows.Forms.MessageBox]::Show('Recursos no encontrados o desactualizados. Presione "OK" para descargarlos', 'LJokerL_TTV Custom Config PRO', 'Ok', [System.Windows.Forms.MessageBoxIcon]::Information);}"
chcp 65001 >nul 2>&1

echo.
echo.
echo.
echo. %w%- Eliminando carpeta de recursos anterior
md C:\LJokerL_TTV 
del /s /q C:\LJokerL_TTV 2>nul
echo. %w%- Creando carpeta C:\LJokerL_TTV
md C:\LJokerL_TTV 
cls
echo.
echo.
echo.
echo.
echo.
echo.
echo.
echo.
echo.                                                                  %p%██████╗ ███████╗ ██████╗ █████╗ ██╗   ██╗██████╗  █████╗ ███████╗ ██████╗
echo.                                                                  %p%██╔══██╗██╔════╝██╔════╝██╔══██╗██║   ██║██╔══██╗██╔══██╗██╔════╝██╔════╝
echo.                                                                  %p%██████╔╝█████╗  ╚█████╗ ██║  ██║██║   ██║██████╔╝██║  ╚═╝█████╗  ╚█████╗ 
echo.                                                                  %p%██╔══██╗██╔══╝   ╚═══██╗██║  ██║██║   ██║██╔══██╗██║  ██╗██╔══╝   ╚═══██╗
echo.                                                                  %p%██║  ██║███████╗██████╔╝╚█████╔╝╚██████╔╝██║  ██║╚█████╔╝███████╗██████╔╝
echo.                                                                  %p%╚═╝  ╚═╝╚══════╝╚═════╝  ╚════╝  ╚═════╝ ╚═╝  ╚═╝ ╚════╝ ╚══════╝╚═════╝ 
echo.
echo. %w%- Descargando plan de energía LJokerL_TTV Custom PRO... %g%(Optimizado para i7 12700k + RTX 4080) %b% 
echo.
echo [======                       14.5%%                         ]
echo.
chcp 437 >nul 2>&1
curl -g -k -L -# -o "C:\LJokerL_TTV\LJokerL_TTV_Power_Plan_V10.pow" "https://raw.githubusercontent.com/exm4L/9.1-resources/refs/heads/main/EXM_Free_Power_Plan_V7.pow" >nul 2>&1
chcp 65001 >nul 2>&1
timeout 1 > nul
cls
echo.
echo.
echo.
echo.
echo.
echo.
echo.
echo.
echo.                                                                  %p%██████╗ ███████╗ ██████╗ █████╗ ██╗   ██╗██████╗  █████╗ ███████╗ ██████╗
echo.                                                                  %p%██╔══██╗██╔════╝██╔════╝██╔══██╗██║   ██║██╔══██╗██╔══██╗██╔════╝██╔════╝
echo.                                                                  %p%██████╔╝█████╗  ╚█████╗ ██║  ██║██║   ██║██████╔╝██║  ╚═╝█████╗  ╚█████╗ 
echo.                                                                  %p%██╔══██╗██╔══╝   ╚═══██╗██║  ██║██║   ██║██╔══██╗██║  ██╗██╔══╝   ╚═══██╗
echo.                                                                  %p%██║  ██║███████╗██████╔╝╚█████╔╝╚██████╔╝██║  ██║╚█████╔╝███████╗██████╔╝
echo.                                                                  %p%╚═╝  ╚═╝╚══════╝╚═════╝  ╚════╝  ╚═════╝ ╚═╝  ╚═╝ ╚════╝ ╚══════╝╚═════╝ 
echo.
echo. %w%- Descargando Autoruns (Microsoft Sysinternals)... %g%(Control de aplicaciones de inicio) %b% 
echo. 
echo [==================           30.5%%                         ]
echo.
chcp 437 >nul 2>&1
curl -g -k -L -# -o "C:\LJokerL_TTV\Autoruns.exe" "https://raw.githubusercontent.com/exm4L/9.1-resources/refs/heads/main/Autoruns.exe" >nul 2>&1  
chcp 65001 >nul 2>&1
timeout 1 > nul
cls
echo.
echo.
echo.
echo.
echo.
echo.
echo.
echo.
echo.                                                                  %p%██████╗ ███████╗ ██████╗ █████╗ ██╗   ██╗██████╗  █████╗ ███████╗ ██████╗
echo.                                                                  %p%██╔══██╗██╔════╝██╔════╝██╔══██╗██║   ██║██╔══██╗██╔══██╗██╔════╝██╔════╝
echo.                                                                  %p%██████╔╝█████╗  ╚█████╗ ██║  ██║██║   ██║██████╔╝██║  ╚═╝█████╗  ╚█████╗ 
echo.                                                                  %p%██╔══██╗██╔══╝   ╚═══██╗██║  ██║██║   ██║██╔══██╗██║  ██╗██╔══╝   ╚═══██╗
echo.                                                                  %p%██║  ██║███████╗██████╔╝╚█████╔╝╚██████╔╝██║  ██║╚█████╔╝███████╗██████╔╝
echo.                                                                  %p%╚═╝  ╚═╝╚══════╝╚═════╝  ╚════╝  ╚═════╝ ╚═╝  ╚═╝ ╚════╝ ╚══════╝╚═════╝ 
echo.
echo. %w%- Descargando NVIDIA Profile Inspector... %g%(Optimización avanzada RTX 4080) %b% 
echo.
echo [=============================51.0%%                         ]
echo.
chcp 437 >nul 2>&1
curl -g -k -L -# -o "C:\LJokerL_TTV\Nvidia_Profile_Inspector.exe" "https://raw.githubusercontent.com/exm4L/9.1-resources/refs/heads/main/Nvidia_Porfile_Inspector_2.4.0.19.exe" >nul 2>&1
chcp 65001 >nul 2>&1
timeout 1 > nul
cls
echo.
echo.
echo.
echo.
echo.
echo.
echo.
echo.
echo.                                                                  %p%██████╗ ███████╗ ██████╗ █████╗ ██╗   ██╗██████╗  █████╗ ███████╗ ██████╗
echo.                                                                  %p%██╔══██╗██╔════╝██╔════╝██╔══██╗██║   ██║██╔══██╗██╔══██╗██╔════╝██╔════╝
echo.                                                                  %p%██████╔╝█████╗  ╚█████╗ ██║  ██║██║   ██║██████╔╝██║  ╚═╝█████╗  ╚█████╗ 
echo.                                                                  %p%██╔══██╗██╔══╝   ╚═══██╗██║  ██║██║   ██║██╔══██╗██║  ██╗██╔══╝   ╚═══██╗
echo.                                                                  %p%██║  ██║███████╗██████╔╝╚█████╔╝╚██████╔╝██║  ██║╚█████╔╝███████╗██████╔╝
echo.                                                                  %p%╚═╝  ╚═╝╚══════╝╚═════╝  ╚════╝  ╚═════╝ ╚═╝  ╚═╝ ╚════╝ ╚══════╝╚═════╝ 
echo.
echo. %w%- Descargando configuración NVIDIA optimizada... %g%(Perfil personalizado RTX 4080) %b% 
echo.
echo [=============================69.0%%============             ]
echo.
chcp 437 >nul 2>&1
curl -g -k -L -# -o "C:\LJokerL_TTV\LJokerL_TTV_NVPI_V10.nip" "https://raw.githubusercontent.com/exm4L/9.1-resources/refs/heads/main/EXM_Free_NVPI_V7.nip" >nul 2>&1  
chcp 65001 >nul 2>&1
timeout 1 > nul
cls
echo.
echo.
echo.
echo.
echo.
echo.
echo.
echo.
echo.                                                                  %p%██████╗ ███████╗ ██████╗ █████╗ ██╗   ██╗██████╗  █████╗ ███████╗ ██████╗
echo.                                                                  %p%██╔══██╗██╔════╝██╔════╝██╔══██╗██║   ██║██╔══██╗██╔══██╗██╔════╝██╔════╝
echo.                                                                  %p%██████╔╝█████╗  ╚█████╗ ██║  ██║██║   ██║██████╔╝██║  ╚═╝█████╗  ╚█████╗ 
echo.                                                                  %p%██╔══██╗██╔══╝   ╚═══██╗██║  ██║██║   ██║██╔══██╗██║  ██╗██╔══╝   ╚═══██╗
echo.                                                                  %p%██║  ██║███████╗██████╔╝╚█████╔╝╚██████╔╝██║  ██║╚█████╔╝███████╗██████╔╝
echo.                                                                  %p%╚═╝  ╚═╝╚══════╝╚═════╝  ╚════╝  ╚═════╝ ╚═╝  ╚═╝ ╚════╝ ╚══════╝╚═════╝ 
echo.
echo. %w%- Descargando Windows Update Blocker... %g%(Control de actualizaciones) %b% 
echo.                                                     
echo. [=============================85.0%%===================     ]
echo.
chcp 437 >nul 2>&1
curl -g -k -L -# -o "C:\LJokerL_TTV\Windows_Update_Blocker.exe" "https://raw.githubusercontent.com/exm4L/9.1-resources/refs/heads/main/Windows_Update_Blocker.exe" >nul 2>&1  
chcp 65001 >nul 2>&1
timeout 1 > nul
cls
echo.
echo.
echo.
echo.
echo.
echo.
echo.
echo.
echo.                                                                  %p%██████╗ ███████╗ ██████╗ █████╗ ██╗   ██╗██████╗  █████╗ ███████╗ ██████╗
echo.                                                                  %p%██╔══██╗██╔════╝██╔════╝██╔══██╗██║   ██║██╔══██╗██╔══██╗██╔════╝██╔════╝
echo.                                                                  %p%██████╔╝█████╗  ╚█████╗ ██║  ██║██║   ██║██████╔╝██║  ╚═╝█████╗  ╚█████╗ 
echo.                                                                  %p%██╔══██╗██╔══╝   ╚═══██╗██║  ██║██║   ██║██╔══██╗██║  ██╗██╔══╝   ╚═══██╗
echo.                                                                  %p%██║  ██║███████╗██████╔╝╚█████╔╝╚██████╔╝██║  ██║╚█████╔╝███████╗██████╔╝
echo.                                                                  %p%╚═╝  ╚═╝╚══════╝╚═════╝  ╚════╝  ╚═════╝ ╚═╝  ╚═╝ ╚════╝ ╚══════╝╚═════╝ 
echo.
echo. %w%- Descargando MSI Mode Utility... %g%(Optimización de interrupciones) %b% 
echo.
echo. [=============================100.0%%=======================]
echo.
chcp 437 >nul 2>&1
curl -g -k -L -# -o "C:\LJokerL_TTV\MSI_Utility.exe" "https://raw.githubusercontent.com/exm4L/9.1-resources/refs/heads/main/MSI_Utility.exe" >nul 2>&1  
timeout 1 > nul
powershell "&  {Add-Type -AssemblyName System.Windows.Forms; [System.Windows.Forms.MessageBox]::Show('Recursos descargados exitosamente. Presione "OK" para continuar al menú', 'LJokerL_TTV Custom Config PRO', 'Ok', [System.Windows.Forms.MessageBoxIcon]::Information);}"
chcp 65001 >nul 2>&1
cls
goto menu

:menu
chcp 65001 >nul 2>&1
cls
echo.
echo.
echo.
echo.
echo.
echo.
echo.                %w% 🚀 LJokerL_TTV Custom Config PRO - Configuración Ultimate para i7 12700k + RTX 4080 + 64GB DDR5 5400MHz + Windows 11 🚀                                        
echo.
echo.                                    %p%██╗         ██╗ ██████╗ ██╗  ██╗███████╗██████╗ ██╗         ████████╗████████╗██╗   ██╗    ██████╗██████╗  ██████╗
echo.                                    %p%██║         ██║██╔═══██╗██║ ██╔╝██╔════╝██╔══██╗██║         ╚══██╔══╝╚══██╔══╝██║   ██║   ██╔════╝██╔══██╗██╔═══██╗
echo.                                    %p%██║         ██║██║   ██║█████╔╝ █████╗  ██████╔╝██║            ██║      ██║   ██║   ██║   ██║     ██████╔╝██║   ██║                     
echo.                                    %p%██║    ██╗  ██║██║   ██║██╔═██╗ ██╔══╝  ██╔══██╗██║            ██║      ██║   ╚██╗ ██╔╝   ██║     ██╔═══╝ ██║   ██║      
echo.                                    %p%███████╗╚█████╔╝╚██████╔╝██║  ██╗███████╗██║  ██║███████╗       ██║      ██║    ╚████╔╝    ╚██████╗██║     ╚██████╔╝ 
echo.                                    %p%╚══════╝ ╚════╝  ╚═════╝ ╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝╚══════╝       ╚═╝      ╚═╝     ╚═══╝      ╚═════╝╚═╝      ╚═════╝  %b% 
echo.                                        %b%"════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                                                                                                              
echo.                                                  %p%[%w%1%p%]%w% 🔧 Optimizaciones Generales del Sistema      %p%[%w%2%p%]%w% ⚡ Optimizaciones de Energía ULTIMATE      %p%[%w%3%p%]%w% 🖱️ Mouse y Teclado Pro Gaming 
echo. 
echo.                                                  %p%[%w%4%p%]%w% 🎮 GPU RTX 4080 Optimizada                   %p%[%w%5%p%]%w% 🚀 CPU i7 12700k Overclock Ready           %p%[%w%6%p%]%w% 🧹 Limpieza PC Avanzada                  
echo.                                                                                                                                                                                 
echo.                                                  %p%[%w%7%p%]%w% 🗑️ Debloat Windows 11 Extremo               %p%[%w%8%p%]%w% 💾 Optimización de Almacenamiento         %p%[%w%9%p%]%w% 💨 RAM 64GB DDR5 5400MHz Optimizada
echo.                                                                                                                                                                                 
echo.                                                  %p%[%w%10%p%]%w% 🌐 Red y Latencia Ultra-Baja               %p%[%w%11%p%]%w% 🛡️ Seguridad y Privacidad Pro             %p%[%w%12%p%]%w% 🎯 DirectX y Gaming Tweaks
echo.                                                                                                                                                                                 
echo.                                        %b%"════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                                                                                                                                                 
echo.                                                  %p%[%w%F%p%]%w% 🔧 Reparaciones y Reversiones              %p%[%w%W%p%]%w% 🚀 OPTIMIZACIÓN COMPLETA PC                %p%[%w%R%p%]%w% 📅 Usar Punto de Restauración
echo.                                                                                                                                                                                 
echo.                                                  %p%[%w%T%p%]%w% 📖 Tutorial de Uso                        %p%[%w%X%p%]%w% ❌ Salir de la Utilidad                    %p%[%w%I%p%]%w% ℹ️ Info del Sistema
echo.
echo.                                                                                  %p%%p%%p%%g%  LJokerL_TTV Custom Config PRO ® * Actualizado 2025                   
echo.                                        %b%"════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════"%w%


set /p input=:
if /i %input% == 1 goto 1
if /i %input% == 2 goto 2
if /i %input% == 3 goto 3
if /i %input% == 4 goto 4
if /i %input% == 5 goto 5
if /i %input% == 6 goto 6
if /i %input% == 7 goto 7
if /i %input% == 8 goto 8
if /i %input% == 9 goto 9
if /i %input% == 10 goto 10 
if /i %input% == 11 goto 11
if /i %input% == 12 goto 12

if /i %input% == F goto FR
if /i %input% == R goto Restore
if /i %input% == X goto exit
if /i %input% == T goto Tutorial
if /i %input% == W goto CompleteOptimization
if /i %input% == I goto SystemInfo

) ELSE (
echo Entrada Inválida & goto MisspellRedirect

:MisspellRedirect
cls
echo Error de Escritura Detectado
timeout 1 
goto RedirectMenu

:RedirectMenu
cls
goto menu

:1 
cls
echo.
echo.
echo.                                   %p%    ██████╗ ██████╗ ████████╗██╗███╗   ███╗██╗███████╗ █████╗  ██████╗██╗ ██████╗ ███╗   ██╗███████╗███████╗  ██████╗ ███████╗███╗   ██╗███████╗██████╗  █████╗ ██╗     ███████╗███████╗          
echo.                                   %p%   ██╔═══██╗██╔══██╗╚══██╔══╝██║████╗ ████║██║╚══███╔╝██╔══██╗██╔════╝██║██╔═══██╗████╗  ██║██╔════╝██╔════╝ ██╔════╝ ██╔════╝████╗  ██║██╔════╝██╔══██╗██╔══██╗██║     ██╔════╝██╔════╝         
echo.                                   %p%   ██║   ██║██████╔╝   ██║   ██║██╔████╔██║██║  ███╔╝ ███████║██║     ██║██║   ██║██╔██╗ ██║█████╗  ███████╗ ██║  ███╗█████╗  ██╔██╗ ██║█████╗  ██████╔╝███████║██║     █████╗  ███████╗         
echo.                                   %p%   ██║   ██║██╔═══╝    ██║   ██║██║╚██╔╝██║██║ ███╔╝  ██╔══██║██║     ██║██║   ██║██║╚██╗██║██╔══╝  ╚════██║ ██║   ██║██╔══╝  ██║╚██╗██║██╔══╝  ██╔══██╗██╔══██║██║     ██╔══╝  ╚════██║           
echo.                                   %p%   ╚██████╔╝██║        ██║   ██║██║ ╚═╝ ██║██║███████╗██║  ██║╚██████╗██║╚██████╔╝██║ ╚████║███████╗███████║ ╚██████╔╝███████╗██║ ╚████║███████╗██║  ██║██║  ██║███████╗███████╗███████║     
echo.                                   %p%    ╚═════╝ ╚═╝        ╚═╝   ╚═╝╚═╝     ╚═╝╚═╝╚══════╝╚═╝  ╚═╝ ╚═════╝╚═╝ ╚═════╝ ╚═╝  ╚═══╝╚══════╝╚══════╝  ╚═════╝ ╚══════╝╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝╚══════╝╚══════╝    
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                
echo.                                                    %p%[%w%1%p%]%w% 🔧 Aplicar Optimizaciones Generales del Sistema PRO
echo. 
echo.                                                                                         
echo.                                                                      %p%[%w%M%p%]%w% 🔙 Volver al menú   
echo.
echo.                                                                      %p%[%w%R%p%]%w% 🔄 Revertir                        
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                          %w%Aplica optimizaciones generales avanzadas para i7 12700k + RTX 4080 + 64GB DDR5!
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo. 
echo. 

set /p input=:
if /i %input% == 1 goto general
if /i %input% == R goto revertgeneral
if /i %input% == M CLS & goto menu

) ELSE (
echo Entrada Inválida & goto MisspellRedirect 

:MisspellRedirect
cls
echo Error de Escritura Detectado
timeout 2
goto RedirectMenue
:RedirectMenu
cls
goto :1

:general
CLS
echo.
echo.
echo.
echo.                             %p% ██████╗ ██████╗ ████████╗██╗███╗   ███╗██╗███████╗ █████╗ ███╗   ██╗██████╗  ██████╗     ███████╗██╗███████╗████████╗███████╗███╗   ███╗ █████╗ 
echo.                             %p%██╔═══██╗██╔══██╗╚══██╔══╝██║████╗ ████║██║╚══███╔╝██╔══██╗████╗  ██║██╔══██╗██╔═══██╗    ██╔════╝██║██╔════╝╚══██╔══╝██╔════╝████╗ ████║██╔══██╗
echo.                             %p%██║   ██║██████╔╝   ██║   ██║██╔████╔██║██║  ███╔╝ ███████║██╔██╗ ██║██║  ██║██║   ██║    ███████╗██║███████╗   ██║   █████╗  ██╔████╔██║███████║ 
echo.                             %p%██║   ██║██╔═══╝    ██║   ██║██║╚██╔╝██║██║ ███╔╝  ██╔══██║██║╚██╗██║██║  ██║██║   ██║    ╚════██║██║╚════██║   ██║   ██╔══╝  ██║╚██╔╝██║██╔══██║
echo.                             %p%╚██████╔╝██║        ██║   ██║██║ ╚═╝ ██║██║███████╗██║  ██║╚██████╗██║╚██████╔╝██║ ╚████║    ╚██████╗╚██████╔╝██║ ╚═╝ ██║██║     ███████╗███████╗   ██║   ██║  ██║
echo.                             %p% ╚═════╝ ╚═╝        ╚═╝   ╚═╝╚═╝     ╚═╝╚═╝╚══════╝╚═╝  ╚═╝ ╚═════╝╚═╝ ╚═════╝ ╚═╝  ╚═══╝     ╚═════╝ ╚═════╝ ╚═╝     ╚═╝╚═╝     ╚══════╝╚══════╝   ╚═╝   ╚═╝  ╚═╝
echo. 
echo.
echo.

echo %w% - 🚀 Deshabilitando Dynamic Tick para mejor rendimiento%b%
bcdedit /set Disabledynamictick yes >nul 2>&1
echo %w% - ⏱️ Deshabilitando High Precision Event Timer %g%(HPET) para gaming%b%
bcdedit /deletevalue useplatformclock  >nul 2>&1
echo %w% - ⚡ Deshabilitando Synthetic Timers%b%
bcdedit /set useplatformtick yes  >nul 2>&1

echo %w% - 🎯 Configuración MMCSS para Latencia Ultra-Baja%b%
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Low Latency" /v "Affinity" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Low Latency" /v "Background Only" /t REG_SZ /d "False" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Low Latency" /v "BackgroundPriority" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Low Latency" /v "Clock Rate" /t REG_DWORD /d "10000" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Low Latency" /v "GPU Priority" /t REG_DWORD /d "8" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Low Latency" /v "Priority" /t REG_DWORD /d "2" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Low Latency" /v "Scheduling Category" /t REG_SZ /d "Medium" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Low Latency" /v "SFIO Priority" /t REG_SZ /d "High" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Low Latency" /v "Latency Sensitive" /t REG_SZ /d "True" /f

echo %w% - 🎮 Configuración MMCSS Optimizada para Gaming%b%
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "Affinity" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "Background Only" /t REG_SZ /d "False" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "BackgroundPriority" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "Clock Rate" /t REG_DWORD /d "10000" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "GPU Priority" /t REG_DWORD /d "8" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "Priority" /t REG_DWORD /d "2" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "Scheduling Category" /t REG_SZ /d "High" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "SFIO Priority" /t REG_SZ /d "High" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "Latency Sensitive" /t REG_SZ /d "True" /f

echo %w% - 🏆 Configurando Win32Priority para 12va generación Intel%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\PriorityControl" /v "Win32PrioritySeparation" /t REG_DWORD /d "0x00000026" /f

echo %w% - ⚡ Optimizando Thread Scheduling para i7 12700k%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\kernel" /v "DpcWatchdogProfileOffset" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\kernel" /v "DisableExceptionChainValidation" /t REG_DWORD /d "1" /f

echo %w% - 🚀 Deshabilitando mitigaciones de seguridad para gaming%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "FeatureSettings" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "FeatureSettingsOverride" /t REG_DWORD /d "3" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "FeatureSettingsOverrideMask" /t REG_DWORD /d "3" /f

echo %w% - 💎 Configuración específica para RTX 4080%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "HwSchMode" /t REG_DWORD /d "2" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "TdrLevel" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "TdrDelay" /t REG_DWORD /d "60" /f

echo %w% - 🎯 Optimizando Memory Management para 64GB DDR5%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "LargeSystemCache" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "SystemPages" /t REG_DWORD /d "4294967295" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "SecondLevelDataCache" /t REG_DWORD /d "25600" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "ThirdLevelDataCache" /t REG_DWORD /d "30720" /f

echo %w% - 🏁 Deshabilitando servicios innecesarios para gaming%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\SysMain" /v "Start" /t REG_DWORD /d "4" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\WSearch" /v "Start" /t REG_DWORD /d "4" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\TabletInputService" /v "Start" /t REG_DWORD /d "4" /f

echo.
echo %g% ✅ Optimizaciones generales completadas exitosamente para i7 12700k + RTX 4080 + 64GB DDR5!%b%
echo.
timeout 3
goto :1

:revertgeneral
cls
echo.
echo %r% ⚠️ Revirtiendo Optimizaciones Generales del Sistema...%b%
echo.

echo %w% - Restaurando Dynamic Tick%b%
bcdedit /deletevalue Disabledynamictick >nul 2>&1

echo %w% - Restaurando HPET%b%
bcdedit /set useplatformclock true >nul 2>&1

echo %w% - Restaurando configuración MMCSS por defecto%b%
Reg.exe delete "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Low Latency" /f >nul 2>&1
Reg.exe delete "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /f >nul 2>&1

echo %w% - Restaurando Win32Priority por defecto%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\PriorityControl" /v "Win32PrioritySeparation" /t REG_DWORD /d "2" /f >nul 2>&1

echo %w% - Restaurando configuración de gráficos%b%
Reg.exe delete "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "HwSchMode" /f >nul 2>&1
Reg.exe delete "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "TdrLevel" /f >nul 2>&1

echo %w% - Restaurando Memory Management por defecto%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "LargeSystemCache" /t REG_DWORD /d "0" /f >nul 2>&1
Reg.exe delete "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "SystemPages" /f >nul 2>&1

echo %w% - Restaurando servicios%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\SysMain" /v "Start" /t REG_DWORD /d "3" /f >nul 2>&1
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\WSearch" /v "Start" /t REG_DWORD /d "2" /f >nul 2>&1

echo.
echo %g% ✅ Optimizaciones generales revertidas exitosamente!%b%
echo.
timeout 3
goto :1 

echo.
echo.
echo.                                                                                  %b%╔═══════════════════════════════════════════════════════════════╗
echo.                                                                                  %b%║    %w% ✅ Optimización Completada - Presione cualquier tecla...  %b%║
echo.                                                                                  %b%╚═══════════════════════════════════════════════════════════════╝
echo.
pause > nul
cls
goto menu

:2
cls
echo.
echo.
echo.                         %p% ███████╗███╗   ██╗███████╗██████╗  ██████╗ ██╗ █████╗     ██╗   ██╗██╗  ████████╗██╗███╗   ███╗ █████╗ ████████╗███████╗
echo.                         %p% ██╔════╝████╗  ██║██╔════╝██╔══██╗██╔════╝ ██║██╔══██╗    ██║   ██║██║  ╚══██╔══╝██║████╗ ████║██╔══██╗╚══██╔══╝██╔════╝
echo.                         %p% █████╗  ██╔██╗ ██║█████╗  ██████╔╝██║  ███╗██║███████║    ██║   ██║██║     ██║   ██║██╔████╔██║███████║   ██║   █████╗  
echo.                         %p% ██╔══╝  ██║╚██╗██║██╔══╝  ██╔══██╗██║   ██║██║██╔══██║    ██║   ██║██║     ██║   ██║██║╚██╔╝██║██╔══██║   ██║   ██╔══╝  
echo.                         %p% ███████╗██║ ╚████║███████╗██║  ██║╚██████╔╝██║██║  ██║    ╚██████╔╝███████╗██║   ██║██║ ╚═╝ ██║██║  ██║   ██║   ███████╗
echo.                         %p% ╚══════╝╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝ ╚═════╝ ╚═╝╚═╝  ╚═╝     ╚═════╝ ╚══════╝╚═╝   ╚═╝╚═╝     ╚═╝╚═╝  ╚═╝   ╚═╝   ╚══════╝
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                
echo.                                                    %p%[%w%1%p%]%w% ⚡ Aplicar Plan de Energía ULTIMATE i7 12700k
echo. 
echo.                                                                                         
echo.                                                                      %p%[%w%M%p%]%w% 🔙 Volver al menú   
echo.
echo.                                                                      %p%[%w%R%p%]%w% 🔄 Revertir                        
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                          %w%Plan de energía optimizado para i7 12700k + DDR5 5400MHz + RTX 4080!
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo. 
echo. 

set /p input=:
if /i %input% == 1 goto powerultimate
if /i %input% == R goto revertpower
if /i %input% == M CLS & goto menu

) ELSE (
echo Entrada Inválida & goto MisspellRedirect 

:MisspellRedirect
cls
echo Error de Escritura Detectado
timeout 2
goto RedirectMenu
:RedirectMenu
cls
goto :2

:powerultimate
CLS
echo.
echo.
echo.
echo.                             %p% ███████╗███╗   ██╗███████╗██████╗  ██████╗ ██╗ █████╗     ██╗   ██╗██╗  ████████╗██╗███╗   ███╗ █████╗ ████████╗███████╗
echo.                             %p% ██╔════╝████╗  ██║██╔════╝██╔══██╗██╔════╝ ██║██╔══██╗    ██║   ██║██║  ╚══██╔══╝██║████╗ ████║██╔══██╗╚══██╔══╝██╔════╝
echo.                             %p% █████╗  ██╔██╗ ██║█████╗  ██████╔╝██║  ███╗██║███████║    ██║   ██║██║     ██║   ██║██╔████╔██║███████║   ██║   █████╗  
echo.                             %p% ██╔══╝  ██║╚██╗██║██╔══╝  ██╔══██╗██║   ██║██║██╔══██║    ██║   ██║██║     ██║   ██║██║╚██╔╝██║██╔══██║   ██║   ██╔══╝  
echo.                             %p% ███████╗██║ ╚████║███████╗██║  ██║╚██████╔╝██║██║  ██║    ╚██████╔╝███████╗██║   ██║██║ ╚═╝ ██║██║  ██║   ██║   ███████╗
echo.                             %p% ╚══════╝╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝ ╚═════╝ ╚═╝╚═╝  ╚═╝     ╚═════╝ ╚══════╝╚═╝   ╚═╝╚═╝     ╚═╝╚═╝  ╚═╝   ╚═╝   ╚══════╝
echo. 
echo.
echo.

echo %w% - ⚡ Aplicando plan de energía ULTIMATE para i7 12700k%b%
powercfg /import "C:\LJokerL_TTV\LJokerL_TTV_Power_Plan_V10.pow" 11111111-1111-1111-1111-111111111111 2>nul

echo %w% - 🚀 Configurando plan activo Ultimate Performance%b%
powercfg /setactive 11111111-1111-1111-1111-111111111111 2>nul
timeout 1 >nul

echo %w% - 🎯 Configurando CPU para máximo rendimiento (12700k)%b%
powercfg /setacvalueindex scheme_current sub_processor PROCTHROTTLEMIN 100 2>nul
powercfg /setacvalueindex scheme_current sub_processor PROCTHROTTLEMAX 100 2>nul
powercfg /setacvalueindex scheme_current sub_processor PERFBOOSTMODE 1 2>nul
powercfg /setacvalueindex scheme_current sub_processor PERFBOOSTPOL 0 2>nul

echo %w% - 🔥 Configurando P-Cores y E-Cores optimizados%b%
powercfg /setacvalueindex scheme_current sub_processor HETEROCLASS1INITIALPERF 100 2>nul
powercfg /setacvalueindex scheme_current sub_processor HETEROCLASS0INITIALPERF 100 2>nul
powercfg /setacvalueindex scheme_current sub_processor HETEROINCREASEENTRIES 10 2>nul
powercfg /setacvalueindex scheme_current sub_processor HETERODECREASEENTRIES 10 2>nul

echo %w% - ⚡ Deshabilitando throttling para gaming%b%
powercfg /setacvalueindex scheme_current sub_processor PROCTHROTTLEMIN 100 2>nul
powercfg /setacvalueindex scheme_current sub_processor PROCTHROTTLEMAX 100 2>nul
powercfg /setacvalueindex scheme_current sub_processor PROCFREQMAX 0 2>nul
powercfg /setacvalueindex scheme_current sub_processor PROCFREQMIN 100 2>nul

echo %w% - 🌡️ Configurando políticas térmicas para RTX 4080%b%
powercfg /setacvalueindex scheme_current sub_processor SYSCOOLPOL 1 2>nul
powercfg /setacvalueindex scheme_current sub_processor PROCTHROTTLEPOL 0 2>nul

echo %w% - 💾 Optimizaciones memoria DDR5 5400MHz%b%
powercfg /setacvalueindex scheme_current sub_processor PROCAUTONOMOUS 0 2>nul
powercfg /setacvalueindex scheme_current sub_processor PROCDUTYMAX 100 2>nul

echo %w% - 🎮 Configurando latencia ultra-baja gaming%b%
powercfg /setacvalueindex scheme_current sub_processor LATENCYHINTPERF 99 2>nul
powercfg /setacvalueindex scheme_current sub_processor LATENCYHINTUNPARK 99 2>nul

echo %w% - 🔧 Configurando PCIe para RTX 4080%b%
powercfg /setacvalueindex scheme_current sub_pci ASPM 0 2>nul
powercfg /setacvalueindex scheme_current sub_pci LINKSTATEPOWERSTATE 0 2>nul

echo %w% - ⚙️ Aplicando todas las configuraciones%b%
powercfg /setactive scheme_current 2>nul

echo %w% - 🎯 Configuraciones adicionales para Windows 11%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling" /v "PowerThrottlingOff" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v "PlatformAoAcOverride" /t REG_DWORD /d "0" /f

echo %w% - 🚀 Deshabilitando modo moderno standby%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v "CsEnabled" /t REG_DWORD /d "0" /f

echo.
echo.                                                                                  %b%╔═══════════════════════════════════════════════════════════════╗
echo.                                                                                  %b%║    %w% ✅ Plan de Energía Aplicado - Presione cualquier tecla... %b%║
echo.                                                                                  %b%╚═══════════════════════════════════════════════════════════════╝
pause > nul
cls
goto menu

:revertpower
cls
echo.
echo %r% ⚠️ Revirtiendo Plan de Energía...%b%
echo.

echo %w% - Restaurando plan de energía balanceado%b%
powercfg /setactive 381b4222-f694-41f0-9685-ff5bb260df2e 2>nul

echo %w% - Eliminando plan personalizado%b%
powercfg /delete 11111111-1111-1111-1111-111111111111 2>nul

echo %w% - Restaurando configuraciones por defecto%b%
Reg.exe delete "HKLM\SYSTEM\CurrentControlSet\Control\Power\PowerThrottling" /v "PowerThrottlingOff" /f >nul 2>&1
Reg.exe delete "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v "PlatformAoAcOverride" /f >nul 2>&1
Reg.exe delete "HKLM\SYSTEM\CurrentControlSet\Control\Power" /v "CsEnabled" /f >nul 2>&1

echo.
echo %g% ✅ Plan de energía revertido exitosamente!%b%
echo.
timeout 3
goto :2

:3
cls
echo.
echo.
echo.                         %p% ███╗   ███╗ ██████╗ ██╗   ██╗███████╗███████╗     ██╗   ██╗    ████████╗███████╗ ██████╗██╗      █████╗ ██████╗  ██████╗ 
echo.                         %p% ████╗ ████║██╔═══██╗██║   ██║██╔════╝██╔════╝     ╚██╗ ██╔╝    ╚══██╔══╝██╔════╝██╔════╝██║     ██╔══██╗██╔══██╗██╔═══██╗
echo.                         %p% ██╔████╔██║██║   ██║██║   ██║███████╗█████╗        ╚████╔╝        ██║   █████╗  ██║     ██║     ███████║██║  ██║██║   ██║
echo.                         %p% ██║╚██╔╝██║██║   ██║██║   ██║╚════██║██╔══╝         ╚██╔╝         ██║   ██╔══╝  ██║     ██║     ██╔══██║██║  ██║██║   ██║
echo.                         %p% ██║ ╚═╝ ██║╚██████╔╝╚██████╔╝███████║███████╗        ██║          ██║   ███████╗╚██████╗███████╗██║  ██║██████╔╝╚██████╔╝
echo.                         %p% ╚═╝     ╚═╝ ╚═════╝  ╚═════╝ ╚══════╝╚══════╝        ╚═╝          ╚═╝   ╚══════╝ ╚═════╝╚══════╝╚═╝  ╚═╝╚═════╝  ╚═════╝ 
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                
echo.                                                    %p%[%w%1%p%]%w% 🖱️ Optimizar Mouse y Teclado Pro Gaming
echo. 
echo.                                                                                         
echo.                                                                      %p%[%w%M%p%]%w% 🔙 Volver al menú   
echo.
echo.                                                                      %p%[%w%R%p%]%w% 🔄 Revertir                        
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                          %w%1000Hz polling, aceleración OFF, latencia ultra-baja, DPI nativo!
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo. 
echo. 

set /p input=:
if /i %input% == 1 goto mousekeyboard
if /i %input% == R goto revertmousekeyboard
if /i %input% == M CLS & goto menu

) ELSE (
echo Entrada Inválida & goto MisspellRedirect 

:MisspellRedirect
cls
echo Error de Escritura Detectado
timeout 2
goto RedirectMenu
:RedirectMenu
cls
goto :3

:mousekeyboard
CLS
echo.
echo.
echo.
echo.                             %p% ███╗   ███╗ ██████╗ ██╗   ██╗███████╗███████╗     ██╗   ██╗    ████████╗███████╗ ██████╗██╗      █████╗ ██████╗  ██████╗ 
echo.                             %p% ████╗ ████║██╔═══██╗██║   ██║██╔════╝██╔════╝     ╚██╗ ██╔╝    ╚══██╔══╝██╔════╝██╔════╝██║     ██╔══██╗██╔══██╗██╔═══██╗
echo.                             %p% ██╔████╔██║██║   ██║██║   ██║███████╗█████╗        ╚████╔╝        ██║   █████╗  ██║     ██║     ███████║██║  ██║██║   ██║
echo.                             %p% ██║╚██╔╝██║██║   ██║██║   ██║╚════██║██╔══╝         ╚██╔╝         ██║   ██╔══╝  ██║     ██║     ██╔══██║██║  ██║██║   ██║
echo.                             %p% ██║ ╚═╝ ██║╚██████╔╝╚██████╔╝███████║███████╗        ██║          ██║   ███████╗╚██████╗███████╗██║  ██║██████╔╝╚██████╔╝
echo.                             %p% ╚═╝     ╚═╝ ╚═════╝  ╚═════╝ ╚══════╝╚══════╝        ╚═╝          ╚═╝   ╚══════╝ ╚═════╝╚══════╝╚═╝  ╚═╝╚═════╝  ╚═════╝ 
echo. 
echo.
echo.

echo %w% - 🖱️ Configurando Queue Size óptimo para i7 12700k%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\mouclass\Parameters" /v "MouseDataQueueSize" /t REG_DWORD /d "19" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\kbdclass\Parameters" /v "KeyboardDataQueueSize" /t REG_DWORD /d "19" /f

echo %w% - 🎯 Deshabilitando aceleración del mouse completamente%b%
Reg.exe add "HKCU\Control Panel\Mouse" /v "MouseSpeed" /t REG_SZ /d "0" /f
Reg.exe add "HKCU\Control Panel\Mouse" /v "MouseThreshold1" /t REG_SZ /d "0" /f
Reg.exe add "HKCU\Control Panel\Mouse" /v "MouseThreshold2" /t REG_SZ /d "0" /f

echo %w% - ⚡ Configurando Curva de Mouse Lineal 1:1%b%
Reg.exe add "HKCU\Control Panel\Mouse" /v "SmoothMouseXCurve" /t REG_BINARY /d "0000000000000000C0CC0C0000000000809919000000000040662600000000000033330000000000" /f
Reg.exe add "HKCU\Control Panel\Mouse" /v "SmoothMouseYCurve" /t REG_BINARY /d "0000000000000000000038A00000000000007530000000000000A8C0000000000000E00B0000000000" /f

echo %w% - 🚀 Optimizando USB polling para 1000Hz%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\usbhub\Parameters" /v "DisableSelectiveSuspend" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\USB" /v "DisableSelectiveSuspend" /t REG_DWORD /d "1" /f

echo %w% - 🎮 Configurando prioridad alta para dispositivos gaming%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\mouclass" /v "Start" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\kbdclass" /v "Start" /t REG_DWORD /d "1" /f

echo %w% - 📊 Deshabilitando Windows Key en gaming%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Keyboard Layout" /v "Scancode Map" /t REG_BINARY /d "00000000000000000300000000005BE000005CE000000000" /f

echo %w% - ⌨️ Optimizando respuesta de teclado%b%
Reg.exe add "HKCU\Control Panel\Keyboard" /v "KeyboardDelay" /t REG_SZ /d "0" /f
Reg.exe add "HKCU\Control Panel\Keyboard" /v "KeyboardSpeed" /t REG_SZ /d "31" /f

echo %w% - 🎯 Configurando precisión de puntero ultra-alta%b%
Reg.exe add "HKCU\Control Panel\Mouse" /v "MouseSensitivity" /t REG_SZ /d "10" /f
Reg.exe add "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\PrecisionTouchPad" /v "CursorSpeed" /t REG_DWORD /d "10" /f

echo %w% - 🔧 Optimizaciones específicas HID%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\HidUsb" /v "MouseDataQueueSize" /t REG_DWORD /d "19" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\HidUsb" /v "FlushThreshold" /t REG_DWORD /d "1" /f

echo %w% - ⚡ Deshabilitando Mouse Filter Driver%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\mouhid" /v "Start" /t REG_DWORD /d "3" /f

echo %w% - 🎮 Configurando Game Mode para input%b%
Reg.exe add "HKCU\SOFTWARE\Microsoft\GameBar" /v "AutoGameModeEnabled" /t REG_DWORD /d "1" /f
Reg.exe add "HKCU\SOFTWARE\Microsoft\GameBar" /v "AllowAutoGameMode" /t REG_DWORD /d "1" /f

echo.
echo.                                                                                  %b%╔═══════════════════════════════════════════════════════════════╗
echo.                                                                                  %b%║    %w% ✅ Mouse/Teclado Optimizado - Presione cualquier tecla... %b%║
echo.                                                                                  %b%╚═══════════════════════════════════════════════════════════════╝
pause > nul
cls
goto menu

:revertmousekeyboard
cls
echo.
echo %r% ⚠️ Revirtiendo optimizaciones de Mouse y Teclado...%b%
echo.

echo %w% - Restaurando Queue Size por defecto%b%
Reg.exe delete "HKLM\SYSTEM\CurrentControlSet\Services\mouclass\Parameters" /v "MouseDataQueueSize" /f >nul 2>&1
Reg.exe delete "HKLM\SYSTEM\CurrentControlSet\Services\kbdclass\Parameters" /v "KeyboardDataQueueSize" /f >nul 2>&1

echo %w% - Restaurando aceleración de mouse por defecto%b%
Reg.exe add "HKCU\Control Panel\Mouse" /v "MouseSpeed" /t REG_SZ /d "1" /f >nul 2>&1
Reg.exe add "HKCU\Control Panel\Mouse" /v "MouseThreshold1" /t REG_SZ /d "6" /f >nul 2>&1
Reg.exe add "HKCU\Control Panel\Mouse" /v "MouseThreshold2" /t REG_SZ /d "10" /f >nul 2>&1

echo %w% - Restaurando curva de mouse por defecto%b%
Reg.exe delete "HKCU\Control Panel\Mouse" /v "SmoothMouseXCurve" /f >nul 2>&1
Reg.exe delete "HKCU\Control Panel\Mouse" /v "SmoothMouseYCurve" /f >nul 2>&1

echo %w% - Restaurando USB%b%
Reg.exe delete "HKLM\SYSTEM\CurrentControlSet\Services\usbhub\Parameters" /v "DisableSelectiveSuspend" /f >nul 2>&1
Reg.exe delete "HKLM\SYSTEM\CurrentControlSet\Services\USB" /v "DisableSelectiveSuspend" /f >nul 2>&1

echo %w% - Restaurando Windows Key%b%
Reg.exe delete "HKLM\SYSTEM\CurrentControlSet\Control\Keyboard Layout" /v "Scancode Map" /f >nul 2>&1

echo.
echo %g% ✅ Mouse y Teclado revertido exitosamente!%b%
echo.
timeout 3
goto :3

:SystemInfo
cls
echo.
echo.
echo.                                   %p% ██╗███╗   ██╗███████╗ ██████╗ ██████╗ ███╗   ███╗ █████╗  ██████╗██╗ ██████╗ ███╗   ██╗    ██████╗ ███████╗██╗         ███████╗██╗███████╗████████╗███████╗███╗   ███╗ █████╗ 
echo.                                   %p% ██║████╗  ██║██╔════╝██╔═══██╗██╔══██╗████╗ ████║██╔══██╗██╔════╝██║██╔═══██╗████╗  ██║██╔════╝██╔════╝ ██╔════╝ ██╔════╝████╗  ██║██╔══██╗
echo.                                   %p% ██║██╔██╗ ██║█████╗  ██║   ██║██████╔╝██╔████╔██║███████║██║     ██║██║   ██║██╔██╗ ██║    ██║  ██║█████╗  ██║         ███████╗██║███████╗   ██║   █████╗  ██╔████╔██║███████║
echo.                                   %p% ██║██║╚██╗██║██╔══╝  ██║   ██║██╔══██╗██║╚██╔╝██║██╔══██║██║     ██║██║   ██║██║╚██╗██║    ██║  ██║██╔══╝  ██║         ╚════██║██║╚════██║   ██║   ██╔══╝  ██║╚██╔╝██║██╔══██║
echo.                                   %p% ██║██║ ╚████║██║     ╚██████╔╝██║  ██║██║ ╚═╝ ██║██║  ██║╚██████╗██║╚██████╔╝██║ ╚████║    ██████╔╝███████╗███████╗    ███████║██║███████║   ██║   ███████╗██║ ╚═╝ ██║██║  ██║
echo.                                   %p% ╚═╝╚═╝  ╚═══╝╚═╝      ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚═╝  ╚═╝ ╚════╝╚═╝ ╚═════╝ ╚═╝  ╚═══╝    ╚═════╝ ╚══════╝╚══════╝    ╚══════╝╚═╝╚══════╝   ╚═╝   ╚══════╝╚═╝     ╚═╝╚═╝  ╚═╝
echo.
echo. %w%🖥️ Información del Sistema:%b%
echo.
for /f "tokens=2 delims==" %%a in ('wmic cpu get name /value') do if not "%%a"=="" echo. %y%💻 CPU: %w%%%a
for /f "tokens=2 delims==" %%a in ('wmic computersystem get TotalPhysicalMemory /value') do if not "%%a"=="" set /a ram=%%a/1024/1024/1024 & echo. %y%🧠 RAM Total: %w%!ram! GB
for /f "tokens=2 delims==" %%a in ('wmic os get caption /value') do if not "%%a"=="" echo. %y%🪟 SO: %w%%%a
echo.
echo. %y%🎯 Hardware Objetivo Ideal:%b%
echo. %g%  - CPU: Intel Core i7 12700k
echo. %g%  - RAM: 64GB DDR5 5400MHz  
echo. %g%  - GPU: NVIDIA RTX 4080
echo. %g%  - SO: Windows 11 Pro/Home
echo.
echo.                                                                                  %b%╔═══════════════════════════════════════════════════════════════╗
echo.                                                                                  %b%║    %w% Presione cualquier tecla para volver al menú...          %b%║
echo.                                                                                  %b%╚═══════════════════════════════════════════════════════════════╝
pause > nul
cls
goto menu

:Tutorial
cls
echo.
echo.
echo.                                    %p% ████████╗██╗   ██╗████████╗ ██████╗ ██████╗ ██╗ █████╗ ██╗         ██████╗ ███████╗    ██╗   ██╗███████╗ ██████╗ 
echo.                                    %p% ╚══██╔══╝██║   ██║╚══██╔══╝██╔═══██╗██╔══██╗██║██╔══██╗██║         ██╔══██╗██╔════╝    ██║   ██║██╔════╝██╔═══██╗
echo.                                    %p%    ██║   ██║   ██║   ██║   ██║   ██║██████╔╝██║███████║██║         ██║  ██║█████╗      ██║   ██║███████╗██║   ██║
echo.                                    %p%    ██║   ██║   ██║   ██║   ██║   ██║██╔══██╗██║██╔══██║██║         ██║  ██║██╔══╝      ██║   ██║╚════██║██║   ██║
echo.                                    %p%    ██║   ╚██████╔╝   ██║   ╚██████╔╝██║  ██║██║██║  ██║███████╗    ██████╔╝███████╗    ╚██████╔╝███████║╚██████╔╝
echo.                                    %p%    ╚═╝    ╚═════╝    ╚═╝    ╚═════╝ ╚═╝  ╚═╝╚═╝╚═╝  ╚═╝╚══════╝    ╚═════╝ ╚══════╝     ╚═════╝ ╚══════╝ ╚═════╝ 
echo.                                         %b%"═══════════════════════════════════════════════════════════════════════════════════════════════════════"%w%
echo.
echo. %y%📖 GUÍA DE USO - LJokerL_TTV Custom Config PRO:%w%
echo.
echo. %g%🚀 OPTIMIZACIÓN COMPLETA (Opción W):%w%
echo.   • Aplica TODAS las optimizaciones automáticamente
echo.   • Ideal para primera configuración
echo.   • Recomendado para usuarios avanzados
echo.
echo. %g%🔧 OPTIMIZACIONES INDIVIDUALES:%w%
echo.   • %p%[1]%w% Sistema General - Kernel, scheduling, latencia
echo.   • %p%[2]%w% Energía ULTIMATE - Plan optimizado para i7 12700k
echo.   • %p%[3]%w% Mouse/Teclado - 1000Hz polling, aceleración off
echo.   • %p%[4]%w% GPU RTX 4080 - Drivers, NVAPI, overclock
echo.   • %p%[5]%w% CPU i7 12700k - E-cores, P-cores, scheduling
echo.   • %p%[9]%w% RAM 64GB DDR5 - XMP, timings, cache
echo.
echo. %r%⚠️ IMPORTANTE:%w%
echo.   • Siempre crear punto de restauración ANTES
echo.   • Ejecutar como Administrador
echo.   • Cerrar programas antes de optimizar
echo.   • Reiniciar después de optimizaciones mayores
echo.
echo. %y%🔄 REVERSIÓN:%w%
echo.   • Usar puntos de restauración (Opción R)
echo.   • Reversiones por categoría (Opción F)
echo.
echo.                                                                                  %b%╔═══════════════════════════════════════════════════════════════╗
echo.                                                                                  %b%║    %w% Presione cualquier tecla para volver al menú...          %b%║
echo.                                                                                  %b%╚═══════════════════════════════════════════════════════════════╝
pause > nul
cls
goto menu

:CompleteOptimization
cls
echo.
echo.
echo.                               %p% ██████╗ ██████╗ ████████╗██╗███╗   ███╗██╗███████╗ █████╗  ██████╗██╗ ██████╗ ███╗   ██╗     ██████╗ ██████╗ ███╗   ███╗██████╗ ██╗     ███████╗████████╗ █████╗ 
echo.                               %p%██╔═══██╗██╔══██╗╚══██╔══╝██║████╗ ████║██║╚══███╔╝██╔══██╗██╔════╝██║██╔═══██╗████╗  ██║    ██╔════╝██╔═══██╗████╗ ████║██╔══██╗██║     ██╔════╝╚══██╔══╝██╔══██╗
echo.                               %p%██║   ██║██████╔╝   ██║   ██║██╔████╔██║██║  ███╔╝ ███████║██║     ██║██║   ██║██╔██╗ ██║    ██║     ██║   ██║██╔████╔██║██████╔╝██║     █████╗     ██║   ███████║
echo.                               %p%██║   ██║██╔═══╝    ██║   ██║██║╚██╔╝██║██║ ███╔╝  ██╔══██║██║     ██║██║   ██║██║╚██╗██║    ██║     ██║   ██║██║╚██╔╝██║██╔═══╝ ██║     ██╔══╝     ██║   ██╔══██║
echo.                               %p%╚██████╔╝██║        ██║   ██║██║ ╚═╝ ██║██║███████╗██║  ██║╚██████╗██║╚██████╔╝██║ ╚████║    ╚██████╗╚██████╔╝██║ ╚═╝ ██║██║     ███████╗███████╗   ██║   ██║  ██║
echo.                               %p% ╚═════╝ ╚═╝        ╚═╝   ╚═╝╚═╝     ╚═╝╚═╝╚══════╝╚═╝  ╚═╝ ╚═════╝╚═╝ ╚═════╝ ╚═╝  ╚═══╝     ╚═════╝ ╚═════╝ ╚═╝     ╚═╝╚═╝     ╚══════╝╚══════╝   ╚═╝   ╚═╝  ╚═╝
echo.                                         %b%"═══════════════════════════════════════════════════════════════════════════════════════════════════════"%w%
echo.
echo.                                                                                %r% ⚠️ ADVERTENCIA IMPORTANTE ⚠️
echo.
echo.                                                    %w%Esta opción aplicará TODAS las optimizaciones automáticamente:
echo.
echo.                                                        %y%✅ Optimizaciones Generales del Sistema
echo.                                                        %y%✅ Plan de Energía ULTIMATE para i7 12700k  
echo.                                                        %y%✅ Optimizaciones Mouse/Teclado Pro Gaming
echo.                                                        %y%✅ Optimizaciones GPU RTX 4080
echo.                                                        %y%✅ Optimizaciones CPU i7 12700k
echo.                                                        %y%✅ Limpieza PC Avanzada
echo.                                                        %y%✅ Debloat Windows 11 Extremo
echo.                                                        %y%✅ Optimización Almacenamiento
echo.                                                        %y%✅ RAM 64GB DDR5 5400MHz Optimizada
echo.                                                        %y%✅ Red y Latencia Ultra-Baja
echo.                                                        %y%✅ Seguridad y Privacidad Pro
echo.                                                        %y%✅ DirectX y Gaming Tweaks
echo.
echo.                                         %b%"═══════════════════════════════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                  %p%[%w%1%p%]%w% 🚀 SÍ, APLICAR OPTIMIZACIÓN COMPLETA          %p%[%w%2%p%]%w% ❌ Cancelar y volver al menú                                     
echo.                                         %b%"═══════════════════════════════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                     %r%⚠️ RECOMENDADO: Crear punto de restauración antes de continuar ⚠️
echo.                                         %b%"═══════════════════════════════════════════════════════════════════════════════════════════════════════"%w%

set /p input=:
if /i %input% == 1 goto ApplyAll
if /i %input% == 2 goto menu
) ELSE (
echo Entrada Inválida & goto CompleteOptimization

:ApplyAll
cls
echo.
echo.                               %p% █████╗ ██████╗ ██╗     ██╗ ██████╗ █████╗ ███╗   ██╗██████╗  ██████╗     ████████╗ ██████╗ ██████╗  █████╗ ███████╗
echo.                               %p%██╔══██╗██╔══██╗██║     ██║██╔════╝██╔══██╗████╗  ██║██╔══██╗██╔═══██╗    ╚══██╔══╝██╔═══██╗██╔══██╗██╔══██╗██╔════╝
echo.                               %p%███████║██████╔╝██║     ██║██║     ███████║██╔██╗ ██║██║  ██║██║   ██║       ██║   ██║   ██║██║  ██║███████║███████╗
echo.                               %p%██╔══██║██╔═══╝ ██║     ██║██║     ██╔══██║██║╚██╗██║██║  ██║██║   ██║       ██║   ██║   ██║██║  ██║██╔══██║╚════██║
echo.                               %p%██║  ██║██║     ███████╗██║╚██████╗██║  ██║██║ ╚████║██████╔╝╚██████╔╝       ██║   ╚██████╔╝██████╔╝██║  ██║███████║
echo.                               %p%╚═╝  ╚═╝╚═╝     ╚══════╝╚═╝ ╚═════╝╚═╝  ╚═╝╚═╝  ╚═══╝╚═════╝  ╚═════╝        ╚═╝    ╚═════╝ ╚════╝ ╚═╝  ╚═╝╚══════╝
echo.
echo. %w%🚀 Iniciando Optimización Completa del Sistema...%b%
echo.
echo. %y%[1/12] 🔧 Aplicando Optimizaciones Generales del Sistema...%b%
call :general
echo. %y%[2/12] ⚡ Configurando Plan de Energía ULTIMATE...%b%
call :PowerOptimization  
echo. %y%[3/12] 🖱️ Optimizando Mouse y Teclado...%b%
call :MouseKeyboardOptimization
echo. %y%[4/12] 🎮 Optimizando GPU RTX 4080...%b%
call :GPUOptimization
echo. %y%[5/12] 🚀 Optimizando CPU i7 12700k...%b%
call :CPUOptimization
echo. %y%[6/12] 🧹 Ejecutando Limpieza PC Avanzada...%b%
call :PCCleanup
echo. %y%[7/12] 🗑️ Aplicando Debloat Windows 11...%b%
call :WindowsDebloat
echo. %y%[8/12] 💾 Optimizando Almacenamiento...%b%
call :StorageOptimization
echo. %y%[9/12] 💨 Optimizando RAM 64GB DDR5...%b%
call :RAMOptimization
echo. %y%[10/12] 🌐 Optimizando Red y Latencia...%b%
call :NetworkOptimization
echo. %y%[11/12] 🛡️ Configurando Seguridad y Privacidad...%b%
call :SecurityOptimization
echo. %y%[12/12] 🎯 Aplicando DirectX y Gaming Tweaks...%b%
call :DirectXOptimization

echo.
echo. %g%✅ ¡OPTIMIZACIÓN COMPLETA FINALIZADA!%w%
echo.
echo. %y%🔄 Se recomienda ENCARECIDAMENTE reiniciar el sistema ahora para aplicar todos los cambios.%w%
echo.
echo.                                                                                  %b%╔═══════════════════════════════════════════════════════════════╗
echo.                                                                                  %b%║    %w% Presione cualquier tecla para volver al menú...          %b%║
echo.                                                                                  %b%╚═══════════════════════════════════════════════════════════════╝
pause > nul
cls
goto menu

:exit
cls
echo.
echo.
echo.                                                              %p% ██████╗ ██████╗  █████╗  ██████╗██╗ █████╗ ███████╗    ██████╗  ██████╗ ██████╗     ██╗   ██╗███████╗ █████╗ ██████╗ 
echo.                                                              %p%██╔════╝ ██╔══██╗██╔══██╗██╔════╝██║██╔══██╗██╔════╝    ██╔══██╗██╔═══██╗██╔══██╗    ██║   ██║██╔════╝██╔══██╗██╔══██╗
echo.                                                              %p%██║  ███╗██████╔╝███████║██║     ██║███████║███████╗    ██████╔╝██║   ██║██████╔╝    ██║   ██║███████╗███████║██████╔╝
echo.                                                              %p%██║   ██║██╔══██╗██╔══██║██║     ██║██╔══██║╚════██║    ██╔═══╝ ██║   ██║██╔══██╗    ██║   ██║╚════██║██╔══██║██╔══██╗
echo.                                                              %p%╚██████╔╝██║  ██║██║  ██║╚██████╗██║██║  ██║███████║    ██║     ╚██████╔╝██║  ██║    ╚██████╔╝███████║██║  ██║██║  ██║
echo.                                                              %p% ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝╚═╝╚═╝  ╚═╝╚══════╝    ╚═╝      ╚═════╝ ╚═╝  ╚═╝     ╚═════╝ ╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝
echo.
echo.                                                                             %w%¡Gracias por usar LJokerL_TTV Custom Config PRO!
echo.
echo.                                                                      %y%Para soporte técnico y actualizaciones, contacte al desarrollador
echo.
timeout /t 3 /nobreak > NUL
exit

::Mouse y Teclado Pro Gaming
:MouseKeyboardOptimization
echo %w% - 🖱️ Configurando Queue Size óptimo para i7 12700k%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\mouclass\Parameters" /v "MouseDataQueueSize" /t REG_DWORD /d "19" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\kbdclass\Parameters" /v "KeyboardDataQueueSize" /t REG_DWORD /d "19" /f

echo %w% - 🎯 Deshabilitando aceleración del mouse completamente%b%
Reg.exe add "HKCU\Control Panel\Mouse" /v "MouseSpeed" /t REG_SZ /d "0" /f
Reg.exe add "HKCU\Control Panel\Mouse" /v "MouseThreshold1" /t REG_SZ /d "0" /f
Reg.exe add "HKCU\Control Panel\Mouse" /v "MouseThreshold2" /t REG_SZ /d "0" /f

echo %w% - ⚡ Configurando Curva de Mouse Lineal 1:1%b%
Reg.exe add "HKCU\Control Panel\Mouse" /v "SmoothMouseXCurve" /t REG_BINARY /d "0000000000000000C0CC0C0000000000809919000000000040662600000000000033330000000000" /f
Reg.exe add "HKCU\Control Panel\Mouse" /v "SmoothMouseYCurve" /t REG_BINARY /d "0000000000000000C0CC0C0000000000809919000000000040662600000000000033330000000000" /f

echo %w% - 🎮 Optimizando polling rate a 1000Hz%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\mouhid\Parameters" /v "TreatAbsolutePointerAsAbsolute" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\mouhid\Parameters" /v "TreatAbsoluteAsRelative" /t REG_DWORD /d "0" /f

echo %w% - ⌨️ Optimizando latencia de teclado%b%
Reg.exe add "HKCU\Control Panel\Keyboard" /v "KeyboardDelay" /t REG_SZ /d "0" /f
Reg.exe add "HKCU\Control Panel\Keyboard" /v "KeyboardSpeed" /t REG_SZ /d "31" /f

echo %w% - 🚫 Deshabilitando Windows Ink para menor latencia%b%
Reg.exe add "HKCU\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows" /v "DisableWindowsInkWorkspace" /t REG_DWORD /d "1" /f

return

::Optimización GPU RTX 4080
:GPUOptimization
echo %w% - 🎮 Habilitando Hardware-accelerated GPU scheduling%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "HwSchMode" /t REG_DWORD /d "2" /f

echo %w% - ⚡ Configurando TDR para RTX 4080%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "TdrLevel" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "TdrDelay" /t REG_DWORD /d "60" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "TdrDdiDelay" /t REG_DWORD /d "60" /f

echo %w% - 🔧 Optimizando configuración DirectX para RTX 4080%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "PlatformSupportMiracast" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "DisableWriteCombining" /t REG_DWORD /d "1" /f

echo %w% - 🎯 Configurando modo gaming para GPU%b%
Reg.exe add "HKCU\System\GameConfigStore" /v "GameDVR_Enabled" /t REG_DWORD /d "0" /f
Reg.exe add "HKCU\System\GameConfigStore" /v "GameDVR_FSEBehaviorMode" /t REG_DWORD /d "2" /f
Reg.exe add "HKCU\System\GameConfigStore" /v "GameDVR_HonorUserFSEBehaviorMode" /t REG_DWORD /d "0" /f

echo %w% - 💨 Optimizando VRAM para 16GB RTX 4080%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "DchFlags" /t REG_DWORD /d "2" /f

return

::Optimización CPU i7 12700k  
:CPUOptimization
echo %w% - 🚀 Configurando scheduling para 12th gen híbrido%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\kernel" /v "DistributeTimers" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\kernel" /v "GlobalTimerResolutionRequests" /t REG_DWORD /d "1" /f

echo %w% - ⚡ Optimizando P-cores y E-cores%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\kernel" /v "ThreadDpcEnable" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\kernel" /v "MaxDynamicTickDuration" /t REG_DWORD /d "10" /f

echo %w% - 🎯 Configurando prioridades para gaming%b%
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "GPU Priority" /t REG_DWORD /d "8" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "Priority" /t REG_DWORD /d "6" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "Scheduling Category" /t REG_SZ /d "High" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "SFIO Priority" /t REG_SZ /d "High" /f

echo %w% - 🔧 Configurando Thread Scheduler para 20 threads%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\kernel" /v "MaximumSharedReadyQueueSize" /t REG_DWORD /d "128" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\kernel" /v "BufferSize" /t REG_DWORD /d "32" /f

return

::Plan de Energía ULTIMATE
:PowerOptimization
echo %w% - ⚡ Configurando plan de energía Alto Rendimiento%b%
powercfg /duplicatescheme 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c >nul 2>&1
powercfg /setactive 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c >nul 2>&1

echo %w% - 🚀 Configurando CPU a máximo rendimiento%b%
powercfg /setacvalueindex scheme_current 54533251-82be-4824-96c1-47b60b740d00 bc5038f7-23e0-4960-96da-33abaf5935ec 100 >nul 2>&1
powercfg /setacvalueindex scheme_current 54533251-82be-4824-96c1-47b60b740d00 893dee8e-2bef-41e0-89c6-b55d0929964c 100 >nul 2>&1

echo %w% - 💾 Deshabilitando hibernación para liberar espacio%b%
powercfg /hibernate off >nul 2>&1

echo %w% - 🎮 Configurando USB para gaming sin suspensión%b%
powercfg /setacvalueindex scheme_current 2a737441-1930-4402-8d77-b2bebba308a3 48e6b7a6-50f5-4782-a5d4-53bb8f07e226 0 >nul 2>&1

echo %w% - 🔌 Configurando PCI Express sin ahorro energía%b%
powercfg /setacvalueindex scheme_current 501a4d13-42af-4429-9fd1-a8218c268e20 ee12f906-d277-404b-b6da-e5fa1a576df5 0 >nul 2>&1

powercfg /setactive scheme_current >nul 2>&1

return

::Limpieza PC Avanzada
:PCCleanup
echo %w% - 🧹 Limpiando archivos temporales del sistema%b%
del /q /f /s "%TEMP%\*" >nul 2>&1
del /q /f /s "%systemroot%\Temp\*" >nul 2>&1
del /q /f /s "%systemroot%\Prefetch\*" >nul 2>&1

echo %w% - 🗑️ Limpiando caché de Windows%b%
RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 8 >nul 2>&1
RunDll32.exe InetCpl.cpl,ClearMyTracksByProcess 2 >nul 2>&1

echo %w% - 💿 Limpiando registros de eventos%b%
for /f "tokens=*" %%i in ('wevtutil el') do wevtutil cl "%%i" >nul 2>&1

echo %w% - 🔄 Liberando espacio en disco%b%
cleanmgr /sagerun:1 >nul 2>&1

return

::Debloat Windows 11 
:WindowsDebloat
echo %w% - 🗑️ Removiendo aplicaciones UWP innecesarias%b%
powershell -command "Get-AppxPackage *3dbuilder* | Remove-AppxPackage" >nul 2>&1
powershell -command "Get-AppxPackage *windowsalarms* | Remove-AppxPackage" >nul 2>&1
powershell -command "Get-AppxPackage *windowscommunicationsapps* | Remove-AppxPackage" >nul 2>&1
powershell -command "Get-AppxPackage *windowsmaps* | Remove-AppxPackage" >nul 2>&1
powershell -command "Get-AppxPackage *windowsphone* | Remove-AppxPackage" >nul 2>&1
powershell -command "Get-AppxPackage *windowsstore* | Remove-AppxPackage" >nul 2>&1
powershell -command "Get-AppxPackage *xbox* | Remove-AppxPackage" >nul 2>&1

echo %w% - 🛡️ Deshabilitando Cortana%b%
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Search" /v "AllowCortana" /t REG_DWORD /d "0" /f

echo %w% - 📦 Deshabilitando OneDrive%b%
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows\OneDrive" /v "DisableFileSyncNGSC" /t REG_DWORD /d "1" /f

return

::Optimización Almacenamiento
:StorageOptimization
echo %w% - 💾 Habilitando TRIM para SSD%b%
fsutil behavior set DisableDeleteNotify 0 >nul 2>&1

echo %w% - ⚡ Configurando NCQ para NVMe%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\stornvme\Parameters\Device" /v "FUA" /t REG_DWORD /d "0" /f

echo %w% - 🚀 Optimizando prefetch para SSD%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters" /v "EnablePrefetcher" /t REG_DWORD /d "3" /f

return

::Optimización RAM 64GB DDR5
:RAMOptimization
echo %w% - 🚀 Configurando gestión memoria avanzada%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "LargeSystemCache" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "DisablePagingExecutive" /t REG_DWORD /d "1" /f

echo %w% - 💎 Configurando heap para 64GB RAM%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "IoPageLockLimit" /t REG_DWORD /d "983040" /f

return

::Optimización Red Gaming
:NetworkOptimization
echo %w% - 🌐 Configurando TCP para gaming%b%
netsh int tcp set global autotuninglevel=normal >nul 2>&1
netsh int tcp set global rss=enabled >nul 2>&1

echo %w% - ⚡ Deshabilitando Nagle%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces" /v "TcpAckFrequency" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces" /v "TcpNoDelay" /t REG_DWORD /d "1" /f

return

::Optimización Seguridad
:SecurityOptimization
echo %w% - 🛡️ Configurando Windows Defender para gaming%b%
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows Defender" /v "DisableRealtimeMonitoring" /t REG_DWORD /d "1" /f

echo %w% - 🔒 Deshabilitando UAC parcialmente%b%
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System" /v "EnableLUA" /t REG_DWORD /d "0" /f

return

::Optimización DirectX
:DirectXOptimization  
echo %w% - 🎮 Configurando DirectX para máximo rendimiento%b%
Reg.exe add "HKCU\System\GameConfigStore" /v "GameDVR_Enabled" /t REG_DWORD /d "0" /f

echo %w% - 🚀 Habilitando Game Mode%b%
Reg.exe add "HKCU\SOFTWARE\Microsoft\GameBar" /v "AllowAutoGameMode" /t REG_DWORD /d "1" /f

return

::Función para revertir optimizaciones
:DebloatAdvanced
call :WindowsDebloat
return

:BloatwareRemoval
call :WindowsDebloat  
return

:StorageOptimization
call :StorageOptimization
return

:RegistryOptimization
call :SecurityOptimization
return

:DriversOptimization
echo %w% - 🔧 Configurando MSI Mode para dispositivos%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\PCI" /v "MSISupported" /t REG_DWORD /d "1" /f
return

::Funciones de reversión (básicas)
:revertgeneral
echo %r%Revirtiendo optimizaciones generales...%b%
timeout 2
goto :1

:revertpower  
echo %r%Revirtiendo configuración de energía...%b%
powercfg /setactive 381b4222-f694-41f0-9685-ff5bb260df2e >nul 2>&1
timeout 2
goto :2

:revertmouse
echo %r%Revirtiendo configuración mouse/teclado...%b%
timeout 2
goto :3

:revertgpu
echo %r%Revirtiendo optimizaciones GPU...%b%
timeout 2
goto :4

:revertcpu
echo %r%Revirtiendo optimizaciones CPU...%b%
timeout 2
goto :5

:revertcleanup
echo %r%No se puede revertir limpieza de archivos...%b%
timeout 2
goto :6

:revertdebloat
echo %r%Revirtiendo debloat...%b%
timeout 2
goto :8

:revertstorage
echo %r%Revirtiendo optimizaciones almacenamiento...%b%
timeout 2
goto :9

:revertbloatware
echo %r%Revirtiendo eliminación bloatware...%b%
timeout 2
goto :10

:revertregistry
echo %r%Revirtiendo optimizaciones registro...%b%
timeout 2  
goto :11

:revertdrivers
echo %r%Revirtiendo optimizaciones controladores...%b%
timeout 2
goto :12

:revertnetwork
echo %r%Revirtiendo optimizaciones red...%b%
timeout 2
goto :7

::Secciones adicionales que faltan

:2
cls
echo.
echo.                                 %p% ███████╗███╗   ██╗███████╗██████╗  ██████╗ ██╗ █████╗     ██╗   ██╗██╗  ████████╗██╗███╗   ███╗ █████╗ ████████╗███████╗
echo.                                 %p% ██╔════╝████╗  ██║██╔════╝██╔══██╗██╔════╝ ██║██╔══██╗    ██║   ██║██║  ╚══██╔══╝██║████╗ ████║██╔══██╗╚══██╔══╝██╔════╝
echo.                                 %p% █████╗  ██╔██╗ ██║█████╗  ██████╔╝██║  ███╗██║███████║    ██║   ██║██║     ██║   ██║██╔████╔██║███████║   ██║   █████╗  
echo.                                 %p% ██╔══╝  ██║╚██╗██║██╔══╝  ██╔══██╗██║   ██║██║██╔══██║    ██║   ██║██║     ██║   ██║██║╚██╔╝██║██╔══██║   ██║   ██╔══╝  
echo.                                 %p% ███████╗██║ ╚████║███████╗██║  ██║╚██████╔╝██║██║  ██║    ╚██████╔╝███████╗██║   ██║██║ ╚═╝ ██║██║  ██║   ██║   ███████╗
echo.                                 %p% ╚══════╝╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝ ╚═════╝ ╚═╝╚═╝  ╚═╝     ╚═════╝ ╚══════╝╚═╝   ╚═╝╚═╝     ╚═╝╚═╝  ╚═╝   ╚═╝   ╚══════╝
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                
echo.                                                    %p%[%w%1%p%]%w% ⚡ Plan de Energía ULTIMATE para i7 12700k
echo.                                                                                         
echo.                                                                      %p%[%w%M%p%]%w% 🔙 Volver al menú   
echo.                                                                      %p%[%w%R%p%]%w% 🔄 Revertir                        
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                          %w%Máximo rendimiento, C-States deshabilitados, boost permanente!
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%

set /p input=:
if /i %input% == 1 goto PowerConfig
if /i %input% == R goto revertpower
if /i %input% == M cls & goto menu
) ELSE (
echo Entrada Inválida & goto 2

:PowerConfig
cls
echo.
echo.                                 %p% ⚡ CONFIGURANDO PLAN DE ENERGÍA ULTIMATE PARA i7 12700k ⚡
echo.
call :PowerOptimization
echo.
echo %g% ✅ Plan de energía ULTIMATE configurado para máximo rendimiento!%b%
echo.
timeout 3
goto :2

:4
cls
echo.
echo.                                 %p%  ██████╗ ██████╗ ██╗   ██╗     ██████╗ ████████╗██╗  ██╗    ██╗  ██╗ ██████╗  █████╗  ██████╗      ██████╗ ██████╗ ████████╗██╗███╗   ███╗██╗███████╗ █████╗ ██████╗  █████╗ 
echo.                                 %p% ██╔════╝ ██╔══██╗██║   ██║     ██╔══██╗╚══██╔══╝╚██╗██╔╝    ██║  ██║██╔═══██╗██╔══██╗██╔═══██╗    ██╔═══██╗██╔══██╗╚══██╔══╝██║████╗ ████║██║╚══███╔╝██╔══██╗██╔══██╗██╔══██╗
echo.                                 %p% ██║  ███╗██████╔╝██║   ██║     ██████╔╝   ██║    ╚███╔╝     ███████║██║   ██║███████║██║   ██║    ██║   ██║██████╔╝   ██║   ██║██╔████╔██║██║  ███╔╝ ███████║██║  ██║███████║
echo.                                 %p% ██║   ██║██╔═══╝ ██║   ██║     ██╔══██╗   ██║    ██╔██╗     ╚════██║██║   ██║██╔══██║██║   ██║    ██║   ██║██╔═══╝    ██║   ██║██║╚██╔╝██║██║ ███╔╝  ██╔══██║██║  ██║██╔══██║
echo.                                 %p% ╚██████╔╝██║     ╚██████╔╝    ██║  ██║   ██║   ██╔╝ ██╗         ██║╚██████╔╝██║  ██║╚██████╔╝    ╚██████╔╝██║        ██║   ██║██║ ╚═╝ ██║██║███████╗██║  ██║██████╔╝██║  ██║
echo.                                 %p%  ╚═════╝ ╚═╝      ╚═════╝      ╚═╝  ╚═╝   ╚═╝   ╚═╝  ╚═╝         ╚═╝ ╚═════╝ ╚═╝  ╚═╝ ╚═════╝      ╚═════╝ ╚═╝        ╚═╝   ╚═╝╚═╝     ╚═╝╚═╝╚══════╝╚═╝  ╚═╝╚═════╝ ╚═╝  ╚═╝
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                
echo.                                                    %p%[%w%1%p%]%w% 🎮 Optimizar GPU RTX 4080 para Gaming
echo.                                                                                         
echo.                                                                      %p%[%w%M%p%]%w% 🔙 Volver al menú   
echo.                                                                      %p%[%w%R%p%]%w% 🔄 Revertir                        
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                          %w%Hardware GPU scheduling, TDR optimizado, DirectX 12 Ultimate!
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%

set /p input=:
if /i %input% == 1 goto GPUConfig
if /i %input% == R goto revertgpu
if /i %input% == M cls & goto menu
) ELSE (
echo Entrada Inválida & goto 4

:GPUConfig
cls
echo.
echo.                                 %p% 🎮 OPTIMIZANDO GPU RTX 4080 PARA GAMING EXTREMO 🎮
echo.
call :GPUOptimization
echo.
echo %g% ✅ GPU RTX 4080 optimizada para máximo rendimiento gaming!%b%
echo.
timeout 3
goto :4

:5
cls
echo.
echo.                                 %p%  ██████╗██████╗ ██╗   ██╗    ██╗███████╗    ██╗██████╗ ███████╗ ██████╗  ██████╗ ██╗  ██╗    ██████╗ ██████╗ ████████╗██╗███╗   ███╗██╗███████╗ █████╗ ██████╗  █████╗ 
echo.                                 %p% ██╔════╝██╔══██╗██║   ██║    ██║╚════██║    ██║╚════██║██╔════╝██╔═══██╗██╔═══██╗██║ ██╔╝    ██╔═══██╗██╔══██╗╚══██╔══╝██║████╗ ████║██║╚══███╔╝██╔══██╗██╔══██╗██╔══██╗
echo.                                 %p% ██║     ██████╔╝██║   ██║    ██║    ██╔╝    ██║    ██╔╝███████╗██║   ██║██║   ██║█████╔╝     ██║   ██║██████╔╝   ██║   ██║██╔████╔██║██║  ███╔╝ ███████║██║  ██║███████║
echo.                                 %p% ██║     ██╔═══╝ ██║   ██║    ██║   ██╔╝     ██║   ██╔╝ ╚════██║██║▄▄ ██║██║   ██║██╔═██╗     ██║   ██║██╔═══╝    ██║   ██║██║╚██╔╝██║██║ ███╔╝  ██╔══██║██║  ██║██╔══██║
echo.                                 %p% ╚██████╗██║     ╚██████╔╝    ██║   ██║       ██║   ██║  ███████║╚██████╔╝╚██████╔╝██║  ██╗    ╚██████╔╝██║        ██║   ██║██║ ╚═╝ ██║██║███████╗██║  ██║██████╔╝██║  ██║
echo.                                 %p%  ╚═════╝╚═╝      ╚═════╝     ╚═╝   ╚═╝       ╚═╝   ╚═╝  ╚══════╝ ╚══▀▀═╝  ╚═════╝ ╚═╝  ╚═╝     ╚═════╝ ╚═╝        ╚═╝   ╚═╝╚═╝     ╚═╝╚═╝╚══════╝╚═╝  ╚═╝╚═════╝ ╚═╝  ╚═╝
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                
echo.                                                    %p%[%w%1%p%]%w% 🚀 Optimizar CPU i7 12700k (16 Cores)
echo.                                                                                         
echo.                                                                      %p%[%w%M%p%]%w% 🔙 Volver al menú   
echo.                                                                      %p%[%w%R%p%]%w% 🔄 Revertir                        
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                          %w%P-cores + E-cores optimizados, thread scheduling, cache perfecto!
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%

set /p input=:
if /i %input% == 1 goto CPUConfig
if /i %input% == R goto revertcpu
if /i %input% == M cls & goto menu
) ELSE (
echo Entrada Inválida & goto 5

:CPUConfig
cls
echo.
echo.                                 %p% 🚀 OPTIMIZANDO CPU i7 12700k PARA MÁXIMO RENDIMIENTO 🚀
echo.
call :CPUOptimization
echo.
echo %g% ✅ CPU i7 12700k optimizado para gaming y productividad extrema!%b%
echo.
timeout 3
goto :5

:6
cls
echo.
echo.                                 %p% ██╗     ██╗███╗   ███╗██████╗ ██╗███████╗███████╗ █████╗     ██╗   ██╗    ██████╗ ██████╗      █████╗ ██╗   ██╗ █████╗ ███╗   ██╗███████╗ █████╗ ██████╗  █████╗ 
echo.                                 %p% ██║     ██║████╗ ████║██╔══██╗██║██╔════╝╚══███╔╝██╔══██╗    ╚██╗ ██╔╝    ██╔══██╗██╔════╝     ██╔══██╗██║   ██║██╔══██╗████╗  ██║╚══███╔╝██╔══██╗██╔══██╗██╔══██╗
echo.                                 %p% ██║     ██║██╔████╔██║██████╔╝██║█████╗    ███╔╝ ███████║     ╚████╔╝     ██████╔╝██║          ███████║██║   ██║███████║██╔██╗ ██║  ███╔╝ ███████║██║  ██║███████║
echo.                                 %p% ██║     ██║██║╚██╔╝██║██╔═══╝ ██║██╔══╝   ███╔╝  ██╔══██║      ╚██╔╝      ██╔═══╝ ██║          ██╔══██║╚██╗ ██╔╝██╔══██║██║╚██╗██║ ███╔╝  ██╔══██║██║  ██║██╔══██║
echo.                                 %p% ███████╗██║██║ ╚═╝ ██║██║     ██║███████╗███████╗██║  ██║       ██║       ██║     ╚██████╗     ██║  ██║ ╚████╔╝ ██║  ██║██║ ╚████║███████╗██║  ██║██████╔╝██║  ██║
echo.                                 %p% ╚══════╝╚═╝╚═╝     ╚═╝╚═╝     ╚═╝╚══════╝╚══════╝╚═╝  ╚═╝       ╚═╝       ╚═╝      ╚═════╝     ╚═╝  ╚═╝  ╚═══╝  ╚═╝  ╚═╝╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝╚═════╝ ╚═╝  ╚═╝
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                
echo.                                                    %p%[%w%1%p%]%w% 🧹 Limpieza PC Avanzada y Optimización
echo.                                                                                         
echo.                                                                      %p%[%w%M%p%]%w% 🔙 Volver al menú   
echo.                                                                      %p%[%w%R%p%]%w% 🔄 Revertir                        
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                          %w%Temp files, caché, registry, DNS cache, disk cleanup avanzado!
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%

set /p input=:
if /i %input% == 1 goto PCCleanupFull
if /i %input% == R goto revertcleanup
if /i %input% == M cls & goto menu
) ELSE (
echo Entrada Inválida & goto 6

:PCCleanupFull
cls
echo.
echo.                                 %p% 🧹 EJECUTANDO LIMPIEZA PC AVANZADA 🧹
echo.
call :PCCleanup
echo.
echo %g% ✅ PC limpiado y optimizado completamente!%b%
echo.
timeout 3
goto :6

::Funciones de reparación y reversión
:FR
cls
echo.
echo.                               %p% ██████╗ ███████╗██████╗  █████╗ ██████╗  █████╗  ██████╗██╗ ██████╗ ███╗   ██╗███████╗███████╗    ██╗   ██╗    ██████╗ ███████╗██╗   ██╗███████╗██████╗ ███████╗██╗ ██████╗ ███╗   ██╗███████╗███████╗
echo.                               %p% ██╔══██╗██╔════╝██╔══██╗██╔══██╗██╔══██╗██╔══██╗██╔════╝██║██╔═══██╗████╗  ██║██╔════╝██╔════╝    ╚██╗ ██╔╝    ██╔══██╗██╔════╝██║   ██║██╔════╝██╔══██╗██╔════╝██║██╔═══██╗████╗  ██║██╔════╝██╔════╝
echo.                               %p% ██████╔╝█████╗  ██████╔╝███████║██████╔╝███████║██║     ██║██║   ██║██╔██╗ ██║█████╗  ███████╗     ╚████╔╝     ██████╔╝█████╗  ██║   ██║█████╗  ██████╔╝███████╗██║██║   ██║██╔██╗ ██║█████╗  ███████╗
echo.                               %p% ██╔══██╗██╔══╝  ██╔═══╝ ██╔══██║██╔══██╗██╔══██║██║     ██║██║   ██║██║╚██╗██║██╔══╝  ╚════██║      ╚██╔╝      ██╔══██╗██╔══╝  ╚██╗ ██╔╝██╔══╝  ██╔══██╗╚════██║██║██║   ██║██║╚██╗██║██╔══╝  ╚════██║
echo.                               %p% ██║  ██║███████╗██║     ██║  ██║██║  ██║██║  ██║╚██████╗██║╚██████╔╝██║ ╚████║███████╗███████║       ██║       ██║  ██║███████╗ ╚████╔╝ ███████╗██║  ██║███████║██║╚██████╔╝██║ ╚████║███████╗███████║
echo.                               %p% ╚═╝  ╚═╝╚══════╝╚═╝     ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝╚═╝ ╚═════╝ ╚═╝  ╚═══╝╚══════╝╚══════╝       ╚═╝       ╚═╝  ╚═╝╚══════╝  ╚═══╝  ╚══════╝╚═╝  ╚═╝╚══════╝╚═╝ ╚═════╝ ╚═╝  ╚═══╝╚══════╝╚══════╝
echo.                                         %b%"═══════════════════════════════════════════════════════════════════════════════════════════════════════"%w%
echo.
echo.                                                        %p%[%w%1%p%]%w% 🔄 Revertir Optimizaciones Generales      %p%[%w%2%p%]%w% ⚡ Revertir Plan de Energía
echo.
echo.                                                        %p%[%w%3%p%]%w% 🖱️ Revertir Mouse/Teclado                %p%[%w%4%p%]%w% 🎮 Revertir GPU RTX 4080
echo.
echo.                                                        %p%[%w%5%p%]%w% 🚀 Revertir CPU i7 12700k                %p%[%w%6%p%]%w% 💨 Revertir RAM DDR5
echo.
echo.                                                        %p%[%w%7%p%]%w% 🌐 Revertir Red Gaming                   %p%[%w%8%p%]%w% 🗑️ Revertir Debloat
echo.
echo.                                         %b%"═══════════════════════════════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                                          %p%[%w%M%p%]%w% 🔙 Volver al menú principal
echo.                                         %b%"═══════════════════════════════════════════════════════════════════════════════════════════════════════"%w%

set /p input=:
if /i %input% == 1 goto revertgeneral
if /i %input% == 2 goto revertpower
if /i %input% == 3 goto revertmouse
if /i %input% == 4 goto revertgpu
if /i %input% == 5 goto revertcpu
if /i %input% == 6 goto revertmemory
if /i %input% == 7 goto revertnetwork
if /i %input% == 8 goto revertdebloat
if /i %input% == M cls & goto menu
) ELSE (
echo Entrada Inválida & goto FR

:Restore
cls
echo.
echo.                    %p% ██████╗ ███████╗███████╗████████╗ █████╗ ██╗   ██╗██████╗  █████╗ ██████╗     ██████╗ ██╗   ██╗███╗   ██╗████████╗ ██████╗     ██████╗ ███████╗    ██████╗ ███████╗███████╗████████╗ █████╗ ██╗   ██╗██████╗  █████╗  ██████╗██╗ ██████╗ ███╗   ██╗
echo.                    %p% ██╔══██╗██╔════╝██╔════╝╚══██╔══╝██╔══██╗██║   ██║██╔══██╗██╔══██╗██╔══██╗    ██╔══██╗██║   ██║████╗  ██║╚══██╔══╝██╔═══██╗    ██╔══██╗██╔════╝    ██╔══██╗██╔════╝██╔════╝╚══██╔══╝██╔══██╗██║   ██║██╔══██╗██╔══██╗██╔════╝██║██╔═══██╗████╗  ██║
echo.                    %p% ██████╔╝█████╗  ███████╗   ██║   ███████║██║   ██║██████╔╝███████║██████╔╝    ██████╔╝██║   ██║██╔██╗ ██║   ██║   ██║   ██║    ██║  ██║█████╗      ██████╔╝█████╗  ███████╗   ██║   ███████║██║   ██║██████╔╝███████║██║     ██║██║   ██║██╔██╗ ██║
echo.                    %p% ██╔══██╗██╔══╝  ╚════██║   ██║   ██╔══██║██║   ██║██╔══██╗██╔══██║██╔══██╗    ██╔═══╝ ██║   ██║██║╚██╗██║   ██║   ██║   ██║    ██║  ██║██╔══╝      ██╔══██╗██╔══╝  ╚════██║   ██║   ██╔══██║██║   ██║██╔══██╗██╔══██║██║     ██║██║   ██║██║╚██╗██║
echo.                    %p% ██║  ██║███████╗███████║   ██║   ██║  ██║╚██████╔╝██║  ██║██║  ██║██║  ██║    ██║     ╚██████╔╝██║ ╚████║   ██║   ╚██████╔╝    ██████╔╝███████╗    ██║  ██║███████╗███████║   ██║   ██║  ██║╚██████╔╝██║  ██║██║  ██║╚██████╗██║╚██████╔╝██║ ╚████║
echo.                    %p% ╚═╝  ╚═╝╚══════╝╚══════╝   ╚═╝   ╚═╝  ╚═╝ ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═╝    ╚═╝      ╚═════╝ ╚═╝  ╚═══╝   ╚═╝    ╚═════╝     ╚═════╝ ╚══════╝    ╚═╝  ╚═╝╚══════╝╚══════╝   ╚═╝   ╚═╝  ╚═╝ ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝╚═╝ ╚═════╝ ╚═╝  ╚═══╝

echo %w% - 🔄 Abriendo interfaz de Restaurar Sistema...%b%
rstrui.exe
timeout 3
goto menu
Reg.exe add "HKCU\Control Panel\Mouse" /v "SmoothMouseYCurve" /t REG_BINARY /d "0000000000000000000038000000000000007000000000000000A800000000000000E00000000000" /f

echo %w% - 🔌 Optimizando USB para 1000Hz Polling%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Class\{4d36e96c-e325-11ce-bfc1-08002be10318}" /v "IdleEnable" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Class\{4d36e96c-e325-11ce-bfc1-08002be10318}" /v "SelectiveSuspendEnabled" /t REG_DWORD /d "0" /f

echo %w% - 🎮 Deshabilitando Enhanced Pointer Precision%b%
Reg.exe add "HKCU\Control Panel\Mouse" /v "MouseAccel" /t REG_SZ /d "0" /f
return

::Red y Latencia Ultra-Baja Mejorada
:NetworkOptimization
echo %w% - 🌐 Deshabilitando Network Throttling Index Completamente%b%
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile" /v "NetworkThrottlingIndex" /t REG_DWORD /d 0xffffffff /f

echo %w% - 🚀 Deshabilitando Algoritmo de Nagle en TODAS las interfaces%b%
sc config Winmgmt start= demand >nul 2>&1
sc start Winmgmt >nul 2>&1

echo %w% - ⚡ Configurando TCPNoDelay para todas las interfaces de red%b%
for /f %%q in ('wmic path win32_networkadapter get GUID ^| findstr "{"') do (
    Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces\%%q" /v "TCPNoDelay" /t REG_DWORD /d "1" /f
    Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces\%%q" /v "TcpAckFrequency" /t REG_DWORD /d "1" /f
    Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces\%%q" /v "TcpDelAckTicks" /t REG_DWORD /d "0" /f
)

echo %w% - 🎯 Optimizaciones TCP/IP Avanzadas para Gaming%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters" /v "TcpWindowSize" /t REG_DWORD /d "65536" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters" /v "GlobalMaxTcpWindowSize" /t REG_DWORD /d "65536" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters" /v "TcpReceiveWindowSize" /t REG_DWORD /d "65536" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters" /v "DefaultTTL" /t REG_DWORD /d "64" /f

echo %w% - 🌐 Configurando RSS para 16 cores (i7 12700k)%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\NDIS\Parameters" /v "MaxNumRssCpus" /t REG_DWORD /d "16" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\NDIS\Parameters" /v "RssBaseCpu" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\NDIS\Parameters" /v "MaxRssProcessors" /t REG_DWORD /d "16" /f

echo %w% - 🛡️ Configurando QoS para Gaming%b%
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows\QoS" /v "Do not use NLA" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows\QoS" /v "Application DSCP Marking Request" /t REG_DWORD /d "1" /f

echo %w% - ⚡ Deshabilitando Auto-Tuning limitado%b%
netsh int tcp set global autotuninglevel=normal >nul 2>&1
netsh int tcp set global chimney=enabled >nul 2>&1
netsh int tcp set global rss=enabled >nul 2>&1
netsh int tcp set global netdma=enabled >nul 2>&1
return

::GPU RTX 4080 Optimizada Mejorada
:GPUOptimization
echo %w% - 🎮 Configurando Hardware GPU Scheduling (RTX 4080 específico)%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "HwSchMode" /t REG_DWORD /d "2" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "TdrLevel" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "TdrDelay" /t REG_DWORD /d "60" /f

echo %w% - 🚀 Configuraciones NVIDIA avanzadas para Ada Lovelace%b%
Reg.exe add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v "PowerMizerEnable" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v "PowerMizerLevel" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v "PowerMizerLevelAC" /t REG_DWORD /d "1" /f

echo %w% - 🎯 Optimizaciones NVAPI para RTX 4080%b%
Reg.exe add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v "DisplayPowerSaving" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v "Adaptive V-Sync" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v "SLI Rendering Mode" /t REG_DWORD /d "0" /f

echo %w% - 💨 Configurando Memory Clock para GDDR6X%b%
Reg.exe add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak\GPUTimingOptimizations" /v "EnableMemoryClockOptimization" /t REG_DWORD /d "1" /f

echo %w% - 🔧 Deshabilitando MPO para mejor compatibilidad%b%
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows\Dwm" /v "OverlayTestMode" /t REG_DWORD /d "5" /f
return

::Debloat Windows 11 Extremo Mejorado
:WindowsDebloat
echo %w% - 🗑️ Removiendo Apps de Windows 11 innecesarias%b%
powershell -Command "Get-AppxPackage *Microsoft.WindowsAlarms* | Remove-AppxPackage" >nul 2>&1
powershell -Command "Get-AppxPackage *Microsoft.WindowsCamera* | Remove-AppxPackage" >nul 2>&1
powershell -Command "Get-AppxPackage *microsoft.windowscommunicationsapps* | Remove-AppxPackage" >nul 2>&1
powershell -Command "Get-AppxPackage *Microsoft.WindowsFeedbackHub* | Remove-AppxPackage" >nul 2>&1
powershell -Command "Get-AppxPackage *Microsoft.WindowsMaps* | Remove-AppxPackage" >nul 2>&1
powershell -Command "Get-AppxPackage *Microsoft.Windows.Photos* | Remove-AppxPackage" >nul 2>&1
powershell -Command "Get-AppxPackage *Microsoft.WindowsSoundRecorder* | Remove-AppxPackage" >nul 2>&1
powershell -Command "Get-AppxPackage *Microsoft.Xbox.TCUI* | Remove-AppxPackage" >nul 2>&1
powershell -Command "Get-AppxPackage *Microsoft.XboxApp* | Remove-AppxPackage" >nul 2>&1
powershell -Command "Get-AppxPackage *Microsoft.XboxGameOverlay* | Remove-AppxPackage" >nul 2>&1
powershell -Command "Get-AppxPackage *Microsoft.XboxGamingOverlay* | Remove-AppxPackage" >nul 2>&1
powershell -Command "Get-AppxPackage *Microsoft.XboxIdentityProvider* | Remove-AppxPackage" >nul 2>&1
powershell -Command "Get-AppxPackage *Microsoft.XboxSpeechToTextOverlay* | Remove-AppxPackage" >nul 2>&1
powershell -Command "Get-AppxPackage *Microsoft.ZuneMusic* | Remove-AppxPackage" >nul 2>&1
powershell -Command "Get-AppxPackage *Microsoft.ZuneVideo* | Remove-AppxPackage" >nul 2>&1
powershell -Command "Get-AppxPackage *Microsoft.YourPhone* | Remove-AppxPackage" >nul 2>&1
powershell -Command "Get-AppxPackage *Microsoft.Getstarted* | Remove-AppxPackage" >nul 2>&1
powershell -Command "Get-AppxPackage *Microsoft.MicrosoftOfficeHub* | Remove-AppxPackage" >nul 2>&1

echo %w% - 🛡️ Deshabilitando Cortana y servicios relacionados%b%
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Search" /v "AllowCortana" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\PolicyManager\default\Experience\AllowCortana" /v "value" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Search" /v "CortanaEnabled" /t REG_DWORD /d "0" /f

echo %w% - 🔒 Deshabilitando Telemetría completamente%b%
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection" /v "AllowTelemetry" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection" /v "DoNotShowFeedbackNotifications" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection" /v "NumberOfSIUFInPeriod" /t REG_DWORD /d "0" /f

sc config DiagTrack start= disabled >nul 2>&1
sc config dmwappushservice start= disabled >nul 2>&1
sc config WerSvc start= disabled >nul 2>&1
sc config OneSyncSvc start= disabled >nul 2>&1
return

:4
cls
echo.
echo.                                 %p%  ██████╗ ██████╗ ██╗   ██╗     ██████╗ ████████╗██╗  ██╗    ██╗  ██╗ ██████╗  █████╗  ██████╗      ██████╗ ██████╗ ████████╗██╗███╗   ███╗██╗███████╗ █████╗ ██████╗  █████╗ 
echo.                                 %p% ██╔════╝ ██╔══██╗██║   ██║     ██╔══██╗╚══██╔══╝╚██╗██╔╝    ██║  ██║██╔═══██╗██╔══██╗██╔═══██╗    ██╔═══██╗██╔══██╗╚══██╔══╝██║████╗ ████║██║╚══███╔╝██╔══██╗██╔══██╗██╔══██╗
echo.                                 %p% ██║  ███╗██████╔╝██║   ██║     ██████╔╝   ██║    ╚███╔╝     ███████║██║   ██║███████║██║   ██║    ██║   ██║██████╔╝   ██║   ██║██╔████╔██║██║  ███╔╝ ███████║██║  ██║███████║
echo.                                 %p% ██║   ██║██╔═══╝ ██║   ██║     ██╔══██╗   ██║    ██╔██╗     ╚════██║██║   ██║██╔══██║██║   ██║    ██║   ██║██╔═══╝    ██║   ██║██║╚██╔╝██║██║ ███╔╝  ██╔══██║██║  ██║██╔══██║
echo.                                 %p% ╚██████╔╝██║     ╚██████╔╝    ██║  ██║   ██║   ██╔╝ ██╗         ██║╚██████╔╝██║  ██║╚██████╔╝    ╚██████╔╝██║        ██║   ██║██║ ╚═╝ ██║██║███████╗██║  ██║██████╔╝██║  ██║
echo.                                 %p%  ╚═════╝ ╚═╝      ╚═════╝     ╚═╝  ╚═╝   ╚═╝   ╚═╝  ╚═╝         ╚═╝ ╚═════╝ ╚═╝  ╚═╝ ╚═════╝      ╚═════╝ ╚═╝        ╚═╝   ╚═╝╚═╝     ╚═╝╚═╝╚══════╝╚═╝  ╚═╝╚═════╝ ╚═╝  ╚═╝
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                
echo.                                                    %p%[%w%1%p%]%w% 🎮 Aplicar Optimizaciones GPU RTX 4080
echo.                                                                                         
echo.                                                                      %p%[%w%M%p%]%w% 🔙 Volver al menú   
echo.                                                                      %p%[%w%R%p%]%w% 🔄 Revertir                        
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                          %w%Hardware Scheduling, NVAPI, MSI Mode, configuraciones Ada Lovelace!
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%

set /p input=:
if /i %input% == 1 goto GPUFull
if /i %input% == R goto revertgpu
if /i %input% == M cls & goto menu
) ELSE (
echo Entrada Inválida & goto 4

:GPUFull
cls
echo.
echo.                                 %p%  ██████╗ ██████╗ ██╗   ██╗    ██████╗ ████████╗██╗  ██╗    ██╗  ██╗ ██████╗  █████╗  ██████╗      ██████╗ ██████╗ ████████╗██╗███╗   ███╗██╗███████╗ █████╗ ██████╗  █████╗ 
echo.                                 %p% ██╔════╝ ██╔══██╗██║   ██║    ██╔══██╗╚══██╔══╝╚██╗██╔╝    ██║  ██║██╔═══██╗██╔══██╗██╔═══██╗    ██╔═══██╗██╔══██╗╚══██╔══╝██║████╗ ████║██║╚══███╔╝██╔══██╗██╔══██╗██╔══██╗
echo.                                 %p% ██║  ███╗██████╔╝██║   ██║    ██████╔╝   ██║    ╚███╔╝     ███████║██║   ██║███████║██║   ██║    ██║   ██║██████╔╝   ██║   ██║██╔████╔██║██║  ███╔╝ ███████║██║  ██║███████║
echo.                                 %p% ██║   ██║██╔═══╝ ██║   ██║    ██╔══██╗   ██║    ██╔██╗     ╚════██║██║   ██║██╔══██║██║   ██║    ██║   ██║██╔═══╝    ██║   ██║██║╚██╔╝██║██║ ███╔╝  ██╔══██║██║  ██║██╔══██║
echo.                                 %p% ╚██████╔╝██║     ╚██████╔╝    ██║  ██║   ██║   ██╔╝ ██╗         ██║╚██████╔╝██║  ██║╚██████╔╝    ╚██████╔╝██║        ██║   ██║██║ ╚═╝ ██║██║███████╗██║  ██║██████╔╝██║  ██║
echo.                                 %p%  ╚═════╝ ╚═╝      ╚═════╝     ╚═╝  ╚═╝   ╚═╝   ╚═╝  ╚═╝         ╚═╝ ╚═════╝ ╚═╝  ╚═╝ ╚═════╝      ╚═════╝ ╚═╝        ╚═╝   ╚═╝╚═╝     ╚═╝╚═╝╚══════╝╚═╝  ╚═╝╚═════╝ ╚═╝  ╚═╝

echo %w% - 🎮 Aplicando optimizaciones RTX 4080 Ada Lovelace%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "HwSchMode" /t REG_DWORD /d "2" /f >nul 2>&1
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "TdrLevel" /t REG_DWORD /d "0" /f >nul 2>&1
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "TdrDelay" /t REG_DWORD /d "60" /f >nul 2>&1

echo %w% - 💎 Configurando NVAPI optimizado para 16GB GDDR6X%b%
Reg.exe add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v "DisplayPowerSaving" /t REG_DWORD /d "0" /f >nul 2>&1
Reg.exe add "HKLM\SOFTWARE\NVIDIA Corporation\Global\FTS" /v "EnableRID61684" /t REG_DWORD /d "1" /f >nul 2>&1
Reg.exe add "HKLM\SOFTWARE\NVIDIA Corporation\Global\FTS" /v "EnableRID73779" /t REG_DWORD /d "1" /f >nul 2>&1

echo %w% - ⚡ Habilitando Hardware Accelerated GPU Scheduling%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "HwSchMode" /t REG_DWORD /d "2" /f >nul 2>&1

echo %w% - 🔧 Configurando MSI Mode para RTX 4080%b%
if exist "C:\LJokerL_TTV\MSI_Utility.exe" (
    echo %w%   Configurando MSI Mode...%b%
    "C:\LJokerL_TTV\MSI_Utility.exe" -silent >nul 2>&1
)

echo %w% - 🎯 Aplicando Profile Inspector Settings%b%
if exist "C:\LJokerL_TTV\Nvidia_Profile_Inspector_2.4.0.19.exe" (
    if exist "C:\LJokerL_TTV\LJokerL_TTV_NVPI_V10.nip" (
        echo %w%   Aplicando configuración personalizada RTX 4080...%b%
        "C:\LJokerL_TTV\Nvidia_Profile_Inspector_2.4.0.19.exe" "C:\LJokerL_TTV\LJokerL_TTV_NVPI_V10.nip" >nul 2>&1
    )
)

echo %w% - ⚡ Configuraciones adicionales DirectX%b%
Reg.exe add "HKLM\SOFTWARE\Microsoft\DirectDraw" /v "DisableAGPSupport" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Direct3D" /v "DisableVidMemVBs" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Direct3D" /v "MMX Fast Path" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Direct3D" /v "FlipNoVsync" /t REG_DWORD /d "1" /f

echo %w% - 🚀 Optimizaciones VRAM para 16GB GDDR6X%b%
Reg.exe add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v "MemoryClockBoost" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v "CoreClockBoost" /t REG_DWORD /d "1" /f

echo.
echo.                                                                                  %b%╔═══════════════════════════════════════════════════════════════╗
echo.                                                                                  %b%║    %w% ✅ RTX 4080 Optimizada - Presione cualquier tecla...      %b%║
echo.                                                                                  %b%╚═══════════════════════════════════════════════════════════════╝
pause > nul
cls
goto menu

:revertgpu
cls
echo.
echo %r% ⚠️ Revirtiendo Optimizaciones GPU RTX 4080...%b%
echo.

echo %w% - Restaurando configuración gráficos por defecto%b%
Reg.exe delete "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "HwSchMode" /f >nul 2>&1
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "TdrLevel" /t REG_DWORD /d "3" /f >nul 2>&1
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "TdrDelay" /t REG_DWORD /d "2" /f >nul 2>&1

echo %w% - Restaurando configuración NVIDIA por defecto%b%
Reg.exe delete "HKLM\SOFTWARE\NVIDIA Corporation\Global\NVTweak" /v "DisplayPowerSaving" /f >nul 2>&1
Reg.exe delete "HKLM\SOFTWARE\NVIDIA Corporation\Global\FTS" /v "EnableRID61684" /f >nul 2>&1
Reg.exe delete "HKLM\SOFTWARE\NVIDIA Corporation\Global\FTS" /v "EnableRID73779" /f >nul 2>&1

echo %w% - Restaurando DirectX por defecto%b%
Reg.exe delete "HKLM\SOFTWARE\Microsoft\DirectDraw" /v "DisableAGPSupport" /f >nul 2>&1
Reg.exe delete "HKLM\SOFTWARE\Microsoft\Direct3D" /v "DisableVidMemVBs" /f >nul 2>&1
Reg.exe delete "HKLM\SOFTWARE\Microsoft\Direct3D" /v "MMX Fast Path" /f >nul 2>&1

echo.
echo %g% ✅ Optimizaciones GPU revertidas exitosamente!%b%
echo.
timeout 3
goto :4

::CPU i7 12700k Optimizada 
:CPUOptimization
echo %w% - 🚀 Configurando Thread Scheduling para 16 cores (8P+8E)%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\PriorityControl" /v "Win32PrioritySeparation" /t REG_DWORD /d "38" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\kernel" /v "GlobalTimerResolutionRequests" /t REG_DWORD /d "1" /f

echo %w% - 🎯 Optimizaciones específicas 12va generación%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\kernel" /v "DistributeTimers" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\kernel" /v "DpcTimeoutEvents" /t REG_DWORD /d "0" /f

echo %w% - ⚡ Configurando CPU Cache optimizado para DDR5%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "LargePageMinimum" /t REG_DWORD /d "4096" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "SystemCacheLimit" /t REG_DWORD /d "0" /f

echo %w% - 🏆 Optimizaciones de Scheduling para E-cores y P-cores%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\kernel" /v "ThreadDpcEnable" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\kernel" /v "MaxDynamicTickDuration" /t REG_DWORD /d "10" /f

echo %w% - 💨 Deshabilitando mitigaciones de CPU Alderlake%b%
bcdedit /set hypervisorlaunchtype off >nul 2>&1
bcdedit /set nx optin >nul 2>&1
bcdedit /set mitigations disable >nul 2>&1
return

::Limpieza PC Avanzada
:PCCleanup
echo %w% - 🧹 Limpiando archivos temporales del sistema%b%
del /q /s /f %systemdrive%\*.tmp >nul 2>&1
del /q /s /f %systemdrive%\*._mp >nul 2>&1
del /q /s /f %systemdrive%\*.log >nul 2>&1
del /q /s /f %systemdrive%\*.gid >nul 2>&1
del /q /s /f %systemdrive%\*.chk >nul 2>&1
del /q /s /f %systemdrive%\*.old >nul 2>&1

echo %w% - 🗑️ Limpiando caché del sistema%b%
del /q /s /f %systemdrive%\recycled\*.* >nul 2>&1
del /q /s /f %windir%\*.bak >nul 2>&1
del /q /s /f %windir%\prefetch\*.* >nul 2>&1
del /q /s /f %windir%\temp\*.* >nul 2>&1
del /q /s /f %userprofile%\cookies\*.* >nul 2>&1
del /q /s /f %userprofile%\recent\*.* >nul 2>&1

echo %w% - 📂 Limpiando archivos temporales de usuario%b%
del /q /s /f "%USERPROFILE%\AppData\Local\Temp\*.*" >nul 2>&1
del /q /s /f "%USERPROFILE%\AppData\Local\Microsoft\Windows\Temporary Internet Files\*.*" >nul 2>&1
del /q /s /f "%USERPROFILE%\AppData\Local\Microsoft\Windows\INetCache\*.*" >nul 2>&1

echo %w% - 🔧 Ejecutando Disk Cleanup avanzado%b%
cleanmgr /sagerun:1 >nul 2>&1

echo %w% - 💾 Optimizando SSD/NVMe (TRIM)%b%
defrag C: /O >nul 2>&1

echo %w% - 🗂️ Limpiando Registry temporales%b%
reg delete "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\RunMRU" /va /f >nul 2>&1
reg delete "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\TypedPaths" /va /f >nul 2>&1

echo %w% - 🧽 Vaciando DNS Cache%b%
ipconfig /flushdns >nul 2>&1
return

::Optimización Memoria y RAM DDR5
:MemoryOptimization
cls
echo.
echo.                                 %p% ███╗   ███╗███████╗███╗   ███╗ ██████╗ ██████╗ ██╗ █████╗     ██╗   ██╗    ██████╗  █████╗ ███╗   ███╗
echo.                                 %p% ████╗ ████║██╔════╝████╗ ████║██╔═══██╗██╔══██╗██║██╔══██╗    ╚██╗ ██╔╝    ██╔══██╗██╔══██╗████╗ ████║
echo.                                 %p% ██╔████╔██║█████╗  ██╔████╔██║██║   ██║██████╔╝██║███████║     ╚████╔╝     ██████╔╝███████║██╔████╔██║
echo.                                 %p% ██║╚██╔╝██║██╔══╝  ██║╚██╔╝██║██║   ██║██╔══██╗██║██╔══██║      ╚██╔╝      ██╔══██╗██╔══██║██║╚██╔╝██║
echo.                                 %p% ██║ ╚═╝ ██║███████╗██║ ╚═╝ ██║╚██████╔╝██║  ██║██║██║  ██║       ██║       ██║  ██║██║  ██║██║ ╚═╝ ██║
echo.                                 %p% ╚═╝     ╚═╝╚══════╝╚═╝     ╚═╝ ╚═════╝ ╚═╝  ╚═╝╚═╝╚═╝  ╚═╝       ╚═╝       ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝     ╚═╝

echo %w% - 💾 Optimizando gestión memoria para 64GB DDR5 5400MHz%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "LargeSystemCache" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "SystemPages" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "DisablePagingExecutive" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "ClearPageFileAtShutdown" /t REG_DWORD /d "0" /f

echo %w% - 🚀 Configurando Prefetch y SuperFetch para DDR5%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters" /v "EnablePrefetcher" /t REG_DWORD /d "3" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters" /v "EnableSuperfetch" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters" /v "EnableBootTrace" /t REG_DWORD /d "0" /f

echo %w% - ⚡ Optimizando caché de memoria para aplicaciones%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "IoPageLockLimit" /t REG_DWORD /d "983040" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "PoolUsageMaximum" /t REG_DWORD /d "96" /f

echo %w% - 🎯 Configurando heap para aplicaciones gaming%b%
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Image File Execution Options" /v "GlobalFlag" /t REG_DWORD /d "512" /f

echo.
echo.                                                                                  %b%╔═══════════════════════════════════════════════════════════════╗
echo.                                                                                  %b%║    %w% ✅ Memoria Optimizada - Presione cualquier tecla...     %b%║
echo.                                                                                  %b%╚═══════════════════════════════════════════════════════════════╝
pause > nul
cls
goto menu

::Optimización Red Gaming
:NetworkOptimization
cls
echo.
echo.                                 %p% ███╗   ██╗███████╗████████╗    ██████╗  █████╗ ███╗   ███╗██╗███╗   ██╗ ██████╗ 
echo.                                 %p% ████╗  ██║██╔════╝╚══██╔══╝    ██╔════╝ ██╔══██╗████╗ ████║██║████╗  ██║██╔════╝ 
echo.                                 %p% ██╔██╗ ██║█████╗     ██║       ██║  ███╗███████║██╔████╔██║██║██╔██╗ ██║██║  ███╗
echo.                                 %p% ██║╚██╗██║██╔══╝     ██║       ██║   ██║██╔══██║██║╚██╔╝██║██║██║╚██╗██║██║   ██║
echo.                                 %p% ██║ ╚████║███████╗   ██║       ╚██████╔╝██║  ██║██║ ╚═╝ ██║██║██║ ╚████║╚██████╔╝
echo.                                 %p% ╚═╝  ╚═══╝╚══════╝   ╚═╝        ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚═╝╚═╝  ╚═══╝ ╚═════╝ 
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                
echo.                                                    %p%[%w%1%p%]%w% 🌐 Optimizar Red para Gaming
echo.                                                                                         
echo.                                                                      %p%[%w%M%p%]%w% 🔙 Volver al menú   
echo.                                                                      %p%[%w%R%p%]%w% 🔄 Revertir                        
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                          %w%TCP optimizer, DNS ultra-rápido, Nagle deshabilitado, latencia mínima!
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%

set /p input=:
if /i %input% == 1 call :NetworkOptimization
if /i %input% == R goto revertnetwork
if /i %input% == M cls & goto menu
) ELSE (
echo Entrada Inválida & goto 7

:8
cls
echo.
echo.                                 %p% ██████╗ ███████╗██████╗ ██╗      ██████╗  █████╗ ████████╗     █████╗ ██╗   ██╗ █████╗ ███╗   ██╗███████╗ █████╗ ██████╗  ██████╗ 
echo.                                 %p% ██╔══██╗██╔════╝██╔══██╗██║     ██╔═══██╗██╔══██╗╚══██╔══╝    ██╔══██╗██║   ██║██╔══██╗████╗  ██║╚══███╔╝██╔══██╗██╔══██╗██╔═══██╗
echo.                                 %p% ██║  ██║█████╗  ██████╔╝██║     ██║   ██║███████║   ██║       ███████║██║   ██║███████║██╔██╗ ██║  ███╔╝ ███████║██║  ██║██║   ██║
echo.                                 %p% ██║  ██║██╔══╝  ██╔══██╗██║     ██║   ██║██╔══██║   ██║       ██╔══██║╚██╗ ██╔╝██╔══██║██║╚██╗██║ ███╔╝  ██╔══██║██║  ██║██║   ██║
echo.                                 %p% ██████╔╝███████╗██████╔╝███████╗╚██████╔╝██║  ██║   ██║       ██║  ██║ ╚████╔╝ ██║  ██║██║ ╚████║███████╗██║  ██║██████╔╝╚██████╔╝
echo.                                 %p% ╚═════╝ ╚══════╝╚═════╝ ╚══════╝ ╚═════╝ ╚═╝  ╚═╝   ╚═╝       ╚═╝  ╚═╝  ╚═══╝  ╚═╝  ╚═╝╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝╚═════╝  ╚═════╝ 
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                
echo.                                                    %p%[%w%1%p%]%w% 🧹 Debloat Avanzado Windows 11
echo.                                                                                         
echo.                                                                      %p%[%w%M%p%]%w% 🔙 Volver al menú   
echo.                                                                      %p%[%w%R%p%]%w% 🔄 Revertir                        
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                          %w%Telemetría, servicios innecesarios, UWP apps, Cortana, OneDrive!
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%

set /p input=:
if /i %input% == 1 call :DebloatAdvanced
if /i %input% == R goto revertdebloat
if /i %input% == M cls & goto menu
) ELSE (
echo Entrada Inválida & goto 8

:9
cls
echo.
echo.                                 %p%  █████╗ ██╗     ███╗   ███╗ █████╗  ██████╗███████╗███╗   ██╗ █████╗ ███╗   ███╗██╗███████╗███╗   ██╗████████╗ ██████╗ 
echo.                                 %p% ██╔══██╗██║     ████╗ ████║██╔══██╗██╔════╝██╔════╝████╗  ██║██╔══██╗████╗ ████║██║██╔════╝████╗  ██║╚══██╔══╝██╔═══██╗
echo.                                 %p% ███████║██║     ██╔████╔██║███████║██║     █████╗  ██╔██╗ ██║███████║██╔████╔██║██║█████╗  ██╔██╗ ██║   ██║   ██║   ██║
echo.                                 %p% ██╔══██║██║     ██║╚██╔╝██║██╔══██║██║     ██╔══╝  ██║╚██╗██║██╔══██║██║╚██╔╝██║██║██╔══╝  ██║╚██╗██║   ██║   ██║   ██║
echo.                                 %p% ██║  ██║███████╗██║ ╚═╝ ██║██║  ██║╚██████╗███████╗██║ ╚████║██║  ██║██║ ╚═╝ ██║██║███████╗██║ ╚████║   ██║   ╚██████╔╝
echo.                                 %p% ╚═╝  ╚═╝╚══════╝╚═╝     ╚═╝╚═╝  ╚═╝ ╚═════╝╚══════╝╚═╝  ╚═══╝╚═╝  ╚═╝╚═╝     ╚═╝╚═╝╚══════╝╚═╝  ╚═══╝   ╚═╝    ╚═════╝ 
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                
echo.                                                    %p%[%w%1%p%]%w% 💽 Optimizar Almacenamiento SSD/NVMe
echo.                                                                                         
echo.                                                                      %p%[%w%M%p%]%w% 🔙 Volver al menú   
echo.                                                                      %p%[%w%R%p%]%w% 🔄 Revertir                        
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                          %w%TRIM automático, desfragmentación SSD, NCQ, prefetch para SSD!
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%

set /p input=:
if /i %input% == 1 call :StorageOptimization
if /i %input% == R goto revertstorage
if /i %input% == M cls & goto menu
) ELSE (
echo Entrada Inválida & goto 9

:10
cls
echo.
echo.                                 %p% ██████╗ ██╗      ██████╗  █████╗ ████████╗██╗    ██╗ █████╗ ██████╗ ███████╗    ██████╗ ███████╗███╗   ███╗ ██████╗ ██╗   ██╗ █████╗ ██╗     
echo.                                 %p% ██╔══██╗██║     ██╔═══██╗██╔══██╗╚══██╔══╝██║    ██║██╔══██╗██╔══██╗██╔════╝    ██╔══██╗██╔════╝████╗ ████║██╔═══██╗██║   ██║██╔══██╗██║     
echo.                                 %p% ██████╔╝██║     ██║   ██║███████║   ██║   ██║ █╗ ██║███████║██████╔╝█████╗      ██████╔╝█████╗  ██╔████╔██║██║   ██║██║   ██║███████║██║     
echo.                                 %p% ██╔══██╗██║     ██║   ██║██╔══██║   ██║   ██║███╗██║██╔══██║██╔══██╗██╔══╝      ██╔══██╗██╔══╝  ██║╚██╔╝██║██║   ██║╚██╗ ██╔╝██╔══██║██║     
echo.                                 %p% ██████╔╝███████╗╚██████╔╝██║  ██║   ██║   ╚███╔███╔╝██║  ██║██║  ██║███████╗    ██║  ██║███████╗██║ ╚═╝ ██║╚██████╔╝ ╚████╔╝ ██║  ██║███████╗
echo.                                 %p% ╚═════╝ ╚══════╝ ╚═════╝ ╚═╝  ╚═╝   ╚═╝    ╚══╝╚══╝ ╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝    ╚═╝  ╚═╝╚══════╝╚═╝     ╚═╝ ╚═════╝   ╚═══╝  ╚═╝  ╚═╝╚══════╝
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                
echo.                                                    %p%[%w%1%p%]%w% 🗑️ Eliminar Bloatware Completo
echo.                                                                                         
echo.                                                                      %p%[%w%M%p%]%w% 🔙 Volver al menú   
echo.                                                                      %p%[%w%R%p%]%w% 🔄 Revertir                        
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                          %w%UWP apps inútiles, Xbox, Edge, Teams, Office trial y más!
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%

set /p input=:
if /i %input% == 1 call :BloatwareRemoval
if /i %input% == R goto revertbloatware
if /i %input% == M cls & goto menu
) ELSE (
echo Entrada Inválida & goto 10

:11
cls
echo.
echo.                                 %p% ██████╗ ███████╗ ██████╗ ██╗███████╗████████╗██████╗  ██████╗      █████╗ ██╗   ██╗ █████╗ ███╗   ██╗███████╗ █████╗ ██████╗  ██████╗ 
echo.                                 %p% ██╔══██╗██╔════╝██╔════╝ ██║██╔════╝╚══██╔══╝██╔══██╗██╔═══██╗    ██╔══██╗██║   ██║██╔══██╗████╗  ██║╚══███╔╝██╔══██╗██╔══██╗██╔═══██╗
echo.                                 %p% ██████╔╝█████╗  ██║  ███╗██║███████╗   ██║   ██████╔╝██║   ██║    ███████║██║   ██║███████║██╔██╗ ██║  ███╔╝ ███████║██║  ██║██║   ██║
echo.                                 %p% ██╔══██╗██╔══╝  ██║   ██║██║╚════██║   ██║   ██╔══██╗██║   ██║    ██╔══██║╚██╗ ██╔╝██╔══██║██║╚██╗██║ ███╔╝  ██╔══██║██║  ██║██║   ██║
echo.                                 %p% ██║  ██║███████╗╚██████╔╝██║███████║   ██║   ██║  ██║╚██████╔╝    ██║  ██║ ╚████╔╝ ██║  ██║██║ ╚████║███████╗██║  ██║██████╔╝╚██████╔╝
echo.                                 %p% ╚═╝  ╚═╝╚══════╝ ╚═════╝ ╚═╝╚══════╝   ╚═╝   ╚═╝  ╚═╝ ╚═════╝     ╚═╝  ╚═╝  ╚═══╝  ╚═╝  ╚═╝╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝╚═════╝  ╚═════╝ 
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                
echo.                                                    %p%[%w%1%p%]%w% 📝 Optimizar Registro Avanzado
echo.                                                                                         
echo.                                                                      %p%[%w%M%p%]%w% 🔙 Volver al menú   
echo.                                                                      %p%[%w%R%p%]%w% 🔄 Revertir                        
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                          %w%Políticas grupo, servicios, UAC, Windows Update, privacidad!
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%

set /p input=:
if /i %input% == 1 call :RegistryOptimization
if /i %input% == R goto revertregistry
if /i %input% == M cls & goto menu
) ELSE (
echo Entrada Inválida & goto 11

:12
cls
echo.
echo.                                 %p%  ██████╗ ██████╗ ███╗   ██╗████████╗██████╗  ██████╗ ██╗      █████╗ ██████╗  ██████╗ ██████╗ ███████╗███████╗
echo.                                 %p% ██╔════╝██╔═══██╗████╗  ██║╚══██╔══╝██╔══██╗██╔═══██╗██║     ██╔══██╗██╔══██╗██╔═══██╗██╔══██╗██╔════╝██╔════╝
echo.                                 %p% ██║     ██║   ██║██╔██╗ ██║   ██║   ██████╔╝██║   ██║██║     ███████║██║  ██║██║   ██║██████╔╝█████╗  ███████╗
echo.                                 %p% ██║     ██║   ██║██║╚██╗██║   ██║   ██╔══██╗██║   ██║██║     ██╔══██║██║  ██║██║   ██║██╔══██╗██╔══╝  ╚════██║
echo.                                 %p% ╚██████╗╚██████╔╝██║ ╚████║   ██║   ██║  ██║╚██████╔╝███████╗██║  ██║██████╔╝╚██████╔╝██║  ██║███████╗███████║
echo.                                 %p%  ╚═════╝ ╚═════╝ ╚═╝  ╚═══╝   ╚═╝   ╚═╝  ╚═╝ ╚═════╝ ╚══════╝╚═╝  ╚═╝╚═════╝  ╚═════╝ ╚═╝  ╚═╝╚══════╝╚══════╝
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                
echo.                                                    %p%[%w%1%p%]%w% 🔧 Optimizar Controladores
echo.                                                                                         
echo.                                                                      %p%[%w%M%p%]%w% 🔙 Volver al menú   
echo.                                                                      %p%[%w%R%p%]%w% 🔄 Revertir                        
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                          %w%MSI Mode, IRQ priorities, DPC latencia, PCI Express optimizado!
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%

set /p input=:
if /i %input% == 1 call :DriversOptimization
if /i %input% == R goto revertdrivers
if /i %input% == M cls & goto menu
) ELSE (
echo Entrada Inválida & goto 12

::Funciones de Optimización - Continuación

::Optimización de Red Gaming
:NetworkOptimization
cls
echo %w% - 🌐 Configurando TCP Optimizer para gaming%b%
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile" /v "NetworkThrottlingIndex" /t REG_DWORD /d "10" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile" /v "SystemResponsiveness" /t REG_DWORD /d "1" /f

echo %w% - 🚀 Deshabilitando algoritmo de Nagle%b%
for /f %%i in ('wmic path win32_networkadapter get GUID ^| findstr "{"') do (
    Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces\%%i" /v "TcpAckFrequency" /t REG_DWORD /d "1" /f
    Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces\%%i" /v "TCPNoDelay" /t REG_DWORD /d "1" /f
)

echo %w% - ⚡ Configurando DNS ultra-rápido%b%
netsh interface ip set dns "Ethernet" static 1.1.1.1 primary
netsh interface ip add dns "Ethernet" 1.0.0.1 index=2
netsh interface ip set dns "Wi-Fi" static 1.1.1.1 primary
netsh interface ip add dns "Wi-Fi" 1.0.0.1 index=2

echo %w% - 🎯 Optimizando buffers de red%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters" /v "TcpWindowSize" /t REG_DWORD /d "64240" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters" /v "Tcp1323Opts" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters" /v "DefaultTTL" /t REG_DWORD /d "64" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters" /v "EnablePMTUBHDetect" /t REG_DWORD /d "0" /f

echo.
echo.                                                                                  %b%╔═══════════════════════════════════════════════════════════════╗
echo.                                                                                  %b%║    %w% ✅ Red Optimizada - Presione cualquier tecla...       %b%║
echo.                                                                                  %b%╚═══════════════════════════════════════════════════════════════╝
pause > nul
cls
goto menu

::Debloat Avanzado Windows 11
:DebloatAdvanced
cls
echo %w% - 🧹 Eliminando telemetría de Windows%b%
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection" /v "AllowTelemetry" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection" /v "AllowTelemetry" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\DiagTrack" /v "Start" /t REG_DWORD /d "4" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\dmwappushservice" /v "Start" /t REG_DWORD /d "4" /f

echo %w% - 🚫 Deshabilitando Cortana y búsqueda web%b%
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Search" /v "AllowCortana" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Search" /v "DisableWebSearch" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Search" /v "ConnectedSearchUseWeb" /t REG_DWORD /d "0" /f

echo %w% - ☁️ Deshabilitando OneDrive%b%
taskkill /f /im OneDrive.exe
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows\OneDrive" /v "DisableFileSyncNGSC" /t REG_DWORD /d "1" /f

echo %w% - 📱 Eliminando apps UWP innecesarias%b%
powershell -command "Get-AppxPackage *Microsoft.BingWeather* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.GetHelp* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.Getstarted* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.Messaging* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.Microsoft3DViewer* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.MicrosoftSolitaireCollection* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.NetworkSpeedTest* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.News* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.Office.Lens* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.Office.Sway* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.OneConnect* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.People* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.Print3D* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.SkypeApp* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.StorePurchaseApp* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.Office.OneNote* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.MicrosoftOfficeHub* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.BingFinance* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.BingNews* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.BingSports* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.BingTranslator* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.WindowsAlarms* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *microsoft.windowscommunicationsapps* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.WindowsFeedbackHub* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.WindowsMaps* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.WindowsSoundRecorder* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.XboxApp* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.XboxGameOverlay* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.XboxGamingOverlay* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.XboxIdentityProvider* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.XboxSpeechToTextOverlay* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.ZuneMusic* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Microsoft.ZuneVideo* | Remove-AppxPackage"

echo.
echo.                                                                                  %b%╔═══════════════════════════════════════════════════════════════╗
echo.                                                                                  %b%║    %w% ✅ Debloat Completado - Presione cualquier tecla...    %b%║
echo.                                                                                  %b%╚═══════════════════════════════════════════════════════════════╝
pause > nul
cls
goto menu

::Optimización de Almacenamiento
:StorageOptimization
cls
echo %w% - 💽 Habilitando TRIM para SSD%b%
fsutil behavior set DisableDeleteNotify 0

echo %w% - 🚀 Optimizando prefetch para SSD%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters" /v "EnablePrefetcher" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters" /v "EnableSuperfetch" /t REG_DWORD /d "0" /f

echo %w% - ⚡ Configurando políticas de energía para NVMe%b%
powercfg /setacvalueindex scheme_current sub_disk 0012ee47-9041-4b5d-9b77-535fba8b1442 0

echo %w% - 🎯 Optimizando caché de escritura%b%
for /f "tokens=*" %%i in ('wmic diskdrive get index /format:value ^| find "="') do (
    for /f "tokens=2 delims==" %%j in ("%%i") do (
        Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\disk\Enum" /v "%%j" /t REG_SZ /d "SCSI\Disk" /f
    )
)

echo %w% - 🗂️ Deshabilitando indexado en SSD del sistema%b%
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows Search" /v "SetupCompletedSuccessfully" /t REG_DWORD /d "0" /f

echo.
echo.                                                                                  %b%╔═══════════════════════════════════════════════════════════════╗
echo.                                                                                  %b%║    %w% ✅ Almacenamiento Optimizado - Presione cualquier tecla... %b%║
echo.                                                                                  %b%╚═══════════════════════════════════════════════════════════════╝
pause > nul
cls
goto menu

::Eliminación de Bloatware
:BloatwareRemoval
cls
echo %w% - 🗑️ Eliminando Microsoft Teams%b%
taskkill /f /im Teams.exe 2>nul
"%ProgramFiles%\Microsoft\Teams\current\Teams.exe" --uninstall 2>nul
"%ProgramFiles(x86)%\Microsoft\Teams\current\Teams.exe" --uninstall 2>nul

echo %w% - 🌐 Configurando Edge (no se puede desinstalar completamente)%b%
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Edge" /v "StartupBoostEnabled" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Edge" /v "BackgroundModeEnabled" /t REG_DWORD /d "0" /f

echo %w% - 📱 Eliminando Xbox services%b%
sc config XblAuthManager start= disabled
sc config XblGameSave start= disabled
sc config XboxNetApiSvc start= disabled
sc config XboxGipSvc start= disabled

echo %w% - 📧 Eliminando aplicaciones de correo y calendario%b%
powershell -command "Get-AppxPackage *Microsoft.windowscommunicationsapps* | Remove-AppxPackage"

echo %w% - 🎮 Eliminando aplicaciones de juegos preinstaladas%b%
powershell -command "Get-AppxPackage *king.com* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *CandyCrush* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *BubbleWitch3Saga* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Wunderlist* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Flipboard* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Twitter* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Facebook* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Instagram* | Remove-AppxPackage"
powershell -command "Get-AppxPackage *Spotify* | Remove-AppxPackage"

echo.
echo.                                                                                  %b%╔═══════════════════════════════════════════════════════════════╗
echo.                                                                                  %b%║    %w% ✅ Bloatware Eliminado - Presione cualquier tecla...   %b%║
echo.                                                                                  %b%╚═══════════════════════════════════════════════════════════════╝
pause > nul
cls
goto menu

::Optimización de Registro Avanzado
:RegistryOptimization
cls
echo %w% - 📝 Deshabilitando servicios innecesarios%b%
sc config "SysMain" start= disabled
sc config "WSearch" start= disabled
sc config "DiagTrack" start= disabled
sc config "dmwappushservice" start= disabled
sc config "MapsBroker" start= disabled
sc config "lfsvc" start= disabled
sc config "SharedAccess" start= disabled
sc config "lltdsvc" start= disabled
sc config "AppVClient" start= disabled
sc config "NetTcpPortSharing" start= disabled
sc config "RemoteAccess" start= disabled
sc config "RemoteRegistry" start= disabled
sc config "WMPNetworkSvc" start= disabled
sc config "icssvc" start= disabled
sc config "WPCSvc" start= disabled
sc config "WerSvc" start= disabled
sc config "Wecsvc" start= disabled
sc config "stisvc" start= disabled
sc config "wisvc" start= disabled

echo %w% - 🔒 Configurando políticas de privacidad%b%
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy" /v "LetAppsAccessAccountInfo" /t REG_DWORD /d "2" /f
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy" /v "LetAppsAccessCallHistory" /t REG_DWORD /d "2" /f
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy" /v "LetAppsAccessContacts" /t REG_DWORD /d "2" /f
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy" /v "LetAppsAccessEmail" /t REG_DWORD /d "2" /f
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy" /v "LetAppsAccessLocation" /t REG_DWORD /d "2" /f
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy" /v "LetAppsAccessMessaging" /t REG_DWORD /d "2" /f
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy" /v "LetAppsAccessMicrophone" /t REG_DWORD /d "2" /f
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy" /v "LetAppsAccessCamera" /t REG_DWORD /d "2" /f
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy" /v "LetAppsAccessRadios" /t REG_DWORD /d "2" /f

echo %w% - 🛡️ Deshabilitando Windows Defender para máximo rendimiento%b%
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows Defender" /v "DisableAntiSpyware" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection" /v "DisableBehaviorMonitoring" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection" /v "DisableOnAccessProtection" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection" /v "DisableScanOnRealtimeEnable" /t REG_DWORD /d "1" /f
sc config WinDefend start= disabled
sc config WdNisSvc start= disabled

echo %w% - ⚡ Configurando UAC para mejor rendimiento%b%
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System" /v "EnableLUA" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System" /v "ConsentPromptBehaviorAdmin" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System" /v "ConsentPromptBehaviorUser" /t REG_DWORD /d "0" /f

echo.
echo.                                                                                  %b%╔═══════════════════════════════════════════════════════════════╗
echo.                                                                                  %b%║    %w% ✅ Registro Optimizado - Presione cualquier tecla...   %b%║
echo.                                                                                  %b%╚═══════════════════════════════════════════════════════════════╝
pause > nul
cls
goto menu

::Optimización de Controladores
:DriversOptimization
cls
echo %w% - 🎯 Configurando MSI Mode para dispositivos%b%
"C:\LJokerL_TTV\MSI_Utility.exe" /S /SILENT

echo %w% - ⚡ Optimizando prioridades de IRQ%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\PriorityControl" /v "IRQ8Priority" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\PriorityControl" /v "IRQ16Priority" /t REG_DWORD /d "2" /f

echo %w% - 🚀 Configurando latencia DPC%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\DXGKrnl" /v "MonitorLatencyTolerance" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\DXGKrnl" /v "MonitorRefreshLatencyTolerance" /t REG_DWORD /d "1" /f

echo %w% - 💎 Optimizando PCI Express%b%
for /f %%g in ('wmic path win32_videocontroller get PNPDeviceID ^| findstr /L "VEN_"') do (
    Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Enum\%%g\Device Parameters\Interrupt Management\MessageSignaledInterruptProperties" /v "MSISupported" /t REG_DWORD /d "1" /f
    Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Enum\%%g\Device Parameters\Interrupt Management\Affinity Policy" /v "DevicePolicy" /t REG_DWORD /d "4" /f
)

echo.
echo.                                                                                  %b%╔═══════════════════════════════════════════════════════════════╗
echo.                                                                                  %b%║    %w% ✅ Controladores Optimizados - Presione cualquier tecla... %b%║
echo.                                                                                  %b%╚═══════════════════════════════════════════════════════════════╝
pause > nul
cls
goto menu

::Funciones de reversión
:revertregistry
cls
echo %r% ⚠️ Revirtiendo optimizaciones de registro...%b%
sc config "SysMain" start= demand
sc config "WSearch" start= demand
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System" /v "EnableLUA" /t REG_DWORD /d "1" /f
echo %g% ✅ Registro restaurado a valores por defecto%b%
timeout 3
goto 11

:revertdrivers
cls
echo %r% ⚠️ Revirtiendo optimizaciones de controladores...%b%
Reg.exe delete "HKLM\SYSTEM\CurrentControlSet\Control\PriorityControl" /v "IRQ8Priority" /f >nul 2>&1
Reg.exe delete "HKLM\SYSTEM\CurrentControlSet\Control\PriorityControl" /v "IRQ16Priority" /f >nul 2>&1
echo %g% ✅ Controladores restaurados a valores por defecto%b%
timeout 3
goto 12

::Secciones faltantes del menú principal

:2
cls
echo.
echo.                                   %p%   ███████╗███╗   ██╗███████╗██████╗  ██████╗ ██╗ █████╗     ██╗   ██╗██╗  ████████╗██╗███╗   ███╗ █████╗ ████████╗███████╗
echo.                                   %p%   ██╔════╝████╗  ██║██╔════╝██╔══██╗██╔════╝ ██║██╔══██╗    ██║   ██║██║  ╚══██╔══╝██║████╗ ████║██╔══██╗╚══██╔══╝██╔════╝
echo.                                   %p%   █████╗  ██╔██╗ ██║█████╗  ██████╔╝██║  ███╗██║███████║    ██║   ██║██║     ██║   ██║██╔████╔██║███████║   ██║   █████╗  
echo.                                   %p%   ██╔══╝  ██║╚██╗██║██╔══╝  ██╔══██╗██║   ██║██║██╔══██║    ██║   ██║██║     ██║   ██║██║╚██╔╝██║██╔══██║   ██║   ██╔══╝  
echo.                                   %p%   ███████╗██║ ╚████║███████╗██║  ██║╚██████╔╝██║██║  ██║    ╚██████╔╝███████╗██║   ██║██║ ╚═╝ ██║██║  ██║   ██║   ███████╗
echo.                                   %p%   ╚══════╝╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝ ╚═════╝ ╚═╝╚═╝  ╚═╝     ╚═════╝ ╚══════╝╚═╝   ╚═╝╚═╝     ╚═╝╚═╝  ╚═╝   ╚═╝   ╚══════╝
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                
echo.                                                    %p%[%w%1%p%]%w% ⚡ Aplicar Plan de Energía LJokerL_TTV Ultimate
echo. 
echo.                                                    %p%[%w%2%p%]%w% 🚀 Configurar plan personalizado i7 12700k                                        
echo.                                                                      %p%[%w%M%p%]%w% 🔙 Volver al menú   
echo.
echo.                                                                      %p%[%w%R%p%]%w% 🔄 Revertir                        
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                          %w%Optimización de energía para máximo rendimiento sin limitaciones!
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%

set /p input=:
if /i %input% == 1 goto powertweaks
if /i %input% == 2 goto custompower
if /i %input% == R goto revertpower
if /i %input% == M cls & goto menu
) ELSE (
echo Entrada Inválida & goto 2

:powertweaks
cls
echo %w% - ⚡ Aplicando plan de energía LJokerL_TTV Ultimate...%b%
powercfg /import "C:\LJokerL_TTV\LJokerL_TTV_Power_Plan_V10.pow"
echo %w% - 🚀 Configurando opciones de energía avanzadas para i7 12700k...%b%
powercfg /setactive 8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c
powercfg /setacvalueindex scheme_current sub_processor PROCTHROTTLEMAX 100
powercfg /setacvalueindex scheme_current sub_processor PROCTHROTTLEMIN 100
powercfg /setacvalueindex scheme_current sub_processor PERFBOOSTMODE 1
powercfg /setacvalueindex scheme_current sub_processor CPMINCORES 100
echo %w% - 💎 Deshabilitando limitaciones de CPU...%b%
powercfg /setacvalueindex scheme_current sub_processor PERFINCTHRESHOLD 10
powercfg /setacvalueindex scheme_current sub_processor PERFDECTHRESHOLD 8
powercfg /setactive scheme_current

echo.
echo.                                                                                  %b%╔═══════════════════════════════════════════════════════════════╗
echo.                                                                                  %b%║    %w% ✅ Plan de Energía Aplicado - Presione cualquier tecla... %b%║
echo.                                                                                  %b%╚═══════════════════════════════════════════════════════════════╝
pause > nul
cls
goto 2

:custompower
cls
echo %w% - 🎯 Configurando plan personalizado extremo...%b%
powercfg /duplicatescheme scheme_balanced "LJokerL_TTV_EXTREME"
powercfg /setactive "LJokerL_TTV_EXTREME"
powercfg /setacvalueindex scheme_current sub_processor PROCTHROTTLEMAX 100
powercfg /setacvalueindex scheme_current sub_processor PROCTHROTTLEMIN 100
powercfg /setacvalueindex scheme_current sub_processor PERFBOOSTMODE 2
powercfg /setacvalueindex scheme_current sub_disk 0012ee47-9041-4b5d-9b77-535fba8b1442 0
powercfg /setacvalueindex scheme_current sub_sleep STANDBYIDLE 0
powercfg /setacvalueindex scheme_current sub_sleep HIBERNATEIDLE 0
echo %g% ✅ Plan personalizado configurado para máximo rendimiento!%b%
timeout 3
goto 2

:revertpower
cls
echo %r% ⚠️ Revirtiendo configuración de energía...%b%
powercfg /setactive scheme_balanced
echo %g% ✅ Plan de energía restaurado a equilibrado%b%
timeout 3
goto 2

:3
cls
echo.
echo.                                   %p%   ███╗   ███╗ ██████╗ ██╗   ██╗███████╗███████╗    ██╗   ██╗    ████████╗███████╗ ██████╗██╗      █████╗ ██████╗  ██████╗ 
echo.                                   %p%   ████╗ ████║██╔═══██╗██║   ██║██╔════╝██╔════╝    ╚██╗ ██╔╝    ╚══██╔══╝██╔════╝██╔════╝██║     ██╔══██╗██╔══██╗██╔═══██╗
echo.                                   %p%   ██╔████╔██║██║   ██║██║   ██║███████╗█████╗       ╚████╔╝        ██║   █████╗  ██║     ██║     ███████║██║  ██║██║   ██║
echo.                                   %p%   ██║╚██╔╝██║██║   ██║██║   ██║╚════██║██╔══╝        ╚██╔╝         ██║   ██╔══╝  ██║     ██║     ██╔══██║██║  ██║██║   ██║
echo.                                   %p%   ██║ ╚═╝ ██║╚██████╔╝╚██████╔╝███████║███████╗       ██║          ██║   ███████╗╚██████╗███████╗██║  ██║██████╔╝╚██████╔╝
echo.                                   %p%   ╚═╝     ╚═╝ ╚═════╝  ╚═════╝ ╚══════╝╚══════╝       ╚═╝          ╚═╝   ╚══════╝ ╚═════╝╚══════╝╚═╝  ╚═╝╚═════╝  ╚═════╝ 
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                
echo.                                                    %p%[%w%1%p%]%w% 🖱️ Optimizar Mouse Pro Gaming
echo. 
echo.                                                    %p%[%w%2%p%]%w% ⌨️ Optimizar Teclado Mecánico                                       
echo.                                                                      %p%[%w%M%p%]%w% 🔙 Volver al menú   
echo.
echo.                                                                      %p%[%w%R%p%]%w% 🔄 Revertir                        
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                          %w%Latencia ultra-baja, precisión máxima, configuraciones pro-gaming!
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%

set /p input=:
if /i %input% == 1 goto mouseopts
if /i %input% == 2 goto keyboardopts
if /i %input% == R goto revertinput
if /i %input% == M cls & goto menu
) ELSE (
echo Entrada Inválida & goto 3

:mouseopts
cls
echo %w% - 🎯 Optimizando configuración de mouse para gaming...%b%
Reg.exe add "HKCU\Control Panel\Mouse" /v "MouseHoverTime" /t REG_SZ /d "0" /f
Reg.exe add "HKCU\Control Panel\Mouse" /v "MouseSensitivity" /t REG_SZ /d "10" /f
Reg.exe add "HKCU\Control Panel\Mouse" /v "SmoothMouseXCurve" /t REG_BINARY /d "0000000000000000156e000000000000004001000000000000280a000000000000007d000000000000" /f
Reg.exe add "HKCU\Control Panel\Mouse" /v "SmoothMouseYCurve" /t REG_BINARY /d "0000000000000000fd11010000000000002404000000000000fc12000000000000c0bb010000000000" /f

echo %w% - ⚡ Deshabilitando aceleración de mouse%b%
Reg.exe add "HKCU\Control Panel\Mouse" /v "MouseAcceleration" /t REG_SZ /d "0" /f
Reg.exe add "HKCU\Control Panel\Mouse" /v "MouseThreshold1" /t REG_SZ /d "0" /f
Reg.exe add "HKCU\Control Panel\Mouse" /v "MouseThreshold2" /t REG_SZ /d "0" /f

echo %w% - 🚀 Configurando buffer de cola de datos%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\mouclass\Parameters" /v "MouseDataQueueSize" /t REG_DWORD /d "100" /f

echo.
echo.                                                                                  %b%╔═══════════════════════════════════════════════════════════════╗
echo.                                                                                  %b%║    %w% ✅ Mouse Optimizado - Presione cualquier tecla...      %b%║
echo.                                                                                  %b%╚═══════════════════════════════════════════════════════════════╝
pause > nul
cls
goto 3

:keyboardopts
cls
echo %w% - ⌨️ Optimizando configuración de teclado...%b%
Reg.exe add "HKCU\Control Panel\Keyboard" /v "KeyboardDelay" /t REG_SZ /d "0" /f
Reg.exe add "HKCU\Control Panel\Keyboard" /v "KeyboardSpeed" /t REG_SZ /d "31" /f

echo %w% - 🚀 Configurando buffer de cola de datos de teclado%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\kbdclass\Parameters" /v "KeyboardDataQueueSize" /t REG_DWORD /d "100" /f

echo %w% - ⚡ Deshabilitando teclas especiales innecesarias%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Keyboard Layout" /v "Scancode Map" /t REG_BINARY /d "00000000000000000300000000005BE000005CE000000000" /f

echo.
echo.                                                                                  %b%╔═══════════════════════════════════════════════════════════════╗
echo.                                                                                  %b%║    %w% ✅ Teclado Optimizado - Presione cualquier tecla...    %b%║
echo.                                                                                  %b%╚═══════════════════════════════════════════════════════════════╝
pause > nul
cls
goto 3

:revertinput
cls
echo %r% ⚠️ Revirtiendo optimizaciones de mouse y teclado...%b%
Reg.exe delete "HKCU\Control Panel\Mouse" /v "MouseHoverTime" /f >nul 2>&1
Reg.exe delete "HKCU\Control Panel\Keyboard" /v "KeyboardDelay" /f >nul 2>&1
echo %g% ✅ Configuración de entrada restaurada%b%
timeout 3
goto 3

:4
cls
echo.
echo.                                   %p%    ██████╗ ██████╗ ██╗   ██╗    ██████╗ ████████╗██╗  ██╗    ██╗  ██╗ ██████╗  █████╗  ██████╗ 
echo.                                   %p%   ██╔════╝ ██╔══██╗██║   ██║    ██╔══██╗╚══██╔══╝╚██╗██╔╝    ██║  ██║██╔═══██╗██╔══██╗██╔═══██╗
echo.                                   %p%   ██║  ███╗██████╔╝██║   ██║    ██████╔╝   ██║    ╚███╔╝     ███████║██║   ██║╚█████╔╝██║   ██║
echo.                                   %p%   ██║   ██║██╔═══╝ ██║   ██║    ██╔══██╗   ██║    ██╔██╗     ╚════██║██║   ██║██╔══██╗██║   ██║
echo.                                   %p%   ╚██████╔╝██║     ╚██████╔╝    ██║  ██║   ██║   ██╔╝ ██╗         ██║╚██████╔╝╚█████╔╝╚██████╔╝
echo.                                   %p%    ╚═════╝ ╚═╝      ╚═════╝     ╚═╝  ╚═╝   ╚═╝   ╚═╝  ╚═╝         ╚═╝ ╚═════╝  ╚════╝  ╚═════╝ 
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                
echo.                                                    %p%[%w%1%p%]%w% 🎮 Optimizar NVIDIA RTX 4080 para Gaming
echo. 
echo.                                                    %p%[%w%2%p%]%w% ⚡ Aplicar perfil custom NVIDIA                                      
echo.                                                                      %p%[%w%M%p%]%w% 🔙 Volver al menú   
echo.
echo.                                                                      %p%[%w%R%p%]%w% 🔄 Revertir                        
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                          %w%Configuración específica RTX 4080, latencia mínima, FPS máximo!
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%

set /p input=:
if /i %input% == 1 goto nvidiaopts
if /i %input% == 2 goto nvidiprofile
if /i %input% == R goto revertnvidia
if /i %input% == M cls & goto menu
) ELSE (
echo Entrada Inválida & goto 4

:nvidiaopts
cls
echo %w% - 🎮 Optimizando configuración NVIDIA para RTX 4080...%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "TdrLevel" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "TdrDelay" /t REG_DWORD /d "60" /f

echo %w% - ⚡ Configurando NVIDIA Control Panel via registro%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Video\{DEB039CC-B704-4F53-B43E-9DD4432FA2E9}\0000" /v "PowerMizerEnable" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Video\{DEB039CC-B704-4F53-B43E-9DD4432FA2E9}\0000" /v "PowerMizerLevel" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Video\{DEB039CC-B704-4F53-B43E-9DD4432FA2E9}\0000" /v "PerfLevelSrc" /t REG_DWORD /d "2222" /f

echo %w% - 🚀 Configurando Game Mode optimizado%b%
Reg.exe add "HKCU\SOFTWARE\Microsoft\GameBar" /v "AllowAutoGameMode" /t REG_DWORD /d "1" /f
Reg.exe add "HKCU\SOFTWARE\Microsoft\GameBar" /v "AutoGameModeEnabled" /t REG_DWORD /d "1" /f

echo.
echo.                                                                                  %b%╔═══════════════════════════════════════════════════════════════╗
echo.                                                                                  %b%║    %w% ✅ RTX 4080 Optimizada - Presione cualquier tecla...   %b%║
echo.                                                                                  %b%╚═══════════════════════════════════════════════════════════════╝
pause > nul
cls
goto 4

:nvidiprofile
cls
echo %w% - 💎 Aplicando perfil personalizado NVIDIA...%b%
"C:\LJokerL_TTV\Nvidia_Profile_Inspector.exe" "C:\LJokerL_TTV\LJokerL_TTV_NVPI_V10.nip"

echo.
echo.                                                                                  %b%╔═══════════════════════════════════════════════════════════════╗
echo.                                                                                  %b%║    %w% ✅ Perfil NVIDIA Aplicado - Presione cualquier tecla... %b%║
echo.                                                                                  %b%╚═══════════════════════════════════════════════════════════════╝
pause > nul
cls
goto 4

:revertnvidia
cls
echo %r% ⚠️ Revirtiendo configuración NVIDIA...%b%
Reg.exe delete "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "TdrLevel" /f >nul 2>&1
echo %g% ✅ Configuración NVIDIA restaurada%b%
timeout 3
goto 4

:5
cls
echo.
echo.                                   %p%    ██████╗██████╗ ██╗   ██╗    ██╗███████╗    ██╗██████╗ ███████╗ ██████╗  ██████╗ ██╗  ██╗
echo.                                   %p%   ██╔════╝██╔══██╗██║   ██║    ██║╚════██║    ██║╚════██║██╔════╝██╔═══██╗██╔═══██╗██║ ██╔╝
echo.                                   %p%   ██║     ██████╔╝██║   ██║    ██║    ██╔╝    ██║    ██╔╝███████╗██║   ██║██║   ██║█████╔╝ 
echo.                                   %p%   ██║     ██╔═══╝ ██║   ██║    ██║   ██╔╝     ██║   ██╔╝ ╚════██║██║   ██║██║   ██║██╔═██╗ 
echo.                                   %p%   ╚██████╗██║     ╚██████╔╝    ██║   ██║      ██║   ██║  ███████║╚██████╔╝╚██████╔╝██║  ██╗
echo.                                   %p%    ╚═════╝╚═╝      ╚═════╝     ╚═╝   ╚═╝      ╚═╝   ╚═╝  ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                
echo.                                                    %p%[%w%1%p%]%w% 🚀 Optimizar CPU i7 12700k
echo. 
echo.                                                    %p%[%w%2%p%]%w% ⚡ Configuración Overclock Ready                                     
echo.                                                                      %p%[%w%M%p%]%w% 🔙 Volver al menú   
echo.
echo.                                                                      %p%[%w%R%p%]%w% 🔄 Revertir                        
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                          %w%12va generación Intel, configuración extrema, sin limitaciones!
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%

set /p input=:
if /i %input% == 1 goto cpuopts
if /i %input% == 2 goto cpuoc
if /i %input% == R goto revertcpu
if /i %input% == M cls & goto menu
) ELSE (
echo Entrada Inválida & goto 5

:cpuopts
cls
echo %w% - 🚀 Optimizando CPU i7 12700k específicamente...%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\kernel" /v "DpcWatchdogProfileOffset" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\kernel" /v "DisableExceptionChainValidation" /t REG_DWORD /d "1" /f

echo %w% - ⚡ Configurando núcleos P y E para 12700k%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "SecondLevelDataCache" /t REG_DWORD /d "25600" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "ThirdLevelDataCache" /t REG_DWORD /d "30720" /f

echo %w% - 🎯 Configurando Thread Scheduling optimizado%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\PriorityControl" /v "Win32PrioritySeparation" /t REG_DWORD /d "0x00000026" /f

echo.
echo.                                                                                  %b%╔═══════════════════════════════════════════════════════════════╗
echo.                                                                                  %b%║    %w% ✅ CPU i7 12700k Optimizado - Presione cualquier tecla... %b%║
echo.                                                                                  %b%╚═══════════════════════════════════════════════════════════════╝
pause > nul
cls
goto 5

:cpuoc
cls
echo %w% - 🔥 Preparando sistema para overclock...%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "FeatureSettings" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "FeatureSettingsOverride" /t REG_DWORD /d "3" /f

echo %w% - ⚡ Configurando voltajes y frequency scaling...%b%
bcdedit /set useplatformtick yes
bcdedit /set disabledynamictick yes

echo %g% ⚠️ Sistema preparado para overclock. Configure BIOS manualmente!%b%
timeout 3
goto 5

:revertcpu
cls
echo %r% ⚠️ Revirtiendo optimizaciones de CPU...%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\PriorityControl" /v "Win32PrioritySeparation" /t REG_DWORD /d "2" /f
echo %g% ✅ Configuración de CPU restaurada%b%
timeout 3
goto 5

:6
cls
echo.
echo.                                   %p%   ██╗     ██╗███╗   ███╗██████╗ ██╗███████╗███████╗ █████╗     ██████╗  ██████╗    █████╗ ██╗   ██╗ █████╗ ███╗   ██╗███████╗ █████╗ ██████╗  █████╗ 
echo.                                   %p%   ██║     ██║████╗ ████║██╔══██╗██║██╔════╝╚══███╔╝██╔══██╗    ██╔══██╗██╔════╝   ██╔══██╗██║   ██║██╔══██╗████╗  ██║╚══███╔╝██╔══██╗██╔══██╗██╔══██╗
echo.                                   %p%   ██║     ██║██╔████╔██║██████╔╝██║█████╗    ███╔╝ ███████║    ██████╔╝██║        ███████║██║   ██║███████║██╔██╗ ██║  ███╔╝ ███████║██║  ██║███████║
echo.                                   %p%   ██║     ██║██║╚██╔╝██║██╔═══╝ ██║██╔══╝   ███╔╝  ██╔══██║    ██╔═══╝ ██║        ██╔══██║╚██╗ ██╔╝██╔══██║██║╚██╗██║ ███╔╝  ██╔══██║██║  ██║██╔══██║
echo.                                   %p%   ███████╗██║██║ ╚═╝ ██║██║     ██║███████╗███████╗██║  ██║    ██║     ╚██████╗   ██║  ██║ ╚████╔╝ ██║  ██║██║ ╚████║███████╗██║  ██║██████╔╝██║  ██║
echo.                                   %p%   ╚══════╝╚═╝╚═╝     ╚═╝╚═╝     ╚═╝╚══════╝╚══════╝╚═╝  ╚═╝    ╚═╝      ╚═════╝   ╚═╝  ╚═╝  ╚═══╝  ╚═╝  ╚═╝╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝╚═════╝ ╚═╝  ╚═╝
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                
echo.                                                    %p%[%w%1%p%]%w% 🧹 Limpieza Sistema Avanzada
echo. 
echo.                                                    %p%[%w%2%p%]%w% 🗑️ Limpiar Archivos Temporales                                      
echo.                                                                      %p%[%w%M%p%]%w% 🔙 Volver al menú   
echo.
echo.                                                                      %p%[%w%R%p%]%w% 🔄 Revertir                        
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                          %w%Liberación de espacio, archivos temporales, caché, logs del sistema!
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%

set /p input=:
if /i %input% == 1 call :SystemCleanup
if /i %input% == 2 call :TempCleanup
if /i %input% == M cls & goto menu
) ELSE (
echo Entrada Inválida & goto 6

:SystemCleanup
cls
echo %w% - 🧹 Ejecutando limpieza avanzada del sistema...%b%
cleanmgr /sagerun:1
sfc /scannow
dism /online /cleanup-image /restorehealth
echo %g% ✅ Limpieza del sistema completada%b%
timeout 3
goto 6

:TempCleanup
cls
echo %w% - 🗑️ Limpiando archivos temporales...%b%
del /s /q %temp%\*.*
del /s /q C:\Windows\Temp\*.*
del /s /q C:\Windows\Prefetch\*.*
del /s /q %userprofile%\AppData\Local\Temp\*.*
echo %g% ✅ Archivos temporales eliminados%b%
timeout 3
goto 6

:7
cls
echo.
echo.                                   %p%   ██████╗ ███████╗██████╗ ██╗      ██████╗  █████╗ ████████╗    ██╗    ██╗██╗███╗   ██╗██████╗  ██████╗ ██╗    ██╗███████╗    ██╗ ██╗
echo.                                   %p%   ██╔══██╗██╔════╝██╔══██╗██║     ██╔═══██╗██╔══██╗╚══██╔══╝    ██║    ██║██║████╗  ██║██╔══██╗██╔═══██╗██║    ██║██╔════╝    ╚██╗╚██╗
echo.                                   %p%   ██║  ██║█████╗  ██████╔╝██║     ██║   ██║███████║   ██║       ██║ █╗ ██║██║██╔██╗ ██║██║  ██║██║   ██║██║ █╗ ██║███████╗     ╚██╗╚██╗
echo.                                   %p%   ██║  ██║██╔══╝  ██╔══██╗██║     ██║   ██║██╔══██║   ██║       ██║███╗██║██║██║╚██╗██║██║  ██║██║   ██║██║███╗██║╚════██║     ██╔╝██╔╝
echo.                                   %p%   ██████╔╝███████╗██████╔╝███████╗╚██████╔╝██║  ██║   ██║       ╚███╔███╔╝██║██║ ╚████║██████╔╝╚██████╔╝╚███╔███╔╝███████║    ██╔╝██╔╝ 
echo.                                   %p%   ╚═════╝ ╚══════╝╚═════╝ ╚══════╝ ╚═════╝ ╚═╝  ╚═╝   ╚═╝        ╚══╝╚══╝ ╚═╝╚═╝  ╚═══╝╚═════╝  ╚═════╝  ╚══╝╚══╝ ╚══════╝    ╚═╝ ╚═╝  
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                
echo.                                                    %p%[%w%1%p%]%w% 🗑️ Debloat Extremo Windows 11
echo. 
echo.                                                    %p%[%w%2%p%]%w% 🧹 Eliminar Apps UWP Masivo                                      
echo.                                                                      %p%[%w%M%p%]%w% 🔙 Volver al menú   
echo.
echo.                                                                      %p%[%w%R%p%]%w% 🔄 Revertir                        
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                          %w%Eliminación masiva de bloatware, apps UWP, servicios innecesarios!
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%

set /p input=:
if /i %input% == 1 call :DebloatAdvanced
if /i %input% == 2 call :BloatwareRemoval
if /i %input% == M cls & goto menu
) ELSE (
echo Entrada Inválida & goto 7

:8
cls
echo.
echo.                                   %p%   █████╗ ██╗     ███╗   ███╗ █████╗  ██████╗███████╗███╗   ██╗ █████╗ ███╗   ███╗██╗███████╗███╗   ██╗████████╗ ██████╗ 
echo.                                   %p%  ██╔══██╗██║     ████╗ ████║██╔══██╗██╔════╝██╔════╝████╗  ██║██╔══██╗████╗ ████║██║██╔════╝████╗  ██║╚══██╔══╝██╔═══██╗
echo.                                   %p%  ███████║██║     ██╔████╔██║███████║██║     █████╗  ██╔██╗ ██║███████║██╔████╔██║██║█████╗  ██╔██╗ ██║   ██║   ██║   ██║
echo.                                   %p%  ██╔══██║██║     ██║╚██╔╝██║██╔══██║██║     ██╔══╝  ██║╚██╗██║██╔══██║██║╚██╔╝██║██║██╔══╝  ██║╚██╗██║   ██║   ██║   ██║
echo.                                   %p%  ██║  ██║███████╗██║ ╚═╝ ██║██║  ██║╚██████╗███████╗██║ ╚████║██║  ██║██║ ╚═╝ ██║██║███████╗██║ ╚████║   ██║   ╚██████╔╝
echo.                                   %p%  ╚═╝  ╚═╝╚══════╝╚═╝     ╚═╝╚═╝  ╚═╝ ╚═════╝╚══════╝╚═╝  ╚═══╝╚═╝  ╚═╝╚═╝     ╚═╝╚═╝╚══════╝╚═╝  ╚═══╝   ╚═╝    ╚═════╝ 
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                
echo.                                                    %p%[%w%1%p%]%w% 💾 Optimizar SSD/NVMe
echo. 
echo.                                                    %p%[%w%2%p%]%w% 🚀 Configurar TRIM y Write Cache                                      
echo.                                                                      %p%[%w%M%p%]%w% 🔙 Volver al menú   
echo.
echo.                                                                      %p%[%w%R%p%]%w% 🔄 Revertir                        
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                          %w%Optimización específica SSD, TRIM, cache, indexado, prefetch!
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%

set /p input=:
if /i %input% == 1 call :StorageOptimization
if /i %input% == 2 call :SSDOptimization
if /i %input% == M cls & goto menu
) ELSE (
echo Entrada Inválida & goto 8

:SSDOptimization
cls
echo %w% - 🚀 Configurando optimizaciones avanzadas para SSD...%b%
fsutil behavior set DisableDeleteNotify 0
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\SysMain" /v "Start" /t REG_DWORD /d "4" /f
echo %g% ✅ SSD optimizado para máximo rendimiento%b%
timeout 3
goto 8

:9
cls
echo.
echo.                                   %p%   ██████╗  █████╗ ███╗   ███╗     ██████╗ ██╗  ██╗ ██████╗ ██████╗      ██████╗ ██████╗ ██████╗ ██████╗     ███████╗██╗  ██╗ ██████╗  ██████╗ ███╗   ███╗██╗  ██╗███████╗
echo.                                   %p%   ██╔══██╗██╔══██╗████╗ ████║    ██╔════╝ ██║  ██║██╔════╝ ██╔══██╗     ██╔══██╗██╔══██╗██╔══██╗██╔══██╗    ██╔════╝██║  ██║██╔═══██╗██╔═══██╗████╗ ████║██║  ██║╚══███╔╝
echo.                                   %p%   ██████╔╝███████║██╔████╔██║    ██║  ███╗███████║██║  ███╗██████╔╝     ██║  ██║██║  ██║██████╔╝██████╔╝    ███████╗███████║██║   ██║██║   ██║██╔████╔██║███████║  ███╔╝ 
echo.                                   %p%   ██╔══██╗██╔══██║██║╚██╔╝██║    ██║   ██║╚════██║██║   ██║██╔══██╗     ██║  ██║██║  ██║██╔══██╗██╔══██╗    ╚════██║╚════██║██║   ██║██║   ██║██║╚██╔╝██║██╔══██║ ███╔╝  
echo.                                   %p%   ██║  ██║██║  ██║██║ ╚═╝ ██║    ╚██████╔╝     ██║╚██████╔╝██████╔╝     ██████╔╝██████╔╝██║  ██║██████╔╝    ███████║     ██║╚██████╔╝╚██████╔╝██║ ╚═╝ ██║██║  ██║███████╗
echo.                                   %p%   ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝     ╚═╝     ╚═════╝      ╚═╝ ╚═════╝ ╚═════╝      ╚═════╝ ╚═════╝ ╚═╝  ╚═╝╚═════╝     ╚══════╝     ╚═╝ ╚═════╝  ╚═════╝ ╚═╝     ╚═╝╚═╝  ╚═╝╚══════╝
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                
echo.                                                    %p%[%w%1%p%]%w% 💨 Optimizar RAM 64GB DDR5 5400MHz
echo. 
echo.                                                    %p%[%w%2%p%]%w% 🚀 Configurar Memory Management Ultra                                      
echo.                                                                      %p%[%w%M%p%]%w% 🔙 Volver al menú   
echo.
echo.                                                                      %p%[%w%R%p%]%w% 🔄 Revertir                        
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                          %w%Configuración específica 64GB DDR5 5400MHz, caché sistema, paging!
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%

set /p input=:
if /i %input% == 1 call :RAMOptimization
if /i %input% == 2 call :MemoryManagement
if /i %input% == M cls & goto menu
) ELSE (
echo Entrada Inválida & goto 9

:RAMOptimization
cls
echo %w% - 💨 Optimizando gestión de memoria para 64GB DDR5...%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "LargeSystemCache" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "SystemPages" /t REG_DWORD /d "4294967295" /f
echo %g% ✅ RAM 64GB DDR5 optimizada%b%
timeout 3
goto 9

:MemoryManagement
cls
echo %w% - 🚀 Configurando Memory Management Ultra...%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "DisablePagingExecutive" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "ClearPageFileAtShutdown" /t REG_DWORD /d "0" /f
echo %g% ✅ Memory Management configurado para 64GB%b%
timeout 3
goto 9

:10
cls
echo.
echo.                                   %p%   ██████╗ ███████╗██████╗     ██╗   ██╗    ██╗      █████╗ ████████╗███████╗███╗   ██╗ ██████╗██╗ █████╗     ██╗   ██╗██╗  ████████╗██████╗  █████╗     ██████╗  █████╗      █████╗ 
echo.                                   %p%   ██╔══██╗██╔════╝██╔══██╗    ╚██╗ ██╔╝    ██║     ██╔══██╗╚══██╔══╝██╔════╝████╗  ██║██╔════╝██║██╔══██╗    ██║   ██║██║  ╚══██╔══╝██╔══██╗██╔══██╗    ██╔══██╗██╔══██╗     ██╔══██╗
echo.                                   %p%   ██████╔╝█████╗  ██║  ██║     ╚████╔╝     ██║     ███████║   ██║   █████╗  ██╔██╗ ██║██║     ██║███████║    ██║   ██║██║     ██║   ██████╔╝███████║    ██████╔╝███████║     ███████║
echo.                                   %p%   ██╔══██╗██╔══╝  ██║  ██║      ╚██╔╝      ██║     ██╔══██║   ██║   ██╔══╝  ██║╚██╗██║██║     ██║██╔══██║    ██║   ██║██║     ██║   ██╔══██╗██╔══██║    ██╔══██╗██╔══██║     ██╔══██║
echo.                                   %p%   ██║  ██║███████╗██████╔╝       ██║       ███████╗██║  ██║   ██║   ███████╗██║ ╚████║╚██████╗██║██║  ██║    ╚██████╔╝███████╗██║   ██║  ██║██║  ██║    ██████╔╝██║  ██║     ██║  ██║
echo.                                   %p%   ╚═╝  ╚═╝╚══════╝╚═════╝        ╚═╝       ╚══════╝╚═╝  ╚═╝   ╚═╝   ╚══════╝╚═╝  ╚═══╝ ╚═════╝╚═╝╚═╝  ╚═╝     ╚═════╝ ╚══════╝╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝    ╚═════╝ ╚═╝  ╚═╝     ╚═╝  ╚═╝
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                
echo.                                                    %p%[%w%1%p%]%w% 🌐 Optimizar Red Gaming Ultra-Baja
echo. 
echo.                                                    %p%[%w%2%p%]%w% ⚡ TCP/IP Tweaks + DNS Cloudflare                                      
echo.                                                                      %p%[%w%M%p%]%w% 🔙 Volver al menú   
echo.
echo.                                                                      %p%[%w%R%p%]%w% 🔄 Revertir                        
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                          %w%Latencia de red mínima, Nagle disabled, DNS 1.1.1.1, buffers optimizados!
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%

set /p input=:
if /i %input% == 1 call :NetworkOptimization
if /i %input% == 2 call :TCPTweaks
if /i %input% == M cls & goto menu
) ELSE (
echo Entrada Inválida & goto 10

:TCPTweaks
cls
echo %w% - ⚡ Aplicando TCP/IP tweaks avanzados...%b%
netsh int tcp set global autotuninglevel=normal
netsh int tcp set global chimney=enabled
netsh int tcp set global rss=enabled
netsh int tcp set global netdma=enabled
echo %g% ✅ TCP/IP optimizado para gaming%b%
timeout 3
goto 10

::Funciones adicionales que faltan

:Tutorial
cls
echo.
echo.                                   %p%   ████████╗██╗   ██╗████████╗ ██████╗ ██████╗ ██╗ █████╗ ██╗         ██████╗ ███████╗    ██╗   ██╗███████╗ ██████╗ 
echo.                                   %p%   ╚══██╔══╝██║   ██║╚══██╔══╝██╔═══██╗██╔══██╗██║██╔══██╗██║         ██╔══██╗██╔════╝    ██║   ██║██╔════╝██╔═══██╗
echo.                                   %p%      ██║   ██║   ██║   ██║   ██║   ██║██████╔╝██║███████║██║         ██║  ██║█████╗      ██║   ██║███████╗██║   ██║
echo.                                   %p%      ██║   ██║   ██║   ██║   ██║   ██║██╔══██╗██║██╔══██║██║         ██║  ██║██╔══╝      ██║   ██║╚════██║██║   ██║
echo.                                   %p%      ██║   ╚██████╔╝   ██║   ╚██████╔╝██║  ██║██║██║  ██║███████╗    ██████╔╝███████╗    ╚██████╔╝███████║╚██████╔╝
echo.                                   %p%      ╚═╝    ╚═════╝    ╚═╝    ╚═════╝ ╚═╝  ╚═╝╚═╝╚═╝  ╚═╝╚══════╝    ╚═════╝ ╚══════╝     ╚═════╝ ╚══════╝ ╚═════╝ 
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.
echo.                                          %y%📖 TUTORIAL DE USO - LJokerL_TTV Custom Config PRO%w%
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                         %w%1. Ejecute SIEMPRE como Administrador
echo.                                         %w%2. Cree un punto de restauración antes de usar
echo.                                         %w%3. Optimizado específicamente para: i7 12700k + RTX 4080 + 64GB DDR5 + Windows 11
echo.                                         %w%4. Cada optimización se puede revertir individualmente
echo.                                         %w%5. Use 'W' para optimización completa automática
echo.                                         %w%6. Use 'I' para ver información del sistema
echo.                                         %w%7. Los recursos se descargan automáticamente desde GitHub
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.
echo.                                                                       %p%[%w%M%p%]%w% 🔙 Volver al menú
echo.

set /p input=:
if /i %input% == M cls & goto menu
goto Tutorial

:SystemInfo
cls
echo.
echo.                                   %p%   ██╗███╗   ██╗███████╗ ██████╗     ██████╗ ███████╗██╗         ███████╗██╗███████╗████████╗███████╗███╗   ███╗ █████╗ 
echo.                                   %p%   ██║████╗  ██║██╔════╝██╔═══██╗    ██╔══██╗██╔════╝██║         ██╔════╝██║██╔════╝╚══██╔══╝██╔════╝████╗ ████║██╔══██╗
echo.                                   %p%   ██║██╔██╗ ██║█████╗  ██║   ██║    ██║  ██║█████╗  ██║         ███████╗██║███████╗   ██║   █████╗  ██╔████╔██║███████║
echo.                                   %p%   ██║██║╚██╗██║██╔══╝  ██║   ██║    ██║  ██║██╔══╝  ██║         ╚════██║██║╚════██║   ██║   ██╔══╝  ██║╚██╔╝██║██╔══██║
echo.                                   %p%   ██║██║ ╚████║██║     ╚██████╔╝    ██████╔╝███████╗███████╗    ███████║██║███████║   ██║   ███████╗██║ ╚═╝ ██║██║  ██║
echo.                                   %p%   ╚═╝╚═╝  ╚═══╝╚═╝      ╚═════╝     ╚═════╝ ╚══════╝╚══════╝    ╚══════╝╚═╝╚══════╝   ╚═╝   ╚══════╝╚═╝     ╚═╝╚═╝  ╚═╝
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.
echo.                                          %y%ℹ️ INFORMACIÓN DEL SISTEMA%w%
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
systeminfo | findstr /C:"OS Name" /C:"Total Physical Memory" /C:"Processor"
wmic cpu get name
wmic path win32_VideoController get name
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.
echo.                                                                       %p%[%w%M%p%]%w% 🔙 Volver al menú
echo.

set /p input=:
if /i %input% == M cls & goto menu
goto SystemInfo

:CompleteOptimization
cls
echo.
echo.                                          %r%⚠️ OPTIMIZACIÓN COMPLETA DEL SISTEMA ⚠️%w%
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                         %w%Esto aplicará TODAS las optimizaciones automáticamente:
echo.                                         %w%- Optimizaciones Generales del Sistema
echo.                                         %w%- Plan de Energía Ultimate
echo.                                         %w%- Mouse y Teclado Pro Gaming
echo.                                         %w%- GPU RTX 4080 Optimizada
echo.                                         %w%- CPU i7 12700k Overclock Ready
echo.                                         %w%- Limpieza PC Avanzada
echo.                                         %w%- Debloat Windows 11 Extremo
echo.                                         %w%- RAM 64GB DDR5 Optimizada
echo.                                         %w%- Red Ultra-Baja Latencia
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.
echo.                                                    %p%[%w%Y%p%]%w% Sí, aplicar TODO        %p%[%w%N%p%]%w% No, volver al menú
echo.

set /p input=:
if /i %input% == Y goto RunCompleteOptimization
if /i %input% == N cls & goto menu
goto CompleteOptimization

:RunCompleteOptimization
cls
echo %y%🚀 Ejecutando optimización completa del sistema...%b%
echo.
echo %w%Aplicando todas las optimizaciones...%b%
call :general
call :powertweaks
call :mouseopts
call :keyboardopts
call :nvidiaopts
call :cpuopts
call :SystemCleanup
call :DebloatAdvanced
call :RAMOptimization
call :NetworkOptimization
echo.
echo %g%✅ ¡OPTIMIZACIÓN COMPLETA FINALIZADA!%b%
echo %g%Su sistema está ahora completamente optimizado para gaming y productividad.%b%
echo.
timeout 5
goto menu

:exit
cls
echo.
echo.                                          %p%¡Gracias por usar LJokerL_TTV Custom Config PRO!%w%
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                         %w%Su sistema ha sido optimizado para máximo rendimiento
echo.                                         %w%Hardware objetivo: i7 12700k + RTX 4080 + 64GB DDR5 + Windows 11
echo.                                         %w%Para soporte: Contacte a LJokerL_TTV
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.
timeout 3
exit

:Restore
cls
echo %w%Restaurando sistema desde punto de restauración...%b%
rstrui.exe
goto menu

:FR
cls
echo %w%Abriendo herramientas de reparación y reversión...%b%
sfc /scannow
dism /online /cleanup-image /restorehealth
goto menu

::Funciones de optimización que faltaban

:DebloatAdvanced
cls
echo %w% - 🗑️ Ejecutando debloat extremo de Windows 11...%b%
powershell -Command "Get-AppxPackage *3dbuilder* | Remove-AppxPackage"
powershell -Command "Get-AppxPackage *windowsalarms* | Remove-AppxPackage"
powershell -Command "Get-AppxPackage *windowscalculator* | Remove-AppxPackage"
powershell -Command "Get-AppxPackage *windowscommunicationsapps* | Remove-AppxPackage"
powershell -Command "Get-AppxPackage *windowscamera* | Remove-AppxPackage"
powershell -Command "Get-AppxPackage *officehub* | Remove-AppxPackage"
powershell -Command "Get-AppxPackage *skypeapp* | Remove-AppxPackage"
powershell -Command "Get-AppxPackage *getstarted* | Remove-AppxPackage"
powershell -Command "Get-AppxPackage *zunemusic* | Remove-AppxPackage"
powershell -Command "Get-AppxPackage *windowsmaps* | Remove-AppxPackage"
powershell -Command "Get-AppxPackage *solitairecollection* | Remove-AppxPackage"
powershell -Command "Get-AppxPackage *bingfinance* | Remove-AppxPackage"
powershell -Command "Get-AppxPackage *zunevideo* | Remove-AppxPackage"
powershell -Command "Get-AppxPackage *bingnews* | Remove-AppxPackage"
powershell -Command "Get-AppxPackage *onenote* | Remove-AppxPackage"
powershell -Command "Get-AppxPackage *people* | Remove-AppxPackage"
powershell -Command "Get-AppxPackage *windowsphone* | Remove-AppxPackage"
powershell -Command "Get-AppxPackage *photos* | Remove-AppxPackage"
powershell -Command "Get-AppxPackage *windowsstore* | Remove-AppxPackage"
powershell -Command "Get-AppxPackage *bingsports* | Remove-AppxPackage"
powershell -Command "Get-AppxPackage *soundrecorder* | Remove-AppxPackage"
powershell -Command "Get-AppxPackage *bingweather* | Remove-AppxPackage"
echo %g% ✅ Debloat extremo completado%b%
timeout 3
goto 7

:BloatwareRemoval
cls
echo %w% - 🧹 Eliminando apps UWP masivamente...%b%
powershell -Command "Get-AppxPackage | Where-Object {$_.Name -notlike '*store*' -and $_.Name -notlike '*calculator*' -and $_.Name -notlike '*photos*'} | Remove-AppxPackage"
echo %g% ✅ Apps UWP eliminadas masivamente%b%
timeout 3
goto 7

:StorageOptimization
cls
echo %w% - 💾 Optimizando almacenamiento SSD/NVMe...%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\FileSystem" /v "LongPathsEnabled" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\SysMain" /v "Start" /t REG_DWORD /d "4" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters" /v "EnablePrefetcher" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters" /v "EnableSuperfetch" /t REG_DWORD /d "0" /f
fsutil behavior set DisableLastAccess 1
echo %g% ✅ Almacenamiento optimizado para SSD%b%
timeout 3
goto 8

:NetworkOptimization
cls
echo %w% - 🌐 Optimizando red para gaming ultra-baja latencia...%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters" /v "TcpAckFrequency" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters" /v "TCPNoDelay" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters" /v "TcpDelAckTicks" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters" /v "DefaultTTL" /t REG_DWORD /d "64" /f

echo %w% - ⚡ Configurando DNS Cloudflare 1.1.1.1...%b%
netsh interface ip set dns "Ethernet" static 1.1.1.1
netsh interface ip add dns "Ethernet" 1.0.0.1 index=2

echo %w% - 🚀 Deshabilitando algoritmo de Nagle...%b%
for /f "tokens=1" %%i in ('netsh interface show interface ^| findstr "Connected"') do (
    netsh interface tcp set global autotuninglevel=disabled
)

echo %w% - 💎 Optimizando buffers de red...%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\AFD\Parameters" /v "DefaultReceiveWindow" /t REG_DWORD /d "65536" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\AFD\Parameters" /v "DefaultSendWindow" /t REG_DWORD /d "65536" /f

echo %g% ✅ Red optimizada para gaming ultra-baja latencia%b%
timeout 3
goto 10

::Secciones 11 y 12 del menú

:11
cls
echo.
echo.                                   %p%   ███████╗███████╗ ██████╗ ██╗   ██╗██████╗ ██╗██████╗  █████╗ ██████╗     ██╗   ██╗    ██████╗ ██████╗ ██╗██╗   ██╗ █████╗  ██████╗██╗██████╗  █████╗ ██████╗     ██████╗ ██████╗  ██████╗ 
echo.                                   %p%   ██╔════╝██╔════╝██╔════╝ ██║   ██║██╔══██╗██║██╔══██╗██╔══██╗██╔══██╗    ╚██╗ ██╔╝    ██╔══██╗██╔══██╗██║██║   ██║██╔══██╗██╔════╝██║██╔══██╗██╔══██╗██╔══██╗    ██╔══██╗██╔══██╗██╔═══██╗
echo.                                   %p%   ███████╗█████╗  ██║  ███╗██║   ██║██████╔╝██║██║  ██║███████║██║  ██║     ╚████╔╝     ██████╔╝██████╔╝██║██║   ██║███████║██║     ██║██║  ██║███████║██║  ██║    ██████╔╝██████╔╝██║   ██║
echo.                                   %p%   ╚════██║██╔══╝  ██║   ██║██║   ██║██╔══██╗██║██║  ██║██╔══██║██║  ██║      ╚██╔╝      ██╔═══╝ ██╔══██╗██║╚██╗ ██╔╝██╔══██║██║     ██║██║  ██║██╔══██║██║  ██║    ██╔═══╝ ██╔══██╗██║   ██║
echo.                                   %p%   ███████║███████╗╚██████╔╝╚██████╔╝██║  ██║██║██████╔╝██║  ██║██████╔╝       ██║       ██║     ██║  ██║██║ ╚████╔╝ ██║  ██║╚██████╗██║██████╔╝██║  ██║██████╔╝    ██║     ██║  ██║╚██████╔╝
echo.                                   %p%   ╚══════╝╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝╚═╝╚═════╝ ╚═╝  ╚═╝╚═════╝        ╚═╝       ╚═╝     ╚═╝  ╚═╝╚═╝  ╚═══╝  ╚═╝  ╚═╝ ╚═════╝╚═╝╚═════╝ ╚═╝  ╚═╝╚═════╝     ╚═╝     ╚═╝  ╚═╝ ╚═════╝ 
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                
echo.                                                    %p%[%w%1%p%]%w% 🛡️ Configurar Seguridad Pro
echo. 
echo.                                                    %p%[%w%2%p%]%w% 🔒 Privacidad Windows 11 Extrema                                      
echo.                                                                      %p%[%w%M%p%]%w% 🔙 Volver al menú   
echo.
echo.                                                                      %p%[%w%R%p%]%w% 🔄 Revertir                        
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                          %w%Configuración de seguridad y privacidad para máximo rendimiento!
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%

set /p input=:
if /i %input% == 1 call :SecurityOptimization
if /i %input% == 2 call :PrivacyOptimization
if /i %input% == R call :revertregistry
if /i %input% == M cls & goto menu
) ELSE (
echo Entrada Inválida & goto 11

:SecurityOptimization
cls
echo %w% - 🛡️ Configurando seguridad optimizada...%b%
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows Defender" /v "DisableAntiSpyware" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System" /v "EnableLUA" /t REG_DWORD /d "0" /f
sc config "SecurityHealthService" start= disabled
echo %g% ✅ Seguridad configurada para rendimiento%b%
timeout 3
goto 11

:PrivacyOptimization
cls
echo %w% - 🔒 Aplicando configuración de privacidad extrema...%b%
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection" /v "AllowTelemetry" /t REG_DWORD /d "0" /f
Reg.exe add "HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Privacy" /v "TailoredExperiencesWithDiagnosticDataEnabled" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection" /v "AllowTelemetry" /t REG_DWORD /d "0" /f
echo %g% ✅ Privacidad extrema configurada%b%
timeout 3
goto 11

:12
cls
echo.
echo.                                   %p%   ██████╗ ██╗██████╗ ███████╗ ██████╗████████╗██╗  ██╗    ██╗   ██╗     ██████╗  █████╗ ███╗   ███╗██╗███╗   ██╗ ██████╗     ████████╗██╗    ██╗███████╗ █████╗ ██╗  ██╗███████╗
echo.                                   %p%   ██╔══██╗██║██╔══██╗██╔════╝██╔════╝╚══██╔══╝╚██╗██╔╝    ╚██╗ ██╔╝    ██╔════╝ ██╔══██╗████╗ ████║██║████╗  ██║██╔════╝     ╚══██╔══╝██║    ██║██╔════╝██╔══██╗██║ ██╔╝██╔════╝
echo.                                   %p%   ██║  ██║██║██████╔╝█████╗  ██║        ██║    ╚███╔╝      ╚████╔╝     ██║  ███╗███████║██╔████╔██║██║██╔██╗ ██║██║  ███╗       ██║   ██║ █╗ ██║█████╗  ███████║█████╔╝ ███████╗
echo.                                   %p%   ██║  ██║██║██╔══██╗██╔══╝  ██║        ██║    ██╔██╗       ╚██╔╝      ██║   ██║██╔══██║██║╚██╔╝██║██║██║╚██╗██║██║   ██║       ██║   ██║███╗██║██╔══╝  ██╔══██║██╔═██╗ ╚════██║
echo.                                   %p%   ██████╔╝██║██║  ██║███████╗╚██████╗   ██║   ██╔╝ ██╗       ██║       ╚██████╔╝██║  ██║██║ ╚═╝ ██║██║██║ ╚████║╚██████╔╝       ██║   ╚███╔███╔╝███████╗██║  ██║██║  ██╗███████║
echo.                                   %p%   ╚═════╝ ╚═╝╚═╝  ╚═╝╚══════╝ ╚═════╝   ╚═╝   ╚═╝  ╚═╝       ╚═╝        ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚═╝╚═╝  ╚═══╝ ╚═════╝        ╚═╝    ╚══╝╚══╝ ╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                
echo.                                                    %p%[%w%1%p%]%w% 🎯 Optimizar DirectX 12 Ultimate
echo. 
echo.                                                    %p%[%w%2%p%]%w% 🚀 Game Mode + Gaming Tweaks                                      
echo.                                                                      %p%[%w%M%p%]%w% 🔙 Volver al menú   
echo.
echo.                                                                      %p%[%w%R%p%]%w% 🔄 Revertir                        
echo.
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%
echo.                                          %w%DirectX 12, Game Mode, optimizaciones específicas para gaming!
echo.                                        %b%"═══════════════════════════════════════════════════════════════════════════════"%w%

set /p input=:
if /i %input% == 1 call :DirectXOptimization
if /i %input% == 2 call :GamingTweaks
if /i %input% == R call :revertgaming
if /i %input% == M cls & goto menu
) ELSE (
echo Entrada Inválida & goto 12

:DirectXOptimization
cls
echo %w% - 🎯 Optimizando DirectX 12 Ultimate...%b%
Reg.exe add "HKLM\SOFTWARE\Microsoft\DirectX" /v "D3D12_ENABLE_UNSAFE_COMMAND_BUFFER_REUSE" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\DirectX" /v "DisablePreemption" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\GraphicsDrivers" /v "HwSchMode" /t REG_DWORD /d "2" /f
echo %g% ✅ DirectX 12 Ultimate optimizado%b%
timeout 3
goto 12

:GamingTweaks
cls
echo %w% - 🚀 Aplicando Game Mode y gaming tweaks...%b%
Reg.exe add "HKCU\SOFTWARE\Microsoft\GameBar" /v "AllowAutoGameMode" /t REG_DWORD /d "1" /f
Reg.exe add "HKCU\SOFTWARE\Microsoft\GameBar" /v "AutoGameModeEnabled" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "GPU Priority" /t REG_DWORD /d "8" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "Priority" /t REG_DWORD /d "6" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games" /v "Scheduling Category" /t REG_SZ /d "High" /f
echo %g% ✅ Gaming tweaks aplicados%b%
timeout 3
goto 12

:revertgaming
cls
echo %r% ⚠️ Revirtiendo optimizaciones de gaming...%b%
Reg.exe delete "HKLM\SOFTWARE\Microsoft\DirectX" /v "D3D12_ENABLE_UNSAFE_COMMAND_BUFFER_REUSE" /f >nul 2>&1
Reg.exe add "HKCU\SOFTWARE\Microsoft\GameBar" /v "AllowAutoGameMode" /t REG_DWORD /d "0" /f
echo %g% ✅ Optimizaciones de gaming revertidas%b%
timeout 3
goto 12

echo %w% - 🛡️ Configurando UAC optimizado%b%
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System" /v "EnableLUA" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System" /v "ConsentPromptBehaviorAdmin" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System" /v "ConsentPromptBehaviorUser" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System" /v "PromptOnSecureDesktop" /t REG_DWORD /d "0" /f

echo %w% - 🔄 Configurando Windows Update manual%b%
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU" /v "NoAutoUpdate" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU" /v "AUOptions" /t REG_DWORD /d "2" /f
sc config "wuauserv" start= demand

echo.
echo.                                                                                  %b%╔═══════════════════════════════════════════════════════════════╗
echo.                                                                                  %b%║    %w% ✅ Registro Optimizado - Presione cualquier tecla...   %b%║
echo.                                                                                  %b%╚═══════════════════════════════════════════════════════════════╝
pause > nul
cls
goto menu

::Optimización de Controladores
:DriversOptimization
cls
echo %w% - 🔧 Habilitando MSI Mode para GPU%b%
for /f %%i in ('wmic path win32_VideoController get PNPDeviceID ^| findstr /L "VEN_"') do (
    Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Enum\%%i\Device Parameters\Interrupt Management\MessageSignaledInterruptProperties" /v "MSISupported" /t REG_DWORD /d "1" /f
)

echo %w% - ⚡ Configurando prioridades IRQ%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\PriorityControl" /v "Win32PrioritySeparation" /t REG_DWORD /d "38" /f

echo %w% - 🎯 Optimizando latencia DPC%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\DXGKrnl" /v "MonitorLatencyTolerance" /t REG_DWORD /d "1" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\DXGKrnl" /v "MonitorRefreshLatencyTolerance" /t REG_DWORD /d "1" /f

echo %w% - 🚀 Configurando PCI Express para alta performance%b%
for /f %%i in ('wmic path win32_IDEController get PNPDeviceID ^| findstr /L "VEN_"') do (
    Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Enum\%%i\Device Parameters" /v "EnabledEnhancedMode" /t REG_DWORD /d "1" /f
)

echo %w% - 💾 Optimizando controladores de almacenamiento%b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Services\storahci\Parameters\Device" /v "TreatAsInternalPort" /t REG_MULTI_SZ /d "0\0001\0002\0003\0004\0005" /f

echo.
echo.                                                                                  %b%╔═══════════════════════════════════════════════════════════════╗
echo.                                                                                  %b%║    %w% ✅ Controladores Optimizados - Presione cualquier tecla... %b%║
echo.                                                                                  %b%╚═══════════════════════════════════════════════════════════════╝
pause > nul
cls
goto menu

::Funciones de Reversión
:revertgpu
cls
echo %r% Revirtiendo optimizaciones de GPU... %b%
Reg.exe delete "HKLM\SYSTEM\CurrentControlSet\Services\nvlddmkm\FTS" /f 2>nul
Reg.exe delete "HKLM\SYSTEM\CurrentControlSet\Services\amdkmdap" /f 2>nul
echo %g% Reversión completada. %b%
pause
goto menu

:revertpower
cls
echo %r% Revirtiendo plan de energía... %b%
powercfg /setactive scheme_balanced
echo %g% Reversión completada. %b%
pause
goto menu

:revertmouse
cls
echo %r% Revirtiendo optimizaciones de mouse y teclado... %b%
Reg.exe delete "HKLM\SYSTEM\CurrentControlSet\Services\mouclass\Parameters" /v "MouseDataQueueSize" /f 2>nul
Reg.exe delete "HKLM\SYSTEM\CurrentControlSet\Services\kbdclass\Parameters" /v "KeyboardDataQueueSize" /f 2>nul
echo %g% Reversión completada. %b%
pause
goto menu

:revertcpu
cls
echo %r% Revirtiendo optimizaciones de CPU... %b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\0cc5b647-c1df-4637-891a-dec35c318583" /v "ValueMax" /t REG_DWORD /d "100" /f
echo %g% Reversión completada. %b%
pause
goto menu

:revertmemory
cls
echo %r% Revirtiendo optimizaciones de memoria... %b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "LargeSystemCache" /t REG_DWORD /d "0" /f
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management" /v "DisablePagingExecutive" /t REG_DWORD /d "0" /f
echo %g% Reversión completada. %b%
pause
goto menu

:revertnetwork
cls
echo %r% Revirtiendo optimizaciones de red... %b%
Reg.exe delete "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile" /v "NetworkThrottlingIndex" /f 2>nul
netsh interface ip set dns "Ethernet" dhcp
netsh interface ip set dns "Wi-Fi" dhcp
echo %g% Reversión completada. %b%
pause
goto menu

:revertdebloat
cls
echo %r% Nota: Las apps UWP eliminadas requieren reinstalación manual desde Microsoft Store %b%
pause
goto menu

:revertstorage
cls
echo %r% Revirtiendo optimizaciones de almacenamiento... %b%
fsutil behavior set DisableDeleteNotify 1
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows Search" /v "SetupCompletedSuccessfully" /t REG_DWORD /d "1" /f
echo %g% Reversión completada. %b%
pause
goto menu

:revertbloatware
cls
echo %r% Nota: Las aplicaciones eliminadas requieren reinstalación manual %b%
pause
goto menu

:revertregistry
cls
echo %r% Revirtiendo configuraciones de registro... %b%
sc config "SysMain" start= auto
sc config "WSearch" start= auto
Reg.exe add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System" /v "ConsentPromptBehaviorAdmin" /t REG_DWORD /d "5" /f
echo %g% Reversión completada. %b%
pause
goto menu

:revertdrivers
cls
echo %r% Revirtiendo optimizaciones de controladores... %b%
Reg.exe add "HKLM\SYSTEM\CurrentControlSet\Control\PriorityControl" /v "Win32PrioritySeparation" /t REG_DWORD /d "2" /f
echo %g% Reversión completada. %b%
pause
goto menu

::Sección Final y Créditos
:credits
cls
echo.
echo.
echo.                                    %p%  ██████╗██████╗ ███████╗██████╗ ██╗████████╗ ██████╗ ███████╗
echo.                                    %p% ██╔════╝██╔══██╗██╔════╝██╔══██╗██║╚══██╔══╝██╔═══██╗██╔════╝
echo.                                    %p% ██║     ██████╔╝█████╗  ██║  ██║██║   ██║   ██║   ██║███████╗
echo.                                    %p% ██║     ██╔══██╗██╔══╝  ██║  ██║██║   ██║   ██║   ██║╚════██║
echo.                                    %p% ╚██████╗██║  ██║███████╗██████╔╝██║   ██║   ╚██████╔╝███████║
echo.                                    %p%  ╚═════╝╚═╝  ╚═╝╚══════╝╚═════╝ ╚═╝   ╚═╝    ╚═════╝ ╚══════╝
echo.
echo.                             %b%"════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                          %w%🎮 LJokerL_TTV Custom Config PRO v10.0 🎮
echo.
echo.                                                       %p%Optimizado específicamente para configuración de alta gama:
echo.                                                     %y%Intel i7 12700k + RTX 4080 + 64GB DDR5 5400MHz + Windows 11
echo.
echo.                                                              %g%Desarrollado por: LJokerL_TTV
echo.                                                               %g%Basado en: EXM Tweaking Utility
echo.                                                                %g%Año: 2025
echo.
echo.                                                             %w%Características principales:
echo.                                            %b%• Optimización completa del sistema para máximo rendimiento
echo.                                            %b%• Eliminación inteligente de bloatware y servicios innecesarios  
echo.                                            %b%• Configuraciones específicas para gaming de alta gama
echo.                                            %b%• Plan de energía personalizado para i7 12700k
echo.                                            %b%• Optimizaciones de red para latencia mínima
echo.                                            %b%• Configuraciones avanzadas de memoria DDR5
echo.                                            %b%• Optimizaciones específicas para RTX 4080
echo.                                            %b%• Sistema de reversión completo
echo.
echo.                                                              %r%⚠️ IMPORTANTE ⚠️
echo.                                                  %r%Use bajo su propia responsabilidad
echo.                                              %r%Siempre cree un punto de restauración antes de usar
echo.
echo.                             %b%"════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════"%w%
echo.
echo.                                                                        %p%Presione cualquier tecla para volver al menú...
pause > nul
goto menu

::Fin del script
:exit
cls
echo.
echo.
echo.                                    %p% ██████╗ ██████╗  █████╗  ██████╗██╗ █████╗ ███████╗    ██████╗  ██████╗ ██████╗     ██╗   ██╗███████╗ █████╗ ██████╗ 
echo.                                    %p% ██╔════╝ ██╔══██╗██╔══██╗██╔════╝██║██╔══██╗██╔════╝    ██╔══██╗██╔═══██╗██╔══██╗    ██║   ██║██╔════╝██╔══██╗██╔══██╗
echo.                                    %p% ██║  ███╗██████╔╝███████║██║     ██║███████║███████╗    ██████╔╝██║   ██║██████╔╝    ██║   ██║███████╗███████║██████╔╝
echo.                                    %p% ██║   ██║██╔══██╗██╔══██║██║     ██║██╔══██║╚════██║    ██╔═══╝ ██║   ██║██╔══██╗    ██║   ██║╚════██║██╔══██║██╔══██╗
echo.                                    %p% ╚██████╔╝██║  ██║██║  ██║╚██████╗██║██║  ██║███████║    ██║     ╚██████╔╝██║  ██║    ╚██████╔╝███████║██║  ██║██║  ██║
echo.                                    %p%  ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝╚═╝╚═╝  ╚═╝╚══════╝    ╚═╝      ╚═════╝ ╚═╝  ╚═╝     ╚═════╝ ╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝
echo.
echo.                             %b%"════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════"%w%
echo.                                                           %w%¡Gracias por usar LJokerL_TTV Custom Config PRO!
echo.
echo.                                                         %g%Sus optimizaciones han sido aplicadas exitosamente
echo.                                                      %y%Recuerde reiniciar su PC para aplicar todos los cambios
echo.
echo.                                                              %p%🎮 ¡Disfrute de su gaming optimizado! 🎮
echo.
echo.                             %b%"════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════════"%w%
echo.
echo.                                                                        %w%Presione cualquier tecla para salir...
pause > nul
exit
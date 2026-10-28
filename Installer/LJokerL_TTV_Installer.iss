; LJokerL_TTV Custom Config PRO v13.0.1 Installer Script
; Inno Setup 6.x

#define AppName "LJokerL TTV Custom Config PRO"
#define AppVersion "13.0.1"
#define AppPublisher "LJokerL_TTV"
#define AppURL "https://github.com/ljokerl/ttv-custom-config"
#define AppExeName "LJokerL.App.exe"

[Setup]
AppId={{A1B2C3D4-E5F6-7890-ABCD-EF1234567890}
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisher={#AppPublisher}
AppPublisherURL={#AppURL}
AppSupportURL={#AppURL}
AppUpdatesURL={#AppURL}
DefaultDirName={autopf}\LJokerL_TTV
DisableProgramGroupPage=yes
LicenseFile=LICENSE.txt
InfoBeforeFile=README.md
OutputDir=..\..\Output
OutputBaseFilename=LJokerL_TTV_Installer_v13.0.1
SetupIconFile=..\..\Resources\icon.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin
MinVersion=10.0.19041

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 6.1; Check: not IsAdminInstallMode

[Files]
Source: "..\..\src\LJokerL.App\bin\Release\net9.0-windows\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\..\Config\*"; DestDir: "{app}\Config"; Flags: ignoreversion recursesubdirs
Source: "..\..\Resources\*"; DestDir: "{app}\Resources"; Flags: ignoreversion recursesubdirs
Source: "..\..\README.md"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\..\USER_GUIDE.md"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\..\LICENSE.txt"; DestDir: "{app}"; Flags: ignoreversion; Check: FileExists

[Icons]
Name: "{autoprograms}\{#AppName}"; Filename: "{app}\{#AppExeName}"
Name: "{autodesktop}\{#AppName}"; Filename: "{app}\{#AppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#AppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(AppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Registry]
; Optional: Register file association for .ljokerl profile files
Root: HKA; Subkey: "Software\Classes\.ljokerl"; ValueType: string; ValueName: ""; ValueData: "LJokerLProfile"; Flags: uninsdeletevalue
Root: HKA; Subkey: "Software\Classes\LJokerLProfile"; ValueType: string; ValueName: ""; ValueData: "LJokerL Optimization Profile"; Flags: uninsdeletekey
Root: HKA; Subkey: "Software\Classes\LJokerLProfile\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#AppExeName}"""""%1"""

[Code]
function InitializeSetup(): Boolean;
begin
  Result := true;
  
  // Check Windows version (Windows 10 19041+ or Windows 11)
  if not IsWindowsVersionOrGreater(10, 0, 19041) then begin
    MsgBox('Este programa requiere Windows 10 (versión 2004) o superior.', mbError, MB_OK);
    Result := false;
  end;
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then begin
    // Post-installation tasks
    Log('Installation completed successfully');
  end;
end;

[UninstallDelete]
Type: filesandordirs; Name: "{app}\Logs"
Type: filesandordirs; Name: "{app}\Benchmarks"

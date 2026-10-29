#Requires -Version 5.1
<#
.SYNOPSIS
    E2E de LJokerL_TTV DENTRO de Windows Sandbox. Lo lanza el LogonCommand de run.wsb.
.DESCRIPTION
    - Escribe resultados INCREMENTALMENTE en <repo mapeado>\E2E\RESULTS.md.
    - Auto-detecta la unidad mapeada buscando el marcador E2E\WindowsSandbox\e2e-inside.ps1 en A:..Z:.
    - Cada escenario va en try/catch independiente con veredicto PASSED/FAILED.
    - S6/S7/S8 mutan SOLO el interior de la sandbox (registro, config de usuario, harness en C:\).
    - Al final escribe '===E2E-DONE===' y apaga la sandbox (Stop-Computer -Force).
#>

$ErrorActionPreference = 'Continue'
try {
    [Net.ServicePointManager]::SecurityProtocol = [Net.ServicePointManager]::SecurityProtocol -bor [Net.SecurityProtocolType]::Tls12
} catch { }

# ---------------------------------------------------------------------------
# Localizacion de la carpeta mapeada
# ---------------------------------------------------------------------------
function Find-MappedDrive {
    foreach ($i in 65..90) {
        $drive = ('{0}:' -f [char]$i)
        $marker = Join-Path $drive 'E2E\WindowsSandbox\e2e-inside.ps1'
        if (Test-Path -LiteralPath $marker) { return $drive }
    }
    return $null
}

$Repo = Find-MappedDrive
if (-not $Repo) {
    Write-Host 'ERROR: no se encontro unidad mapeada A-Z con E2E\WindowsSandbox\e2e-inside.ps1'
    exit 1
}

$ResultsPath    = Join-Path $Repo 'E2E\RESULTS.md'
$TranscriptPath = Join-Path $Repo 'E2E\WindowsSandbox\transcript-inside.txt'
$AppBinDir      = Join-Path $Repo 'src\LJokerL.App\bin\Release\net9.0-windows'
$AppExe         = Join-Path $AppBinDir 'LJokerL.App.exe'
$AppConfigDir   = Join-Path $AppBinDir 'Config'
$SolutionPath   = Join-Path $Repo 'LJokerL.sln'

try { Start-Transcript -Path $TranscriptPath -Force | Out-Null } catch { }

# ---------------------------------------------------------------------------
# Helpers de resultados incrementales
# ---------------------------------------------------------------------------
function Add-Res { param([string]$Text)
    try { Add-Content -LiteralPath $ResultsPath -Value $Text -Encoding UTF8 } catch { }
}

function Start-Sec { param([string]$Title)
    Add-Res ''
    Add-Res ('## ' + $Title)
    Add-Res ('- Inicio: ' + (Get-Date).ToString('yyyy-MM-dd HH:mm:ss'))
}

function End-Sec { param([string]$Id, [string]$Name, [bool]$Ok, [string[]]$Details)
    $verdict = 'FAILED'
    if ($Ok) { $verdict = 'PASSED' }
    Add-Res ('### ' + $Id + ' ' + $Name + ' - ' + $verdict)
    foreach ($d in $Details) { if ($d) { Add-Res ('- ' + $d) } }
    Write-Host ('[E2E] ' + $Id + ' ' + $verdict)
}

function Update-SessionPath {
    $env:Path = [Environment]::GetEnvironmentVariable('Path', 'Machine') + ';' +
                [Environment]::GetEnvironmentVariable('Path', 'User')
}

function Get-DotNetExe {
    Update-SessionPath
    $candidate = Join-Path $env:ProgramFiles 'dotnet\dotnet.exe'
    if (Test-Path -LiteralPath $candidate) { return $candidate }
    $cmd = Get-Command dotnet -ErrorAction SilentlyContinue
    if ($cmd) { return $cmd.Source }
    return $null
}

function Test-DotNetSdk9 {
    $dn = Get-DotNetExe
    if (-not $dn) { return $false }
    $script:DotNetExe = $dn
    $sdks = (& $dn --list-sdks 2>&1) | Out-String
    Write-Host $sdks
    return ($sdks -match '(?m)^9\.')
}

function Get-FileFromWeb { param([string]$Url, [string]$Dest)
    try {
        Start-BitsTransfer -Source $Url -Destination $Dest -ErrorAction Stop
        return $true
    } catch { }
    try {
        Invoke-WebRequest -Uri $Url -OutFile $Dest -UseBasicParsing -ErrorAction Stop
        return $true
    } catch { return $false }
}

# ---------------------------------------------------------------------------
# Cabecera de resultados (sobrescribe)
# ---------------------------------------------------------------------------
Set-Content -LiteralPath $ResultsPath -Encoding UTF8 -Value @(
    '# E2E Windows Sandbox - LJokerL_TTV',
    '',
    ('- Fecha: ' + (Get-Date).ToString('yyyy-MM-dd HH:mm:ss')),
    ('- Repo mapeado dentro de la sandbox: ' + $Repo),
    ('- Equipo / usuario: ' + $env:COMPUTERNAME + ' / ' + $env:USERNAME),
    '',
    'Escenarios S0-S9. S6/S7/S8 mutan SOLO el interior de la sandbox (registro, harness en C:\e2e-harness, config de usuario).',
    'Estado: arrancando escenarios...'
)

# ===========================================================================
# S0 ENTORNO
# ===========================================================================
Start-Sec 'S0 ENTORNO'
$ok0 = $true
$det0 = New-Object System.Collections.Generic.List[string]
try {
    $cv = Get-ItemProperty -LiteralPath 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion' -ErrorAction Stop
    $det0.Add(('SO: {0} {1} (build {2}.{3})' -f $cv.ProductName, $cv.DisplayVersion, $cv.CurrentBuildNumber, $cv.UBR))

    $cs = Get-CimInstance -ClassName Win32_ComputerSystem
    $ramGb = [math]::Round($cs.TotalPhysicalMemory / 1GB, 2)
    $det0.Add(('RAM total: {0} GB' -f $ramGb))

    # Espejo de la logica de SystemRestoreService.IsEnabled(): DisableSR==1 => deshabilitado.
    $srDisabled = $false
    try {
        $srKey = Get-ItemProperty -LiteralPath 'HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore' -ErrorAction Stop
        if ($srKey.DisableSR -is [int] -and $srKey.DisableSR -eq 1) { $srDisabled = $true }
        $det0.Add(('Registro SystemRestore DisableSR: ' + $(if ($null -ne $srKey.DisableSR) { $srKey.DisableSR } else { '<ausente>' })))
    } catch {
        $det0.Add('Registro SystemRestore: clave ausente o ilegible (IsEnabled() devolveria true)')
    }
    if ($srDisabled) {
        $det0.Add('System Restore: DESHABILITADO (esperado en sandbox)')
    } else {
        $det0.Add('System Restore: HABILITADO (NO esperado en sandbox)')
        $ok0 = $false
    }

    $isAdmin = ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
    if ($isAdmin) { $det0.Add('Elevacion de administrador: si') } else { $det0.Add('Elevacion de administrador: NO'); $ok0 = $false }
} catch {
    $ok0 = $false
    $det0.Add('EXCEPCION: ' + $_.Exception.Message)
}
End-Sec 'S0' 'ENTORNO' $ok0 $det0.ToArray()

# ===========================================================================
# S1 SDK .NET 9
# ===========================================================================
Start-Sec 'S1 SDK .NET 9'
$ok1 = $false
$det1 = New-Object System.Collections.Generic.List[string]
$script:DotNetExe = $null
try {
    if (Test-DotNetSdk9) {
        $ok1 = $true
        $det1.Add('SDK 9 ya presente: ' + $script:DotNetExe)
    } else {
        $winget = Get-Command winget -ErrorAction SilentlyContinue
        if ($winget) {
            $det1.Add('Instalando via winget (Microsoft.DotNet.SDK.9)...')
            & winget install --id Microsoft.DotNet.SDK.9 -e --silent --accept-source-agreements --accept-package-agreements --disable-interactivity 2>&1 |
                ForEach-Object { Write-Host $_ }
            $det1.Add('winget exit: ' + $LASTEXITCODE)
        } else {
            $det1.Add('winget no disponible en la imagen; se usa descarga directa')
        }

        if (-not (Test-DotNetSdk9)) {
            $det1.Add('Fallback: descarga https://aka.ms/dotnet/9.0/dotnet-sdk-win-x64.exe')
            $installer = Join-Path $env:TEMP 'dotnet-sdk-win-x64.exe'
            $downloaded = Get-FileFromWeb -Url 'https://aka.ms/dotnet/9.0/dotnet-sdk-win-x64.exe' -Dest $installer
            if ($downloaded) {
                $p = Start-Process -FilePath $installer -ArgumentList '/install', '/quiet', '/norestart' -Wait -PassThru
                $det1.Add('Instalador SDK exit: ' + $p.ExitCode)
            } else {
                $det1.Add('ERROR: descarga del instalador SDK fallida')
            }
        }

        $ok1 = Test-DotNetSdk9
        if ($ok1) {
            $sdksNow = (& $script:DotNetExe --list-sdks 2>&1) | Out-String
            $det1.Add(('dotnet --list-sdks: ' + ($sdksNow.Trim() -replace "`r`n", ' | ')))
        }
    }
} catch {
    $det1.Add('EXCEPCION: ' + $_.Exception.Message)
}
End-Sec 'S1' 'SDK' $ok1 $det1.ToArray()

if (-not $ok1) {
    Add-Res ''
    Add-Res 'S2-S9 no ejecutados: sin SDK .NET 9 no procede.'
    Add-Res ''
    Add-Res ('Fin: ' + (Get-Date).ToString('yyyy-MM-dd HH:mm:ss'))
    Add-Res '===E2E-DONE==='
    try { Stop-Transcript } catch { }
    Start-Sleep -Seconds 3
    Stop-Computer -Force
    exit 1
}

# ===========================================================================
# S2 BUILD
# ===========================================================================
Start-Sec 'S2 BUILD (dotnet build LJokerL.sln -c Release)'
$ok2 = $false
$det2 = New-Object System.Collections.Generic.List[string]
try {
    $buildLog = Join-Path $env:TEMP 'e2e-build.log'
    & $script:DotNetExe build $SolutionPath -c Release --nologo > $buildLog 2>&1
    $code2 = $LASTEXITCODE
    $errorCount = (Select-String -LiteralPath $buildLog -Pattern 'error ' -ErrorAction SilentlyContinue | Measure-Object).Count
    $det2.Add(('dotnet build exit: ' + $code2))
    $det2.Add(('Lineas con "error ": ' + $errorCount))
    if ($code2 -eq 0 -and $errorCount -eq 0) {
        $ok2 = $true
    } else {
        $tail = Get-Content -LiteralPath $buildLog -Tail 15 -ErrorAction SilentlyContinue
        foreach ($line in $tail) { $det2.Add('  | ' + $line) }
    }
} catch {
    $det2.Add('EXCEPCION: ' + $_.Exception.Message)
}
End-Sec 'S2' 'BUILD' $ok2 $det2.ToArray()

# ===========================================================================
# S3 TESTS
# ===========================================================================
Start-Sec 'S3 TESTS (dotnet test LJokerL.sln -c Release)'
$ok3 = $false
$det3 = New-Object System.Collections.Generic.List[string]
try {
    $testLog = Join-Path $env:TEMP 'e2e-test.log'
    $trxDir = 'C:\e2e-trx'
    if (Test-Path -LiteralPath $trxDir) { Remove-Item -LiteralPath $trxDir -Recurse -Force -ErrorAction SilentlyContinue }
    & $script:DotNetExe test $SolutionPath -c Release --nologo --results-directory $trxDir --logger 'trx;LogFileName=e2e-results.trx' > $testLog 2>&1
    $code3 = $LASTEXITCODE
    $det3.Add(('dotnet test exit: ' + $code3))

    $summaryLine = (Select-String -LiteralPath $testLog -Pattern 'Passed!|Failed!|Passed:|Superado|Erroneo' -ErrorAction SilentlyContinue |
        Select-Object -Last 1).Line
    if ($summaryLine) { $det3.Add('Resumen VSTest: ' + $summaryLine.Trim()) }

    $trxRaw = ''
    Get-ChildItem -Path (Join-Path $trxDir '*.trx') -ErrorAction SilentlyContinue | ForEach-Object {
        $trxRaw += (Get-Content -LiteralPath $_.FullName -Raw)
    }
    if ($trxRaw) {
        $passed = ([regex]::Matches($trxRaw, 'outcome="Passed"')).Count
        $failed = ([regex]::Matches($trxRaw, 'outcome="Failed"')).Count
        $det3.Add(('TRX: passed=' + $passed + ' failed=' + $failed + ' (esperado 78)'))
        if ($code3 -eq 0 -and $failed -eq 0 -and $passed -gt 0) { $ok3 = $true }
    } else {
        $tail = Get-Content -LiteralPath $testLog -Tail 15 -ErrorAction SilentlyContinue
        foreach ($line in $tail) { $det3.Add('  | ' + $line) }
    }
} catch {
    $det3.Add('EXCEPCION: ' + $_.Exception.Message)
}
End-Sec 'S3' 'TESTS' $ok3 $det3.ToArray()

# ===========================================================================
# S4 CLI REAL
# ===========================================================================
Start-Sec 'S4 CLI REAL (LJokerL.App.exe)'
$ok4 = $false
$det4 = New-Object System.Collections.Generic.List[string]
try {
    if (-not (Test-Path -LiteralPath $AppExe)) {
        throw ('No existe el exe: ' + $AppExe)
    }
    $det4.Add('Exe: ' + $AppExe)

    function Invoke-Cli { param([string[]]$CliArgs, [string]$Tag)
        $outFile = Join-Path $env:TEMP ('cli-' + $Tag + '.txt')
        if (Test-Path -LiteralPath $outFile) { Remove-Item -LiteralPath $outFile -Force -ErrorAction SilentlyContinue }
        & $AppExe @CliArgs > $outFile 2>&1
        $code = $LASTEXITCODE
        $text = ''
        if (Test-Path -LiteralPath $outFile) { $text = [System.IO.File]::ReadAllText($outFile) }
        return @{ ExitCode = $code; Output = $text }
    }

    $a = Invoke-Cli -CliArgs @('--list-profiles') -Tag 'list'
    $aOk = ($a.ExitCode -eq 0) -and ($a.Output -match 'gaming_pro')
    $det4.Add(('- list-profiles: exit=' + $a.ExitCode + ' gaming_pro=' + $(if ($a.Output -match 'gaming_pro') { 'si' } else { 'NO' })))

    $b = Invoke-Cli -CliArgs @('--help') -Tag 'help'
    $bOk = ($b.ExitCode -eq 0) -and ($b.Output -match '--list-profiles')
    $det4.Add(('- help: exit=' + $b.ExitCode + ' usage=' + $(if ($b.Output -match '--list-profiles') { 'si' } else { 'NO' })))

    $c = Invoke-Cli -CliArgs @('--apply-profile', 'gaming_pro', '--dry-run') -Tag 'dryrun'
    $cOk = ($c.ExitCode -eq 0) -and ($c.Output -match 'acciones')
    $det4.Add(('- apply gaming_pro --dry-run: exit=' + $c.ExitCode + ' menciona acciones=' + $(if ($c.Output -match 'acciones') { 'si' } else { 'NO' })))
    $dryLine = ($c.Output -split "`r?`n" | Where-Object { $_ -match 'dry-run' } | Select-Object -First 1)
    if ($dryLine) { $det4.Add('  | ' + $dryLine.Trim()) }

    $d = Invoke-Cli -CliArgs @('--apply-profile', 'gaming_pro') -Tag 'real'
    $dOk = ($d.ExitCode -eq 2)
    $det4.Add(('- apply gaming_pro SIN dry-run: exit=' + $d.ExitCode + ' (barrera de seguridad, esperado 2)'))

    if ($aOk) { } else { $det4.Add('DETALLE a: ' + ($a.Output.Substring(0, [Math]::Min(300, $a.Output.Length)))) }
    if (-not $dOk) { $det4.Add('DETALLE d: ' + ($d.Output.Substring(0, [Math]::Min(300, $d.Output.Length)))) }

    $ok4 = $aOk -and $bOk -and $cOk -and $dOk
} catch {
    $det4.Add('EXCEPCION: ' + $_.Exception.Message)
}
End-Sec 'S4' 'CLI REAL' $ok4 $det4.ToArray()

# ===========================================================================
# S5 GUI SMOKE
# ===========================================================================
Start-Sec 'S5 GUI SMOKE (20 s de vida y cierre limpio)'
$ok5 = $false
$det5 = New-Object System.Collections.Generic.List[string]
try {
    $stale = Get-Process -Name 'LJokerL.App' -ErrorAction SilentlyContinue
    if ($stale) { $stale | Stop-Process -Force; Start-Sleep -Seconds 2 }

    $p = Start-Process -FilePath $AppExe -PassThru
    Start-Sleep -Seconds 20
    $alive = Get-Process -Id $p.Id -ErrorAction SilentlyContinue
    if ($alive) {
        $ok5 = $true
        $det5.Add('Proceso vivo tras 20 s (PID ' + $p.Id + ')')
        $null = $alive.CloseMainWindow()
        Start-Sleep -Seconds 8
        $still = Get-Process -Id $p.Id -ErrorAction SilentlyContinue
        if ($still) {
            $still | Stop-Process -Force
            $det5.Add('Cierre: CloseMainWindow no basto; Stop-Process aplicado')
        } else {
            $det5.Add('Cierre: ventana cerrada correctamente (CloseMainWindow)')
        }
    } else {
        $exitInfo = 'n/d'
        try { $exitInfo = [string]$p.ExitCode } catch { }
        $det5.Add('FALLO: el proceso murio solo antes de 20 s. ExitCode: ' + $exitInfo)
    }
} catch {
    $det5.Add('EXCEPCION: ' + $_.Exception.Message)
}
End-Sec 'S5' 'GUI SMOKE' $ok5 $det5.ToArray()

# ===========================================================================
# S6 MOTOR REAL: harness en C:\e2e-harness (fuera del repo mapeado)
# ===========================================================================
Start-Sec 'S6 MOTOR REAL (apply + revert del perfil balanced, mutacion en sandbox)'
$ok6 = $false
$det6 = New-Object System.Collections.Generic.List[string]
try {
    $harnessDir = 'C:\e2e-harness'
    New-Item -ItemType Directory -Path $harnessDir -Force | Out-Null

    $csprojContent = @'
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net9.0-windows</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>disable</Nullable>
    <AssemblyName>E2eHarness</AssemblyName>
    <RootNamespace>E2eHarness</RootNamespace>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="__REPO__\src\LJokerL.Core\LJokerL.Core.csproj" />
  </ItemGroup>
</Project>
'@
    $csprojContent = $csprojContent.Replace('__REPO__', $Repo)
    Set-Content -LiteralPath (Join-Path $harnessDir 'E2eHarness.csproj') -Value $csprojContent -Encoding Ascii

    $programContent = @'
using System.Globalization;
using System.Text.Json;
using LJokerL.Core.Models;
using LJokerL.Core.Services;
using Microsoft.Win32;

internal static class Program
{
    private static int Main(string[] args)
    {
        var mode = args.Length > 0 ? args[0].Trim().ToLowerInvariant() : string.Empty;
        var configRoot = args.Length > 1 ? args[1] : string.Empty;
        var logPath = args.Length > 2 ? args[2] : string.Empty;

        if (mode == "srcheck")
        {
            Console.WriteLine("SR_ENABLED=" + (new SystemRestoreService().IsEnabled() ? "true" : "false"));
            return 0;
        }

        var bundle = new ConfigLoader().LoadAll(configRoot);
        var profiles = bundle.Profiles != null ? bundle.Profiles.Profiles : new List<ProfileDefinition>();
        var profile = profiles.FirstOrDefault(p => string.Equals(p.Id, "balanced", StringComparison.OrdinalIgnoreCase));
        if (profile == null)
        {
            Console.WriteLine("ERROR=profile_balanced_not_found");
            return 3;
        }

        var tweakMap = ToMap(bundle.Tweaks != null ? bundle.Tweaks.Tweaks : null, t => t.Id);
        var serviceMap = ToMap(bundle.Services != null ? bundle.Services.Services : null, s => s.Id);
        var appMap = ToMap(bundle.Apps != null ? bundle.Apps.Apps : null, a => a.Id);

        if (mode == "failsafe")
        {
            return RunFailSafe();
        }
        if (mode == "apply")
        {
            return RunApply(profile, tweakMap, serviceMap, appMap, logPath);
        }

        Console.WriteLine("ERROR=unknown_mode");
        return 2;
    }

    private static int RunFailSafe()
    {
        var sr = new SystemRestoreService();
        var enabled = sr.IsEnabled();
        Console.WriteLine("SR_ENABLED=" + (enabled ? "true" : "false"));

        var gate = SystemRestoreGate.Decide(true, enabled, null, DateTime.UtcNow, false);
        Console.WriteLine("GATE_DECISION=" + gate.Decision);
        Console.WriteLine("GATE_REASON=" + gate.Reason);
        Console.WriteLine("GATE_MESSAGE=" + gate.Message);

        var ok = !enabled
                 && gate.Decision == SystemRestoreDecision.SkipFailSafe
                 && gate.Reason == SystemRestoreSkipReason.SystemRestoreDisabled;

        if (ok)
        {
            // Replica el flujo de MainViewModel.ApplySelectedProfile con
            // CreateRestorePointBeforeApply=true: el gate bloquea ANTES de tocar el motor.
            Console.WriteLine("APPLIED_ACTIONS=0");
        }

        Console.WriteLine("RESULT=" + (ok ? "PASS" : "FAIL"));
        return ok ? 0 : 6;
    }

    private static int RunApply(
        ProfileDefinition profile,
        Dictionary<string, TweakDefinition> tweakMap,
        Dictionary<string, ServiceDefinition> serviceMap,
        Dictionary<string, AppPackageDefinition> appMap,
        string logPath)
    {
        var logger = new ExecutionLogger(logPath);
        var engine = new ExecutionEngine(logger);
        var contextId = "profile:" + profile.Id + ":e2e:" + DateTime.UtcNow.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);

        var attemptedTweaks = new List<string>();
        var attemptedServices = new List<string>();
        var attemptedApps = new List<string>();
        var appliedActions = 0;

        foreach (var id in profile.Tweaks)
        {
            if (tweakMap.TryGetValue(id, out var tweak))
            {
                var report = engine.ApplyTweak(tweak, contextId);
                appliedActions += report.Actions.Count(a => a.Success);
            }
            attemptedTweaks.Add(id);
        }

        foreach (var id in profile.Services)
        {
            if (serviceMap.TryGetValue(id, out var service))
            {
                var report = engine.ApplyService(service, contextId);
                appliedActions += report.Actions.Count(a => a.Success);
            }
            attemptedServices.Add(id);
        }

        foreach (var id in profile.Apps)
        {
            if (appMap.TryGetValue(id, out var app))
            {
                var report = engine.RemoveApp(app, contextId);
                appliedActions += report.Actions.Count(a => a.Success);
            }
            attemptedApps.Add(id);
        }

        Console.WriteLine("CONTEXT=" + contextId);
        Console.WriteLine("APPLIED_ACTIONS_OK=" + appliedActions);

        var entries = logger.ReadEntries()
            .Where(e => e.ContextId == contextId
                        && string.Equals(e.Mode, "apply", StringComparison.OrdinalIgnoreCase)
                        && e.Success)
            .ToList();
        var regEntries = entries
            .Where(e => string.Equals(e.ActionType, "registry", StringComparison.OrdinalIgnoreCase))
            .ToList();

        Console.WriteLine("LOGGED_APPLIED=" + entries.Count);
        Console.WriteLine("REGISTRY_APPLIED=" + regEntries.Count);
        Console.WriteLine("PREVIOUSVALUE_PRESENT=" + (regEntries.Any(e => !string.IsNullOrEmpty(e.PreviousValue)) ? "true" : "false"));

        var ok = entries.Count > 0;
        var spot = regEntries.FirstOrDefault(e => !string.IsNullOrEmpty(e.PreviousValue)) ?? regEntries.FirstOrDefault();
        if (spot == null)
        {
            Console.WriteLine("SPOTCHECK=NO_REGISTRY_ENTRY");
        }
        else
        {
            var keyPath = spot.Target ?? string.Empty;
            var valueName = spot.RegistryName ?? string.Empty;
            Console.WriteLine("SPOTKEY=" + keyPath + "\\" + valueName);

            var current = ReadRegistryString(keyPath, valueName);
            Console.WriteLine("APPLIED_EXPECTED=" + (spot.Value ?? "<null>"));
            Console.WriteLine("APPLIED_ACTUAL=" + (current ?? "<null>"));
            var appliedMatch = string.Equals(current, spot.Value, StringComparison.Ordinal);
            Console.WriteLine("APPLIED_MATCH=" + (appliedMatch ? "true" : "false"));
            ok = ok && appliedMatch;

            var revertedActions = 0;
            var revertContext = contextId + ":revert";
            foreach (var id in attemptedTweaks)
            {
                if (tweakMap.TryGetValue(id, out var tweak))
                {
                    revertedActions += engine.RevertTweak(tweak, revertContext).Actions.Count(a => a.Success);
                }
            }
            foreach (var id in attemptedServices)
            {
                if (serviceMap.TryGetValue(id, out var service))
                {
                    revertedActions += engine.RevertService(service, revertContext).Actions.Count(a => a.Success);
                }
            }
            foreach (var id in attemptedApps)
            {
                if (appMap.TryGetValue(id, out var app))
                {
                    revertedActions += engine.RestoreApp(app, revertContext).Actions.Count(a => a.Success);
                }
            }
            Console.WriteLine("REVERT_ACTIONS_OK=" + revertedActions);

            var after = ReadRegistryString(keyPath, valueName);
            Console.WriteLine("REVERTED_EXPECTED=" + (spot.PreviousValue ?? "<deleted>"));
            Console.WriteLine("REVERTED_ACTUAL=" + (after ?? "<null>"));
            var revertedMatch = spot.PreviousValue == null
                ? after == null
                : string.Equals(after, spot.PreviousValue, StringComparison.Ordinal);
            Console.WriteLine("REVERTED_MATCH=" + (revertedMatch ? "true" : "false"));
            ok = ok && revertedMatch;
        }

        Console.WriteLine("RESULT=" + (ok ? "PASS" : "FAIL"));
        return ok ? 0 : 5;
    }

    private static Dictionary<string, T> ToMap<T>(IEnumerable<T>? items, Func<T, string> keySelector)
        where T : class
    {
        var map = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
        if (items == null)
        {
            return map;
        }

        foreach (var item in items)
        {
            var key = keySelector(item);
            if (!string.IsNullOrWhiteSpace(key) && !map.ContainsKey(key))
            {
                map.Add(key, item);
            }
        }

        return map;
    }

    private static string? ReadRegistryString(string path, string name)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            var parts = path.Trim().Split('\\', 2);
            if (parts.Length < 2)
            {
                return null;
            }

            RegistryKey? root = parts[0].ToUpperInvariant() switch
            {
                "HKLM" or "HKEY_LOCAL_MACHINE" => Registry.LocalMachine,
                "HKCU" or "HKEY_CURRENT_USER" => Registry.CurrentUser,
                "HKCR" or "HKEY_CLASSES_ROOT" => Registry.ClassesRoot,
                "HKU" or "HKEY_USERS" => Registry.Users,
                "HKCC" or "HKEY_CURRENT_CONFIG" => Registry.CurrentConfig,
                _ => null
            };
            if (root == null)
            {
                return null;
            }

            using (root)
            using (var key = root.OpenSubKey(parts[1]))
            {
                if (key == null)
                {
                    return null;
                }

                var raw = key.GetValue(name, null, RegistryValueOptions.DoNotExpandEnvironmentNames);
                if (raw == null)
                {
                    return null;
                }

                RegistryValueKind kind;
                try { kind = key.GetValueKind(name); } catch { kind = RegistryValueKind.String; }
                if (kind == RegistryValueKind.DWord || kind == RegistryValueKind.QWord)
                {
                    return Convert.ToInt64(raw, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
                }
                if (kind == RegistryValueKind.MultiString)
                {
                    return JsonSerializer.Serialize((string[])raw);
                }
                if (kind == RegistryValueKind.Binary)
                {
                    return Convert.ToBase64String((byte[])raw);
                }
                return raw.ToString();
            }
        }
        catch
        {
            return null;
        }
    }
}
'@
    Set-Content -LiteralPath (Join-Path $harnessDir 'Program.cs') -Value $programContent -Encoding UTF8

    $harnessLog = Join-Path $env:TEMP 'e2e-harness-build.log'
    & $script:DotNetExe build (Join-Path $harnessDir 'E2eHarness.csproj') -c Release --nologo > $harnessLog 2>&1
    $buildCode = $LASTEXITCODE
    $det6.Add(('harness build exit: ' + $buildCode))
    if ($buildCode -ne 0) {
        Get-Content -LiteralPath $harnessLog -Tail 15 | ForEach-Object { $det6.Add('  | ' + $_) }
        throw 'Fallo al compilar el harness'
    }

    $harnessExe = Join-Path $harnessDir 'bin\Release\net9.0-windows\E2eHarness.exe'
    $runLog = Join-Path $env:TEMP 'e2e-harness-run.log'
    $engineLog = Join-Path $harnessDir 'Logs\execution.jsonl'
    & $harnessExe apply $AppConfigDir $engineLog > $runLog 2>&1
    $runCode = $LASTEXITCODE
    foreach ($line in (Get-Content -LiteralPath $runLog)) { if ($line) { $det6.Add($line) } }

    $ok6 = ($runCode -eq 0)
} catch {
    $det6.Add('EXCEPCION: ' + $_.Exception.Message)
}
End-Sec 'S6' 'MOTOR REAL' $ok6 $det6.ToArray()

# ===========================================================================
# S7 RESTORE POINT FAIL-SAFE en entorno hostil
# ===========================================================================
Start-Sec 'S7 RESTORE POINT FAIL-SAFE (System Restore deshabilitado)'
$ok7 = $false
$det7 = New-Object System.Collections.Generic.List[string]
try {
    $harnessExe = 'C:\e2e-harness\bin\Release\net9.0-windows\E2eHarness.exe'
    $runLog7 = Join-Path $env:TEMP 'e2e-harness-failsafe.log'
    $failSafeLog = 'C:\e2e-harness\Logs\execution-failsafe.jsonl'
    & $harnessExe failsafe $AppConfigDir $failSafeLog > $runLog7 2>&1
    $runCode7 = $LASTEXITCODE
    foreach ($line in (Get-Content -LiteralPath $runLog7)) { if ($line) { $det7.Add($line) } }

    $gateSkip = (Select-String -LiteralPath $runLog7 -Pattern 'GATE_DECISION=SkipFailSafe' -Quiet)
    $reasonOk = (Select-String -LiteralPath $runLog7 -Pattern 'GATE_REASON=SystemRestoreDisabled' -Quiet)
    $ok7 = ($runCode7 -eq 0) -and $gateSkip -and $reasonOk
} catch {
    $det7.Add('EXCEPCION: ' + $_.Exception.Message)
}
End-Sec 'S7' 'RESTORE FAIL-SAFE' $ok7 $det7.ToArray()

# ===========================================================================
# S8 WATCHER
# ===========================================================================
Start-Sec 'S8 WATCHER (notepad -> balanced, via App real)'
$ok8 = $false
$det8 = New-Object System.Collections.Generic.List[string]
try {
    # 1) settings.json: ruta REAL del watcher (%APPDATA%\LJokerL_TTV\settings.json,
    #    MainViewModel.ResolveSettingsPath). Se escribe tambien en %LOCALAPPDATA%\LJokerL\
    #    (ruta que usa App.xaml.cs para el tema) por cobertura.
    $settingsJson = @'
{
  "MonitoringEnabled": true,
  "MetricsLoggingEnabled": false,
  "MonitoringIntervalSeconds": 2,
  "CreateRestorePointBeforeApply": false,
  "EnableGameWatcher": true,
  "DarkMode": false
}
'@
    $settingsDir = Join-Path $env:APPDATA 'LJokerL_TTV'
    New-Item -ItemType Directory -Path $settingsDir -Force | Out-Null
    Set-Content -LiteralPath (Join-Path $settingsDir 'settings.json') -Value $settingsJson -Encoding Ascii
    $det8.Add('settings.json escrito en ' + $settingsDir)

    $localDir = Join-Path $env:LOCALAPPDATA 'LJokerL'
    New-Item -ItemType Directory -Path $localDir -Force | Out-Null
    Set-Content -LiteralPath (Join-Path $localDir 'settings.json') -Value $settingsJson -Encoding Ascii

    # 2) Activar mapping notepad -> balanced en el Config de salida del App
    $gwPath = Join-Path $AppConfigDir 'game_watcher.json'
    $gw = Get-Content -LiteralPath $gwPath -Raw | ConvertFrom-Json
    foreach ($m in $gw.mappings) {
        if ($m.processName -eq 'notepad') { $m.enabled = $true }
    }
    $gw | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath $gwPath -Encoding UTF8
    $det8.Add('game_watcher.json: mapping notepad->balanced habilitado en ' + $gwPath)

    # 3) Log del App limpio
    $appLog = Join-Path $AppBinDir 'Logs\execution.jsonl'
    if (Test-Path -LiteralPath $appLog) { Remove-Item -LiteralPath $appLog -Force }

    # 4) Lanzar App (GUI), 10 s, notepad, 25 s
    $app = Start-Process -FilePath $AppExe -PassThru
    Start-Sleep -Seconds 10
    $np = Start-Process -FilePath 'notepad.exe' -PassThru
    Start-Sleep -Seconds 25

    $entries = 0
    $appliedBalanced = 0
    foreach ($candidate in @($appLog, (Join-Path $env:TEMP 'ljokerl_execution.jsonl'))) {
        if (Test-Path -LiteralPath $candidate) {
            $lines = Get-Content -LiteralPath $candidate -ErrorAction SilentlyContinue
            if ($null -eq $entries -or $entries -eq 0) { $entries = @($lines).Count }
            $appliedBalanced += @($lines | Where-Object { $_ -match '"Mode":"apply"' -and $_ -match '"Success":true' -and $_ -match 'profile:balanced' }).Count
        }
    }
    $appAlive = $null -ne (Get-Process -Id $app.Id -ErrorAction SilentlyContinue)
    $det8.Add(('App vivo tras 45 s: ' + $(if ($appAlive) { 'si' } else { 'NO' })))
    $det8.Add(('Lineas en execution.jsonl: ' + $entries))
    $det8.Add(('Entradas apply del perfil balanced: ' + $appliedBalanced))
    $ok8 = ($appliedBalanced -gt 0)

    # 5) Limpieza
    Get-Process -Name 'notepad' -ErrorAction SilentlyContinue | Stop-Process -Force
    $appNow = Get-Process -Id $app.Id -ErrorAction SilentlyContinue
    if ($appNow) {
        $null = $appNow.CloseMainWindow()
        Start-Sleep -Seconds 5
        if (Get-Process -Id $app.Id -ErrorAction SilentlyContinue) {
            Get-Process -Id $app.Id -ErrorAction SilentlyContinue | Stop-Process -Force
        }
    }
} catch {
    $det8.Add('EXCEPCION: ' + $_.Exception.Message)
    Get-Process -Name 'notepad' -ErrorAction SilentlyContinue | Stop-Process -Force
}
End-Sec 'S8' 'WATCHER' $ok8 $det8.ToArray()

# ===========================================================================
# S9 INSTALADOR E2E
# ===========================================================================
Start-Sec 'S9 INSTALADOR (Inno Setup + CreateInstaller.ps1)'
$ok9 = $false
$det9 = New-Object System.Collections.Generic.List[string]
try {
    $iscc = "${env:ProgramFiles(x86)}\Inno Setup 6\iscc.exe"
    if (-not (Test-Path -LiteralPath $iscc)) {
        $winget = Get-Command winget -ErrorAction SilentlyContinue
        if ($winget) {
            $det9.Add('Instalando Inno Setup via winget...')
            & winget install --id JRSoftware.InnoSetup -e --silent --accept-source-agreements --accept-package-agreements --disable-interactivity 2>&1 |
                ForEach-Object { Write-Host $_ }
        } else {
            $det9.Add('winget no disponible; descarga directa de is.exe')
        }
        if (-not (Test-Path -LiteralPath $iscc)) {
            $det9.Add('Fallback: descarga https://jrsoftware.org/download.php/is.exe')
            $isInstaller = Join-Path $env:TEMP 'innosetup-is.exe'
            if (Get-FileFromWeb -Url 'https://jrsoftware.org/download.php/is.exe' -Dest $isInstaller) {
                $p = Start-Process -FilePath $isInstaller -ArgumentList '/SILENT', '/SUPPRESSMSGBOXES', '/NORESTART' -Wait -PassThru
                $det9.Add('instalador Inno Setup exit: ' + $p.ExitCode)
            } else {
                $det9.Add('ERROR: descarga de is.exe fallida')
            }
        }
    }

    if (-not (Test-Path -LiteralPath $iscc)) {
        $isccAlt = "$env:ProgramFiles\Inno Setup 6\iscc.exe"
        if (Test-Path -LiteralPath $isccAlt) { $iscc = $isccAlt }
    }

    if (Test-Path -LiteralPath $iscc) {
        $det9.Add('iscc localizado: ' + $iscc)
        $instLog = Join-Path $env:TEMP 'e2e-installer.log'
        & powershell.exe -NoProfile -ExecutionPolicy Bypass -File (Join-Path $Repo 'Installer\CreateInstaller.ps1') > $instLog 2>&1
        $code9 = $LASTEXITCODE
        $det9.Add(('CreateInstaller.ps1 exit: ' + $code9))

        $installer = Join-Path $Repo 'Output\LJokerL_TTV_Installer_v13.0.1.exe'
        if (Test-Path -LiteralPath $installer) {
            $sizeMb = [math]::Round((Get-Item -LiteralPath $installer).Length / 1MB, 2)
            $det9.Add(('Installer generado: ' + $installer + ' (' + $sizeMb + ' MB)'))
            $ok9 = ($code9 -eq 0) -and ($sizeMb -gt 0.5)
        } else {
            $det9.Add('FALLO: no se encontro ' + $installer)
            Get-Content -LiteralPath $instLog -Tail 15 -ErrorAction SilentlyContinue | ForEach-Object { $det9.Add('  | ' + $_) }
        }
    } else {
        $det9.Add('FALLO: iscc.exe no disponible tras los intentos de instalacion')
    }
} catch {
    $det9.Add('EXCEPCION: ' + $_.Exception.Message)
}
End-Sec 'S9' 'INSTALADOR' $ok9 $det9.ToArray()

# ===========================================================================
# Cierre: marcador final y apagado de la sandbox
# ===========================================================================
Add-Res ''
Add-Res ('Fin: ' + (Get-Date).ToString('yyyy-MM-dd HH:mm:ss'))
Add-Res '===E2E-DONE==='

try { Stop-Transcript } catch { }
Start-Sleep -Seconds 3
Stop-Computer -Force
exit 0

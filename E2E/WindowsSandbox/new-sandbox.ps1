#Requires -Version 5.1
<#
.SYNOPSIS
    Generador (HOST) de la sandbox E2E de LJokerL_TTV.
.DESCRIPTION
    - Verifica (solo lectura) que la caracteristica Windows Sandbox esta habilitada.
    - Escribe E2E\WindowsSandbox\run.wsb mapeando la raiz del repo (ReadOnly=false)
      con SandboxFolder determinista C:\mnt\repo.
    - El LogonCommand lanza e2e-inside.ps1 desde C:\mnt\repo (ruta fija).
    - Arranca la sandbox con WindowsSandbox.exe.
    No muta nada del sistema en el host: solo crea run.wsb dentro de E2E\WindowsSandbox\.
#>

[CmdletBinding()]
param(
    [string]$RepoRoot
)

$ErrorActionPreference = 'Stop'

if ([string]::IsNullOrWhiteSpace($RepoRoot)) {
    $RepoRoot = (Resolve-Path -LiteralPath (Join-Path $PSScriptRoot '..\..')).Path
}
$RepoRoot = (Resolve-Path -LiteralPath $RepoRoot).ProviderPath

$insideScript = Join-Path $RepoRoot 'E2E\WindowsSandbox\e2e-inside.ps1'
$solution = Join-Path $RepoRoot 'LJokerL.sln'
if (-not (Test-Path -LiteralPath $insideScript)) { throw "Falta el script interior: $insideScript" }
if (-not (Test-Path -LiteralPath $solution)) { throw "Falta la solucion: $solution" }

# ---------------------------------------------------------------------------
# 1) Comprobacion (solo lectura) de la caracteristica Windows Sandbox
# ---------------------------------------------------------------------------
$featureState = $null
try {
    $feature = Get-WindowsOptionalFeature -Online -FeatureName 'Containers-DisposableClientVM' -ErrorAction Stop
    $featureState = $feature.State
} catch {
    Write-Warning ("No se pudo consultar la caracteristica (requiere elevacion): " + $_.Exception.Message)
}

$sandboxExe = Join-Path $env:windir 'System32\WindowsSandbox.exe'
$sandboxExePresent = Test-Path -LiteralPath $sandboxExe

if ($featureState -eq 'Disabled') {
    Write-Host 'ERROR: la caracteristica Windows Sandbox (Containers-DisposableClientVM) esta DESHABILITADA.'
    Write-Host 'Habilitala en "Activar o desactivar las caracteristicas de Windows" y vuelve a ejecutar.'
    exit 1
}
if (-not $sandboxExePresent) {
    Write-Host 'ERROR: WindowsSandbox.exe no esta presente en System32; la caracteristica no esta instalada.'
    exit 1
}
if ($null -eq $featureState) {
    Write-Host 'AVISO: estado de la caracteristica no consultable sin elevacion; se continua porque WindowsSandbox.exe esta presente.'
} else {
    Write-Host ("Caracteristica Windows Sandbox: " + $featureState)
}

# ---------------------------------------------------------------------------
# 2) run.wsb
#    <SandboxFolder>C:\mnt\repo</SandboxFolder> fija el punto de montaje
#    determinista: sin esa etiqueta, la carpeta mapeada cae en el escritorio
#    de WDAGUtilityAccount en lugar de C:\mnt. El LogonCommand referencia
#    directamente esa ruta, sin descubrimiento en tiempo de ejecucion.
# ---------------------------------------------------------------------------
$logonRaw = 'powershell -ExecutionPolicy Bypass -File C:\mnt\repo\E2E\WindowsSandbox\e2e-inside.ps1'
$logonEscaped = [System.Security.SecurityElement]::Escape($logonRaw)

$wsbContent = @"
<Configuration>
  <MappedFolders>
    <MappedFolder>
      <HostFolder>$RepoRoot</HostFolder>
      <SandboxFolder>C:\mnt\repo</SandboxFolder>
      <ReadOnly>false</ReadOnly>
    </MappedFolder>
  </MappedFolders>
  <LogonCommand>$logonEscaped</LogonCommand>
</Configuration>
"@

$wsbPath = Join-Path $PSScriptRoot 'run.wsb'
Set-Content -LiteralPath $wsbPath -Value $wsbContent -Encoding Ascii
Write-Host ("run.wsb generado: " + $wsbPath)
Write-Host ("HostFolder mapeado: " + $RepoRoot + "  (se montara en C:\mnt\repo)")

# ---------------------------------------------------------------------------
# 3) Arrancar la sandbox
# ---------------------------------------------------------------------------
Write-Host 'Arrancando Windows Sandbox...'
Start-Process -FilePath $sandboxExe -ArgumentList ('"' + $wsbPath + '"')

Start-Sleep -Seconds 10
$proc = Get-Process -Name 'WindowsSandbox' -ErrorAction SilentlyContinue
if ($proc) {
    Write-Host 'Windows Sandbox en marcha. Los resultados apareceran en E2E\RESULTS.md (sondeo incremental).'
    exit 0
} else {
    Write-Host 'ERROR: Windows Sandbox no parece haber arrancado (proceso WindowsSandbox ausente).'
    Write-Host 'Comprueba que la virtualizacion esta habilitada y que la caracteristica esta activa.'
    exit 2
}

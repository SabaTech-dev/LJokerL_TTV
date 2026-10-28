#Requires -Version 5.1
<#
.SYNOPSIS
    Creates a portable version of LJokerL TTV Custom Config PRO
.DESCRIPTION
    Packages the published application into a portable ZIP file
.NOTES
    Version: 13.0
    Date: 2026-01-31
#>

[CmdletBinding()]
param(
    [string]$Configuration = "Release",
    [string]$Version = "13.0"
)

$ErrorActionPreference = "Stop"

# Paths
$ProjectRoot = Split-Path -Parent $PSScriptRoot
$PublishDir = Join-Path $ProjectRoot "src\LJokerL.App\bin\$Configuration\net9.0-windows\publish"
$OutputDir = Join-Path $ProjectRoot "Output"
$PortableDir = Join-Path $OutputDir "LJokerL_TTV_Portable_v$Version"
$ZipFile = Join-Path $OutputDir "LJokerL_TTV_Portable_v$Version.zip"

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "LJokerL TTV Portable Builder v$Version" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

# Step 1: Build the application
Write-Host "Step 1: Building application..." -ForegroundColor Yellow
try {
    dotnet publish (Join-Path $ProjectRoot "src\LJokerL.App") -c $Configuration
    if ($LASTEXITCODE -ne 0) {
        throw "Build failed with exit code $LASTEXITCODE"
    }
    Write-Host "Build successful" -ForegroundColor Green
} catch {
    Write-Host "Build failed: $_" -ForegroundColor Red
    exit 1
}

# Step 2: Create output directory
Write-Host "`nStep 2: Creating portable structure..." -ForegroundColor Yellow
if (Test-Path $PortableDir) {
    Remove-Item -Path $PortableDir -Recurse -Force
}
New-Item -ItemType Directory -Path $PortableDir -Force | Out-Null

# Step 3: Copy published files
Write-Host "Step 3: Copying application files..." -ForegroundColor Yellow
Copy-Item -Path "$PublishDir\*" -Destination $PortableDir -Recurse -Force

# Step 4: Copy configuration
Write-Host "Step 4: Copying configuration files..." -ForegroundColor Yellow
$ConfigSource = Join-Path $ProjectRoot "Config"
$ConfigDest = Join-Path $PortableDir "Config"
if (Test-Path $ConfigSource) {
    Copy-Item -Path $ConfigSource -Destination $ConfigDest -Recurse -Force
}

# Step 5: Copy resources
Write-Host "Step 5: Copying resources..." -ForegroundColor Yellow
$ResourcesSource = Join-Path $ProjectRoot "Resources"
$ResourcesDest = Join-Path $PortableDir "Resources"
if (Test-Path $ResourcesSource) {
    Copy-Item -Path $ResourcesSource -Destination $ResourcesDest -Recurse -Force
}

# Step 6: Copy documentation
Write-Host "Step 6: Copying documentation..." -ForegroundColor Yellow
$Docs = @("README.md", "USER_GUIDE.md", "BUILD_INSTRUCTIONS.md", "CHANGELOG.md", "LICENSE.txt")
foreach ($doc in $Docs) {
    $docPath = Join-Path $ProjectRoot $doc
    if (Test-Path $docPath) {
        Copy-Item -Path $docPath -Destination $PortableDir -Force
    }
}

# Step 7: Create README_Portable.txt
Write-Host "Step 7: Creating portable README..." -ForegroundColor Yellow
$PortableReadme = @"
LJOKERL TTV CUSTOM CONFIG PRO v$Version - PORTABLE VERSION
========================================================

IMPORTANTE / IMPORTANT
======================
This is a PORTABLE version. No installation required.
Just extract and run LJokerL.App.exe

Esto es una version PORTABLE. No requiere instalacion.
Solo extrae y ejecuta LJokerL.App.exe

REQUIREMENTS / REQUISITOS
=========================
- Windows 10 (version 2004) or later
- Windows 11
- Administrator privileges (for system modifications)
- Privilegios de administrador (para modificaciones del sistema)

HOW TO USE / COMO USAR
======================
1. Extract this ZIP file to any folder
2. Right-click LJokerL.App.exe
3. Select "Run as administrator"
4. Use the interface to:
   - Select optimization profiles
   - Run benchmarks
   - Monitor system in real-time
   - Apply/revert system tweaks

FIRST RUN / PRIMERA EJECUCION
=============================
On first run, the application will:
- Detect your hardware
- Show hardware-specific recommendations
- Create default folders (Logs, Benchmarks)

En la primera ejecucion, la aplicacion:
- Detectara tu hardware
- Mostrara recomendaciones especificas
- Creara carpetas por defecto (Logs, Benchmarks)

SUPPORT / SOPORTE
=================
For support, visit: https://github.com/ljokerl/ttv-custom-config

Para soporte, visita: https://github.com/ljokerl/ttv-custom-config

LICENSE / LICENCIA
==================
See LICENSE.txt for licensing information.

---
Built: $(Get-Date -Format "yyyy-MM-dd HH:mm:ss")
Version: $Version
"@

$PortableReadme | Out-File -FilePath (Join-Path $PortableDir "README_PORTABLE.txt") -Encoding UTF8

# Step 8: Create ZIP
Write-Host "`nStep 8: Creating ZIP archive..." -ForegroundColor Yellow
if (Test-Path $ZipFile) {
    Remove-Item -Path $ZipFile -Force
}

Add-Type -Assembly System.IO.Compression.FileSystem
[System.IO.Compression.ZipFile]::CreateFromDirectory($PortableDir, $ZipFile, 'Optimal', $false)

# Summary
Write-Host "`n==========================================" -ForegroundColor Green
Write-Host "PORTABLE BUILD COMPLETE!" -ForegroundColor Green
Write-Host "==========================================" -ForegroundColor Green
Write-Host "Output: $ZipFile" -ForegroundColor White
Write-Host "Size: $([math]::Round((Get-Item $ZipFile).Length / 1MB, 2)) MB" -ForegroundColor White
Write-Host "`nTo test:" -ForegroundColor Yellow
Write-Host "1. Extract $ZipFile" -ForegroundColor White
Write-Host "2. Run LJokerL.App.exe as Administrator" -ForegroundColor White
Write-Host ""

#Requires -Version 5.1
<#
.SYNOPSIS
    Creates the installer for LJokerL TTV Custom Config PRO using Inno Setup
.DESCRIPTION
    Builds the application and creates an installer using Inno Setup
.NOTES
    Version: 13.0
    Requires: Inno Setup 6.x (iscc.exe)
#>

[CmdletBinding()]
param(
    [string]$Configuration = "Release",
    [string]$Version = "13.0.1"
)

$ErrorActionPreference = "Stop"

# Paths
$ProjectRoot = Split-Path -Parent $PSScriptRoot
$InnoScript = Join-Path $PSScriptRoot "LJokerL_TTV_Installer.iss"
$OutputDir = Join-Path $ProjectRoot "Output"

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "LJokerL TTV Installer Builder v$Version" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

# Check for Inno Setup
$InnoPath = "${env:ProgramFiles(x86)}\Inno Setup 6\iscc.exe"
if (-not (Test-Path $InnoPath)) {
    $InnoPath = "$env:ProgramFiles\Inno Setup 6\iscc.exe"
}

if (-not (Test-Path $InnoPath)) {
    Write-Host "ERROR: Inno Setup 6 not found!" -ForegroundColor Red
    Write-Host "Please install Inno Setup 6 from: https://jrsoftware.org/isdl.php" -ForegroundColor Yellow
    exit 1
}

Write-Host "Found Inno Setup: $InnoPath" -ForegroundColor Green

# Step 1: Build the application
Write-Host "`nStep 1: Building application..." -ForegroundColor Yellow
try {
    dotnet publish (Join-Path $ProjectRoot "src\LJokerL.App") -c $Configuration
    if ($LASTEXITCODE -ne 0) {
        throw "Build failed with exit code $LASTEXITCODE"
    }
    Write-Host "✓ Build successful" -ForegroundColor Green
} catch {
    Write-Host "✗ Build failed: $_" -ForegroundColor Red
    exit 1
}

# Step 2: Create output directory
Write-Host "`nStep 2: Preparing output directory..." -ForegroundColor Yellow
if (-not (Test-Path $OutputDir)) {
    New-Item -ItemType Directory -Path $OutputDir -Force | Out-Null
}

# Step 3: Compile installer
Write-Host "`nStep 3: Compiling installer..." -ForegroundColor Yellow

# Check if Inno script exists
if (-not (Test-Path $InnoScript)) {
    Write-Host "ERROR: Inno Setup script not found: $InnoScript" -ForegroundColor Red
    exit 1
}

# Create temporary modified script with version
$TempScript = Join-Path $env:TEMP "LJokerL_TTV_Installer_temp.iss"
$ScriptContent = Get-Content $InnoScript -Raw
$ScriptContent = $ScriptContent -replace '#define AppVersion "13.0.1"', "#define AppVersion `"$Version`""
$ScriptContent | Out-File -FilePath $TempScript -Encoding UTF8

try {
    & $InnoPath $TempScript
    if ($LASTEXITCODE -ne 0) {
        throw "Inno Setup compilation failed with exit code $LASTEXITCODE"
    }
    Write-Host "✓ Installer compiled successfully" -ForegroundColor Green
} catch {
    Write-Host "✗ Installer compilation failed: $_" -ForegroundColor Red
    exit 1
} finally {
    # Clean up temp script
    if (Test-Path $TempScript) {
        Remove-Item $TempScript -Force
    }
}

# Find the generated installer
$Installer = Get-ChildItem -Path $OutputDir -Filter "LJokerL_TTV_Installer_v*.exe" | Sort-Object LastWriteTime -Descending | Select-Object -First 1

if ($Installer) {
    Write-Host "`n==========================================" -ForegroundColor Green
    Write-Host "INSTALLER BUILD COMPLETE!" -ForegroundColor Green
    Write-Host "==========================================" -ForegroundColor Green
    Write-Host "File: $($Installer.FullName)" -ForegroundColor White
    Write-Host "Size: $([math]::Round($Installer.Length / 1MB, 2)) MB" -ForegroundColor White
    Write-Host "`nTo test:" -ForegroundColor Yellow
    Write-Host "1. Run the installer on a clean Windows 10/11 machine" -ForegroundColor White
    Write-Host "2. Verify it installs to $env:ProgramFiles\LJokerL_TTV\" -ForegroundColor White
    Write-Host "3. Test desktop shortcut and Start Menu entries" -ForegroundColor White
    Write-Host ""
} else {
    Write-Host "ERROR: Installer file not found in output directory" -ForegroundColor Red
    exit 1
}

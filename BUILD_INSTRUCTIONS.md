# LJokerL_TTV Custom Config PRO v13.0 - Build Scripts

## Build Instructions

### Prerequisites
1. .NET 9.0 SDK installed
2. Windows 10/11 machine
3. (Optional) Inno Setup 6.x for installer creation

### Build Steps

#### 1. Build Release Version
```powershell
# Build the application
dotnet publish src/LJokerL.App -c Release

# Output will be in:
# src/LJokerL.App/bin/Release/net9.0-windows/publish/
```

#### 2. Create Portable Version
```powershell
# Run the portable creation script
Installer/CreatePortable.ps1
```

#### 3. Create Installer (requires Inno Setup)
```powershell
# Run the installer creation script
Installer/CreateInstaller.ps1
```

## Output Structure

### Portable Version (`Output/LJokerL_TTV_Portable_v13.0/`)
```
LJokerL_TTV_Portable_v13.0/
├── LJokerL.App.exe          # Main application
├── LJokerL.Core.dll         # Core library
├── Config/                  # Configuration files
│   ├── tweaks.json
│   ├── services.json
│   ├── apps_bloatware.json
│   ├── profiles.json
│   └── hardware_rules.json
├── Resources/               # External tools
├── Logs/                    # Log directory (created at runtime)
├── Benchmarks/              # Benchmark results (created at runtime)
├── README.md                # Documentation
└── USER_GUIDE.md            # User guide
```

### Installed Version (`C:\Program Files\LJokerL_TTV\`)
Same structure as portable, but installed in Program Files with:
- Start Menu shortcuts
- Desktop shortcut (optional)
- Uninstaller
- File associations (optional)

## Features

### Core Features
- ✅ Real-time system monitoring (CPU, GPU, RAM, Disk, Network)
- ✅ Benchmark suite (CPU, Memory, Disk, Network, System)
- ✅ Hardware detection with override support
- ✅ 14 hardware-based optimization rules
- ✅ Profile-based system optimization
- ✅ Execution logging with JSONL format
- ✅ Rollback support for all changes

### Technical Specifications
- **Framework**: .NET 9.0
- **UI**: WPF (Windows Presentation Foundation)
- **Architecture**: x64
- **Target OS**: Windows 10 (2004+) / Windows 11
- **Privileges**: Administrator (for system modifications)

## Version History

### v13.0 (Current)
- Migrated to WPF native UI (no more PowerShell/Web)
- Added real-time monitoring dashboard
- Added benchmark comparison system
- Added 14 hardware-specific optimization rules
- Added GPU usage monitoring (universal)
- Added CPU temperature placeholder
- Removed legacy PowerShell scripts
- Removed WebMonitor component

## License

See LICENSE.txt for details.

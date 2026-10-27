---
description: "Use when editing Installer PowerShell scripts, Inno Setup files, publish steps, or packaging flow."
applyTo:
  - "Installer/**/*.ps1"
  - "Installer/**/*.iss"
---
# Installer Guidelines

- Keep the packaging flow aligned with `dotnet publish src/LJokerL.App -c Release`.
- Preserve Windows-only assumptions and admin requirements.
- Keep version strings, output paths, and source paths in sync across scripts and Inno Setup.
- Never hardcode machine-specific paths or secrets.
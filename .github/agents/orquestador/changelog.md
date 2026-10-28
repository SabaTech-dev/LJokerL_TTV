# Orquestador changelog

## 2026-05-08
- Initial scaffold: router agent, central Copilot guide, file-specific instructions, and specialist agents.
- Added tracker files for TODO, memory, and changelog persistence.

## 2026-05-28

HISTORICAL: the PowerShell script and its state directory referenced below were later removed from the repo; the current flow is the C# app (`src/LJokerL.App`).

- Routed the smooth-VRR PowerShell script (`Apply-Win11-SmoothVRR-RTX4080.ps1`) to `security-auditor` for hardening around native command allow-listing, AppX handling, USB selective suspend error tolerance, and `$PROFILE` shadowing.
- Routed execution validation to `build-verifier`; after iterative hardening, the elevated Windows PowerShell 5.1 run completed successfully with `Fallidos: 0` and generated `smooth_vrr_*_backup.json` plus `smooth_vrr_*_report.json` in the script's state directory.
- Updated `Config/tweaks.json` to use the host-validated GUID form for USB selective suspend in AC/DC, and refined the script summary to separate `already-applied` / `already-absent` from real skips; validated again with an elevated real run yielding `Aplicados: 29`, `Ya estaban aplicados: 24`, `Ya no estaban / no aplicaba: 23`, `Omitidos reales: 0`, `Fallidos: 0`.
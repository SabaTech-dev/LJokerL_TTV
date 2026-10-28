# LJokerL_TTV Copilot Guide

## Purpose
This is the central Copilot index for the workspace. Keep it short, link-first, and aligned with the workspace customizations under `.github/`.

## Repository map
- `src/LJokerL.App/`: WPF UI and MVVM. Main entry points: `MainWindow.xaml.cs` and `ViewModels/MainViewModel.cs`.
- `src/LJokerL.Core/`: business logic, hardware detection, execution, rollback, config loading, and benchmarking helpers.
- `src/LJokerL.Tests/`: standard xUnit test suite; tests are executed with `dotnet test`.
- `Config/`: JSON catalogs for tweaks, services, apps, profiles, and hardware rules.
- `Installer/`: PowerShell packaging scripts and Inno Setup assets.
- `Reports/`: validation and analysis reports.
- `Resources/`: bundled external tools and assets.

## Global rules
- The app is Windows-only; system changes require administrator privileges.
- Keep UI behavior in `LJokerL.App`, business logic in `LJokerL.Core`, and configuration data in `Config/*.json`.
- `ExecutionEngine` is the source of truth for apply/revert/rollback behavior.
- Respect `LJOKERL_POWERSHELL_PATH` when the Core or scripts launch PowerShell.
- Never edit generated artifacts under `bin/`, `obj/`, `Logs/`, `Benchmarks/Results/`, or `Output/` unless the task explicitly asks for generated output.
- Preserve MVVM boundaries and command refresh behavior when touching view models.

## Build and verify
- `dotnet publish src/LJokerL.App -c Release`
- `dotnet test LJokerL.sln -c Release --nologo`
- `Installer/CreatePortable.ps1`
- `Installer/CreateInstaller.ps1` (requires Inno Setup)

## Customization index

### Workspace instructions
- [App WPF rules](./instructions/app-wpf.instructions.md)
- [Core execution rules](./instructions/core.instructions.md)
- [Custom test runner rules](./instructions/tests.instructions.md)
- [Installer rules](./instructions/installer.instructions.md)
- [Config catalog rules](./instructions/config.instructions.md)

### Custom agents
- [Orquestador](./agents/orquestador.agent.md) — pure router and tracker
- [Security auditor](./agents/security-auditor.agent.md) — SonarQube/Kiuwan/CWE/CVE remediation
- [Build verifier](./agents/build-verifier.agent.md) — publish, tests, and packaging validation
- [Docs maintainer](./agents/docs-maintainer.agent.md) — keeps customization docs aligned

### Prompts
- [Orquestador smoke test](./prompts/orquestador-smoke-test.prompt.md)
- [Security executive report](./prompts/security-executive-report.prompt.md)

### Skills
- [Workspace customization bootstrap](./skills/workspace-customization-bootstrap/SKILL.md)
- [Security audit remediation](./skills/security-audit-remediation/SKILL.md)

## Recommended workflow
1. Start with `Orquestador` for workspace-wide or multi-step customization work.
2. Let the file-specific instruction attach automatically when editing matching files.
3. Use prompts for repeatable tasks like smoke tests or report generation.
4. Mirror durable state in `./agents/orquestador/todo.md`, `memory.md`, and `changelog.md`.
5. Route validation to `build-verifier` or `security-auditor` when a change needs proof.
6. Use skills when the workflow is repeatable enough to benefit from a bundled procedure.

## MCPs and hooks
- No workspace MCPs are configured yet. When one is added, document the server name, tool scope, and intended routing here.
- No deterministic hooks are added yet. Add hooks only when a rule must be enforced at runtime instead of guided in instructions.

## Maintenance rules
- Keep each customization file focused on one concern.
- Use narrow `applyTo` patterns.
- Prefer links to the larger docs in the repo instead of copying their contents here.
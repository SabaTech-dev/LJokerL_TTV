---
description: "Use when editing Core services such as ExecutionEngine, HardwareDetector, AutoConfigurationEngine, ConfigLoader, monitoring, benchmarking, or rollback logic."
applyTo:
  - "src/LJokerL.Core/**/*.cs"
---
# Core Guidelines

- `ExecutionEngine` is the source of truth for apply/revert/rollback.
- Treat external commands, registry access, and PowerShell as security-sensitive.
- Validate input, keep side effects logged, and prefer safe allow-lists over concatenation.
- Preserve rollback metadata and model compatibility when changing action flows.
- When touching hardware or config services, keep null handling and fallback behavior explicit.
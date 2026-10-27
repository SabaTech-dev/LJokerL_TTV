---
name: security-audit-remediation
description: 'Analyze and remediate SonarQube, Kiuwan, CWE, CVE, injection, secrets, path traversal, unsafe command execution, or validation findings for LJokerL_TTV.'
argument-hint: 'Describe the findings report or the code area to audit'
---

# Security Audit Remediation

## When to Use
- Auditing security findings from SonarQube or Kiuwan
- Fixing unsafe command, registry, file-system, or PowerShell usage
- Hardening validation, error handling, and allow-lists
- Preparing a client-facing security summary

## Procedure
1. Read the finding report, relevant code, and the security-oriented instructions.
2. Classify issues by risk and exploitability.
3. Apply the smallest safe fix that addresses the root cause.
4. Route to [build verifier](../../agents/build-verifier.agent.md) for validation.
5. Record what changed and why in the report and trackers.

## References
- [Security auditor](../../agents/security-auditor.agent.md)
- [Core rules](../../instructions/core.instructions.md)
- [Build verifier](../../agents/build-verifier.agent.md)
- [Executive report prompt](../../prompts/security-executive-report.prompt.md)

---
description: "Use when editing the custom test runner or any test-like code under src/LJokerL.Tests."
applyTo:
  - "src/LJokerL.Tests/**/*.cs"
---
# Test Runner Guidelines

- `TestRunner.cs` is a custom console runner, not xUnit.
- Add tests where the runner expects them and preserve exit codes and summary output.
- Keep tests deterministic and repository-relative.
- If models or config names change, update the runner and the docs together.
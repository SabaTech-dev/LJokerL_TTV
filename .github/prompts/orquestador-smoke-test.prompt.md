---
description: "Test the Orquestador router with representative workspace tasks."
name: "Orquestador Smoke Test"
agent: "orquestador"
argument-hint: "Use the current workspace task or a representative scenario"
---
Test the `Orquestador` agent against three representative workspace scenarios:

1. A WPF UI change in `src/LJokerL.App`.
2. A security finding that should be routed to `security-auditor`.
3. A build or packaging validation task that should be routed to `build-verifier`.

For each scenario, ask the router to:
- choose the best route (instruction, prompt, skill, or specialized agent)
- explain why it chose that route
- update its TODO, memory, and changelog trackers
- list any files it would touch and any validation it would run

Then report whether the routing was correct, what was missing, and which customization file should be added or refined next.
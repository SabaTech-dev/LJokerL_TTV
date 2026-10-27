---
name: workspace-customization-bootstrap
description: 'Bootstrap or refresh Copilot customizations for LJokerL_TTV. Use when creating or updating AGENTS, copilot-instructions, instructions, agents, prompts, skills, trackers, or lightweight docs.'
argument-hint: 'Describe the customization goal or area to refresh'
---

# Workspace Customization Bootstrap

## When to Use
- Creating the initial Copilot workspace setup
- Adding a new area-specific instruction
- Updating router agents, prompts, or changelog files
- Refactoring or de-duplicating existing customization files

## Procedure
1. Read the central guide at [copilot-instructions](../../copilot-instructions.md) and the relevant existing customization files.
2. Identify whether the task needs an instruction, prompt, agent, or skill.
3. Prefer one concern per file and narrow `applyTo` patterns.
4. Update the central index and tracker files (`todo.md`, `memory.md`, `changelog.md`) when the change is durable.
5. Validate links, names, and descriptions before finishing.

## References
- [Orquestador](../../agents/orquestador.agent.md)
- [Docs maintainer](../../agents/docs-maintainer.agent.md)
- [App WPF rules](../../instructions/app-wpf.instructions.md)
- [Core rules](../../instructions/core.instructions.md)

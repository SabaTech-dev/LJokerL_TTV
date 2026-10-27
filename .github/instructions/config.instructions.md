---
description: "Use when editing JSON catalogs in Config or when updating profiles, tweaks, services, app lists, or hardware rules."
applyTo: "Config/*.json"
---
# Config Catalog Guidelines

- Keep JSON valid and stable.
- Treat ids, names, and references as public contracts; update dependent profiles, tests, and docs when they change.
- Keep safe / advanced / experimental intent consistent with the validation reports.
- Prefer small, explicit records over clever structure.
- When adding or removing entries, re-check references in profiles and reports.
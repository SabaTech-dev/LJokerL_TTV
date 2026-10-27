---
description: "Use when editing WPF views, view models, command bindings, UI state, or app-layer behavior in src/LJokerL.App."
applyTo:
  - "src/LJokerL.App/**/*.cs"
  - "src/LJokerL.App/**/*.xaml"
---
# WPF App Guidelines

- Keep UI logic in `LJokerL.App`, not in Core.
- Prefer `ObservableObject`, `RelayCommand`, and explicit command refresh patterns.
- Keep code-behind thin; move behavior into view models or services.
- When changing `MainViewModel`, update filters, notifications, and `CanExecute` refreshes together.
- Keep bindings predictable and null-safe.
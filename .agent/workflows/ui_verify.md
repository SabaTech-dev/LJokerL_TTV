---
description: Workflow for verifying the WPF User Interface.
---

# UI Verification Workflow

This workflow validates the WPF Control Center UI.

## WPF UI Verification

1. **Launch App**
   - Run the WPF app (`LJokerL.App.exe`) or `dotnet run` from `src/LJokerL.App`.

2. **Visual Inspection**
   - Confirm the header and profile list render correctly.
   - Verify Monitoring metrics update and progress bars animate.
   - Verify Benchmark panel shows progress and historical runs.

3. **Input Handling**
   - Test numeric-only fields (cores/threads/ram/build) and validate input filtering.

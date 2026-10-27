---
description: Workflow to build, verify, and package the LJokerL_TTV WPF release.
---

# Deploy Release Workflow

This workflow guides the process of creating a new release of the LJokerL_TTV Custom Config PRO.

## Steps

1. **Verify Codebase Integrity**
   - Ensure WPF app builds and Config JSON files are valid.
   - Verify `Config/*.json` versions match expectations.

2. **Update Version Number**
   - Update version metadata in docs and release notes as needed.

3. **Build Executable**
   - Publish the WPF app.
   ```bash
   dotnet publish src/LJokerL.App -c Release
   ```

4. **Smoke Test**
   - Launch the published `LJokerL.App.exe` and verify Monitoring/Benchmark panels load.

5. **Verify Artifacts**
   - Check that `LJokerL.App.exe` exists in the publish output.
   - Check that `Config` and `Reports` folders are included.

6. **Create Release Package**
   - Zip the publish output plus `Config`, `Reports`, and `Legacy` (optional).

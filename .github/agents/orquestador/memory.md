# Orquestador memory

- Workspace: `LJokerL_TTV`
- Canonical customization index: `.github/copilot-instructions.md`
- Keep customizations link-first and minimal.
- Prefer specialized agents over broad, do-everything instructions.
- HISTORICAL note (superseded by the C# app flow): the retired smooth-VRR PowerShell script was hardened via `security-auditor` and validated via `build-verifier`; its final elevated Windows PowerShell 5.1 run completed with `Fallidos: 0`. The script and its state directory no longer exist in this repo.
- Host-specific note: on this Windows 25H2 host, USB selective suspend works with subgroup `2a737441-1930-4402-8d77-b2bebba308a3` and setting `48e6b7a6-50f5-4782-a5d4-53bb8f07e226`; alias-based `SUB_USB USBSELECTIVE` failed.
- Reporting note: `already-applied` and `already-absent` are now separated from real skips so the final summary reflects actual pending work.
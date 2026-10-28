# Publicación en winget (Windows Package Manager)

Guía para publicar LJokerL TTV Custom Config PRO en el catálogo oficial
[microsoft/winget-pkgs](https://github.com/microsoft/winget-pkgs).

## Pasos

1. Genera el instalador con `Installer/CreateInstaller.ps1` y calcula su hash:
   `Get-FileHash .\Output\LJokerL_TTV_Installer_v13.0.1.exe -Algorithm SHA256`.
2. Haz un fork de `microsoft/winget-pkgs` y añade el manifiesto en
   `manifests/s/SabaTechDev/LJokerL_TTV/13.0.1/SabaTechDev.LJokerL_TTV.yaml`
   (manifiesto monolítico, `ManifestType: singleton`).
3. Sustituye `<SHA256>` por el hash del paso 1 y ajusta la versión y la URL
   de la release si procede.
4. Valida el YAML con [winget-validate](https://aka.ms/winget-validate)
   (`winget-validate <archivo>.yaml`) o con el esquema JSON de la primera línea.
5. Abre una PR hacia `microsoft/winget-pkgs`; su CI valida el manifiesto y un
   revisor lo aprueba.

## Plantilla de manifiesto

Los switches silenciosos son de Inno Setup de serie; la clave
`InstallerSwitches` del esquema v1.x declara a winget cuáles usar en cada
modo. Copia, descomenta y sustituye los placeholders:

```yaml
# yaml-language-server: $schema=https://aka.ms/winget-manifest.singleton.1.6.0.schema.json
PackageIdentifier: SabaTechDev.LJokerL_TTV
PackageVersion: 13.0.1
PackageName: LJokerL TTV Custom Config PRO
Publisher: SabaTechDev
# License: MIT
# LicenseUrl: https://github.com/SabaTech-dev/LJokerL_TTV/blob/develop/LICENSE
# ShortDescription: Centro de control de tweaks y perfiles para streaming de Twitch.
# Tags:
#   - twitch
#   - streaming
#   - tweaks
Installers:
  - Architecture: x64
    InstallerType: inno
    InstallerUrl: https://github.com/SabaTech-dev/LJokerL_TTV/releases/download/v13.0.1/LJokerL_TTV_Installer_v13.0.1.exe
    InstallerSha256: <SHA256>
    InstallerSwitches:
      Silent: /VERYSILENT /NORESTART
      SilentWithProgress: /SILENT /NORESTART
    # UpgradeBehavior: install
ManifestType: singleton
ManifestVersion: 1.6.0
```

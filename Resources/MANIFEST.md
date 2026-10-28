# Manifiesto SHA-256 de `Resources/` (binarios externos no versionados)

- **Propósito**: verificación de integridad de los binarios de terceros bajo
  `Resources/` que NO se versionan en el repositorio (política P0-5 documentada
  en [DEPENDENCIES.md](../DEPENDENCIES.md); tarea P2-3 del TODO). Si un binario
  local no coincide con el hash de esta tabla, no debe ejecutarse: debe volver
  a obtenerse de su distribución oficial.
- **Fecha de generación**: 27-10-2026 (PowerShell `Get-FileHash -Algorithm
  SHA256` sobre Windows).
- **Cobertura**: todos los `*.exe` y `*.zip` de `Resources/` (primer nivel y
  subdirectorios `ActualizarNvidiaDLSS/` y `SCEHUB-1.2.0/`): 25 ficheros,
  69,07 MiB en total.
- **Cómo regenerar un hash**: `Get-FileHash -Algorithm SHA256 <fichero>`.
  Las rutas de la tabla son relativas a `Resources/`.
- **Origen**: distribución oficial del fabricante de cada herramienta (nombre,
  versión conocida, propósito y licencia en [DEPENDENCIES.md](../DEPENDENCIES.md)).
  No se incluyen URLs de descarga.
- **Nota sobre `.dll`**: existen 16 ficheros `.dll` en `Resources/`
  (principalmente las DLL de NVIDIA DLSS en `ActualizarNvidiaDLSS/`); no se
  listan individualmente en esta tabla, quedan sujetos a la misma política de
  no versionado y pueden verificarse con el mismo comando.
- **Nota**: `SCEHUB-1.2.0/SCEWIN_64.exe` y
  `SCEHUB-1.2.0/5.05.01.0002/SCEWIN_64.exe` comparten hash y tamaño: es la
  misma copia del binario distribuida en dos ubicaciones del paquete.

| Fichero (ruta relativa a `Resources/`) | Tamaño (MiB) | Origen | SHA-256 |
| --- | --- | --- | --- |
| `ActualizarNvidiaDLSS/nvidiaDlssGlom.exe` | 0,24 | Distribución oficial del fabricante (ver DEPENDENCIES.md) | a7a511d01d56eb84acb14b3998997472913aa533d460180887623ad7122ad7d5 |
| `autoruns.exe` | 1,70 | Distribución oficial del fabricante (ver DEPENDENCIES.md) | 6111a70da65153e6ded71eae2057bf6760f340476261f6e15a80479daf9724eb |
| `Counter Control.exe` | 3,42 | Distribución oficial del fabricante (ver DEPENDENCIES.md) | beb1c8eb2166b758aebb7c60ee82291de5e8f1d2eac8b362d6b24aa65a0c62b4 |
| `CrystalMark3D25_1_0_0.exe` | 7,49 | Distribución oficial del fabricante (ver DEPENDENCIES.md) | 6521d013d8b7d7bb45056579f8b1c75087fdf70e70ec6e220e1c72756fe43c1b |
| `GoInterruptPolicy.exe` | 7,04 | Distribución oficial del fabricante (ver DEPENDENCIES.md) | 45bb6d4a08d0129f2b3b82ec15d8d80a76489f3a148a6ad6b579364160e46456 |
| `innoextract.exe` | 1,16 | Distribución oficial del fabricante (ver DEPENDENCIES.md) | 946bd06d8b3722a791fea4d84f496139eef6faef61640924cb72f5f1998a68e1 |
| `InSpectre.exe` | 0,13 | Distribución oficial del fabricante (ver DEPENDENCIES.md) | 615bcb71dcd0f22b5c9aff6ff942d8fc97de61c133624a198a0caaa3e354d2b4 |
| `MemTest64.exe` | 3,10 | Distribución oficial del fabricante (ver DEPENDENCIES.md) | e0e3e405f90a44fedef0ccc0b428c65e753d39df7963c3c23fe3ae77d36971f9 |
| `MPOGPUFIX.exe` | 3,51 | Distribución oficial del fabricante (ver DEPENDENCIES.md) | 8c5ae5236a032a053c4204e1b874443e17b91a2e9a400e9a8f3c43934eec54aa |
| `MSI_util_v3.exe` | 0,04 | Distribución oficial del fabricante (ver DEPENDENCIES.md) | 695800afad96f858a3f291b7df21c16649528f13d39b63fb7c233e5676c8df6f |
| `NVCleanstall_1.19.0.exe` | 3,81 | Distribución oficial del fabricante (ver DEPENDENCIES.md) | 9dd36ef956af927cf41fa441f91b329a7973e13965e4e7d70e6fa9c1df1cade6 |
| `NVPI-R.exe` | 1,09 | Distribución oficial del fabricante (ver DEPENDENCIES.md) | a5139d1c974b861c15c5e2d670cd2a8120fe74dbdff57a12796ad74833018ba4 |
| `Optimizer_tmp.exe` | 2,45 | Distribución oficial del fabricante (ver DEPENDENCIES.md) | 03a234060541b686ac4265754aff43df9325c21383f90e17f831e67965d717f8 |
| `PowerSettingsExplorer.exe` | 0,10 | Distribución oficial del fabricante (ver DEPENDENCIES.md) | c098aef498e9255b06d96d7af3fa71dfbcd137348d499fc109e213e834fea9db |
| `PresentMon-2.4.1-x64.exe` | 0,88 | Distribución oficial del fabricante (ver DEPENDENCIES.md) | d74183e7ae630f72cd3690be0373ecbfdc6cbb86578148aab8fa2a7166068f34 |
| `SCEHUB-1.2.0/5.05.01.0002/SCEWIN_64.exe` | 0,65 | Distribución oficial del fabricante (ver DEPENDENCIES.md) | 2d78e7bb62fcab44c54fe853f51e8836ada45ea374027423bcb6d7a5db46502b |
| `SCEHUB-1.2.0/SCEWIN_64.exe` | 0,65 | Distribución oficial del fabricante (ver DEPENDENCIES.md) | 2d78e7bb62fcab44c54fe853f51e8836ada45ea374027423bcb6d7a5db46502b |
| `SCEHUB-1.2.0/UEFIExtract.exe` | 1,12 | Distribución oficial del fabricante (ver DEPENDENCIES.md) | 2c48e86a279bc00885ed83e6d46d8a2fc3418539475c9c6eb79be0f8a83d50c8 |
| `SCEHUB-1.2.0/UEFIFind.exe` | 0,82 | Distribución oficial del fabricante (ver DEPENDENCIES.md) | cb69ff2320dc6c6217300dc6adb01833b5f14cd4bef0747b6200bdbe75dbfad1 |
| `SCEHUB-1.2.0/UEFITool.exe` | 14,58 | Distribución oficial del fabricante (ver DEPENDENCIES.md) | bb3b75f9946701ba7c3c04e17b0cbdb7376257d07ce8440adf085b63cbef24c5 |
| `TCPOptimizer.exe` | 0,65 | Distribución oficial del fabricante (ver DEPENDENCIES.md) | 0a49dc0d2ce725af347df632539b70afcfd22b38e285920b515143332a5511e9 |
| `ThrottleStop_9.7.3.zip` | 1,71 | Distribución oficial del fabricante (ver DEPENDENCIES.md) | 7b5e1a650a845cdab200bd6a7766d8f56ae458f633a6f2f3df5a9ec72847098a |
| `Windows_Update_Blocker.exe` | 0,76 | Distribución oficial del fabricante (ver DEPENDENCIES.md) | 15fccf8c018bbbed14664d5a5528cdf087b9032543be2169d78ab25d141d2b2c |
| `WindowsReset.zip` | 0,56 | Distribución oficial del fabricante (ver DEPENDENCIES.md) | f92b9176127e767d7af55f96e516c4fc9b20989af4190b73d2c8f0e32059ee94 |
| `winscript-portable.exe` | 11,40 | Distribución oficial del fabricante (ver DEPENDENCIES.md) | 53601f90a5cb12b532bc455683d8bbeba0b30a0c1ce26bb3b938f7a916393a62 |

Get-Process GameBar* -ErrorAction SilentlyContinue | Stop-Process -Force
Get-Process Gaming* -ErrorAction SilentlyContinue | Stop-Process -Force
Get-AppxPackage -AllUsers *Xbox* | Remove-AppxPackage -AllUsers -ErrorAction SilentlyContinue
Get-AppxPackage -AllUsers *Gaming* | Remove-AppxPackage -AllUsers -ErrorAction SilentlyContinue
Get-AppxPackage -AllUsers *GameBar* | Remove-AppxPackage -AllUsers -ErrorAction SilentlyContinue
Get-AppxProvisionedPackage -Online | Where-Object {$_.DisplayName -match "Xbox"} | Remove-AppxProvisionedPackage -Online
reg add "HKLM\SOFTWARE\Policies\Microsoft\Windows\GameDVR" /v AllowGameDVR /t REG_DWORD /d 0 /f
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR" /v AppCaptureEnabled /t REG_DWORD /d 0 /f
reg add "HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR" /v GameDVR_Enabled /t REG_DWORD /d 0 /f
sc stop GameBarPresenceWriter
sc config GameBarPresenceWriter start= disabled
schtasks /Delete /TN "\Microsoft\XblGameSave\XblGameSaveTask" /F
schtasks /Delete /TN "\Microsoft\XblGameSave\XblGameSaveTaskLogon" /F
schtasks /Delete /TN "\Microsoft\Windows\GameBarPresenceWriter" /F
Remove-Item -Path "C:\Program Files\WindowsApps\Microsoft.Xbox*" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item -Path "C:\Program Files\WindowsApps\Microsoft.Gaming*" -Recurse -Force -ErrorAction SilentlyContinue
reg add "HKLM\SOFTWARE\Policies\Microsoft\WindowsStore" /v AutoDownload /t REG_DWORD /d 2 /f

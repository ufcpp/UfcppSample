param([string] $src)

$line = '━' * 40

Write-Host $line; . C:\Windows\Microsoft.NET\Framework64\v2.0.50727\csc.exe -t:library $src | Write-Host -ForegroundColor DarkCyan
Write-Host $line; . C:\Windows\Microsoft.NET\Framework64\v3.5\csc.exe       -t:library $src | Write-Host -ForegroundColor DarkMagenta
Write-Host $line; . C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe -t:library $src | Write-Host -ForegroundColor DarkGreen
Write-Host $line; . 'C:\Program Files (x86)\MSBuild\12.0\Bin\csc.exe'       -t:library $src | Write-Host -ForegroundColor DarkRed
Write-Host $line; . 'C:\Program Files (x86)\MSBuild\14.0\Bin\csc.exe'       -t:library $src | Write-Host -ForegroundColor DarkBlue

param([string] $src)

. C:\Windows\Microsoft.NET\Framework64\v2.0.50727\csc.exe -t:library $src | Write-Host -ForegroundColor DarkCyan
. C:\Windows\Microsoft.NET\Framework64\v3.5\csc.exe       -t:library $src | Write-Host -ForegroundColor DarkMagenta
. C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe -t:library $src | Write-Host -ForegroundColor DarkGreen
. 'C:\Program Files (x86)\MSBuild\12.0\Bin\csc.exe'       -t:library $src | Write-Host -ForegroundColor DarkRed
. 'C:\Program Files (x86)\MSBuild\14.0\Bin\csc.exe'       -t:library $src | Write-Host -ForegroundColor DarkBlue

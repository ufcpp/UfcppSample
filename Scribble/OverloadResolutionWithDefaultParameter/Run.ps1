. 'C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe' .\X\X.sln
cp .\X\bin\Debug\X.dll .

$compilers = @(
    @{ 'compiler' = $env:windir + '\Microsoft.NET\Framework64\v2.0.50727\csc.exe'; 'color' = 'DarkCyan' }
    @{ 'compiler' = $env:windir + '\Microsoft.NET\Framework64\v3.5\csc.exe'      ; 'color' = 'DarkMagenta' }
    @{ 'compiler' = $env:windir + '\Microsoft.NET\Framework64\v4.0.30319\csc.exe'; 'color' = 'DarkGreen' }
    @{ 'compiler' = ${env:ProgramFiles(x86)} + '\MSBuild\12.0\Bin\csc.exe'       ; 'color' = 'DarkRed' }
    @{ 'compiler' = ${env:ProgramFiles(x86)} + '\MSBuild\14.0\Bin\csc.exe'       ; 'color' = 'DarkBlue' }
)

$line = '„ª' * 40

foreach($item in $compilers)
{
    . {
        $line

        . $item.compiler -out:out.exe Program.cs -r:X.dll

        if(Test-Path out.exe)
        {
            .\out.exe
            [void](rm out.exe)
        }
    } | Write-Host -ForegroundColor $item.color
}

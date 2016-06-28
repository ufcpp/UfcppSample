$msbuild = ls 'C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe'

. $msbuild ..\CodeAwareLibrarySample.sln /t:rebuild /p:Configuration=Release

if (-not (Test-Path bin)) { mkdir bin }

cp ..\FluentArithmetic\bin\Release\FluentArithmetic.dll bin
cp ..\FluentArithmeticAnalyzer\FluentArithmeticAnalyzer\bin\Release\FluentArithmeticAnalyzer.dll bin

.\NuGet.exe pack .\FluentArithmetic.nuspec

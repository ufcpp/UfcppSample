# build
$msbuild = (Get-VSSetupInstance)[0].InstallationPath + '\MSBuild\15.0\Bin\MSBuild.exe'

& $msbuild /p:Configuration=Release

cp .\ClassLibrary.A\bin\Release\ClassLibrary.A.1.0.0.nupkg .
cp .\ClassLibrary.B\bin\Release\ClassLibrary.B.1.0.0.nupkg .

dotnet publish -c release -f netcoreapp2.0

# run with net47 → ClassLibrary.B
.\ConsoleApp1\bin\Release\net47\ConsoleApp1.exe

# run with netcoreapp2.0 Framework Dependent → ClassLibrary.A
dotnet .\ConsoleApp1\bin\Release\netcoreapp2.0\ConsoleApp1.dll

# run with netcoreapp2.0 Self-contained → ClassLibrary.B
dotnet .\ConsoleApp1\bin\Release\netcoreapp2.0\publish\ConsoleApp1.dll

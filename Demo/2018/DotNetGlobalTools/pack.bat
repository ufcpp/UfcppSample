dotnet pack -c release cszip/cszip.csproj
dotnet pack -c release csunzip/csunzip.csproj
dotnet pack -c release xstatic/xstatic.csproj
mkdir packages
copy cszip\bin\release\*.nupkg packages
copy csunzip\bin\release\*.nupkg packages
copy xstatic\bin\release\*.nupkg packages

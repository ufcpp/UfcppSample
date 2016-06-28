mkdir bin
msbuild
.\NuGet.exe pack .\MinimumAsyncBridge.nuspec -NoPackageAnalysis -OutputDirectory .
.\NuGet.exe pack .\MinimumThreadingBridge.nuspec -NoPackageAnalysis -OutputDirectory .
.\NuGet.exe pack .\MvvmBridge.nuspec -NoPackageAnalysis -OutputDirectory .

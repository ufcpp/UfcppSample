$csc = 'C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\Roslyn\csc.exe'

& $csc a.cs /t:library /debug:portable /embed
& $csc b.cs /t:library /debug:embedded /embed
& $csc c.cs /t:library /debug:portable
& $csc d.cs /t:library /debug:embedded

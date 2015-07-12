all: MyMath.dll test.exe

MyMath.dll: Complex.cs CartesianComplex.cs PolarComplex.cs AssemblyInfo.cs
	csc /t:library /out:MyMath.dll Complex.cs CartesianComplex.cs PolarComplex.cs AssemblyInfo.cs

test.exe: test.cs
	csc /r:MyMath.dll test.cs

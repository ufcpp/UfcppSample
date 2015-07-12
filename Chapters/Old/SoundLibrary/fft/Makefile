CL   = "C:\Program Files\Microsoft Visual Studio .NET 2003\Vc7\bin\cl.exe"
CLOPTION = /CLR /nologo /c
LINK = "C:\Program Files\Microsoft Visual Studio .NET 2003\Vc7\bin\link.exe"

all:fft.dll

fftsg.obj: fftsg.c
	$(CL) $(CLOPTION) fftsg.c

fft.obj: fft.cpp
	$(CL) $(CLOPTION) fft.cpp

fft.dll: fftsg.obj fft.obj
	$(LINK) -dll /out:fft.dll /nologo fftsg.obj fft.obj

clean:
	rm *.obj

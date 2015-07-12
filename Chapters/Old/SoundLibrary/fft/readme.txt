FFT はネットから落としてきたライブラリを使っています。
作者はこちら↓。

    Copyright(C) 1996-2001 Takuya OOURA
    email: ooura@mmm.t.u-tokyo.ac.jp
    download: http://momonga.t.u-tokyo.ac.jp/~ooura/fft.html

Managed C++ を使ってるんでコンパイルには cl.exe が必要です。
(.NET Framework SDK を入れると、
 "C:\Program Files\Microsoft Visual Studio .NET\Vc7\bin\cl.exe"
 にインストールされます。)

Visual C++ .net は持っていないので、コマンドラインでコンパイルしました。
Makefile も付けておきます。
make の実行前に vsvars32.bat を実行する必要があります。

(vsvars32.bat のフルパスは
"C:\Program Files\Microsoft Visual Studio .NET 2003\Vc7\bin\vsvars32.bat")

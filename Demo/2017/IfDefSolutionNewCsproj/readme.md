## 背景

要件・背景に関しては[IfDefSolution](../IfDefSolution)の方のreadme参照。

同じことをSDK-basedなcsproj (Visual Studio 2017から使えるようになった新形式のcsproj)でできるかを試してみたプロジェクト。

結論だけ書くと、

- TargetFramework が .NET Framework (net46 とか)ならおおむね問題なく動く
- Visual Studio 2017 Update 3 (15.3)までは、以下の不具合あり
  - Web 向けではコンパイルできない(`Sdk="Microsoft.NET.Sdk.Web"`が入ってるとダメ)
  - TargetFramework が netcoreapp だとコンパイルできない
- この不具合は Update 4 (15.4)で治ったっぽい

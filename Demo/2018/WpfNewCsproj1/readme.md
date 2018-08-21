# SDK-based WPF apps

今年に入って、WPF プロジェクトでも SDK-based な csproj 満足に使えるようになってそう。

参考:

- https://github.com/dotnet/project-system/issues/1467#issuecomment-362963386

(2月くらいのコメントなので、たぶん、VS 15.7 から。)

このフォルダー以下にあるのは、

- テンプレ通りに WPF プロジェクトを作成
- 上記 URL の内容に沿って csproj を書き換え
- AssemblyInfo.cs を削除

ってやったもの。

param([string]$path)

. .\ZipLib.ps1

# テンポラリファイル準備
$name = [IO.Path]::GetFileName($path)
$fullname = [IO.Path]::GetFileName($path)
$tempPath = [IO.Path]::GetTempPath()

# コピー＆不要ファイルの削除
cp $fullname $tempPath -Recurse -Force

Push-Location $tempPath

rm ($name + '\*.suo') -Force
rm ($name + '\*\obj') -Recurse -Force
rm ($name + '\*\bin') -Recurse -Force

# ZIP 作成
$tempZip = [IO.Path]::GetTempFileName()
mv $tempZip ($tempZip + '.zip')
$tempZip = ($tempZip + '.zip')

New-Zip $tempZip

ls $name | Add-Zip $tempZip

# 一時ファイル消去
rm $name -Recurse -Force

# 作った ZIP のコピー
Pop-Location
mv $tempZip ('.\' + $name + '.zip') -Force

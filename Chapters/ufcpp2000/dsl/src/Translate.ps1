param($inFile, $outFile)

$xml = [xml](Get-Content $inFile)

$result =
@"
class $($xml.class.name)
{

"@

foreach($var in $xml.class.var)
{
	$result +=
@"
	$($var.type) $($var.name);

"@
}

$result +=
@"
}

"@

$result | Set-Content $outFile

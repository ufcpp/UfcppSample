$sourceCodes = ls Func*.cs -Recurse | ?{ ($_ -notmatch '\\obj\\') -and ($_ -notmatch '\\Properties\\') }

foreach ($src in $sourceCodes)
{
    cls

    $x = $src.FullName.Split('\\')
    $name = $x[$x.Length - 2] + '/' + $x[$x.Length - 1]
    Write-Host $name
    Write-Host

    .\read-summary.ps1 $src
    Write-Host

    .\csc.ps1 $src

    Write-Host
    Write-Host [press enter to continue]
    Read-Host
}



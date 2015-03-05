$sourceCodes = @(
    '.\VS2015_CS6\KatakanaMiddleDot.cs';
    '.\VS2015_CS6\DefiniteAssignment.cs';
)

foreach ($src in $sourceCodes)
{
    cls
    Write-Host $src
    .\csc.ps1 $src
    Read-Host
}



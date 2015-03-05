param([string] $src)

$state = 0

Get-Content $src | % {
    if($state -eq 0)
    {
        if($_ -match '\<summary\>')
        {
            $state = 1
        }
    }
    elseif($state -eq 1)
    {
        if($_ -match '\<\/summary\>')
        {
            $state = 2
        }
        else
        {
            $_.Replace('///', '')
        }
    }
}

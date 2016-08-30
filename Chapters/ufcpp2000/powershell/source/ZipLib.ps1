# usage:
# New-Zip sample.zip

function New-Zip
{
	param([string]$zipfilename)
	set-content $zipfilename ("PK" + [char]5 + [char]6 + ("$([char]0)" * 18))
	(dir $zipfilename).IsReadOnly = $false
}

# usage:
# ls sample/* | Add-Zip sample.zip

function Add-Zip
{
	param([string]$zipfilename)

	# convert to full path
	$zipfilename = ls $zipfilename | %{$_.FullName}

	if(-not (test-path($zipfilename)))
	{
		set-content $zipfilename ("PK" + [char]5 + [char]6 + ("$([char]0)" * 18))
		(dir $zipfilename).IsReadOnly = $false	
	}
	
	$shellApplication = new-object -com shell.application
	$zipPackage = $shellApplication.NameSpace($zipfilename)

	foreach($file in $input) 
	{ 
            $zipPackage.CopyHere($file.FullName)
            Start-sleep -milliseconds 500
	}
}

function Get-Zip
{
	param([string]$zipfilename)

	# convert to full path
	$zipfilename = ls $zipfilename | %{$_.FullName}

	if(test-path($zipfilename))
	{
		$shellApplication = new-object -com shell.application
		$zipPackage = $shellApplication.NameSpace($zipfilename)
		$zipPackage.Items() | Select Path
	}
}

function Extract-Zip
{
	param([string]$zipfilename, [string] $destination)

	# convert to full path
	$zipfilename = ls $zipfilename | %{$_.FullName}

	if(test-path($zipfilename))
	{	
		$shellApplication = new-object -com shell.application
		$zipPackage = $shellApplication.NameSpace($zipfilename)
		$destinationFolder = $shellApplication.NameSpace($destination)
		$destinationFolder.CopyHere($zipPackage.Items())
	}
}

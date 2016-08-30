function var([string] $name, $default = $null)
{
	$fields = $classDef.fields
	$fields[$name] = $default
}

function method([string] $name, $body)
{
	$methods = $classDef.methods
	$methods[$name] = $body
}

function new($body)
{
	$classDef.new = $body
}

function global:Def-Class([string] $ClassName, $definition)
{
	$classDef = @{fields = @{}; methods = @{}; new = {} }

	& $definition

	Set-Variable ('_ClassPrefix' + $ClassName) $classDef -scope global
}

function CallWithVarArgs
{
	param($method, $varargs)

	$1 = $varargs[0]
	$2 = $varargs[1]
	$3 = $varargs[2]
	$4 = $varargs[3]
	$5 = $varargs[4]
	$6 = $varargs[5]
	$7 = $varargs[6]
	$8 = $varargs[7]
	$9 = $varargs[8]
	$l = $varargs.Length

	if($l -eq 0)     { & $method }
	elseif($l -eq 1) { & $method $1 }
	elseif($l -eq 2) { & $method $1 $2 }
	elseif($l -eq 3) { & $method $1 $2 $3 }
	elseif($l -eq 4) { & $method $1 $2 $3 $4 }
	elseif($l -eq 5) { & $method $1 $2 $3 $4 $5 }
	elseif($l -eq 6) { & $method $1 $2 $3 $4 $5 $6 }
	elseif($l -eq 7) { & $method $1 $2 $3 $4 $5 $6 $7 }
	elseif($l -eq 8) { & $method $1 $2 $3 $4 $5 $6 $7 $8 }
	else             { & $method $1 $2 $3 $4 $5 $6 $7 $8 $9 }
}

function global:New-Instance
{
	param($ClassName)

	$obj = @{}
	$classDef = (Get-Variable ('_ClassPrefix' + $ClassName) -scope global).Value
	$obj._class = $classDef

	foreach($key in $classDef.fields.Keys)
	{
		$obj[$key] = $classDef.fields[$key]
	}

	$_ = $obj
	CallWithVarArgs $classDef.new $args

	$obj
}

function global:Call-Method
{
	param($instance, $MethodName)

	$method = $instance._class.methods[$MethodName]
	$_ = $instance

	CallWithVarArgs $method $args
}

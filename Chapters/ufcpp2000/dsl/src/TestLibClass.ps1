Def-Class Point {
	var x
	var y

	new {
		param($x = 0, $y = 0)
		$_.x = $x
		$_.y = $y
	}

	method abs {
		[Math]::Sqrt($_.x * $_.x + $_.y * $_.y)
	}

	method scale {
		param($a)
		$_.x = $a * $_.x
		$_.y = $a * $_.y
	}
}

$x = New-Instance Point
$y = New-Instance Point 3 4

function show($x)
{
	'({0}, {1}), abs = {2}' -f $x.x, $x.y, (Call-Method $x abs)
}

show $x
show $y

Call-Method $y scale 2
show $y

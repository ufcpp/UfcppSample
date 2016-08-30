if ($hand -eq $null -or $hand.Length -eq 0)
{
    return $null
}

$count = $table.Length

if ($count -eq 0)
{
    $count = 1
}

if ($hand.Length -lt $count)
{
    return $null
}

for ($i = 0; $i -lt 50; $i++)
{
    $result = [CardGame.Card]::Shuffle($hand, $count)

    if([Daifugou.Game]::CanPlay($result, $table, $rank, $suit, $mode, $revolution))
    {
        return $result
    }
}

$null

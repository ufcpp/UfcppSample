from Daifugou import Game

shuffle = lambda lis: [i[1] for i in sorted([[rand.NextDouble(), i] for i in lis], lambda x, y: cmp(x[0], y[0]))]

def play():
	if hand == None or  hand.Length == 0:
		return None

	count = table.Length

	if count == 0:
		count = 1

	if hand.Length < count:
		return None

	for dummy in range(50):
		x = shuffle(hand)
		x = x[0:count]

		if Game.CanPlay(x, table, rank, suit, mode, revolution):
			return x

	return None

result = play()

using RgiSequenceFinder;

foreach (var s in Data.RgiEmojiSequenceList)
{
    if (GraphemeBreak.IsKeycap(s) > 0)
    {
        System.Console.WriteLine(s);
    }
}

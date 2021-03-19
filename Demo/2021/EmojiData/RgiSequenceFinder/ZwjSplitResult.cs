namespace RgiSequenceFinder
{
    /// <summary>
    /// ZWJ 分割するときに一緒に skin tone の抽出をしておいた方が2度手間にならなくて計算量お得なので一緒に詰め込む。
    /// </summary>
    /// <remarks>
    /// 中身に <see cref="Byte8"/> を使っているものの、
    /// ZWJ 8つも入った文字、RGI には入らないだろうし、末尾1個を <see cref="SkinTonePair"/> で使うことにした。
    /// </remarks>
    public readonly struct ZwjSplitResult
    {
        public const int MaxLength = 7;

        private readonly Byte8 _bytes;

        public ZwjSplitResult(Byte8 zwjPositions, SkinTonePair skinTones)
        {
            zwjPositions.V7 = skinTones.Value;
            _bytes = zwjPositions;
        }

        public byte this[int index] => _bytes[index];

        public SkinTonePair SkinTones => new(_bytes.V7);
    }
}

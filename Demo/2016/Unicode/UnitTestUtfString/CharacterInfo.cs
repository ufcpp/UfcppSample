using System;
using System.Collections.Generic;
using static System.Text.Encoding;

namespace UnitTestUtfString
{
    struct CharacterInfo
    {
        public int codePoint;
        public string @char;
        public byte[] utf8;
        public byte[] utf16;
        public byte[] utf32;

        public static IEnumerable<CharacterInfo> GetCharacters(string s)
        {
            var bytes = UTF32.GetBytes(s);

            for (int i = 0; i < bytes.Length; i += 4)
            {
                var cp = new CharacterInfo();

                var utf32 = new byte[4];
                Array.Copy(bytes, i, utf32, 0, 4);
                cp.utf32 = utf32;

                cp.codePoint = (utf32[0] << 0)
                    | (utf32[1] << 8)
                    | (utf32[2] << 16)
                    | (utf32[3] << 24);

                var decoded = UTF32.GetString(utf32);
                cp.@char = decoded;
                cp.utf16 = Unicode.GetBytes(decoded);
                cp.utf8 = UTF8.GetBytes(decoded);

                yield return cp;
            }
        }
    }
}

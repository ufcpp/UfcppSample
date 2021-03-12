using System.Text;

namespace EmojiData
{
    class Encoder
    {
        public static string ToString(Rune[] runes)
        {
            var utf16Count = 0;
            foreach (var r in runes)
            {
                if (r.IsBmp) utf16Count++;
                else utf16Count += 2;
            }

            return string.Create(utf16Count, runes, (buffer, state) =>
            {
                foreach (var r in state)
                {
                    if (r.IsBmp)
                    {
                        buffer[0] = (char)r.Value;
                        buffer = buffer.Slice(1);
                    }
                    else
                    {
                        r.EncodeToUtf16(buffer);
                        buffer = buffer.Slice(2);
                    }
                }
            });
        }

        private static char Hex(int i) => i < 10
            ? (char)(i + '0')
            : (char)(i - 10 + 'A');

        public static string ToEscapedString(Rune[] runes)
        {
            var utf16Count = 0;
            foreach (var r in runes)
            {
                if (r.IsBmp) utf16Count += 6;
                else utf16Count += 10;
            }

            return string.Create(utf16Count, runes, (buffer, state) =>
            {
                foreach (var r in state)
                {
                    buffer[0] = '\\';
                    if (r.IsBmp)
                    {
                        buffer[1] = 'u';
                        buffer[2] = Hex(0xF & (r.Value >> 12));
                        buffer[3] = Hex(0xF & (r.Value >> 8));
                        buffer[4] = Hex(0xF & (r.Value >> 4));
                        buffer[5] = Hex(0xF & (r.Value));
                        buffer = buffer.Slice(6);
                    }
                    else
                    {
                        buffer[1] = 'U';
                        buffer[2] = '0';
                        buffer[3] = '0';
                        buffer[4] = '0';
                        buffer[5] = Hex(0xF & (r.Value >> 16));
                        buffer[6] = Hex(0xF & (r.Value >> 12));
                        buffer[7] = Hex(0xF & (r.Value >> 8));
                        buffer[8] = Hex(0xF & (r.Value >> 4));
                        buffer[9] = Hex(0xF & (r.Value));
                        buffer = buffer.Slice(10);
                    }
                }
            });
        }
    }
}

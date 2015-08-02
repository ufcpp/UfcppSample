using MiniMessagePack;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using ValueTuples.Reflection;

namespace ValueTuples.Serialization
{
    //todo: DateTime
    // decimal はどうしよう？

    //todo: 型情報のハッシュ値くらいは取れる仕組みを用意してないと、送信側・受信側で型が狂ってた時の判定難しくなるかも。

    //todo: エラー処理。try-catch して、その時unpackしてるRecordFieldInfoが何かを詰めた例外を再throwする

    /// <summary>
    /// 送信側・受信側の双方で同じ型情報を持っている前提で、
    /// メンバー名とかの型情報一切なしで、
    /// ・何番目のフィールドが値を持っているかだけフラグ化して最初に詰める。
    /// ・フィールドを前から順に、値を持っている箇所だけ、値を再帰的にシリアライズする。
    /// という方式でシリアライズする。
    /// </summary>
    /// <remarks>
    /// メンバーが32個以上ある型のシリアライズはできない。
    /// (64個までは増やせるけどそこまでする必要もないと判断。演算をケチった。)
    ///
    /// 内部的に MessagePack を使っているものの、数値と文字列(UTF8)のpack/unpackしか使ってない。
    /// 複合型とか配列の詰め方は自前実装。
    /// </remarks>
    public class MyBinaryFactory : ISerializerFactory
    {
        public IDeserializer GetDeserializer(Stream stream) => new MyBinaryDeserializer(stream);

        public ISerializer GetSerializer(Stream stream) => new MyBinarySerializer(stream);
    }

    internal class MyBinarySerializer : ISerializer
    {
        MiniMessagePacker _packer = new MiniMessagePacker();
        Stream _s;

        public MyBinarySerializer(Stream s)
        {
            _s = s;
        }

        private void Pack(int val) => _packer.Pack(_s, val);
        private void Pack(uint val) => _packer.Pack(_s, val);
        private void Pack(object val) => _packer.Pack(_s, val);

        public void Dispose() => _s.Dispose();

        public void Serialize(object value)
        {
            if (value == null) return;

            var info = TypeRepository.Get(value.GetType());

            if(info.IsSimple)
            {
                Pack(value);
                return;
            }

            if (info.IsArray)
            {
                var array = (IList)value;
                Pack(array.Count);
                foreach (var item in array)
                {
                    Serialize(item);
                }
                return;
            }

            if(info.IsChild)
            {
                Pack(info.Discriminator.Value);
            }

            var accessor = info.GetAccessor(value);

            var flags = 0u;
            var i = 1u;
            foreach (var f in info.Fields)
            {
                var x = accessor.Get(f.Index);
                if (x != null) flags |= i;
                i <<= 1;
            }
            Pack(flags);

            foreach (var f in info.Fields)
            {
                var x = accessor.Get(f.Index);
                Serialize(x);
            }
        }
    }

    internal class MyBinaryDeserializer : IDeserializer
    {
        MiniMessagePacker _packer = new MiniMessagePacker();
        Stream _s;

        public MyBinaryDeserializer(Stream s)
        {
            _s = s;
        }

        private Number UnpackNumber() => _packer.UnpackNumber(_s);
        private string UnpackString() => (string)_packer.Unpack(_s);

        public void Dispose() => _s.Dispose();

        public object Deserialize(Type t)
        {
            if(t.IsPrimitive)
            {
                if (t == typeof(int)) { return (int)UnpackNumber(); }
                if (t == typeof(double)) { return (double)UnpackNumber(); }
                if (t == typeof(bool)) { return (bool)UnpackNumber(); }
                if (t == typeof(long)) { return (long)UnpackNumber(); }
                if (t == typeof(byte)) { return (byte)UnpackNumber(); }
                if (t == typeof(sbyte)) { return (sbyte)UnpackNumber(); }
                if (t == typeof(short)) { return (short)UnpackNumber(); }
                if (t == typeof(ushort)) { return (ushort)UnpackNumber(); }
                if (t == typeof(uint)) { return (uint)UnpackNumber(); }
                if (t == typeof(ulong)) { return (ulong)UnpackNumber(); }
                if (t == typeof(float)) { return (float)UnpackNumber(); }
            }
            else if (t == typeof(string)) { return UnpackString(); }

            var info = TypeRepository.Get(t);

            if(info.IsArray)
            {
                var length = (int)UnpackNumber();
                var array = info.GetArray(length);
                var elem = ((ArrayTypeInfo)info).ElementType.Type;

                for (int i = 0; i < length; i++)
                {
                    var item = Deserialize(elem);
                    array.SetValue(item, i);
                }
                return array;
            }

            if(info.IsBase)
            {
                var discriminator = (int)UnpackNumber();
                info = info.GetType(discriminator);
            }

            var value = info.GetInstance();
            var accessor = info.GetAccessor(value);
            var count = info.Fields.Count();

            var flags = (uint)UnpackNumber();

            foreach (var field in info.Fields)
            {
                if ((flags & 1) == 0) continue;

                flags >>= 1;

                var x = Deserialize(field.Type.Type);
                accessor.Set(field.Index, x);
            }

            return value;
        }
    }
}

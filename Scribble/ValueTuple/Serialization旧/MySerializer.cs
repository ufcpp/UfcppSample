using System;
using System.IO;

namespace ValueTuples.Serialization旧
{
    public class MySerializer : ISerializer
    {
        StreamWriter _s;

        public MySerializer(StreamWriter s)
        {
            _s = s;
        }

        public void Serialize(IRecord record)
        {
            var tr = record as ITypedRecord;
            if (tr == null) throw new NotSupportedException();
            var info = tr.GetInfo();
            var value = record.Value;
            var count = value.Count;

            //_s.WriteLine(count);

            for (int i = 0; i < count; i++)
            {
                _s.Write(info.GetKey(i));

                var x = value[i];
                if(x != null)
                {
                    var xr = x as IRecord;
                    if(xr == null)
                    {
                        _s.Write('\t');
                        _s.Write(x.ToString());
                        _s.WriteLine();
                    }
                    else
                    {
                        _s.WriteLine('\\');
                        Serialize(xr);
                    }
                }
            }
        }
    }

    public class MyDeserializer : IDeserializer
    {
        StreamReader _s;

        public MyDeserializer(StreamReader s)
        {
            _s = s;
        }

        public void Deserialize(IRecord record)
        {
            var tr = record as ITypedRecord;
            if (tr == null) throw new NotSupportedException();
            var info = tr.GetInfo();
            var value = record.Value;
            var count = value.Count;

            for (int i = 0; i < count; i++)
            {
                var line = _s.ReadLine();
                var tab = line.IndexOf('\t');
                if(tab >0)
                {

                }

                //_s.Write(info.GetKey(i));

                //var x = value[i];
                //if (x != null)
                //{
                //    var xr = x as IRecord;
                //    if (xr == null)
                //    {
                //        _s.Write('/');
                //        _s.Write(x.ToString());
                //        _s.WriteLine();
                //    }
                //    else
                //    {
                //        _s.WriteLine('\\');
                //        Serialize(xr);
                //    }
                //}
            }

            record.Value = value;
        }
    }
}

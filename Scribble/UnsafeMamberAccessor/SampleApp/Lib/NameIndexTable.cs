using System.Collections.Generic;
using System.Text.Utf8;
using System.Runtime.CompilerServices;
using System;
using static System.Reflection.BindingFlags;

namespace SampleApp.Lib
{
    public struct NameIndexTable
    {
        private Dictionary<Utf8String, int> _table;

        public bool IsNull => _table == null;

        public NameIndexTable(Type t) : this((TupleElementNamesAttribute)t
                .GetField("_value", NonPublic | Instance)
                .GetCustomAttributes(typeof(TupleElementNamesAttribute), false)[0]) { }

        public NameIndexTable(TupleElementNamesAttribute att)
        {
            var names = att.TransformNames;
            _table = new Dictionary<Utf8String, int>();
            for (int i = 0; i < names.Count; i++)
                _table.Add(new Utf8String(names[i]), i);
        }

        public int this[Utf8String name] => _table[name];
    }
}

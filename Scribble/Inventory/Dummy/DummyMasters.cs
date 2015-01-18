using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoriesSample.Dummy
{
    class DummyMasters
    {
        public static IReadOnlyDictionary<int, UnitMaster> UnitMasters = new Dictionary<int, UnitMaster>
        {
            {  100, new UnitMaster( 100,  true, 1000, 1000) },
            {  101, new UnitMaster( 101,  true, 1500, 700) },
            {  102, new UnitMaster( 102,  true, 700, 2000) },
            { 1000, new UnitMaster(1000, false, 500, 500) },
            { 1001, new UnitMaster(1001, false, 300, 800) },
            { 1002, new UnitMaster(1002, false, 450, 500) },
            { 1003, new UnitMaster(1003, false, 900, 50) },
            { 1004, new UnitMaster(1004, false, 700, 200) },
            { 1005, new UnitMaster(1005, false, 1200, 0) },
        };

        private readonly static IReadOnlyList<UnitMaster> _leaders = UnitMasters.Values.Where(x => x.IsLeader).ToArray();
        private readonly static IReadOnlyList<UnitMaster> _servants = UnitMasters.Values.Where(x => !x.IsLeader).ToArray();

        private readonly Random _random = new Random();

        private UnitMaster GetRandom(IReadOnlyList<UnitMaster> list)
        {
            var i = _random.Next(0, list.Count);
            return list[i];
        }

        public UnitMaster GetRandomLeader() => GetRandom(_leaders);
        public UnitMaster GetRandomServant() => GetRandom(_servants);
    }
}

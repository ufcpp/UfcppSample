using Inventories;
using InventoriesSample.DataModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoriesSample.Dummy
{
    class DummyApi : IApi
    {
        private DummyMasters _dummy = new DummyMasters();

        private List<Unit> _units = new List<Unit>();
        private int _id;

        public DummyApi()
        {
            _units.Add(_dummy.GetRandomLeader().GetInstance(_id++));

            for (int i = 0; i < 5; i++)
                _units.Add(_dummy.GetRandomServant().GetInstance(_id++));
        }

        public async Task<SyncResult> Login(int authToken)
        {
            await Task.Delay(500);

            return new SyncResult { Units = _units.ToArray() };
        }

        public async Task<ItemChangedArgs<Unit>[]> HireUnit(int masterId)
        {
            await Task.Delay(500);

            return HireUnitInternal(masterId).ToArray();
        }

        private IEnumerable<ItemChangedArgs<Unit>> HireUnitInternal(int masterId)
        {
            // No such unit master.
            if (!DummyMasters.UnitMasters.ContainsKey(masterId)) yield break;

            // A leader unit must be single.
            if (DummyMasters.UnitMasters[masterId].IsLeader) yield break;

            // Special offer! Buy one Get one!
            if (masterId == 1005)
            {
                yield return ItemChangedArgs.Add(DummyMasters.UnitMasters[masterId].GetInstance(_id++));
                yield return ItemChangedArgs.Add(DummyMasters.UnitMasters[masterId].GetInstance(_id++));
            }
            else
            {
                yield return ItemChangedArgs.Add(DummyMasters.UnitMasters[masterId].GetInstance(_id++));
            }
        }

        public async Task<ItemChangedArgs<Unit>[]> FireUnit(int id)
        {
            await Task.Delay(500);

            return FireUnitInternal(id).ToArray();
        }

        private IEnumerable<ItemChangedArgs<Unit>> FireUnitInternal(int id)
        {
            var u = _units.FirstOrDefault(x => x.Id == id);

            // No such unit.
            if (u == null) yield break;

            // Fire!
            _units.Remove(u);
            yield return ItemChangedArgs.Remove<Unit>(id);

            // If the leader is fired, a new leader is hired.
            if (DummyMasters.UnitMasters[u.MasterId].IsLeader)
            {
                var newLeader = _dummy.GetRandomLeader().GetInstance(_id++);
                _units.Add(newLeader);
                yield return ItemChangedArgs.Add(newLeader);
            }
        }
    }
}

using InventoriesSample.DataModels;

namespace InventoriesSample.Dummy
{
    class UnitMaster
    {
        public int MasterId { get; }
        public bool IsLeader { get; }
        public int MaxLife { get; }
        public int MaxEnergy { get; }

        public UnitMaster(int masterId, bool isLeader, int maxLife, int maxEnergy)
        {
            MasterId = masterId;
            IsLeader = isLeader;
            MaxLife = maxLife;
            MaxEnergy = maxEnergy;
        }

        public Unit GetInstance(int id)
            => IsLeader ?
            (Unit)new LeaderUnit { Id = id, MasterId = MasterId, Life = MaxLife, Energy = MaxEnergy } :
            (Unit)new ServantUnit { Id = id, MasterId = MasterId, Life = MaxLife, Energy = MaxEnergy };
    }
}

using Inventories;
using InventoriesSample.DataModels;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using static System.Console;

namespace InventoriesSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var units = new DictionaryInventory<Unit>();

            // subscribe all units.
            units.Changed
                .Subscribe(arg => WriteLine($"{arg.Action} id: {arg.Id ?? arg.Item?.Id}, type: {arg.Item?.GetType()?.Name}, master id: {arg.Item?.MasterId}"));

            // subscribe the leader.
            units.AsObservableEnumerable()
                .Select(u => u as LeaderUnit)
                .Where(u => u != null)
                .Subscribe(leader => WriteLine($"leader updated. current life: {leader.Life}, energy: {leader.Energy}"));

            RunAsync(units, new Dummy.DummyApi()).Wait();
        }

        static async Task RunAsync(DictionaryInventory<Unit> units, IApi api)
        {
            var sync = await api.Login(0);
            units.Reset(sync.Units);

            var r = new Random();

            // fires 5 times randomly
            for (int i = 0; i < 5; i++)
            {
                var ids = units.Ids.ToArray();
                var d = await api.FireUnit(ids[r.Next(0, ids.Length)]);
                foreach (var x in d) units.Change(x);
            }

            // hires 5 times randomly
            for (int i = 0; i < 5; i++)
            {
                var d = await api.HireUnit(r.Next(1000, 1006));
                foreach (var x in d) units.Change(x);
            }
        }
    }
}

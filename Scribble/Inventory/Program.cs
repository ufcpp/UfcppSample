using Inventories;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace InventoriesSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var units = new DictionaryInventory<Unit>();

            units.AsObservableEnumerable()
                .Select(u => u as LeaderUnit)
                .Where(u => u != null)
                .Subscribe(leader => Console.WriteLine("leader updated. current life: \{leader.Life}, energy: \{leader.Energy}"));
        }
    }
}

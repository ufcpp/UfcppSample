using System.Threading.Tasks;

namespace XprojLibrary
{
    public class Class1 : Common.IInterface1
    {
        public string Name => "net46/xproj";

        public async Task<int> GetAsync()
        {
            await Task.Delay(1);
            return 462;
        }
    }
}

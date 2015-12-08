using System.Threading.Tasks;

namespace ClassLibraryNet35
{
    public class Class1 : Common.IInterface1
    {
        public string Name => "net35/packages.config";

        public async Task<int> GetAsync()
        {
            await Task.Delay(1);
            return 350;
        }
    }
}

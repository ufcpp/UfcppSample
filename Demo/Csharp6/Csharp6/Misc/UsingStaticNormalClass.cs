using System.Threading.Tasks;
using static System.Threading.Tasks.Task;
using static System.TimeSpan;

class UsingStaticNormalClass
{
    public async Task XAsync()
    {
        // TimeSpan.FromSeconds
        var sec = FromSeconds(1);

        // Task.Delay
        await Delay(sec);
    }
}

using SysMonitor.NET;
using SysMonitor.NET.Resources;

namespace Example;

internal class Program
{
    static void Main()
    {
        ResourceMonitor monitor = ResourceMonitor.Default;
        monitor.OnUpdate += (sender, data) =>
        {
            Console.WriteLine(data.ToString());
        };
        monitor.Start();
    }
}

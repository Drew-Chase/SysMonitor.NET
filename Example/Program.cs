using Chase.CommonLib.Math;
using Hardware.Info;
using SysMonitor.NET;
using System.Diagnostics;

namespace Example;

internal class Program
{
    static void Main()
    {
        ResourceMonitor monitor = ResourceMonitor.Default;
        monitor.OnUpdate += (sender, data) =>
        {
            Console.Clear();
            if (data.CPU != null)
            {
                Console.WriteLine($"CPU: \n" +
                    $"\tSystem - {data.CPU?.System:P2}\n" +
                    $"\tApplication - {data.CPU?.Application:P2}");
            }
            if (data.RAM != null)
            {
                Console.WriteLine($"RAM: \n" +
                    $"\tSystem - {AdvancedFileInfo.SizeToString((long)(data.RAM?.System ?? 0))} / {AdvancedFileInfo.SizeToString((long)(data.RAM?.Max ?? 0))} ({(data.RAM?.System / (double)(data.RAM?.Max ?? 0)):P2})\n" +
                    $"\tApplication - {AdvancedFileInfo.SizeToString((long)(data.RAM?.Application ?? 0))} / {AdvancedFileInfo.SizeToString((long)(data.RAM?.Max ?? 0))} ({(data.RAM?.Application / (double)(data.RAM?.Max ?? 0)):P2})");
            }
            if (data.Disk != null)
            {
                Console.WriteLine($"Disk: \n" +
                    $"\tSystem - r:{AdvancedFileInfo.SizeToString((long)(data.Disk?.Read ?? 0))}/s w:{AdvancedFileInfo.SizeToString((long)(data.Disk?.Write ?? 0))}/s");
            }
            if (data.Networking != null)
            {
                Console.WriteLine($"Networking: \n" +
                    $"\tSystem - r:{AdvancedFileInfo.SizeToString((long)(data.Networking?.Read ?? 0))}/s w:{AdvancedFileInfo.SizeToString((long)(data.Networking?.Write ?? 0))}/s");
            }
        };

        monitor.Start();

        //// END PROGRAM!
        Console.ReadLine();
        monitor.Stop();

        return;

    }
}

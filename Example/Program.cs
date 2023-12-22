using Chase.CommonLib.Math;
using SysMonitor.NET;
using SysMonitor.NET.Resources;

namespace Example;

internal class Program
{
    private static void Main()
    {
        ResourceMonitor monitor = ResourceMonitor.Default;
        monitor.OnUpdate += (sender, data) =>
        {
            Console.Clear();
            if (data.CPU != null)
            {
                CPUResult cpuInfo = data.CPU;
                Console.WriteLine($"CPU: \n" +
                                  $"\tSystem - {cpuInfo.System:P2}\n" +
                                  $"\tApplication - {cpuInfo.Application:P2}");
            }

            if (data.RAM != null)
            {
                RAMResult ramInfo = data.RAM;
                double systemRamUsage = ramInfo.System / (double)(ramInfo.Max);
                double appRamUsage = ramInfo.Application / (double)(ramInfo.Max);

                Console.WriteLine($"RAM: \n" +
                                  $"\tSystem - {AdvancedFileInfo.SizeToString((long)(ramInfo.System))} / " +
                                  $"{AdvancedFileInfo.SizeToString((long)(ramInfo.Max))} ({systemRamUsage:P2})\n" +
                                  $"\tApplication - {AdvancedFileInfo.SizeToString((long)(ramInfo.Application))} / " +
                                  $"{AdvancedFileInfo.SizeToString((long)(ramInfo.Max))} ({appRamUsage:P2})");
            }

            if (data.Disk != null)
            {
                RWData? diskInfo = data.Disk;
                Console.WriteLine($"Disk: \n" +
                                  $"\tSystem - r:{AdvancedFileInfo.SizeToString((long)(diskInfo?.Read ?? 0))}/s " +
                                  $"w:{AdvancedFileInfo.SizeToString((long)(diskInfo?.Write ?? 0))}/s");
            }

            if (data.Networking != null)
            {
                RWData? networkInfo = data.Networking;
                Console.WriteLine($"Networking: \n" +
                                  $"\tSystem - r:{AdvancedFileInfo.SizeToString((long)(networkInfo?.Read ?? 0))}/s " +
                                  $"w:{AdvancedFileInfo.SizeToString((long)(networkInfo?.Write ?? 0))}/s");
            }

        };

        monitor.Start();

        //// END PROGRAM!
        Console.ReadLine();
        monitor.Stop();

        return;
    }
}
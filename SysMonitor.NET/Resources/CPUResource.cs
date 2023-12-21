using System.Diagnostics;

namespace SysMonitor.NET.Resources;

public class CPUResource(string name, double maxValue, double minValue) : ResourceItemBase<double>(name, maxValue, minValue)
{
    public override double GetApplicationUsage()
    {
        throw new NotImplementedException();
    }

    public override double GetSystemUsage()
    {
        long startTime = Environment.TickCount64;
        Process[] startProcesses = Process.GetProcesses();
        while ((Environment.TickCount64 - startTime) < 100) // Wait 100ms
        {
        }

        Process[] endProcesses = Process.GetProcesses();
        double startTotalProcessTime = startProcesses.Sum(p =>
        {
            try
            {
                return p.TotalProcessorTime.TotalMilliseconds;
            }
            catch (UnauthorizedAccessException)
            {
                return 0;
            }
        });
        double endTotalProcessTime = endProcesses.Sum(p =>
        {
            try
            {
                return p.TotalProcessorTime.TotalMilliseconds;
            }
            catch (UnauthorizedAccessException)
            {
                return 0;
            }
        });
        double totalProcessTime = endTotalProcessTime - startTotalProcessTime;
        double totalSystemTime = Environment.TickCount64 - startTime;
        double timePerCore = totalProcessTime / Environment.ProcessorCount * totalSystemTime;
        double adjustedTimePerCore = timePerCore * 10;
        return Math.Min(adjustedTimePerCore, 1);

    }
}

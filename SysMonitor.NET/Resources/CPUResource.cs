using System.Diagnostics;

namespace SysMonitor.NET.Resources;

public class CPUResource : ResourceItemBase<double>
{
    public CPUResource() : base("CPU", 1d, 0d)
    {
        Result = new CPUResult() { Max = MaxValue, Min = MinValue };
    }
    public override double GetApplicationUsage()
    {

        long startTime = Environment.TickCount64;

        TimeSpan startUsage = Process.GetCurrentProcess().TotalProcessorTime;

        while ((Environment.TickCount64 - startTime) < 100)
        {
        }

        TimeSpan currentUsage = Process.GetCurrentProcess().TotalProcessorTime;


        double elapsedMilliseconds = Environment.TickCount64 - startTime;

        double value = Math.Min((currentUsage - startUsage).TotalMilliseconds / (Environment.ProcessorCount * elapsedMilliseconds), 1);
        ((CPUResult)Result).Application = value;
        return value;
    }

    public override double GetSystemUsage()
    {
        long startTime = Environment.TickCount64;

        Process[] processesBefore = Process.GetProcesses();

        while ((Environment.TickCount64 - startTime) < 100)
        {
        }

        Process[] processesAfter = Process.GetProcesses();

        double totalProcessTimeBefore = processesBefore.Sum(p =>
        {
            try
            {
                return p.TotalProcessorTime.TotalMilliseconds;
            }
            catch
            {
                return 0;
            }
        });

        double totalProcessTimeAfter = processesAfter.Sum(p =>
        {
            try
            {
                return p.TotalProcessorTime.TotalMilliseconds;
            }
            catch
            {
                return 0;
            }
        });

        double elapsedMilliseconds = Environment.TickCount64 - startTime;

        double value = Math.Min((totalProcessTimeAfter - totalProcessTimeBefore) / ((Environment.ProcessorCount / 2) * elapsedMilliseconds) * 10, 1);
        ((CPUResult)Result).System = value;
        return value;
    }

}
